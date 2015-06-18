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
using NLog;

namespace Damascus.Web.Controllers
{
    public class WorkflowController : Controller
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IReplyStore ReplyStore { get; set; }
        public Logger Logger { get; set; }
        
        public WorkflowController(IReplyStore replyStore, WorkflowEngine workflowEngine)
        {   	
            Logger = LogManager.GetLogger(GetType().FullName);
            ReplyStore = replyStore;
            WorkflowEngine = workflowEngine;
        }
        
        [Route("api/workflow/call")]
        public ContentResult Call(string type, string step, TwillioInput input)
        {
            try
            {
                Logger.Info("Received a PhoneCall Workflow");
                
                var parameters = FillParametersDict(input);
    
                var workflowContext = new WorkflowContext()
                {
                    WorkflowId = parameters.ContainsKey("id") ? parameters["id"] : Guid.NewGuid().ToString(),
                    WorkflowType = type,
                    WorkflowStep = step,
                    DataKey = parameters["Phone"].NormalizePhone(),
                    Parameters = parameters
                };
                
                Logger.Info("Finished PhoneCall Workflow");
                return Content(WorkflowEngine.Process(workflowContext), "text/xml");
             }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
                
                Context.Response.StatusCode = 500;
                return Content(ex.Message, "text/xml");
            }
        }
        
        [Route("api/workflow/sms")]
        public ContentResult Sms(string type, string step, TwillioInput input)
        {
            try
            {
                Logger.Info("Received an inbound SMS");
                
                var parameters = FillParametersDict(input);
                                
                Logger.Info("Workflow Info: ");
                Logger.Info("Type: " + type);
                Logger.Info("Step: " + step);
                
                var workflowContext = new WorkflowContext()
                {
                    WorkflowType = type,
                    WorkflowStep = step,
                    DataKey = input.From.NormalizePhone(),
                    Parameters = parameters
                };
                
                Logger.Info("Finished inbound SMS");
                return Content(WorkflowEngine.Process(workflowContext), "text/xml");
             }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                return Content(ex.Message, "text/xml");
            }
        }
        
        [Route("api/workflow/email")]
        public ContentResult Email(string type, string step, TwillioInput input)
        {
            try
            {
                var parameters = FillParametersDict(input);
    
                var workflowContext = new WorkflowContext()
                {
                    WorkflowType = type,
                    WorkflowStep = step,
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
                Logger.Error(ex.ToString());
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
