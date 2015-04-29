using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Message;

namespace Damascus.Workflow
{
    [WorkflowName("javi")]
    public class JaviCustomWorkflow: PhoneCallWorkflow
    {
        public JaviCustomWorkflow()
        {
            var step1 = new TwimlStep(){
                Twiml = "<?xml version='1.0' encoding='UTF-8'?><Response><Gather timeout='10' finishOnKey='*'><Say>Please enter your pin number and then press star.</Say></Gather></Response>"
            };
            var step2 = new TwimlStep(){
                Twiml = "<?xml version='1.0' encoding='UTF-8'?><Response><Say>This is a second message after you input.</Say></Response>"
            };

            this.Steps = new Dictionary<string,IStep>()
            {
                { "1", step1 },
                { "2", step2 },
            };
        }

    }
}
