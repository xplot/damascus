using System;
using System.Reflection;
using System.Collections.Generic;
using Castle.Windsor;
using Damascus.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Damascus.Core
{
    public class ReplyStore:IReplyStore
    {
        private List<WorkflowStepReply> stepReplyConfiguration;
        
        public ReplyStore()
        {
            this.stepReplyConfiguration = new List<WorkflowStepReply>();
        }
        
        public ReplyStore(IEnumerable<WorkflowStepReply> replyConfiguration )
        {
            this.stepReplyConfiguration = new List<WorkflowStepReply>(replyConfiguration);
        }
        
        public void RegisterWorkflow(Type workflowType)
        {
            //Registering Sms Reponses
            var sms_replies = (SmsReplyConfigurationAttribute)workflowType.GetCustomAttribute(typeof(SmsReplyConfigurationAttribute));
            if(sms_replies != null)
                this.stepReplyConfiguration.AddRange(sms_replies.SmsStepReplies);
                
            //Here other Workflow Reply Configuration has to happen    
               
        }
        
        public WorkflowStepReply GetWorkflowConfigFromReply(string reply)
        {
            if(stepReplyConfiguration == null)
                throw new Exception("Call Bootstrapp first");

            if (string.IsNullOrEmpty(reply))
                return null;

            foreach (var x in stepReplyConfiguration)
            {
                foreach (var validReply in x.ValidReplies)
                {
                    if (Regex.IsMatch(reply, validReply, RegexOptions.IgnoreCase))
                        return x;
                }
            }

            return null;
        }

        


    }
}
