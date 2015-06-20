using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using Damascus.Message.Command;
using NServiceBus;
using System;
using NLog;

namespace Damascus.Web.Controllers
{
    [Authenticate]
    public class MessageController : Controller
    {
        public IBus Bus { get; set; }
        public IMessageChannelManager MessageChannelManager { get; set; }
        public Logger Logger { get; set; }
        
        public MessageController(ILoggerFactory loggerFactory, IMessageChannelManager messageChannelManager)
        {
            Logger = LogManager.GetLogger(GetType().FullName);
            this.MessageChannelManager = messageChannelManager;
        }
        
    	[Route("api/message/sms")]
        public string Sms([FromBody]CreateSmsMessage createSmsMessage)
        {
            try{
                Logger.Info("Sms Message received");
                Logger.Info("Sms sent to: " + createSmsMessage.PhoneNumber);
                Logger.Info("Sms message: " + createSmsMessage.Message);
            
                return MessageChannelManager.PostSms(createSmsMessage);
            }
            catch(Exception ex){
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                throw ex;
            }
        }
        
        [Route("api/message/call")]
        public string Call([FromBody]CreateCallMessage createCallMessage)
        {
            try{
                Logger.Info("Phone Call message received");
                Logger.Info("Phone Call to: " + createCallMessage.PhoneNumber);
    
                return MessageChannelManager.PostCall(createCallMessage);
            }
            catch(Exception ex){
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                throw ex;
            }
        }

        [Route("api/message/email")]
        public string Email([FromBody]CreateEmailMessage createEmailMessage)
        {
            try{
                Logger.Info("CreateEmailMessage message received");
                Logger.Info("CreateEmailMessage to: " + createEmailMessage.Address);
                Logger.Info("CreateEmailMessage Subject: " + createEmailMessage.Address);
                
                return MessageChannelManager.PostEmail(createEmailMessage);
            }
            catch(Exception ex){
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                throw ex;
            }
        }
    }
}