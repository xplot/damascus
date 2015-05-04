using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Damascus.Message;
using Damascus.Message.Command;
using RestSharp;
using Twilio;

namespace Damascus.Core
{
    public interface ISender
    {
    }

    public interface ISmsSender
    {
        void SendSms(CreateSmsMessage smsMessage);
    }

    public interface IFacebookEventSender
    {
        void CreateFacebookEvent(FacebookEventMessage facebookEventMessage);
        void CreateFacebookPost(FacebookEventMessage facebookEventMessage);
    }

    public interface ICallSender
    {
        void SendCall(CreateCallMessage smsMessage);
    }

    public interface IEmailSender
    {
        ITemplateManager TemplateManager { get; set; }
        void SendEmail(CreateEmailMessage emailMessage);
    }

    public interface IApiSender
    {
        void CallApi(ServiceCallMessage serviceCallMessage);
    }
    
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

    public class TwillioSmsSender : ISmsSender
    {
        public TwillioConfig TwillioConfig { get; set; }

        public void SendSms(CreateSmsMessage smsMessage)
        {
            var twilio = new TwilioRestClient(TwillioConfig.AccountSid, TwillioConfig.AuthToken);
            twilio.SendSmsMessage(TwillioConfig.SmsOutPhone, smsMessage.PhoneNumber, smsMessage.Message, twilioResult =>
            {
                if (twilioResult.RestException != null)
                    throw new Exception();
            });
        }

    }

    public class EmailSender : IEmailSender
    {
        public ITemplateManager TemplateManager { get; set; }
        public SmtpConfig SmtpConfig { get; set; }

        public void SendEmail(CreateEmailMessage emailMessage)
        {
            try
            {
                var body = TemplateManager.Fill(TemplateManager.GetTemplate(emailMessage.BodyTemplate), emailMessage.BodyData);

                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(emailMessage.Address);
                mailMsg.From = new MailAddress(emailMessage.Sender);

                // Subject and multipart/alternative Body
                mailMsg.Subject = emailMessage.Subject;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(SmtpConfig.SmtpServer, Convert.ToInt32(SmtpConfig.Port));
                if (!string.IsNullOrEmpty(SmtpConfig.Username))
                {
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpConfig.Username, SmtpConfig.Password);
                    smtpClient.Credentials = credentials;    
                }
                
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ApiSender : IApiSender
    {
        public void CallApi(ServiceCallMessage serviceCallMessage)
        {
            var client = new RestClient();
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(serviceCallMessage.Url, Method.POST);
            request.RequestFormat = DataFormat.Json;

            foreach (var x in serviceCallMessage.Headers)
            {
                request.AddHeader(x.Key, x.Value); // adds to POST or URL querystring based on Method    
            }

            request.AddBody(serviceCallMessage.Payload);

            // execute the request
            client.Execute(request);
        }

    }
}
