using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;
using Damascus.Core;
using Castle.Windsor;
using Damascus.Web;
using NLog;
namespace Damascus.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController: Controller  
	{
		public Logger Logger { get; set; }
		public AuthenticationManager AuthenticationManager { get; set; }
		
		public AuthController(AuthenticationManager authManager)
		{
			this.Logger = LogManager.GetLogger(GetType().FullName);
			this.AuthenticationManager = authManager;
            
		}
		
        [HttpPost]
        [Authenticate]
	    public void Register([FromBody]Damascus.Web.User user)
	    {
	        Logger.Info("Register a new user");
            
            try
            {
                if(user == null)
                    throw new Exception("User format is invalid");
                
                AuthenticationManager.Register(user);
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                throw ex;
            }
	    }
        
        [HttpPost]
        public string Authenticate(string username, string password)
	    {
	        Logger.Info("Authenticating a user");
            
            try
            {
                var session = AuthenticationManager.Authenticate(username, password);
                if(session != null)
                    return session.Token;
                else
                {
                    Context.Response.StatusCode = 401;
                    return "Invalid username or password";    
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                throw ex;
            }    
	    }
        
        public string Validate(string token)
	    {
	        Logger.Info("Validating a Token");
            try
            {
                var session = AuthenticationManager.Validate(token);
                if(session != null)
                    return session.Token;
                else
                {
                    Context.Response.StatusCode = 401;
                    return "Invalid token";    
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
                Context.Response.StatusCode = 500;
                throw ex;
            }    
	    }
	}
}
