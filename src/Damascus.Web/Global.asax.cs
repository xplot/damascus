using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Damascus.Core;
using Damascus.Core.AutoMapper;
using NServiceBus;
using NServiceBus.ObjectBuilder;

namespace Damascus.Web
{
    //Check this out: http://docs.particular.net/nservicebus/using-azure-servicebus-as-transport-in-nservicebus
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IWindsorContainer container;

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            //Json by Default
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            //IOC
            BootstrapContainer(GlobalConfiguration.Configuration);
        }

        private static IWindsorContainer BootstrapContainer(HttpConfiguration config)
        {
            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new NServiceBusControllerActivator()));
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new NServiceBusControllerActivator());
            AutoMapperConfig.CreateMaps();

            container = new WindsorContainer();
            Configure.Serialization.Json();

            NServiceBus.Configure
            .With(typeof(WebApiApplication).ReferencedAssemblies())
            .CastleWindsorBuilder(container)
            .InMemorySubscriptionStorage()
            .UseTransport<AzureStorageQueue>()
            .ForMVC(container)
            //.TraceLogger()
            .UnicastBus()
                .ImpersonateSender(false)
            .DisableTimeoutManager()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            return container;
        }

        
    }


    public static class ConfigureMvc3
    {
        public static IEnumerable<Assembly> ReferencedAssemblies(this Type type)
        {
            var assemblies = type.Assembly
               .GetReferencedAssemblies()
               .Select(n => Assembly.Load(n))
               .ToList();
            
            return assemblies;
        }

        public static Configure ForMVC(this Configure configure, IWindsorContainer container)
        {
            //container.Register(Component.For<IControllerFactory>().Instance(new DefaultControllerFactory(new NServiceBusControllerActivator())));
            container.Install(FromAssembly.This());

            DependencyResolver.SetResolver(new NServiceIoCContainer(container, configure.Builder));

            // Set the MVC dependency resolver to use our resolver
            DependencyResolver.SetResolver(new NServiceIoCContainer(container, configure.Builder));

            // Required by the fluent configuration semantics
            return configure;
        }
    }
}