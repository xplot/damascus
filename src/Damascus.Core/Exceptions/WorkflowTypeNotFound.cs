using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core.Exceptions
{
    public class WorkflowTypeNotFound:Exception
    {
        public WorkflowTypeNotFound(string message) : base(message)
        {
        }
    }
}
