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
                AccountSid = "1111",
                AuthToken = "1111",
                SmsOutPhone = "2222",
                CallPhone = "3333",
                VoiceCallbackUrl = "4444", 
                EmailCallbackUrl = "5555" 
            }));
        }
    }
}
