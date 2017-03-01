using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalysisPortal.Helpers.ActionFilters
{
    public class ExcludeFilterAttribute : FilterAttribute
    {
        private Type filterType;

        public ExcludeFilterAttribute(Type filterType)
        {
            this.filterType = filterType;
        }

        public Type FilterType
        {
            get
            {
                return this.filterType;
            }
        }
    }
}