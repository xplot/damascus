using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Damascus.Core;
using Damascus.Message;
using Damascus.Workflow;
using System.Collections.Generic;
using Twilio.TwiML.Mvc;

namespace Damascus.Web.Controllers
{
    public class WorkflowController : BaseController
    {
        public WorkflowEngine WorkflowEngine { get; set; }
        public IReplyStore ReplyStore { get; set; }

        public ActionResult Call()
        {
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
        }

        public ActionResult Sms()
        {
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
        }

        public ActionResult Email()
        {
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
        }

        private StepInput FillParametersDict()
        {
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
        }
    }
}
