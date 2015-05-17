using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration.Proxy;
using Damascus.Message;

namespace Damascus.Core
{
    public interface ITemplateManager
    {
        string GetTemplate(BodyTemplate template);

        void SaveTemplate(string templateId);

        string Fill(string templateId, IDictionary<string, string> data);
    }

    public abstract class TemplateManager:ITemplateManager
    {
        public const string START_KEY_SEPARATOR = "{{";
        public const string END_KEY_SEPARATOR = "}}";

        public abstract string GetTemplate(BodyTemplate template);

        public abstract void SaveTemplate(string templateId);

        public virtual string Fill(string body, IDictionary<string, string> data)
        {
            if (data == null)
                return body;

            foreach (var key in data.Keys)
            {
                body = body.Replace(TemplateKey(key), data[key]);
            }

            return body;
        }

        private string TemplateKey(string name)
        {
            return START_KEY_SEPARATOR + name + END_KEY_SEPARATOR;
        }
    }

    public class MemoryTemplateManager : TemplateManager
    {

        private Dictionary<string,string>template_dict = new Dictionary<string, string> ();
        private Dictionary<string, string> cache = new Dictionary<string, string>();
        private const string template2 = "";

        public override string GetTemplate(BodyTemplate template)
        {
            if (!string.IsNullOrEmpty(template.Body))
                return template.Body;
            else if (template.Url != null)
            {
                if (cache.ContainsKey(template.Url))
                    return cache[template.Url];

                var templateBody = GetTemplateFromUrl(template.Url);
                cache[template.Url] = templateBody;
                return templateBody;
            }

            return string.Empty;
        }

        private string GetTemplateFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the stream associated with the response.
            using (Stream receiveStream = response.GetResponseStream())
            {
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                return readStream.ReadToEnd();
            }
        }

        public override void SaveTemplate(string templateId)
        {
            //Do Nothing
        }
    }
}
