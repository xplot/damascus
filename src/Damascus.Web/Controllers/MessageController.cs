using System.Diagnostics;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using Damascus.Message.Command;
using NServiceBus;

namespace Damascus.Web.Controllers
{
    //[RequireAuthorization]
    public class MessageController : Controller
    {
        public IBus Bus { get; set; }
        
        public IMessageChannelManager MessageChannelManager { get; set; }

        public string Sms([FromBody]CreateSmsMessage createSmsMessage)
        {
            Trace.WriteLine(createSmsMessage);

            return MessageChannelManager.PostSms(createSmsMessage);
        }

        public string Call([FromBody]CreateCallMessage createCallMessage)
        {
            Trace.WriteLine(createCallMessage);

            return MessageChannelManager.PostCall(createCallMessage);
        }

        [HttpPost]
        public string Email([FromBody]CreateEmailMessage createEmailMessage)
        {
            Trace.WriteLine(createEmailMessage);

            return MessageChannelManager.PostEmail(createEmailMessage);
        }
    }
}