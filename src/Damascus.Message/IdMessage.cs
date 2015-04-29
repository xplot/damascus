using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Damascus.Message
{
    public interface IdMessage:IMessage
    {
        string Id { get; set; }
    }
}
