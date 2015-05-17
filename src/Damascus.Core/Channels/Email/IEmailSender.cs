using System;
using Damascus.Message;
using Damascus.Message.Command;

namespace Damascus.Core
{
    public interface IEmailSender
    {
        ITemplateManager TemplateManager { get; set; }
        void SendEmail(CreateEmailMessage emailMessage);
    }
}