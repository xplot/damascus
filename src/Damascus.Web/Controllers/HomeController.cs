using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using Damascus.Core;

namespace Damascus.Web.Controllers
{
    public class HomeController  
	{
	    public IActionResult Index()
	    {
	        return new RedirectResult("/index.html");
	    }
	}
}
