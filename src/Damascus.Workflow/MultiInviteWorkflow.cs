﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Damascus.Core;
using Damascus.Message;
using NServiceBus;

namespace Damascus.Workflow
{
    [WorkflowName("multi_invite")]
    public class MultiInviteWorkflow : Damascus.Core.Workflow
    {
        public TwillioConfig TwillioConfig { get; set; }
        public IDataSerializer DataSerializer { get; set; }
        public IBus Bus { get; set; }

        public MultiInviteWorkflow()
        {
            this.Steps = new Dictionary<string, IStep>(){
                { "create", new FunctionStep(CreateInvite) },
                { "invite_contacts", new FunctionStep(InviteAttendees) },
                { "cancel", new FunctionStep(CancelInvite) },
            };
        }

        private string CreateInvite(IStepInput input)
        {
            var invite = input as InviteInput;
            var inviteId = invite.InviteId;

            if (!IsCallRepeated(invite))
                EnsureCallIsNotRepeated(invite);
            else
                return "<response>Invite sent already</response>";

            Trace.WriteLine("Starting to Send invite with Id: " + inviteId);
            this.Data.Update(invite.ToDict());

            ShareToSocialNetworks(invite);

            return "<response>Invite sent succesfully</response>";
        }


        private string CancelInvite(IStepInput input)
        {
            var CancelAttendees = (InviteAttendeesInput)input;
            var invite = InviteInput.FromDict(this.Data);

            if (!IsCallRepeated(invite))
                EnsureCallIsNotRepeated(invite);
            else
                return "<response>Invite cancelled already</response>";

            //This is how we ensure we dont contact the same people over the same message again
            //Think duplicate calls to the API
            if (DataSerializer.DeserializeData(WorkflowEngine.GetDataKey("multi_invite", CancelAttendees.UniqueCallId)) != null)
                return "<response>Contacts have been notified already</response>";
            else
                DataSerializer.SerializeData(WorkflowEngine.GetDataKey("multi_invite", CancelAttendees.UniqueCallId), new DataStorage());

            foreach (var contact in CancelAttendees.Attendees)
            {
                CancelAttendee(invite, contact);
            }

            return "<response>Event cancelled succesfully</response>";
        }

        private string InviteAttendees(IStepInput input)
        {
            var inviteAttendees = (InviteAttendeesInput)input;
            if(!this.Data.ContainsKey("invite_unique_id"))
                return "<response>The invite with id " + inviteAttendees.InviteId + " doesnt exists, please create the invite first</response>";

            if (!IsCallRepeated(inviteAttendees))
                EnsureCallIsNotRepeated(inviteAttendees);
            else
                return "<response>Contacts notified already</response>";
            
            var invite = InviteInput.FromDict(this.Data);
            foreach (var contact in inviteAttendees.Attendees)
            {
                InviteAttendee(invite, contact);
            }

            return "<response>Contacts notified succesfully</response>";
        }


        private void InviteAttendeeEmail(InviteInput invite, Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Email))
                return;

            var templateManager = new MemoryTemplateManager();
            var bodyData = GetTemplateContextData(invite, contact);

            Bus.Send(new CreateEmailMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Address = contact.Email,
                Sender = "invite@voiceflows.com",
                Subject = templateManager.Fill(invite.EmailTemplate.Subject, bodyData),
                BodyTemplate = invite.EmailTemplate,
                BodyData = bodyData,
            });
        }

        private void CancelAttendeeEmail(InviteInput invite, Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Email))
                return;

            var templateManager = new MemoryTemplateManager();
            var bodyData = GetTemplateContextData(invite, contact);

            Bus.Send(new CreateEmailMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Address = contact.Email,
                Sender = "invite@voiceflows.com",
                Subject = templateManager.Fill(invite.CancelEmailTemplate.Subject, bodyData),
                BodyTemplate = invite.CancelEmailTemplate,
                BodyData = bodyData,
            });
        }

        private void InviteAttendeeSms(InviteInput invite, Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Phone))
                return;

            var date = invite.Start.ToString();
            if (invite.End != null)
                date += "to " + invite.End.ToString();

            Bus.Send(new CreateSmsMessage()
            {
                PhoneNumber = contact.Phone,
                Message = string.Format("You are hereby invited to {0} event on {1}, want to go? YES/NO", invite.Title, date),
                Id = Guid.NewGuid().ToString()
            });
        }

        private void CancelAttendeeSms(InviteInput invite, Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Phone))
                return;
            Bus.Send(new CreateSmsMessage()
            {
                PhoneNumber = contact.Phone,
                Message = string.Format("The event {0} on {1}, have been cancelled by the host", invite.Title, invite.Start.ToString()),
                Id = Guid.NewGuid().ToString()
            });
        }

        private void InviteAttendeePhone(InviteInput invite, Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Phone))
                return;

            Bus.Send(new CreateCallMessage()
            {
                PhoneNumber = contact.Phone,
                Id = Guid.NewGuid().ToString(),
                Parameters = new Dictionary<string, string>(){
                    { "step", "callin"},
                    {"type", "invite"},
                }
            });
        }

        private void CancelAttendeePhone(InviteInput invite, Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Phone))
                return;

            Bus.Send(new CreateCallMessage()
            {
                PhoneNumber = contact.Phone,
                Id = Guid.NewGuid().ToString(),
                Parameters = new Dictionary<string, string>(){
                    { "step", "cancel"},
                    {"type", "invite"},
                }
            });
        }

        private void InviteAttendee(InviteInput invite, Contact contact)
        {
            var workflowData = new DataStorage(invite.ToDict().Merge(contact.ToDict()));
            
            InviteAttendeeEmail(invite, contact);
            DataSerializer.SerializeData(WorkflowEngine.GetDataKey("invite", contact.Email), workflowData);

            if (!string.IsNullOrEmpty(contact.Phone))
            {
                contact.Phone = contact.Phone.NormalizePhone();

                InviteAttendeeSms(invite, contact);
                InviteAttendeePhone(invite, contact);

                DataSerializer.SerializeData(WorkflowEngine.GetDataKey("invite", contact.Phone), workflowData);
            }
            
        }

        private void CancelAttendee(InviteInput invite, Contact contact)
        {
            var workflowData = new DataStorage(invite.ToDict().Merge(contact.ToDict()));

            CancelAttendeeEmail(invite, contact);
            DataSerializer.SerializeData(WorkflowEngine.GetDataKey("invite", contact.Email), workflowData);

            if (!string.IsNullOrEmpty(contact.Phone))
            {
                contact.Phone = contact.Phone.NormalizePhone();

                CancelAttendeeSms(invite, contact);
                CancelAttendeePhone(invite, contact);

                DataSerializer.SerializeData(WorkflowEngine.GetDataKey("invite", contact.Phone), workflowData);
            }
        }

        public void ShareToSocialNetworks(InviteInput invite)
        {
            if (invite.SharingOptions == null)
                return;

            var reducedInvite = Mapper.Map<InviteInput, ReducedInviteInput>(invite);

            if (!string.IsNullOrEmpty(invite.SharingOptions.FacebookAccessToken))
            {
                Trace.WriteLine("Sharing to Facebook");
                Bus.Send(new FacebookEventMessage()
                {
                    Invite = reducedInvite,
                    AccessToken = invite.SharingOptions.FacebookAccessToken
                });
            }

        }

        public Dictionary<string, string> GetTemplateContextData(InviteInput invite, Contact contact)
        {
            var template_data = invite.ToDict().Merge(contact.ToDict());
            
            template_data["ACKNOWLEDGE_LINK"] = TwillioConfig.EmailCallbackUrl + "?email=" + contact.Email +
                    "&type=invite&step=emailin&contactId=" + contact.ContactId;
            template_data["fullname"] = (string.IsNullOrEmpty(contact.Name)) ? " there" : contact.Name;

            return template_data;
        }

        public bool IsCallRepeated(IUniqueCall uniqueCall)
        {
            return this.Data.ContainsKey("call_ids") && this.Data["call_ids"].Contains(uniqueCall.UniqueCallId);
        }

        public void EnsureCallIsNotRepeated(IUniqueCall uniqueCall)
        {
            var call_ids = (this.Data.ContainsKey("call_ids")) ? this.Data["call_ids"] : "";
            call_ids += uniqueCall.UniqueCallId;
            this.Data["call_ids"] = call_ids;
        }
    }
}
