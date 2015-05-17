using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;
using NServiceBus;


namespace Damascus.Web.Controllers
{
    public class WorkflowController : Controller
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IReplyStore ReplyStore { get; set; }
        public ILogger Logger { get; set; }
        
        public WorkflowController(ILoggerFactory loggerFactory,
                                    IReplyStore replyStore,
                                    WorkflowEngine workflowEngine)
        {   	
            
            Logger = loggerFactory.CreateLogger(typeof(InviteController).FullName);
            ReplyStore = replyStore;
            WorkflowEngine = workflowEngine;
        }
        
        [Route("api/workflow/call")]
        public ContentResult Call(TwillioInput input)
        {
            try
            {
                Logger.LogInformation("Received a Phone hit ");
                
                var parameters = FillParametersDict(input);
                
                if (!parameters.ContainsKey("type") || !parameters.ContainsKey("step"))
                    throw new Exception("We could not obtain the type of your workflow from your " +
                                        "input, try setting the type parameter when posting to this service");
    
                var workflowContext = new WorkflowContext()
                {
                    WorkflowId = parameters.ContainsKey("id") ? parameters["id"] : Guid.NewGuid().ToString(),
                    WorkflowType = parameters["type"],
                    WorkflowStep = parameters["step"],
                    DataKey = parameters["Phone"].NormalizePhone(),
                    Parameters = parameters
                };
                
                Logger.LogInformation("Return Phone hit");
                return Content(WorkflowEngine.Process(workflowContext), "text/xml");
             }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                
                Context.Response.StatusCode = 500;
                return Content(ex.Message, "text/xml");
            }
        }
        
        [Route("api/workflow/sms")]
        public ContentResult Sms(TwillioInput input)
        {
            try
            {
                Logger.LogInformation("Received an SMS Call hit ");
                
                var parameters = FillParametersDict(input);
                var workflowInfo = ReplyStore.GetWorkflowConfigFromReply(input.Body);
                
                Logger.LogInformation("Workflow Info: ");
                Logger.LogInformation("Type: " + workflowInfo.WorkflowType);
                Logger.LogInformation("Step: " + workflowInfo.WorkflowStep);
                
                var workflowContext = new WorkflowContext()
                {
                    WorkflowType = workflowInfo.WorkflowType,
                    WorkflowStep = workflowInfo.WorkflowStep,
                    DataKey = parameters["Phone"].NormalizePhone(),
                    Parameters = parameters
                };
                
                Logger.LogInformation("Return SMS hit");
                return Content(WorkflowEngine.Process(workflowContext), "text/xml");
             }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                
                Context.Response.StatusCode = 500;
                return Content(ex.Message, "text/xml");
            }
        }
        
        [Route("api/workflow/email")]
        public ContentResult Email(TwillioInput input)
        {
            try
            {
                var parameters = FillParametersDict(input);
    
                var workflowContext = new WorkflowContext()
                {
                    WorkflowType = parameters["type"],
                    WorkflowStep = parameters["step"],
                    DataKey = parameters["Email"],
                    Parameters = parameters
                };
                
                var response = WorkflowEngine.Process(workflowContext);
    
        	   //if (response.StartsWith("http"))
               //   return this.Redirect(response);
                    
                return Content(response, "text/xml");
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                
                Context.Response.StatusCode = 500;
                return Content(ex.Message, "text/xml");
            }
        }

        private StepInput FillParametersDict(TwillioInput twillioInput)
        {
            var result = new Dictionary<string, string>();
            
            foreach (var queryVar in this.Request.Form)
            {
                result[queryVar.Key] = (queryVar.Value != null && queryVar.Value.Length > 0 )? queryVar.Value[0]: null ;
            }
            
            foreach (var queryVar in this.Request.Query)
            {
                result[queryVar.Key] = (queryVar.Value != null && queryVar.Value.Length > 0 )? queryVar.Value[0]: null ;
            }

            result["Body"] = (twillioInput.SmsSid!= null)?twillioInput.Body:twillioInput.Digits;
            result["TwilioId"] = twillioInput.SmsSid ?? twillioInput.CallSid;
            result["Phone"] = (twillioInput.Direction == "inbound" || !string.IsNullOrEmpty(twillioInput.SmsSid)) ? twillioInput.From : twillioInput.To;
            result["Email"] = result.ContainsKey("email")? result["email"]:null;
            
            return new StepInput(){Input = result};
        }
        
        private string DictToString(Dictionary<string,string> dict)
        {
            return string.Join(";", dict.Select(x => x.Key + "=" + x.Value));
        }
    }
}
