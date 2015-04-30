/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel;

namespace Damascus.Web
{
    public class WindsorControllerActionInvoker : ControllerActionInvoker
    {
        private readonly IKernel kernel;

        public WindsorControllerActionInvoker(IKernel kernel)
        {
            this.kernel = kernel;
        }

        protected override AuthorizationContext InvokeAuthorizationFilters(ControllerContext controllerContext, IList<IAuthorizationFilter> filters, ActionDescriptor actionDescriptor)
        {
            if (filters == null)
        	{
        		throw new ArgumentNullException("filters");
        	}

            foreach (IAuthorizationFilter authorizationFilter in filters)
            {
                this.kernel.InjectProperties(authorizationFilter);
            }
            return base.InvokeAuthorizationFilters(controllerContext, filters, actionDescriptor);
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(
                ControllerContext controllerContext,
                IList<IActionFilter> filters,
                ActionDescriptor actionDescriptor,
                IDictionary<string, object> parameters)
        {
            if (filters == null)
        	{
        		throw new ArgumentNullException("filters");
        	}

            foreach (IActionFilter actionFilter in filters)
            {
                this.kernel.InjectProperties(actionFilter);
            }
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }
}
*/