using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using System;
using System.Configuration;
using Damascus.Core;
using log4net;

namespace Damascus.MessageChannel
{
    public class ManagersInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            container.Register(AllTypes.FromAssemblyContaining(typeof(IMessageChannelManager))
                .Pick()
                .If(Component.IsInNamespace("Damascus.Core"))
                .If(t => t.Name.EndsWith("Manager", StringComparison.Ordinal) || t.Name.EndsWith("Sender", StringComparison.Ordinal))
                .WithService.DefaultInterfaces()
                .Configure(c => c.LifestyleSingleton()));

            container.Register(Component.For<TwillioConfig>().Instance(new TwillioConfig()
            {
                AccountSid = SettingManager.Get("TwillioAccountSid"),//"ACca775b6d44d34b80a1632effeddbcedd",
                AuthToken = SettingManager.Get("TwillioAuthToken"),//"238e98899d2fc49efb559fd9f841e8c6",
                SmsOutPhone = SettingManager.Get("TwillioSmsOutPhone"),//"786-465-2251",
                CallPhone = SettingManager.Get("TwillioCallPhone"),//"786-465-2251",
                VoiceCallbackUrl = SettingManager.Get("TwillioBaseUrl") + "/call", //www.voiceflows.com
                EmailCallbackUrl = SettingManager.Get("TwillioBaseUrl") + "/email", //www.voiceflows.com
            }));

            container.Register(Component.For<SmtpConfig>().Instance(new SmtpConfig()
            {
                Username = "xxxxxxx",
                Password = "xxxxxxx",
                Port =587,
                SmtpServer = "smtp.sendgrid.net",
            }));

        }

    }
}
