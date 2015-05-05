using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;

using NServiceBus;

using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Logging;
using Config = Microsoft.Framework.ConfigurationModel;

using NLog;
using NLog.Targets;
using NLog.Config;

using Damascus.Message.Command;

namespace Damascus.MessageChannel
{
    public class Program
    {
        public void Main(string[] args)
        {
            
            var container = ConfigureContainer();
            
            ConfigureLogging(container);
            var configuration = ConfigureNSB(container);
            
            using (IStartableBus startableBus = Bus.Create(configuration))
            {
                IBus bus = startableBus.Start();
                
                var key = "1";

                while(key != "x"){
                    
                    if(key == "email"){
                        bus.SendLocal(new CreateEmailMessage());
                    }
                    else if(key == "sms"){
                        bus.SendLocal(new CreateSmsMessage());
                    }
                    else if(key == "phone"){
                        bus.SendLocal(new CreateCallMessage());
                    }
                    
                    System.Console.WriteLine("Press x to exit ");
                    key = System.Console.ReadLine();
                }
            }
        }

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
            configuration.DisableFeature<NServiceBus.Features.TimeoutManager>();
            configuration.Transactions().Disable();

            configuration.UsePersistence<InMemoryPersistence>();

            configuration.EnableInstallers();

            // Castle with a container instance
            configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));

            return configuration;

        }
        
        private void ConfigureLogging(IWindsorContainer container)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
            };
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Info, consoleTarget));

            LogManager.Configuration = config;
            NServiceBus.Logging.LogManager.Use<NLogFactory>();
            
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLog.LogFactory(config));
            
            container.Register(
                Component.For<ILoggerFactory>().Instance(loggerFactory)
            );
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
