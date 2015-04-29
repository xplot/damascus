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
    public interface IReplyStore
    {
        
        WorkflowStepReply GetWorkflowConfigFromReply(string reply);
        
        void BootstrappWorkflowConfig();

    }


    public class WorkflowStepReply
    {
        public DateTime Date { get; set; }
        public bool IsValid { get; set; }
        public string WorkflowStep { get; set; }
        public string WorkflowType { get; set; }
        public List<string> ValidReplies { get; set; }
    }

    public class WorkflowIdPhone
    {
        public string WorkflowId { get; set; }
        public string Date { get; set; }
    }
}
