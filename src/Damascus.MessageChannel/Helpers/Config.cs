using System;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Configuration.Json;
using Microsoft.Framework.Logging;
using System.IO;
using Damascus.Core;

namespace Damascus.MessageChannel
{
	public class Settings: ISettings
	{
        string baseDirectory;
        public Settings() {
            baseDirectory = Environment.CurrentDirectory + "/";
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