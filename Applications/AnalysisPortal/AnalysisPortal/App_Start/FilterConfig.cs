using System.Web;
using System.Web.Mvc;
using AnalysisPortal.Helpers.ActionFilters;

namespace AnalysisPortal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new NoCacheFilterAttribute());

            //filters.Add(new JavaScriptCacheFilterAttribute());
        }
    }
}