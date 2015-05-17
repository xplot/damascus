using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Message;
using Damascus.Message.Command;
using RestSharp;
using Twilio;
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;

namespace Damascus.Core
{
    public class TwillioSmsSender : ISmsSender
    {
        public TwillioConfig TwillioConfig { get; set; }
    	public ILogger Logger { get; set; }
        
        public TwillioSmsSender(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(typeof(TwillioSmsSender).FullName);
        }
        
        public void SendSms(CreateSmsMessage smsMessage)
        {
            Logger.LogInformation("SendSms START");
            
            Logger.LogInformation("Message Sent to: " + smsMessage.PhoneNumber);
            Logger.LogInformation("Message message body: " + smsMessage.Message);
            
            var twilio = new TwilioRestClient(TwillioConfig.AccountSid, TwillioConfig.AuthToken);
            twilio.SendSmsMessage(TwillioConfig.SmsOutPhone, smsMessage.PhoneNumber, smsMessage.Message, twilioResult =>
            {
                if (twilioResult.RestException != null)
                {
                    Logger.LogError(twilioResult.RestException.Message);
                    throw new Exception();
                }
                    
            });
            
            Logger.LogInformation("SendSms END");
        }

    }
}