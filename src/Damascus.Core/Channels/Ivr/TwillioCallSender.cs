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

namespace Damascus.Core
{
    public class TwillioCallSender : ICallSender
    {
        public TwillioConfig TwillioConfig { get; set; }
        public string BaseUrl { get; set; }

        public void SendCall(CreateCallMessage callMessage)
        {
            var twilio = new TwilioRestClient(TwillioConfig.AccountSid, TwillioConfig.AuthToken);
            var options = new CallOptions();
            options.Url = TwilioIvrWriter.GetCallbackUrl(TwillioConfig.VoiceCallbackUrl, callMessage.Parameters);
            options.To = callMessage.PhoneNumber;
            options.From = TwillioConfig.CallPhone;
            var call = twilio.InitiateOutboundCall(options);
        }
    }
}