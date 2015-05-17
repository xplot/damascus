using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Damascus.Core
{
    public class WorkflowStepReply
    {
        public string WorkflowType { get; set; }
        public string WorkflowStep { get; set; }
        public IEnumerable<string> ValidReplies { get; set; }
    }

}