using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Castle.Windsor;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Microsoft.Framework.DependencyInjection;
using MDI = Microsoft.Framework.DependencyInjection;
using NLog;
namespace Damascus.Web
{
	
    public class MixedWindsorServiceProvider : IServiceProvider
    {
        private readonly IWindsorContainer _container;
        private readonly IServiceProvider _fallback;
    	
        private Logger Logger;
        
        public MixedWindsorServiceProvider(IServiceProvider fallback, IWindsorContainer container)
        {
            _container = container;
            _fallback = fallback;
            this.Logger = LogManager.GetLogger(GetType().FullName);
        }

        public object GetService(Type serviceType)
        {
            //Logger.Info("Resolving: " + serviceType.FullName);
            
            object serviceInstance = null;
            try
            {
                serviceInstance = _container.Resolve(serviceType);    
            }
            catch(Exception ex)
            {
                //We silently go to the fallback
                //Logger.Error(ex.ToString());
            }
            
            //If we reached here it means, no resolution, then we go to the fallback
            if(serviceInstance == null)
                serviceInstance =  _fallback.GetService(serviceType);
            
            //Logger.Info("Resolved to: " + serviceInstance);
            
            return serviceInstance;
            
        }

        public static bool IsIEnumerableOfT(Type type)
        {
            return type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }

    public class WindsorServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IWindsorContainer _container;

        public WindsorServiceScopeFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public IServiceScope CreateScope()
        {
            return new WindsorServiceScope(_container);
        }
    }

    public class WindsorServiceScope : IServiceScope
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDisposable _scope;

        public WindsorServiceScope(IWindsorContainer container)
        {
            _scope = container.BeginScope();
            _serviceProvider = container.Resolve<IServiceProvider>();
        }

        public IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
