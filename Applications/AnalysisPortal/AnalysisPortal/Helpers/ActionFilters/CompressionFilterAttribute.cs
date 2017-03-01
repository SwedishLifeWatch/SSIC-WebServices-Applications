using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalysisPortal.Helpers.ActionFilters
{
    public class CompressFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpRequestBase request = filterContext.HttpContext.Request;
            //string acceptEncoding = request.Headers["Accept-Encoding"];
            //if (string.IsNullOrEmpty(acceptEncoding)) return;

            //acceptEncoding = acceptEncoding.ToUpperInvariant();
            //HttpResponseBase response = filterContext.HttpContext.Response;
            //if (acceptEncoding.Contains("GZIP"))
            //{
            //    response.AppendHeader("Content-encoding", "gzip");
            //    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            //}
            //else if (acceptEncoding.Contains("DEFLATE"))
            //{
            //    response.AppendHeader("Content-encoding", "deflate");
            //    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            //}
            AddCompression(filterContext.HttpContext);
        }     

        public static void AddCompression(HttpContextBase httpContext)
        {
            bool? isCompressionAdded = (bool?)httpContext.Items["compressionAdded"];
            if (isCompressionAdded.GetValueOrDefault(false))
            {
                return;
            }

            HttpRequestBase request = httpContext.Request;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return;
            }

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