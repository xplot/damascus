using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Message;

namespace Damascus.Workflow
{
    [WorkflowName("empty")]
    public class EmptyWorkflow:PhoneCallWorkflow
    {

        public override string Execute(string step, IStepInput input)
        {
            return "<?xml version='1.0' encoding='UTF-8'?><Response><Hangup/></Response>";
        }
    }
}
