using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Authorization;

using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using Microsoft.Framework.OptionsModel;

using Microsoft.Framework.Runtime;
using Microsoft.Framework.ConfigurationModel.Json;
using Microsoft.Framework.ConfigurationModel;
using ILogger = Microsoft.Framework.Logging.ILogger;
using DI = Microsoft.Framework.DependencyInjection;
using Damascus.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.Lifestyle;

using NServiceBus;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Config;

using NLog;
using NLog.Targets;
using NLog.Config;

namespace Damascus.Web
{
    public class Startup
    {
        private WindsorContainer container;
        private IHostingEnvironment environment;
        
        public Startup(IHostingEnvironment env)
        {
            //var x = 2/0;
            this.environment = env;
            this.container = new WindsorContainer();
            ConfigureLogging();
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(DI.IServiceCollection services)
        {   
            services.AddMvc().Configure<MvcOptions>(options =>{
    	       	
//                 options.Filters.Add(new ProducesAttribute("application/json"));   
//                    
//                 options.InputFormatters.Clear();
//                 options.OutputFormatters.Clear();
//                            
//                 var jsonInputFormatter = new JsonInputFormatter();
//                 jsonInputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
//                 options.InputFormatters.Add(jsonInputFormatter);
//                 
//                 var jsonOutputFormatter = new JsonOutputFormatter();
//                 jsonOutputFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
//                 options.OutputFormatters.Add(jsonOutputFormatter);
                
            });
            services.AddOptions();

            //Please do not remove this line, it breaks Injection if not included
            services.Configure<AuthorizationOptions>(options =>
            {
            });
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            
            app.UseDefaultFiles();
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();
            
            //To resolve index.html-like files
            app.UseDefaultFiles();
            
            // Add MVC to the request pipeline.
            app.UseMvc();

            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");   
            
            ConfigureContainer(app);
            
            app.ApplicationServices = this.container.Resolve<IServiceProvider>();
           
            ConfigureBus();
        }
        
        private void ConfigureContainer(IApplicationBuilder app)
        {
            this.container.Register(Component.For<IWindsorContainer>().Instance(this.container));
            this.container.Register(
                Component.For<IServiceProvider>()
                        .Instance(new MixedWindsorServiceProvider(app.ApplicationServices, this.container))
            );
            
            this.container.Register(Component.For<IServiceScopeFactory>().ImplementedBy<WindsorServiceScopeFactory>());
            
            this.container.Register(
                Component.For<IHostingEnvironment>()
                        .Instance(this.environment)
            );

            this.container.Register(
                Component.For<ISettings>()
                        .ImplementedBy<Settings>()
            );

            this.container.Install(
                new ManagersInstaller(),
                new ReplyStoreInstaller(),
                new WorkflowEngineInstaller(),
                new DBInstaller()
            );

            container.BeginScope();
        }

        private void ConfigureBus()
        {
            var configuration = new BusConfiguration();
            var conventionsBuilder = configuration.Conventions();
            var Settings = container.Resolve<ISettings>();

            conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Bus") && t.Namespace.EndsWith("Commands"));
            conventionsBuilder.DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Bus") && t.Namespace.EndsWith("Events"));

            configuration.EndpointName("Damascus.Web");
            configuration.UseSerialization<JsonSerializer>();
            configuration.AssembliesToScan(AllAssemblies.Matching("Damascus.Message").And("NServiceBus"));
            configuration.UseTransport<SqlServerTransport>().ConnectionString(Settings.Get("connection"));
            configuration.Transactions().Disable();

            configuration.UsePersistence<InMemoryPersistence>();

            configuration.EnableInstallers();
            
            // Castle with a container instance
            configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(this.container));

            var bus = Bus.Create(configuration);
            bus.Start();

        }
        
        private void ConfigureLogging()
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
            
//             var factory = container.Resolve<ILoggerFactory>();
//             factory.AddNLog(new NLog.LogFactory(config));
        }

    }

    

}
