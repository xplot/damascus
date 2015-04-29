using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Mvc;
using Castle.DynamicProxy.Generators;
using NServiceBus;

namespace Damascus.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiWithAction",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                "InviteRoute",
                "api/invite",
                new { controller = "Invite", action = "CreateInvite" }
            );
            config.Routes.MapHttpRoute(
                "InviteContactsRoute",
                "api/invite/attendees",
                new { controller = "Invite", action = "InviteAttendees" }
            );
            config.Routes.MapHttpRoute(
                "CancelInviteRoute",
                "api/invite/cancel",
                new { controller = "Invite", action = "CancelInvite" }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }
    }
}
