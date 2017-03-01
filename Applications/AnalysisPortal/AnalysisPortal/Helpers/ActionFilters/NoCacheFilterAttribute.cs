using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalysisPortal.Helpers.ActionFilters
{
    /// <summary>
    /// With this filter you can tell the browser to not cache the page requested.
    /// </summary>
    public class NoCacheFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //if (filterContext.GetCustomAttributes(typeof(SkipMyGlobalActionFilterAttribute), false).Any())
            //{
            //    return;
            //}

            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.AppendHeader("Cache-Control", "no-store");
            filterContext.HttpContext.Response.AppendHeader("pragma", "no-cache");

            base.OnResultExecuting(filterContext);
        }
    }

    public class SkipMyGlobalActionFilterAttribute : Attribute
    {
    }

    public class MyGlobalActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipMyGlobalActionFilterAttribute), false).Any())
            {
                return;
            }

            // here do whatever you was intending to do 
        }
    } 

    public class JavaScriptCacheFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //filterContext.HttpContext.Response.ContentEncoding.c
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddMonths(1));
            //filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            //filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            //filterContext.HttpContext.Response.Cache.SetNoStore();
            //filterContext.HttpContext.Response.AppendHeader("Cache-Control", "public");
            //filterContext.HttpContext.Response.AppendHeader("pragma", "no-cache");

            base.OnResultExecuting(filterContext);
        }        

//        <urlCompression doStaticCompression="true" />
//<staticContent>
//    <!-- Set expire headers to 1 year for static content-->
//<clientCache cacheControlCustom="public" cacheControlMode="UseMaxAge" cacheControlMaxAge="365.00:00:00" />
    }
}