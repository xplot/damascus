/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using System.Web.Mvc;

namespace Damascus.Web
{
    public class WindsorControllerActionInvokerInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
        	{
        		throw new ArgumentNullException("container");
        	}
            container.Register(Component.For<IActionInvoker>().ImplementedBy<WindsorControllerActionInvoker>().LifeStyle.Transient);
        }
    }
}
*/