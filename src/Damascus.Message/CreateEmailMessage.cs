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
        
        public override string ToString()
        {
            var result = string.Empty;
            if(!string.IsNullOrEmpty(Sender))
                result += " Sender: " + Sender;
            if(!string.IsNullOrEmpty(Address))
                result += " Address: " + Address;
            if(!string.IsNullOrEmpty(Subject))
                result += " Subject: " + Subject;
                
            return result;
        }
    }
    
}
