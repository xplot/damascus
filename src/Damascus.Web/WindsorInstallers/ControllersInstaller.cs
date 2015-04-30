/*
using System.Web.Http;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web.Mvc;

namespace Damascus.Web
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            container.Register(Types.FromThisAssembly()
                .BasedOn<IController>()
                .If(Component.IsInNamespace("Damascus.Web.Controllers"))
                .If(t => t.Name.EndsWith("Controller", StringComparison.Ordinal))
                .Configure(c => c.LifestyleTransient()));

            container.Register(Types.FromThisAssembly()
                .BasedOn<ApiController>()
                .If(Component.IsInNamespace("Damascus.Web.Controllers"))
                .If(t => t.Name.EndsWith("Controller", StringComparison.Ordinal))
                .Configure(c => c.LifestyleTransient()));
        }
    }
}
*/