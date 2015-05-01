using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Authorization;
using ILogger = Microsoft.Framework.Logging.ILogger;
using DI = Microsoft.Framework.DependencyInjection;
using Damascus.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.Lifestyle;

using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Damascus.Web
{
    public class Startup
    {
        private WindsorContainer container;

        public Startup(IHostingEnvironment env)
        {
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(DI.IServiceCollection services)
        {   
            services.AddMvc();
            services.AddOptions();

            //Please do not remove this line, it breaks Injection if not included
            services.Configure<AuthorizationOptions>(options =>
            {
            });

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            //services.AddWebApiConventions();
            //services.AddInstance(typeof(TwillioConfig),new TwillioConfig()
            //{
            //    AccountSid = "1111",
            //    AuthToken = "1111",
            //    SmsOutPhone = "2222",
            //    CallPhone = "3333",
            //    VoiceCallbackUrl = "4444",
            //    EmailCallbackUrl = "5555"
            //});

            ConfigureContainer(services);
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();
            
            //Error
            //app.UseErrorPage()

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");   

            app.ApplicationServices =  container.Resolve<IServiceProvider>();
        }
        
        private void ConfigureLogging(ILoggerFactory loggerfactory )
        {
//             loggerfactory.AddConsole();
//             loggerfactory.AddConsole((category, logLevel) => logLevel >= LogLevel.Critical && category.Equals(typeof(Program).FullName));   
        }
        
        private void ConfigureContainer(DI.IServiceCollection services)
        {
            this.container = new WindsorContainer();
            this.container.Install(
                new ManagersInstaller(),
                new ReplyStoreInstaller(),
                new WorkflowEngineInstaller()
            );
            
            container.Populate(services);

            //Huge Patch do not remove until we find out what's happenning here
            container.Register(
                Component.For(typeof(IEnumerable<Microsoft.AspNet.Mvc.Core.IActionDescriptorProvider>))
                        .ImplementedBy(typeof(List<Microsoft.AspNet.Mvc.Core.IActionDescriptorProvider>))
            );

            container.BeginScope();
        }
    }
    
}
