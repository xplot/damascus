using System;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Logging;
using Config = Microsoft.Framework.ConfigurationModel;
using System.IO;
using Damascus.Core;

namespace Damascus.MessageChannel
{
	public class Settings: ISettings
	{
        string baseDirectory;
        public Settings() {
            baseDirectory = Environment.CurrentDirectory + "/";
            System.Console.WriteLine(baseDirectory);
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
                        _config.AddJsonFile(baseDirectory +"config.local.json");    
                    else    
                        _config.AddJsonFile(baseDirectory +"config.json");
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