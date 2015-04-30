/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel;
using System.Reflection;
using Castle.MicroKernel.ComponentActivator;
using System.Globalization;

namespace Damascus.Web
{
    public static class WindsorExtensions
    {
        public static void InjectProperties(this IKernel kernel, object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }

            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && kernel.HasComponent(property.PropertyType))
                {
                    var value = kernel.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format(CultureInfo.InvariantCulture, 
                            "Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                        throw new ComponentActivatorException(message, ex, null);
                    }
                }
            }
        }
    }
}
*/