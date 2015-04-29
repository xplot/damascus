using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using NServiceBus.ObjectBuilder;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace Damascus.Web
{
    public class NServiceBusControllerActivator : IControllerActivator, IHttpControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return DependencyResolver.Current
                 .GetService(controllerType) as IController;
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