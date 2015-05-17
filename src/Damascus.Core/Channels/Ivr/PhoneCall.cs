using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Message;
using Newtonsoft.Json;

namespace Damascus.Core
{
    public abstract class PhoneCallWorkflow : Workflow
    {
        public IIvrXmlWriter IvrWriter { get; set; }
    }

    public abstract class Step : IStep
    {
        public abstract string Execute(IStepInput input);
    }

    public class TwimlStep : Step
    {
        public string Twiml { get; set; }

        public override string Execute(IStepInput input)
        {
            return Twiml;
        }
    }
}
