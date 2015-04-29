using System;
using System.Collections.Generic;
using Castle.Windsor;
using Damascus.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Damascus.Core
{
    public class ReplyStore:IReplyStore
    {
        private List<WorkflowStepReply> possibleReplies;

        public WorkflowStepReply GetWorkflowConfigFromReply(string reply)
        {
            if(possibleReplies == null)
                throw new Exception("Call Bootstrapp first");

            if (string.IsNullOrEmpty(reply))
                return null;

            foreach (var x in possibleReplies)
            {
                if (x.IsValid)
                {
                    foreach (var validReply in x.ValidReplies)
                    {
                        if (Regex.IsMatch(reply, validReply, RegexOptions.IgnoreCase))
                            return x;
                    }
                }
            }

            return null;
        }

        public void BootstrappWorkflowConfig()
        {
            possibleReplies = new List<WorkflowStepReply>()
            {
                new WorkflowStepReply()
                {
                    IsValid = true,
                    ValidReplies = new List<string>(){"yes","no"},
                    WorkflowStep = "smsin",
                    WorkflowType = "invite"
                }
            };
        }


    }
}
