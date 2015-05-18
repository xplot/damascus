using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Damascus.Core;
using Damascus.Workflow;

namespace Damascus.Web
{
    public class ReplyStoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            var replyStore = new ReplyStore();
            replyStore.RegisterWorkflow(typeof(InviteWorkflow));
            
            container.Register(Component.For<IReplyStore>()
                .Instance(replyStore)
                .LifestyleSingleton());
        }
    }
}
