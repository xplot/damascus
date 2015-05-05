using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

namespace Damascus.Message.Command
{
    
    [TimeToBeReceived("01:00:00")]
    public class CreateEmailMessage:IdMessage
    {
        public string Id { get; set; }

        public string Sender { get; set; }

        public string Address { get; set; }

        public string Subject { get; set; }

        public Dictionary<string,string> BodyData { get; set; }

        public BodyTemplate BodyTemplate { get; set; }
    }
    
}
