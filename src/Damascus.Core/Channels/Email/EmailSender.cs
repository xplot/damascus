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

namespace Damascus.Core
{
    public class EmailSender : IEmailSender
    {
        public ITemplateManager TemplateManager { get; set; }
        public SmtpConfig SmtpConfig { get; set; }

        public void SendEmail(CreateEmailMessage emailMessage)
        {
            try
            {
                var body = TemplateManager.Fill(TemplateManager.GetTemplate(emailMessage.BodyTemplate, emailMessage.BodyData), emailMessage.BodyData);

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
}