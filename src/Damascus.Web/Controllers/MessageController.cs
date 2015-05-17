using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;
using Microsoft.AspNet.Mvc;
using Damascus.Core;
using Damascus.Message;
using Damascus.Message.Command;
using NServiceBus;
using System;

namespace Damascus.Web.Controllers
{
    //[RequireAuthorization]
    public class MessageController : Controller
    {
        public IBus Bus { get; set; }
        public IMessageChannelManager MessageChannelManager { get; set; }
        public ILogger Logger { get; set; }
        
        public MessageController(ILoggerFactory loggerFactory, IMessageChannelManager messageChannelManager)
        {
            Logger = loggerFactory.CreateLogger(typeof(InviteController).FullName);
            this.MessageChannelManager = messageChannelManager;
        }
        
    	[Route("api/message/sms")]
        public string Sms([FromBody]CreateSmsMessage createSmsMessage)
        {
            try{
                Logger.LogInformation("Sms Message received");
                Logger.LogInformation("Sms sent to: " + createSmsMessage.PhoneNumber);
                Logger.LogInformation("Sms message: " + createSmsMessage.Message);
            
                return MessageChannelManager.PostSms(createSmsMessage);
            }
            catch(Exception ex){
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        
        [Route("api/message/call")]
        public string Call([FromBody]CreateCallMessage createCallMessage)
        {
            try{
                Logger.LogInformation("Phone Call message received");
                Logger.LogInformation("Phone Call to: " + createCallMessage.PhoneNumber);
    
                return MessageChannelManager.PostCall(createCallMessage);
            }
            catch(Exception ex){
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        [Route("api/message/email")]
        public string Email([FromBody]CreateEmailMessage createEmailMessage)
        {
            try{
                Logger.LogInformation("CreateEmailMessage message received");
                Logger.LogInformation("CreateEmailMessage to: " + createEmailMessage.Address);
                Logger.LogInformation("CreateEmailMessage Subject: " + createEmailMessage.Address);
                
                return MessageChannelManager.PostEmail(createEmailMessage);
            }
            catch(Exception ex){
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}