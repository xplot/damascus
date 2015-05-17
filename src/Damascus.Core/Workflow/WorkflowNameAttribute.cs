using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core
{
    public class WorkflowNameAttribute:Attribute
    {
        public string Name { get; set; }
        public WorkflowNameAttribute(string name)
        {
            Name = name;
        }
    }
}
