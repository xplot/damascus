/*
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Damascus.Web
{
    public class StringArrayCsvModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object resultData = new string[0];
            if (bindingContext != null)
            {

                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (valueProviderResult == null)
                    return new string[0];
                resultData = valueProviderResult.AttemptedValue.Split(new[] { ',' });
            }
            return resultData;
        }
    }

	public class GuidArrayCsvModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
            List<Guid> result = new List<Guid>();
            if (bindingContext != null)
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (valueProviderResult == null)
                    return new Guid[0];

                string[] guids = valueProviderResult.AttemptedValue.Split(new[] { ',' });

                foreach (var guidString in guids)
                {
                    result.Add(new Guid(guidString));
                }
            }
			return result.ToArray();
		}
	}
}
*/