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
                
                AccountSid = Settings.Get("Twillio.AccountSid"),
                AuthToken = Settings.Get("Twillio.AuthToken"),
                SmsOutPhone = Settings.Get("Twillio.SmsOutPhone"),
                CallPhone = Settings.Get("Twillio.CallPhone"),
                VoiceCallbackUrl = Settings.Get("Twillio.BaseUrl") + "/call", 
                EmailCallbackUrl = Settings.Get("Twillio.BaseUrl") + "/email", 
                
            }));

            container.Register(Component.For<SmtpConfig>().Instance(new SmtpConfig()
            {
                Username = Settings.Get("Smtp.Username"),
                Password = Settings.Get("Smtp.Password"),
                Port = int.Parse(Settings.Get("Smtp.Port")),
                SmtpServer = Settings.Get("Smtp.Host"),
            }));

        }

    }
}