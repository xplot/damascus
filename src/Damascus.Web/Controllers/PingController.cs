using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using Microsoft.Framework.Logging;
using ILogger = Microsoft.Framework.Logging.ILogger;

using Damascus.Core;
using Damascus.Web;


namespace Damascus.Web.Controllers
{
    public class XXX
    {
        public string Value { get; set; }    
    }
    
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        public PingController()
		{
			
		}
        
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return "Ping response!";
        }
        
        [HttpPost]
        public XXX Post([FromBody]XXX x)
        {
            return x;
        }
    }
}
