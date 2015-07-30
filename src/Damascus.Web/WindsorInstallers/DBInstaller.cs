using System.IO;
using System.Web;
using Castle.MicroKernel.Registration;
using System;
using System.Configuration;
using Damascus.Core;
using Damascus.Message;
using System.Data.SqlClient;
using NLog;

namespace Damascus.Web
{
    public class DBInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            var Settings = container.Resolve<ISettings>();
            var Logger = LogManager.GetLogger("Startup");
            var xconnection = new SqlConnection(Settings.Get("connection"));
            
            Logger.Info("FIrst connection created");
            
//             container.Register(
//                 Component.For<SqlConnection>()
//                 .UsingFactoryMethod(
//                     () => {
//                         try{
//                             var connection = new SqlConnection(Settings.Get("connection"));
//                             connection.Open();
//                             return connection;    
//                         }
//                         catch(Exception ex)
//                         {
//                             Logger.Error(ex.ToString());
//                             return null;
//                         }
//                         
//                     }
//                 )
//                 .OnDestroy(
//                     x=> x.Dispose()
//                 )
//                 .LifestyleScoped()   
//             );
//             
            
            container.Register(
				Component.For<AuthenticationStore>()
				.ImplementedBy<AuthenticationStore>()
                .LifestyleScoped()
			);
        }
    }
}
