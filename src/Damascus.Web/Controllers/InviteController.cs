using System;
using System.Diagnostics;
using System.Web.Http;
using Damascus.Core;
using Damascus.Message;
using Damascus.Web.Helpers;
using Damascus.Workflow;

namespace Damascus.Web.Controllers
{
    [RequireAuthorization]
    public class InviteController : ApiController
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IDataSerializer DataSerializer { get; set; }

        public string CreateInvite(InviteInput input)
        {
            if (input == null)
                this.InternalServerError();

            Trace.WriteLine("New invite is posted: " + input.InviteId);

            var previous_invite = DataSerializer.DeserializeData(
                WorkflowEngine.GetDataKey("multi_invite", input.InviteId)
            );

            if (previous_invite != null)
                return "This invitation has been already sent";

            var workflowContext = new WorkflowContext()
            {
                WorkflowStep = "create",
                DataKey = input.InviteId,
                WorkflowType = "multi_invite",
                WorkflowId = input.InviteId,
                Parameters = input,
            };

            return WorkflowEngine.Process(workflowContext);
        }

        public string InviteAttendees(InviteAttendeesInput input)
        {
            
            if (input == null)
                this.InternalServerError();

            Trace.WriteLine("Contacts are going to be invited for invite: " + input.InviteId);

            var workflowContext = new WorkflowContext()
            {
                WorkflowStep = "invite_contacts",
                DataKey = input.InviteId,
                WorkflowType = "multi_invite",
                WorkflowId = input.InviteId,
                Parameters = input,
            };

            return WorkflowEngine.Process(workflowContext);
        }

        public string CancelInvite(InviteAttendeesInput input)
        {
            if (input == null)
                this.InternalServerError();

            Trace.WriteLine("Invite is Cancelled: " + input.InviteId);

            var workflowContext = new WorkflowContext()
            {
                WorkflowStep = "cancel",
                DataKey = input.InviteId,
                WorkflowType = "multi_invite",
                WorkflowId = input.InviteId,
                Parameters = input,
            };

            return WorkflowEngine.Process(workflowContext);
        }
    }
}
