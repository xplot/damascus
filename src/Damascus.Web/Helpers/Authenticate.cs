using System;
using System.Text;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Authorization;
using System.Net.Http;
using System.Net.Http.Formatting;
using NLog;

namespace Damascus.Web
{
    public class Authenticate : ActionFilterAttribute
    {
        private AuthenticationManager authManager;
        
        public Logger Logger{ get; set; }
        public Authenticate()
        {
            Logger = LogManager.GetLogger(GetType().FullName);            
        }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            Logger.Info("Authenticate Request");
            try
            {
                this.authManager =  Startup.container.Resolve<IServiceProvider>().GetService(typeof(AuthenticationManager)) as AuthenticationManager;
                var authorized = false;
            
                authorized = BasicAuth(actionContext);
                
                //Then we try token based auth
                if(!authorized)
                {
                    var authorizationHeader = GetHeader("session_token", actionContext.HttpContext.Request.Headers);
                    if(string.IsNullOrEmpty(authorizationHeader))
                        authorizationHeader = GetRequestVar("session_token", actionContext.HttpContext.Request);
                    if(!string.IsNullOrEmpty(authorizationHeader))
                        authorized = authManager.Validate(authorizationHeader) != null;
                }
                
                if(!authorized)
                {
                    throw new Exception("Invalid authorization token.");
                }
                
                Logger.Info("Request authenticated successfully");
                await next();
            }
            catch(Exception ex)
            {
                actionContext.HttpContext.Response.StatusCode = 401;
                Logger.Error(ex.ToString());
                throw ex;
            }
            
        }
        
        private bool BasicAuth(ActionExecutingContext actionContext)
        {
            var auth = GetHeader("Authorization", actionContext.HttpContext.Request.Headers);
            if (!string.IsNullOrEmpty(auth) && auth.StartsWith("Basic")) {
                var credentials = auth.Replace("Basic ", "");
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
                
                int separatorIndex = credentials.IndexOf(':');
                if (separatorIndex >= 0) {
                    string userName = credentials.Substring(0, separatorIndex);
                    string password = credentials.Substring(separatorIndex + 1);
                    if (authManager.Authenticate(userName, password) != null)
                        return true;
                }
            }
            
            return false;
        }
        
        private string GetHeader(string headerName, IHeaderDictionary headers)
        {
            if(!headers.ContainsKey(headerName))
                return null;
            return headers[headerName];
        }
        
        private string GetRequestVar(string requestVar, HttpRequest request)
        {
            var value = string.Empty;
            if(request.HasFormContentType)
                value = request.Form[requestVar];
            
            if(string.IsNullOrEmpty(value))
            {
                value = request.Query[requestVar];
            }
            
            return value;
        }
        
    }
}