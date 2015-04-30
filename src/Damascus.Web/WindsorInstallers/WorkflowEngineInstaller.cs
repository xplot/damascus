/*
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
    public class WorkflowEngineInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            IDataSerializer serializer;
            if (ConfigurationManager.AppSettings["serializer"] != null &&
                ConfigurationManager.AppSettings["serializer"] == "redis")
            {
                var connection = ConnectionMultiplexer.Connect("voiceflows.redis.cache.windows.net, password=0DF8T+8zJ77KWJYar33aqSFgeG/NTjtqMo5oJU1b3n4=");
                serializer = new RedisSerializer { Cache = connection.GetDatabase() };
            }
            else
                serializer = new MemorySerializer();
            
            container.Register(Component.For<WorkflowEngine>()
                     .Instance(WorkflowSetup.Configure(typeof(JaviCustomWorkflow).Assembly)
                     .WithSerializer(serializer)
                     .WithCastleWindsor(container).WorkflowEngine));

            container.Register(Component.For<IDataSerializer>().Instance(serializer));
        }
    }
}
*/