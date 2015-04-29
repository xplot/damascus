using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Damascus.Message;
using log4net;
using NServiceBus;

namespace Damascus.Core
{
    public interface IMessageChannelManager
    {
        IBus Bus { get; set; }
        string PostCall(CreateCallMessage call);
        string PostSms(CreateSmsMessage sms);
        string PostEmail(CreateEmailMessage email);
    }

    public class MessageChannelManager:IMessageChannelManager
    {
        public IBus Bus { get; set; }

        public string PostCall(CreateCallMessage call)
        {
            if (string.IsNullOrEmpty(call.Id))
                call.Id = Guid.NewGuid().ToString();

            Bus.Send(call);
            return call.Id;
        }

        public string PostSms(CreateSmsMessage sms)
        {
            if (string.IsNullOrEmpty(sms.Id))
                sms.Id = Guid.NewGuid().ToString();

            Bus.Send(sms);
            return sms.Id;
        }

        public string PostEmail(CreateEmailMessage email)
        {
            if (string.IsNullOrEmpty(email.Id))
                email.Id = Guid.NewGuid().ToString();

            Bus.Send(email);
            return email.Id;
        }
    }
}
