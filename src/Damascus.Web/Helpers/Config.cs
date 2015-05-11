using System;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Logging;
using Config = Microsoft.Framework.ConfigurationModel;
using System.IO;
using Microsoft.Framework.Runtime;

namespace Damascus.Web
{
	public class Settings
	{
        string baseDirectory;
        public Settings(IApplicationEnvironment appEnvironment) {
            baseDirectory = appEnvironment.ApplicationBasePath;
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
                        _config.AddJsonFile("config.local.json");    
                    else    
                        _config.AddJsonFile("config.json");
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