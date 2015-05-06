using System;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Logging;
using Config = Microsoft.Framework.ConfigurationModel;

namespace Damascus.Web
{
	public class Settings
	{
		private static Config.Configuration _config;
        private static Config.Configuration Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config.Configuration();
                    _config.AddJsonFile("config.json");//Parametize this....for Prod

                }
                return _config;
            }
        }
		
		public static string Get(string key)
		{
			return Configuration[key];
		}
	}
}