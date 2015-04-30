using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using System.Collections.Generic;

namespace Damascus.Web.Controllers
{
    public class WorkflowController : BaseController
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IReplyStore ReplyStore { get; set; }

        public string Call()
        {
            /*
            var parameters = FillParametersDict();

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
 
            return Content(WorkflowEngine.Process(workflowContext), "text/xml");
            */
            return null;
        }

        public string Sms()
        {
            /*
            var parameters = FillParametersDict();
            var workflowInfo = ReplyStore.GetWorkflowConfigFromReply(Body);

            var workflowContext = new WorkflowContext()
            {
                WorkflowType = workflowInfo.WorkflowType,
                WorkflowStep = workflowInfo.WorkflowStep,
                DataKey = parameters["Phone"].NormalizePhone(),
                Parameters = parameters
            };

            return Content(WorkflowEngine.Process(workflowContext), "text/xml");
            */
            return null;
        }

        public string Email()
        {
            /*
            var parameters = FillParametersDict();

            var workflowContext = new WorkflowContext()
            {
                WorkflowType = parameters["type"],
                WorkflowStep = parameters["step"],
                DataKey = parameters["Email"],
                Parameters = parameters
            };
            var response = WorkflowEngine.Process(workflowContext);

            if (response.StartsWith("http"))
                return this.Redirect(response);
            return Content(response, "text/xml");
            */
            return null;
        }

        private StepInput FillParametersDict()
        {
            /*
            var result = new Dictionary<string, string>();

            foreach (var queryVar in this.Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(queryVar) && Request.QueryString[queryVar] != null)
                    result[queryVar] = Request.QueryString[queryVar];

            }

            result["Body"] = (SmsSid!= null)?Body:Digits;
            result["TwilioId"] = SmsSid ?? CallSid;
            result["Phone"] = (Direction == "inbound" || !string.IsNullOrEmpty(SmsSid)) ? From : To;
            result["Email"] = result.ContainsKey("email")? result["email"]:null;

            return new StepInput(){Input = result};
            */
            return null;
        }
    }
}
