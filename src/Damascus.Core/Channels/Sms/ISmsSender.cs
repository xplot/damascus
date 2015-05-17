using System;
using Damascus.Message;
using Damascus.Message.Command;


namespace Damascus.Core
{
    public interface ISmsSender
    {
        void SendSms(CreateSmsMessage smsMessage);
    }
}