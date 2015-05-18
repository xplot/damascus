using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Configuration;
using Damascus.Core;
using Damascus.Message;
using Damascus.Workflow;
using ServiceStack.Redis;

namespace Damascus.Web
{
    public class WorkflowEngineInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            
            var Settings = container.Resolve<Settings>();
            
            IDataSerializer serializer;
            if (Settings.Get("serializer") == "redis")
            {
                serializer = new RedisSerializer { ClientManager = new PooledRedisClientManager() };
            }
            else
                serializer = new MemorySerializer();
            
            container.Register(Component.For<WorkflowEngine>()
                     .Instance(WorkflowSetup.Configure(typeof(InviteWorkflow).Assembly)
                     .WithSerializer(serializer)
                     .WithCastleWindsor(container).WorkflowEngine));

            container.Register(Component.For<IDataSerializer>().Instance(serializer));
        }
    }
}
