using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Damascus.Message.Command
{
    [TimeToBeReceived("01:00:00")]
    public class CreateCallMessage:IdMessage
    {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }

        public Dictionary<string,string> Parameters { get; set; }
    }
}
