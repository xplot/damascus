using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Damascus.Core;
using Damascus.Message;
using Damascus.Message.Command;
using Microsoft.Framework.Logging;
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
        public ILogger Logger { get; set; }
        
        public MessageChannelHandler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(typeof(MessageChannelHandler).FullName);
        }
        
        public void Handle(CreateCallMessage message)
        {
            Logger.LogInformation("CreateCallMessage");
            CallSender.SendCall(message);
        }

        public void Handle(CreateSmsMessage message)
        {
            Logger.LogInformation("CreateSmsMessage");
            SmsSender.SendSms(message);
        }

        public void Handle(CreateEmailMessage message)
        {
            Logger.LogInformation("CreateEmailMessage");
            Logger.LogInformation(message.ToString());
            
            EmailSender.SendEmail(message);
        }

        public void Handle(ServiceCallMessage message)
        {
            Logger.LogInformation("ServiceCallMessage");
            ApiSender.CallApi(message);
        }

        public void Handle(FacebookEventMessage message)
        {
            Logger.LogInformation("FacebookEventMessage");
            FacebookEventSender.CreateFacebookPost(message);
        }
    }
}
