using System;
using System.Collections.Generic;
using AutoMapper;
using Damascus.Core;
using Damascus.Core.AutoMapper;
using Damascus.Message;
using Damascus.Message.Command;
using Newtonsoft.Json;
using NServiceBus;
using System.Diagnostics;

namespace Damascus.Workflow
{
    [WorkflowName("invite")]
    [SmsReplyConfiguration("invite", new string[]
        {
            "smsin - yes,no",        
        }
    )]
    public class InviteWorkflow : Damascus.Core.Workflow
    {
        public IIvrXmlWriter XmlWriter { get; set; }
        public IBus Bus { get; set; }
        public TwillioConfig TwillioConfig { get; set; }
        public ISettings Settings { get; set; }

        public const string START_STEP = "allout";

        public InviteWorkflow()
        {
            this.Steps = new Dictionary<string, IStep>()
            {
                {"callin", new FunctionStep(InviteCallReply)},
                {"callreply", new FunctionStep(InviteCallReplyResult)},
                {"smsin", new FunctionStep(InviteSmsReply)},
                {"emailin", new FunctionStep(InviteEmailReply)},
            };
        }
        
        private InviteInput GetInvite()
        {
            var inviteId = Data["invite_id"];
            
            Trace.WriteLine("Trying to find invite by id: " + inviteId);
            
            return InviteInput.FromDict(DataSerializer.DeserializeData(
                WorkflowEngine.GetDataKey("multi_invite", inviteId)
            ));
        }

        private string InviteSmsReply(IStepInput input)
        {
            var phoneNumber = input["Phone"].NormalizePhone();
            
            if(!IsAttendeeInvited())
            {
                Bus.Send( "Damascus.MessageChannel", new CreateSmsMessage()
                {
                    PhoneNumber = phoneNumber,
                    Message = "You havent been invited",
                    Id = Guid.NewGuid().ToString()
                });
                return "Success";
            }
            
            var invite = GetInvite();
            var message = string.Empty;
            
            if (Data.ContainsKey("responded"))
                message = "You already responded to the invitation!";
            else if(input["Body"].ToLower() == "no")
            {
                message = string.Format("Im so sorry to hear that you cannot come to {0}", invite.Title);
                Bus.Send( "Damascus.MessageChannel", GetApiCall("sms", "no"));    
            }
            else if(input["Body"].ToLower() == "yes")
            {
                if(InviteIsInThePast(invite))
                    message = string.Format("I am sorry but you are late. {0} happened on {0}. You can no longer come. Good bye.", invite.Title, invite.Start);
                else if(InviteIsFull(invite))
                    message = string.Format("I am sorry but {0}, has reached full capacity. Please contact the host. Good bye", invite.Title, invite.Start);    
                else
                {
                    message = string.Format("I am glad you coming to {0}. Remember we start at {1}", invite.Title, invite.Start);
                    Bus.Send( "Damascus.MessageChannel", GetApiCall("sms", "yes"));
                }
            }   
            else
                message = string.Format("I am sorry but I didnt understand your response. Reply YES if you want to come, NO if not");
            
            Bus.Send( "Damascus.MessageChannel", new CreateSmsMessage()
            {
                PhoneNumber = phoneNumber,
                Message = message,
                Id = Guid.NewGuid().ToString()
            });
            
            return "Success";
            
        }

        private string InviteCallReply(IStepInput input)
        {
            if(!IsAttendeeInvited())
            {
                XmlWriter.SayMessage("You have not been invited to this event, hang up");
                return XmlWriter.ToString();
            }
            
            var invite = GetInvite();
            var contact = Contact.FromDict(this.Data);

            XmlWriter.EnterNumber(
                string.Format("Hello {0}, You have been invited to {1} on {2}. Please press 1 if you want to come, 2 if not.",
                    contact.Name, 
                    invite.Title,
                    invite.Start.ToString()
                ),
                new Dictionary<string, string>()
                {
                    {"step", "callreply"},
                    {"type", "invite"}
                }
            );
            
            return XmlWriter.ToString();
        }

        private string InviteCallReplyResult(IStepInput input)
        {
            
            var message = string.Empty;
            var invite = GetInvite();
            
            if(input["Body"] == "2")
            {
                XmlWriter.SayMessage("I am really sorry you can't come to " + invite.Title);
                Bus.Send( "Damascus.MessageChannel", GetApiCall("voice", "no"));
            }
            else if(input["Body"] == "1")
            {
                if(InviteIsInThePast(invite))
                    XmlWriter.SayMessage("I am sorry but you are late to the event. You can no longer come. Good bye.");
                else if(InviteIsFull(invite))
                    XmlWriter.SayMessage("I am sorry but this event, has reached full capacity. Please contact the host. Good bye");
                else //This is when the invite is accepting people in
                {
                    XmlWriter.SayMessage("I am really glad, that you will be coming to " + invite.Title);
                    Bus.Send( "Damascus.MessageChannel", GetApiCall("voice", "yes"));        
                }
            }
            else
            {
                XmlWriter.EnterNumber(
                    "I am sorry but I didnt understand your response, you pressed the wrong key. Please press 1 if you want to come. Press 2 if not coming",
                    new Dictionary<string, string>()
                    {
                        {"step", "callreply"},
                        {"type", "invite"}
                    }
                );
                
            }
            return XmlWriter.ToString();
        }

        private string InviteEmailReply(IStepInput input)
        {
            throw new NotImplementedException();
        }

        private ServiceCallMessage GetApiCall(string channel, string response)
        {
            return new ServiceCallMessage()
            {
                Url =
                    Settings.Get("imeet.api.url") + "/invite/attendees/" + Data["unique_id"] + "/response",
                Method = "POST",
                Headers = new Dictionary<string, string>()
                {
                    {"Content-Type", "application/json"}
                },
                Format = "json",
                
                Payload = new Dictionary<string, string>()
                {
                    {"channel", channel},
                    {"response", response},
                }
            };
        }
        
        private bool IsAttendeeInvited()
        {
            return Data["invite_unique_id"] != null;            
        }
        
        private bool InviteIsInThePast(InviteInput invite)
        {
            return invite.Start <= DateTime.Now;            
        }
        
        private bool InviteIsFull(InviteInput invite)
        {
            if(invite.MaxAttendees <= 0)
                return false;
            
            return invite.MaxAttendees <= invite.Confirmed;  
        }

    }
}
