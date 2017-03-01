using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalysisPortal.Helpers.ActionFilters
{
    public class ExcludeFilterProvider : IFilterProvider
    {
        private FilterProviderCollection filterProviders;
        //private IFilterProvider[] filters;

        public ExcludeFilterProvider(IFilterProvider[] filters)
        {
            this.filterProviders = new FilterProviderCollection(filters);
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            Filter[] filters = this.filterProviders.GetFilters(controllerContext, actionDescriptor).ToArray();

            IEnumerable<ExcludeFilterAttribute> excludeFilters = from f in filters where f.Instance is ExcludeFilterAttribute select f.Instance as ExcludeFilterAttribute;

            List<Type> filterTypesToRemove = new List<Type>();
            foreach (ExcludeFilterAttribute excludeFilter in excludeFilters)
            {
                filterTypesToRemove.Add(excludeFilter.FilterType);
            }

            IEnumerable<Filter> res = from filter in filters where !filterTypesToRemove.Contains(filter.Instance.GetType()) select filter;
            return res;
        }
    }
}