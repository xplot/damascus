using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core
{
    public class SmsReplyConfigurationAttribute:Attribute
    {
        public IList<WorkflowStepReply> SmsStepReplies { get; set; }
        public SmsReplyConfigurationAttribute(string workflowType, string[]replies)
        {
            SmsStepReplies = new List<WorkflowStepReply>();
            foreach(var reply in replies)
            {
                var split = reply.Split('-');
                SmsStepReplies.Add(new WorkflowStepReply(){
                    WorkflowType = workflowType,
                    WorkflowStep = split[0].Trim(),
                    ValidReplies = split[1].Split(',')
                });
            }
        }
    }
}
