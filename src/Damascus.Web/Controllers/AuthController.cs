using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;
using Damascus.Core;

namespace Damascus.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController: Controller  
	{
		public ILogger Logger { get; set; }
		public AuthenticationManager AuthenticationManager { get; set; }
		
		public AuthController(ILoggerFactory loggerFactory, AuthenticationManager authManager)
		{
			Logger = loggerFactory.CreateLogger(typeof(AuthController).FullName);
			this.AuthenticationManager = authManager;
		}
		
        
	    public void Register([FromBody]User user)
	    {
	        Logger.LogInformation("Register a new user");
            Logger.LogInformation(this.Request.ToRaw());
            
            try
            {
                AuthenticationManager.Register(user);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                
                Context.Response.StatusCode = 500;
            }    
	    }
        
        
        public Session Authenticate(string username, string password)
	    {
	        Logger.LogInformation("Authenticating a user");
            Logger.LogInformation(this.Request.ToRaw());
            
            try
            {
                return AuthenticationManager.Authenticate(username, password);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                
                Context.Response.StatusCode = 500;
                return null;
            }    
	    }
        
       
        public Session Validate(string token)
	    {
	        Logger.LogInformation("Validating a Token");
            Logger.LogInformation(this.Request.ToRaw());
            
            try
            {
                return AuthenticationManager.Validate(token);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogError(ex.StackTrace);
                
                Context.Response.StatusCode = 500;
                return null;
            }    
	    }
	}
}
