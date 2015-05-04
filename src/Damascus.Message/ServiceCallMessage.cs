using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Damascus.Message.Command
{
    public class ServiceCallMessage : IMessage
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public Dictionary<string, string> Payload { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
