using System;
using Damascus.Message;
using Damascus.Message.Command;


namespace Damascus.Core
{
    public interface IApiSender
    {
        void CallApi(ServiceCallMessage serviceCallMessage);
    }
}