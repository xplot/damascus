using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Message
{
    public class CreateSmsMessage:IdMessage
    {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Message { get; set; }
    }
}
