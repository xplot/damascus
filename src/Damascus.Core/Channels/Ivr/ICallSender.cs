using System;
using Damascus.Message;
using Damascus.Message.Command;


namespace Damascus.Core
{
    public interface ICallSender
    {
        void SendCall(CreateCallMessage smsMessage);
    }
}