using System;
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

namespace Damascus.Web
{
	public static class WindsorRegistration
{
    public static void Populate(
            this IWindsorContainer container,
            IEnumerable<MDI.ServiceDescriptor> descriptors)
    {
        container.Register(Component.For<IWindsorContainer>().Instance(container));
        container.Register(Component.For<IServiceProvider>().ImplementedBy<WindsorServiceProvider>());
        container.Register(Component.For<IServiceScopeFactory>().ImplementedBy<WindsorServiceScopeFactory>());

        container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

        Register(container, descriptors);
    }

    private static void Register(
            IWindsorContainer container,
            IEnumerable<MDI.ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            if (descriptor.ImplementationType != null)
            {
                // Test if the an open generic type is being registered
                var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                if (serviceTypeInfo.IsGenericTypeDefinition)
                {
                    container.Register(Component.For(descriptor.ServiceType)
                                            .ImplementedBy(descriptor.ImplementationType)
                                            .ConfigureLifecycle(descriptor.Lifetime)
                                            .OnlyNewServices());
                }
                else
                {
                    container.Register(Component.For(descriptor.ServiceType)
                                            .ImplementedBy(descriptor.ImplementationType)
                                            .ConfigureLifecycle(descriptor.Lifetime)
                                            .OnlyNewServices());
                }
            }
            else if (descriptor.ImplementationFactory != null)
            {
                var service1 = descriptor;
                container.Register(Component.For(descriptor.ServiceType)
                        .UsingFactoryMethod<object>(c =>
                        {
                            var builderProvider = container.Resolve<IServiceProvider>();
                            return
                                service1.ImplementationFactory(builderProvider);
                        })
                        .ConfigureLifecycle(descriptor.Lifetime)
                        .OnlyNewServices());
            }
            else
            {
                container.Register(Component.For(descriptor.ServiceType)
                        .Instance(descriptor.ImplementationInstance)
                        .ConfigureLifecycle(descriptor.Lifetime)
                        .OnlyNewServices());
            }
        }
    }

    private static ComponentRegistration<object> ConfigureLifecycle(
            this ComponentRegistration<object> registrationBuilder,
            ServiceLifetime lifecycleKind)
    {
        switch (lifecycleKind)
        {
            case ServiceLifetime.Singleton:
                registrationBuilder.LifestyleSingleton();
                break;
            case ServiceLifetime.Scoped:
                registrationBuilder.LifestyleScoped();
                break;
            case ServiceLifetime.Transient:
                registrationBuilder.LifestyleTransient();
                break;
        }

        return registrationBuilder;
    }

    private class WindsorServiceProvider : IServiceProvider
    {
        private readonly IWindsorContainer _container;

        public WindsorServiceProvider(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }

    private class WindsorServiceScopeFactory : IServiceScopeFactory
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

    private class WindsorServiceScope : IServiceScope
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
}
