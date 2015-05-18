using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Configuration;
using Damascus.Core;
using Damascus.Message;
using Damascus.Workflow;

using StackExchange.Redis;

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
                var connection = ConnectionMultiplexer.Connect("127.0.0.1");
                serializer = new RedisSerializer { Cache = connection.GetDatabase() };
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
