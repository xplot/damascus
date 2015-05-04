using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Damascus.Core;
using Damascus.Message;
using Damascus.Message.Command;
using NServiceBus;

namespace Damascus.MessageChannel
{
    public class MessageChannelHandler : 
        IHandleMessages<CreateCallMessage>, 
        IHandleMessages<CreateSmsMessage>, 
        IHandleMessages<CreateEmailMessage>,
        IHandleMessages<ServiceCallMessage>,
        IHandleMessages<FacebookEventMessage>
    {
        public IMessageChannelManager MessageChannelManager { get; set; }
        public ISmsSender SmsSender { get; set; }
        public ICallSender CallSender { get; set; }
        public IEmailSender EmailSender { get; set; }
        public IApiSender ApiSender { get; set; }

        public IFacebookEventSender FacebookEventSender { get; set; }

        public void Handle(CreateCallMessage message)
        {
            CallSender.SendCall(message);
        }

        public void Handle(CreateSmsMessage message)
        {
            SmsSender.SendSms(message);
        }

        public void Handle(CreateEmailMessage message)
        {
            EmailSender.SendEmail(message);
        }

        public void Handle(ServiceCallMessage message)
        {
            ApiSender.CallApi(message);
        }

        public void Handle(FacebookEventMessage message)
        {
            FacebookEventSender.CreateFacebookPost(message);
        }
    }
}
