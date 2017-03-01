using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtDatabanken.WebApplication.Helpers.ActionFilters
{
    /// <summary>
    /// Filter attribute for adding compression on result.
    /// </summary>
    public class CompressFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            AddCompression(filterContext.HttpContext);
        }

        /// <summary>
        /// Adds the compression.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public static void AddCompression(HttpContextBase httpContext)
        {
            bool? isCompressionAdded = (bool?)httpContext.Items["compressionAdded"];
            if (isCompressionAdded.GetValueOrDefault(false))
            {
                return;
            }

            HttpRequestBase request = httpContext.Request;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();
            HttpResponseBase response = httpContext.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }

            httpContext.Items["compressionAdded"] = true;
        }
    }
}