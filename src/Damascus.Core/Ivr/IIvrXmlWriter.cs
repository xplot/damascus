using System.Collections.Generic;
using System.Text;
using Castle.Core;
using Castle.Windsor.Configuration.Interpreters.XmlProcessor.ElementProcessors;
using Twilio.TwiML;

namespace Damascus.Core
{
    public interface IIvrXmlWriter
    {
        void EnterNumber(
            string message, 
            Dictionary<string,string> urlParameters, 
            int amountOfDigis = 1, 
            int wait_time = 10
        );
        void SayMessage(string message);
        void Hangup();
        string ToString();
    }

    public class TwilioIvrWriter : IIvrXmlWriter
    {
        private TwilioResponse response;
        public TwillioConfig TwillioConfig { get; set; }

        public TwilioIvrWriter()
        {
            response = new TwilioResponse();
        }

        public void EnterNumber(
            string message, 
            Dictionary<string, string> urlParameters, 
            int amountOfDigis = 1,
            int wait_time = 10
        )
        {
            response.BeginGather(new
            {
                action = GetCallbackUrl(TwillioConfig.VoiceCallbackUrl, urlParameters), 
                numDigits = amountOfDigis
            });
            response.Say(message);
            response.EndGather();
        }

        public void SayMessage(string message)
        {
            response.Say(message);
        }

        public void Hangup()
        {
            response.Hangup();
            response.ToString();
        }

        public override string ToString()
        {
            return response.ToString();
        }

        public static string GetCallbackUrl(string baseUrl, Dictionary<string, string> parameters)
        {
            if (!baseUrl.Contains("?"))
                baseUrl += "?";

            if (parameters != null)
                foreach (var x in parameters)
                {
                    baseUrl += string.Format("{0}={1}&", x.Key, x.Value);
                }
            return baseUrl;
        }

    }
}