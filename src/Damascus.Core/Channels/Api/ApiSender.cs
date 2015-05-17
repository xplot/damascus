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

namespace Damascus.Core
{
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