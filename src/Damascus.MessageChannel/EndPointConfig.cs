using Castle.Windsor;
using Castle.Windsor.Installer;
using NServiceBus;

namespace Damascus.MessageChannel
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Serialization.Json();

            var container = new WindsorContainer();
            container.Install(FromAssembly.This());

            Configure.With()
                .CastleWindsorBuilder(container)
                .InMemorySubscriptionStorage()
                .DisableTimeoutManager()
                .AzureConfigurationSource()
                .UseTransport<AzureStorageQueue>()
                ;
            
            
        }
    }
}
