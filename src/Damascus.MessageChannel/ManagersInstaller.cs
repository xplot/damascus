using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using System;
using System.Configuration;
using Damascus.Core;

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
                /*
                AccountSid = SettingManager.Get("TwillioAccountSid"),
                AuthToken = SettingManager.Get("TwillioAuthToken"),
                SmsOutPhone = SettingManager.Get("TwillioSmsOutPhone"),
                CallPhone = SettingManager.Get("TwillioCallPhone"),
                VoiceCallbackUrl = SettingManager.Get("TwillioBaseUrl") + "/call", 
                EmailCallbackUrl = SettingManager.Get("TwillioBaseUrl") + "/email", 
                */
            }));

            container.Register(Component.For<SmtpConfig>().Instance(new SmtpConfig()
            {
                Username = "xxx",
                Password = "xxx",
                Port =587,
                SmtpServer = "smtp.sendgrid.net",
            }));

        }

    }
}