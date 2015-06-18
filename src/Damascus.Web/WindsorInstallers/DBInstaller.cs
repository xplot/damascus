using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using System;
using System.Configuration;
using Damascus.Core;
using Damascus.Message;
using System.Data.SqlClient;

namespace Damascus.Web
{
    public class DBInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            var Settings = container.Resolve<ISettings>();

            container.Register(
					Component.For<AuthenticationStore>()
					.ImplementedBy<AuthenticationStore>()
					.OnCreate(auth => {
						auth.Connection = new SqlConnection(Settings.Get("connection"));
						auth.Connection.Open();
					}).OnDestroy(auth => auth.Dispose())
					.LifestyleTransient()
				);
        }
    }
}
