/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Globalization;
using Microsoft.AspNet.Mvc;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using NServiceBus.ObjectBuilder;
using Microsoft.Framework.DependencyInjection;
//using Microsoft.Framework.DependencyInjection.Windsor;

namespace Damascus.Web
{
    public class NServiceBusControllerActivator : IControllerActivator //, IHttpControllerActivator
    {
        public object Create(ActionContext context, Type controllerType)
        {
            return DependencyResolver.Current
                 .GetService(controllerType);
        }
        
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return DependencyResolver.Current
                 .GetService(controllerType) as IHttpController;
        }
        
    }

    public class NServiceBusScopeContainer : IDependencyScope
    {
        protected IWindsorContainer _container;
        protected IBuilder _builder;
        private readonly IDisposable scope;

        public NServiceBusScopeContainer(IWindsorContainer container, IBuilder builder)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;
            _builder = builder;
            scope = _container.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType) ? _builder.Build(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _builder.BuildAll(serviceType);
            //return _container.ResolveAll(serviceType).Cast<object>().ToArray();
        }

        public void Dispose()
        {
            this.scope.Dispose();
        }
    }


    public class NServiceIoCContainer : NServiceBusScopeContainer, IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        public NServiceIoCContainer(IWindsorContainer container, IBuilder builder)
            : base(container, builder)
        {
        }

        public IDependencyScope BeginScope()
        {
            return new NServiceBusScopeContainer(this._container, this._builder);
        }
    }
}
*/