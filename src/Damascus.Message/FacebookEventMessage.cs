using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Damascus.Message.Command
{
    public class FacebookEventMessage:IMessage
    {
        public string AccessToken { get; set; }
        public ReducedInviteInput Invite { get; set; }
    }
}
