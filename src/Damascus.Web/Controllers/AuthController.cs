using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;
using Damascus.Core;
using Castle.Windsor;

namespace Damascus.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController: Controller  
	{
		public ILogger Logger { get; set; }
		public AuthenticationManager AuthenticationManager { get; set; }
		private IWindsorContainer container;
		public AuthController(ILoggerFactory loggerFactory, IWindsorContainer container)
		{
			Logger = loggerFactory.CreateLogger(typeof(AuthController).FullName);
			//this.AuthenticationManager = authManager;
            this.container = container;
		}
		
	    public void Register([FromBody]User user)
	    {
	        Logger.LogInformation("Register a new user");
            Logger.LogInformation(this.Request.ToRaw());
            
            try
            {
                if(user == null)
                    throw new Exception("User format is invalid");
                    
                AuthenticationManager = container.Resolve<AuthenticationManager>();
                AuthenticationManager.Register(user);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw ex;
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
                Logger.LogError(ex.ToString());
                throw ex;
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
                Logger.LogError(ex.ToString());
                throw ex;
            }    
	    }
	}
}
