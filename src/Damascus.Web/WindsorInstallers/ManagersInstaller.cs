using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using System;
using System.Configuration;
using Damascus.Core;
using Damascus.Message;

namespace Damascus.Web
{
    public class ManagersInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            var Settings = container.Resolve<Settings>();

            container.Register(AllTypes.FromAssemblyContaining(typeof(IMessageChannelManager))
                .Pick()
                .If(Component.IsInNamespace("Damascus.Core"))
                .If(t => t.Name.EndsWith("Manager", StringComparison.Ordinal))
                .WithService.DefaultInterfaces()
                .Configure(c => c.LifestyleTransient()));

            container.Register(AllTypes.FromAssemblyContaining(typeof(IStep))
                .Pick()
                .If(Component.IsInNamespace("Damascus.Core"))
                .If(t => t.Name.EndsWith("Step", StringComparison.Ordinal))
                .Configure(c => c.LifestyleTransient()));

            container.Register(Component.For<IIvrXmlWriter>().ImplementedBy<TwilioIvrWriter>().LifestyleTransient());

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
                
                SenderAddress = Settings.Get("Smtp.SenderAddress"),
            }));
        }
    }
}
