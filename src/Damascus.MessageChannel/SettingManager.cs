using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.MessageChannel
{
    public static class SettingManager
    {

        public static string Get(string setting)
        {
            try
            {
                //if (RoleEnvironment.IsAvailable)
                //    return RoleEnvironment.GetConfigurationSettingValue(setting);
                //else
                    return ConfigurationManager.AppSettings[setting].ToString();
            }
            catch
            {
                return ConfigurationManager.AppSettings[setting].ToString();
            }
        }
    }
}
