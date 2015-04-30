/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.Security.Cryptography;
using System.Text;

namespace Damascus.Web.Helpers
{
    public class RequireAuthorization : ActionFilterAttribute
    {
        private readonly byte[] privateKey = {56,12,55,95,1,9,47,123,67,41,54,86,96,28,33};
        
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            
            var httpDate = DateTime.Parse(GetHeader("Date", actionContext.HttpContext.Request.Headers));
            var authorizationHeader = GetHeader("Authorization", actionContext.HttpContext.Request.Headers);

            var response = ValidateDateHeader(httpDate);
            if (response != null)
            {
                actionContext.Response = response;
                return;
            }

            response = ValidateAuthorizationHeader(authorizationHeader);
            if (response != null)
            {
                actionContext.Response = response;
                return;
            }

            var httpDateHeader = GetHeader("Date", actionContext.HttpContext.Request.Headers);
            using (var hmacsha256 = new HMACSHA256(privateKey))
            {
                var hash = Convert.ToBase64String(hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(httpDateHeader)));
                if (authorizationHeader != string.Format("Voiceflows {0}", hash))
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("Invalid authorization token.")
                    };
            }
        }

        public HttpResponseMessage ValidateDateHeader(DateTimeOffset? dateHeader)
        {
            if (dateHeader == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest){
                    Content = new StringContent("Date header is required")
                };
            
            if(dateHeader.Value.CompareTo(DateTimeOffset.Now.AddMinutes(-2)) < 0 
               || dateHeader.Value.CompareTo(DateTimeOffset.Now.AddMinutes(1)) > 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest){
                    Content = new StringContent("Date header value is out of range of valid times.")
                };
            return null;
        }

        public HttpResponseMessage ValidateAuthorizationHeader(string authorizationHeader)
        {
            if (authorizationHeader == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest){
                    Content = new StringContent("Authorization header is required")
                };
            return null;
        }

        private string GetHeader(string headerName, HttpRequestHeaders headers)
        {
            IEnumerable<string> temp;
            return !headers.TryGetValues(headerName, out temp) ? null : temp.FirstOrDefault();
        }
    }
}

*/