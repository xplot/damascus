using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Message;

namespace Damascus.Workflow
{
    public class FunctionStep : IStep
    {
        private Func<IStepInput,string> toExecute;

        public FunctionStep(Func<IStepInput, string> toExec)
        {
            this.toExecute = toExec;
        }

        public string Execute(IStepInput input)
        {
            return toExecute(input);
        }
    }
}
