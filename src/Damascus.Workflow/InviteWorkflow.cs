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

        private string InviteSmsReply(IStepInput input)
        {
            if (Data["invite_unique_id"] != null)
            {
                string message = (input["Body"].ToLower() == "yes")
                    ? "I'm glad you coming!"
                    : "Im so sorry to hear that you cannot come";

                if (Data.ContainsKey("responded"))
                    message = "You already responded to the invitation";

                var phoneNumber = input["Phone"].NormalizePhone();

                Bus.Send( "Damascus.MessageChannel", new CreateSmsMessage()
                {
                    PhoneNumber = phoneNumber,
                    Message = message,
                    Id = Guid.NewGuid().ToString()
                });

                Bus.Send( "Damascus.MessageChannel", GetApiCall("sms",input["Body"]));

                return "<response>Response To Confirmation sent succesfully</response>";
            }
            else
            {
                return "<Response>You havent been invited</Response>";
            }
        }

        private string InviteCallReply(IStepInput input)
        {
            if (Data["invite_unique_id"] != null)
            {
                var invite = InviteInput.FromDict(this.Data);
                var contact = Contact.FromDict(this.Data);

                var message = "Hello {0}, You have been invited to {1} on {2}. Please press 1 if you want to come, 2 if not.";

                message = string.Format(
                    message, 
                    contact.Name, 
                    invite.Title,
                    invite.Start.ToString()
                );

                XmlWriter.EnterNumber(
                    message,
                    new Dictionary<string, string>()
                    {
                        {"step", "callreply"},
                        {"type", "invite"}
                    }
                );
            }
            else
            {
                XmlWriter.SayMessage("You have not been invited to this event, hang up");
            }

            return XmlWriter.ToString();
        }

        private string InviteCallReplyResult(IStepInput input)
        {
            var message = string.Empty;
            message = input["Body"] == "1" ? "I'm glad you comming!" : "I'm dissapointed";

            Bus.Send( "Damascus.MessageChannel", GetApiCall("voice", (input["Body"] == "1")?"yes":"no"));

            XmlWriter.SayMessage(message);
            return XmlWriter.ToString();
        }

        private string InviteEmailReply(IStepInput input)
        {
            if (Data["invite_unique_id"] != null)
            {
                var invite = InviteInput.FromDict(this.Data);
                var contact = Contact.FromDict(this.Data);

                Bus.Send( "Damascus.MessageChannel", new CreateEmailMessage()
                {
                    Id = Guid.NewGuid().ToString(),
                    Address = contact.Email,
                    Sender = "invite@voiceflows.com",
                    Subject = "Thanks for your Reply",
                    BodyTemplate = invite.ResponseEmailTemplate
                });

                Bus.Send( "Damascus.MessageChannel", GetApiCall("email", "yes"));

                if (invite.ResponseEmailTemplate != null && !string.IsNullOrEmpty(invite.ResponseEmailTemplate.RedirectUrl))
                {
                    var templateManager = new MemoryTemplateManager();
                    return templateManager.Fill(invite.ResponseEmailTemplate.RedirectUrl, this.Data);
                }
                else
                    return "<Response>Thank you for your response</Response>";
            }
            else
                return "<Response>You havent been invited</Response>";
        }

        private ServiceCallMessage GetApiCall(string channel, string response)
        {
            return new ServiceCallMessage()
            {
                Url =
                    "http://invite.voiceflows.com/api/invite/attendees/" + Data["unique_id"] + "/response",
                Method = "POST",
                Headers = new Dictionary<string, string>()
                {
                    {"Content-Type", "application/json"}
                },
                Payload = new Dictionary<string, string>()
                {
                    {"channel", channel},
                    {"response", response},
                }
            };
        }

    }
}
