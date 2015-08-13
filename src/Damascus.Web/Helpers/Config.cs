using System;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Configuration.Json;
using System.IO;
using Microsoft.AspNet.Hosting;
using Damascus.Core;

namespace Damascus.Web
{
	public class Settings: ISettings
	{
        string baseDirectory;
        public Settings(IHostingEnvironment appEnvironment) {
            baseDirectory = appEnvironment.WebRootPath + "/../";
        }

		private static IConfiguration _config;
        private IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var configurationBuilder = new ConfigurationBuilder(baseDirectory);
                    
                    if (File.Exists(baseDirectory + "config.local.json"))
                        configurationBuilder.AddJsonFile("config.local.json");    
                    else    
                        configurationBuilder.AddJsonFile("config.json");
                    _config = configurationBuilder.Build();
                }
                return _config;
            }
        }
		
		public string Get(string key)
		{
			return Configuration[key];
		}
	}
}