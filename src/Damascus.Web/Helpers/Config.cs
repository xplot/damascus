using System;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Logging;
using Config = Microsoft.Framework.ConfigurationModel;
using System.IO;
using Microsoft.AspNet.Hosting;

namespace Damascus.Web
{
	public class Settings
	{
        string baseDirectory;
        public Settings(IHostingEnvironment appEnvironment) {
            baseDirectory = appEnvironment.WebRootPath + "/../";
        }

		private static Config.Configuration _config;
        private Config.Configuration Configuration
        {
            get
            {
                if (_config == null)
                {
                    
                    _config = new Config.Configuration();
                    if (File.Exists(baseDirectory + "/config.local.json"))
                        _config.AddJsonFile(baseDirectory + "config.local.json");    
                    else    
                        _config.AddJsonFile(baseDirectory + "config.json");
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