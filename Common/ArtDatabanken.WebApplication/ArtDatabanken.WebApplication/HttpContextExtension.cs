using System;
using System.Web;

namespace ArtDatabanken.WebApplication
{
    /// <summary>
    /// Contains extension methods to the HttpContext class.
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// Get web site name.
        /// </summary>
        /// <param name='httpContext'>Encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>Web site name.</returns>
        public static String GetWebSiteName(this HttpContext httpContext)
        {
            String webSiteName;

            webSiteName = httpContext.Request.PhysicalApplicationPath;
            webSiteName = webSiteName.Substring(0, webSiteName.LastIndexOf("\\"));
            webSiteName = webSiteName.Substring(webSiteName.LastIndexOf("\\") + 1);
            return webSiteName;
        }
    }
}
