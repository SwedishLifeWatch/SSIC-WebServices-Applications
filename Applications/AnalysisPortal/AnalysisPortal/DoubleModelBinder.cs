using System;
using System.Globalization;
using System.Web.Mvc;

namespace AnalysisPortal
{
    public class DoubleModelBinder : IModelBinder
    {        
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string numStr = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
            double res;

            if (!double.TryParse(numStr, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out res))
            {
                if (bindingContext.ModelType == typeof(double?))
                {
                    return null;
                }

                throw new ArgumentException();
            }
            
            return res;
        }
    }
}