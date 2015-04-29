//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using App.Authentication;
//using Castle.MicroKernel.Registration;
//using Castle.MicroKernel.SubSystems.Configuration;
//using Castle.Windsor;

//namespace App.WindsorInstallers
//{
//    public class AuthenticatorsInstaller : IWindsorInstaller
//    {
//        public void Install(IWindsorContainer container, IConfigurationStore store)
//        {
//            if (container == null)
//            {
//                throw new ArgumentNullException("container");
//            }

//            //container.Register(Component.For<IAuthenticator>().ImplementedBy<GoogleAuthenticator>().Named("google").LifeStyle.Singleton);
//            //container.Register(Component.For<IAuthenticator>().ImplementedBy<FacebookAuthenticator>().Named("facebook").LifeStyle.Singleton);
//        }
//    }
//}