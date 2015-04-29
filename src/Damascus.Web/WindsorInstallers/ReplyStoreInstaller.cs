using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Castle.MicroKernel.Registration;
using Damascus.Core;
using Damascus.Core.Serialization;
using Damascus.Workflow;
using StackExchange.Redis;

namespace Damascus.Web.WindsorInstallers
{
    public class ReplyStoreInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            var replyStore = new ReplyStore();
            replyStore.BootstrappWorkflowConfig();

            container.Register(Component.For<IReplyStore>()
                .Instance(replyStore)
                .LifestyleSingleton());
        }
    }
}