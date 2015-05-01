using System;
using System.Diagnostics;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;

namespace Damascus.Web.Controllers
{
    
    public class InviteController : Controller
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IDataSerializer DataSerializer { get; set; }
        public ILogger Logger { get; set; }
        
        
        public InviteController(ILoggerFactory loggerFactory)
        {   	
            Logger = loggerFactory.CreateLogger(typeof(InviteController).FullName);
        }
        
        [Route("api/invite")]
        public string CreateInvite(InviteInput input)
        {
            /*
            if (input == null)
                throw new Exception("Invite format is not valid");

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
            */
            
            Logger.LogInformation("invite executed");
            return "hello";
        }
    	[Route("api/invite/attendees")]
        public string InviteAttendees(InviteAttendeesInput input)
        {
         
            /*   
            if (input == null)
                throw new Exception("Invite format is not valid");

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
            */
            return "attendees";
        }
        
        [Route("api/invite/cancel")]
        public string CancelInvite(InviteAttendeesInput input)
        {
            /*
            if (input == null)
                throw new Exception("Invite format is not valid");

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
            */
            return "cancel";
        }
    }
}
