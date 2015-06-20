using System;
using System.Diagnostics;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using NLog;
using NServiceBus;

namespace Damascus.Web.Controllers
{
    
    [Authenticate]
    public class InviteController : Controller
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IDataSerializer DataSerializer { get; set; }
        public Logger Logger { get; set; }

        public InviteController(WorkflowEngine engine, IDataSerializer serializer)
        {   	
            Logger = LogManager.GetLogger(GetType().FullName);
            DataSerializer = serializer;
            WorkflowEngine = engine;
        }
        
        [Route("api/invite")]
        public string CreateInvite([FromBody]InviteInput input)
        {
            Logger.Info("Creating an Invite");
            
            try
            {
                
                if (input == null || input.InviteId == null)
                    throw new Exception("Invite format is not valid");
                
                Logger.Info("InviteID: " + input.InviteId);
                
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
            catch(Exception ex)
            {
                 Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                return null;
            }    
        }
        
    	[Route("api/invite/attendees")]
        public string InviteAttendees([FromBody]InviteAttendeesInput input)
        {
            Logger.Info("Request to post invite Attendees");
            
            try
            {
                
                if (input == null)
                    throw new Exception("Invite attendees format is not valid");
    
                Logger.Info("Invite " + input.InviteId);
    
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
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                return null;
            }    
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
