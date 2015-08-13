using System;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Logging;
using Config = Microsoft.Framework.ConfigurationModel;
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

		private static Config.Configuration _config;
        private Config.Configuration Configuration
        {
            get
            {
                if (_config == null)
                {
                    try{
                       
                    _config = new Config.Configuration();
                    if (File.Exists(baseDirectory + "config.local.json"))
                        _config.AddJsonFile(baseDirectory + "config.local.json");    
                    else    
                        _config.AddJsonFile(baseDirectory + "config.json");
                    }
                    catch(Exception ex){
                        Console.WriteLine(_config);
                        Console.WriteLine(baseDirectory + "config.local.json");
                        Console.WriteLine(File.Exists(baseDirectory + "/config.local.json"));
                        Console.WriteLine("Exception");
                        Console.WriteLine(ex);
                        throw;
                    }
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