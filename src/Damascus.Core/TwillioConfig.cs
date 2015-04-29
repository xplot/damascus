using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core
{
    public class TwillioConfig
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string SmsOutPhone { get; set; }
        public string CallPhone { get; set; }
        public string VoiceCallbackUrl { get; set; }
        public string EmailCallbackUrl { get; set; }
    }

    public class SmtpConfig
    {
        public string SmtpServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }

    }
}
