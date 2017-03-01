using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// This class contains extension methods for the UrlHelper class.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Converts a relative URL to an absolute URL.
        /// </summary>
        /// <param name="urlHelper">The UrlHelper.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <returns>An absolute URL.</returns>
        public static string ToAbsoluteUrl(this UrlHelper urlHelper, string relativePath)
        {
            if (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.Insert(0, "~");
            }

            if (!relativePath.StartsWith("~/"))
            {
                relativePath = relativePath.Insert(0, "~/");
            }

            Uri uri = new Uri(relativePath, UriKind.RelativeOrAbsolute);

            // If the URI is not already absolute, rebuild it based on the current request.
            if (!uri.IsAbsoluteUri)
            {
                Uri requestUrl = urlHelper.RequestContext.HttpContext.Request.Url;
                UriBuilder builder = new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port);

                builder.Path = System.Web.VirtualPathUtility.ToAbsolute(relativePath);
                uri = builder.Uri;
            }

            return uri.ToString();
        }
    }
}
