using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Diagnostics;

namespace AnalysisPortal.Helpers
{
    /// <summary>
    /// This class contains extension methods for UrlHelper
    /// </summary>
    public static class UrlHelperExtensions
    {
        private static readonly object ThisLock = new object();

        /// <summary>
        /// Crates an url that also have a version query string appended.
        /// In this way the user will always get the latest version of the JavaScript/Css/.. file
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns></returns>
        public static string DatedContent(this UrlHelper urlHelper, string contentPath)
        {
            string strResult = GetDatedContentValue(urlHelper, contentPath);
            if (strResult != null)
            {
                return strResult;
            }

            lock (ThisLock)
            {
                strResult = GetDatedContentValue(urlHelper, contentPath);
                if (strResult != null)
                {
                    return strResult;
                }

                const string cacheKey = "DatedContentsDictionary";
                Dictionary<string, string> dicDatedContents = HttpRuntime.Cache[cacheKey] as Dictionary<string, string>;
                if (dicDatedContents == null)
                {
                    dicDatedContents = new Dictionary<string, string>();
                    HttpRuntime.Cache.Insert(cacheKey, dicDatedContents, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);                    
                }
                string strDatedPath;
                if (dicDatedContents.TryGetValue(contentPath, out strDatedPath))
                {
                    return urlHelper.Content(strDatedPath);
                }
                strDatedPath = GetDatedPath(contentPath);
                dicDatedContents.Add(contentPath, strDatedPath);
                return urlHelper.Content(strDatedPath);
            }
        }

        private static string GetDatedPath(string contentPath)
        {
            string modifiedDate = GetModifiedDate(contentPath);
            var datedPath = new StringBuilder(contentPath);
            if (!string.IsNullOrEmpty(modifiedDate))
            {
                datedPath.AppendFormat(
                    "{0}m={1}",
                    contentPath.IndexOf('?') >= 0 ? '&' : '?',
                    GetModifiedDate(contentPath));
            }
            string strDatedPath = datedPath.ToString();
            return strDatedPath;
        }

        private static string GetDatedContentValue(UrlHelper urlHelper, string contentPath)
        {
            const string cacheKey = "DatedContentsDictionary";
            Dictionary<string, string> dicDatedContents = HttpRuntime.Cache[cacheKey] as Dictionary<string, string>;
            if (dicDatedContents == null)
            {
                return null;
            }

            string strDatedPath;
            if (dicDatedContents.TryGetValue(contentPath, out strDatedPath))
            {
                return urlHelper.Content(strDatedPath);
            }
            return null;
        }

        /// <summary>
        /// Gets a files modified date as a string.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static string GetModifiedDate(string filePath)
        {
            try
            {                
                return System.IO.File.GetLastWriteTime(HostingEnvironment.MapPath(filePath)).ToString("yyyyMMddhhmmss");
            }
            catch (Exception)
            {
                return null;
            }            
        }
    }
}