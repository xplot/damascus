using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Configuration;
using Damascus.Core;
using Damascus.Message;
using Damascus.Workflow;

namespace Damascus.Web
{
    public class WorkflowEngineInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            IDataSerializer serializer;
            /*if (ConfigurationManager.AppSettings["serializer"] != null &&
                ConfigurationManager.AppSettings["serializer"] == "redis")
            {
                var connection = ConnectionMultiplexer.Connect("voiceflows.redis.cache.windows.net, password=0DF8T+8zJ77KWJYar33aqSFgeG/NTjtqMo5oJU1b3n4=");
                serializer = new RedisSerializer { Cache = connection.GetDatabase() };
            }
            else*/
                serializer = new MemorySerializer();
            
            container.Register(Component.For<WorkflowEngine>()
                     .Instance(WorkflowSetup.Configure(typeof(InviteWorkflow).Assembly)
                     .WithSerializer(serializer)
                     .WithCastleWindsor(container).WorkflowEngine));

            container.Register(Component.For<IDataSerializer>().Instance(serializer));
        }
    }
}
