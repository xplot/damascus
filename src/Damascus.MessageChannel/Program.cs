using Castle.Windsor;
using Castle.Windsor.Installer;
using NServiceBus;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Config = Microsoft.Framework.ConfigurationModel;

namespace Damascus.MessageChannel
{
    public class Program
    {
        public void Main(string[] args)
        {
            var container = ConfigureContainer();
            var configuration = ConfigureNSB(container);

            using (IStartableBus bus = Bus.Create(configuration))
            {
                bus.Start();
            }

            System.Console.WriteLine(" Hello world");
        }

        private IWindsorContainer container;

        private IWindsorContainer ConfigureContainer()
        {
            var container = new WindsorContainer();
            container.Install(
                new ManagersInstaller()
            );

            return container;
        }


        public BusConfiguration ConfigureNSB(IWindsorContainer container)
        {
            var configuration = new BusConfiguration();

            var conventionsBuilder = configuration.Conventions();

            conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Bus") && t.Namespace.EndsWith("Commands"));
            conventionsBuilder.DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Bus") && t.Namespace.EndsWith("Events"));

            configuration.EndpointName("Damascus.MessageChannel");
            configuration.UseSerialization<JsonSerializer>();
            configuration.AssembliesToScan(AllAssemblies.Matching("Damascus.Message").And("NServiceBus"));
            configuration.UseTransport<SqlServerTransport>().ConnectionString(Configuration["connection"]);
            configuration.Transactions().Disable();

            configuration.UsePersistence<InMemoryPersistence>();

            configuration.EnableInstallers();

            // Castle with a container instance
            configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(this.container));

            return configuration;

        }

        private static Config.Configuration _config;
        public static Config.Configuration Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config.Configuration();
                    _config.AddJsonFile("Config/local.json");//Parametize this....for Prod

                }
                return _config;
            }
        }
    }
}
