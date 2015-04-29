using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core
{
    public interface IWorkflowBuilder
    {
        IWorkflow BuilWorkflow(string key);
    }
}
