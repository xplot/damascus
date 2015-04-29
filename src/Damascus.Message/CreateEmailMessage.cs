using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Damascus.Message
{
    public class CreateEmailMessage:IdMessage
    {
        public string Id { get; set; }

        public string Sender { get; set; }

        public string Address { get; set; }

        public string Subject { get; set; }

        public Dictionary<string,string> BodyData { get; set; }

        public BodyTemplate BodyTemplate { get; set; }
    }

    public class BodyTemplate
    {
        public string Body { get; set; }
        public string Url { get; set; }
        
        public override string ToString()
        {
            return string.Format(
                "{0};_;{1}",
                this.Body,
                this.Url
            );
        }

        public static BodyTemplate FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new EmailTemplate();

            var args = value.Split(new string[] { ";_;" }, StringSplitOptions.None);

            return new BodyTemplate()
            {
                Body = args[0],
                Url = args[1],
            };
        }
    }

    public class EmailTemplate:BodyTemplate
    {
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "redirect_url")]
        public string RedirectUrl { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0};_;{1};_;{2};_;{3}",
                this.Subject,
                this.Body,
                this.Url,
                this.RedirectUrl
                );
        }

        public static EmailTemplate FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new EmailTemplate();

            var args = value.Split(new string[]{";_;"},StringSplitOptions.None);

            return new EmailTemplate()
            {
                Subject = args[0],
                Body = args[1],
                Url = args[2],
                RedirectUrl = args[3]
            };
        }
    }
}
