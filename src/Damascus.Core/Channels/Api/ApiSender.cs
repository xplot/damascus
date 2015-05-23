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
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;

namespace Damascus.Core
{
   public class ApiSender : IApiSender
    {
        public ILogger Logger { get; set; }
        
        public ApiSender(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(typeof(ApiSender).FullName);
        }
        
        public void CallApi(ServiceCallMessage serviceCallMessage)
        {
            Logger.LogInformation("Calling the following URL: " + serviceCallMessage.Url);
            
            var client = new RestClient(serviceCallMessage.Url);
			var request = new RestRequest();
			request.Method = (serviceCallMessage.Method == "POST") ? Method.POST : Method.GET;

            foreach (var keyValue in serviceCallMessage.Headers) 
			{
				request.AddHeader(keyValue.Key, keyValue.Value);
			}
    	   
           
            if(serviceCallMessage.Format == "json")
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(serviceCallMessage.Payload);
            }
            else
            {
                foreach (var keyValue in serviceCallMessage.Payload) 
    			{
    				request.AddParameter(keyValue.Key, keyValue.Value);
    			}    
            }
           
			// execute the request
			IRestResponse response = client.Execute(request);
			
            Logger.LogInformation(response.Content);
        }

    }
}