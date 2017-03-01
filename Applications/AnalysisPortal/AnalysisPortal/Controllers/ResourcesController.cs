using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Resources;
using System.Collections;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using Resources;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used to render JavaScript files that contains
    /// resource strings in various languages.
    /// </summary>
    public class ResourcesController : Controller
    {
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        //[ExcludeFilter(typeof(NoCacheFilterAttribute), Order = 0)]
        //[JavaScriptCacheFilterAttribute(Order = 1)]        

        /// <summary>
        /// Creates a JavaScript file with all entries in the requested
        /// resource file (.resx) that exist in the folder JavaScriptResources.
        /// </summary>
        /// <param name="resxFileName">Name of the .resx resource file.</param>
        /// <param name="culture">The culture (eg. "sv")</param>
        /// <returns>A JavaScript file</returns>
        [ExcludeFilter(typeof(NoCacheFilterAttribute), Order = 0)]
        [OutputCache(Duration = 60 * 5, VaryByParam = "culture", Order = 1)]
        public ActionResult GetResourcesJavaScript(string resxFileName, string culture)
        {            
            string cacheKey = string.Format("JsFile: {0}, Culture {1}", resxFileName, culture);
            string strJavaScript = this.HttpContext.Cache.Get(cacheKey) as string;

            if (string.IsNullOrEmpty(strJavaScript))
            {                
                Dictionary<string, string> resourceDictionary = GetAllStringPropertiesFromResourceFile();                
                string json = Serializer.Serialize(resourceDictionary);                
                strJavaScript =
                    string.Format(
                        "var AnalysisPortal = AnalysisPortal || {{}};" + Environment.NewLine +
                        "AnalysisPortal.Resources = {0};", json);
                this.HttpContext.Cache.Insert(cacheKey, strJavaScript, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(20));           
            return JavaScript(strJavaScript);
        }

        /// <summary>
        /// Gets all string properties from the Resources.Resource file.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetAllStringPropertiesFromResourceFile()
        {            
            IEnumerable<PropertyInfo> properties = typeof(Resources.Resource).GetProperties()
                                                    .Where(p => p.PropertyType == typeof(string));
            var dicStringProperties = new Dictionary<string, string>();

            foreach (PropertyInfo p in properties)
            {
                string strValue = (string)p.GetValue(null, null);                    
                dicStringProperties.Add(p.Name, strValue);
            }
            return dicStringProperties;
        }
     
#if DEBUG

        /// <summary>
        /// Creates vsdoc JavaScript file for Labels.resx.
        /// Should only be available in Debug mode
        /// Be sure to check out AnalysisPortal.Resources-vsdoc.js before executing this action.
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateVsDocJavaScript()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            Dictionary<string, string> resourceDictionary = GetAllStringPropertiesFromResourceFile();
            //string[] keys = resourceDictionary.Keys.ToArray();
            //foreach (string key in keys)
            //{
            //    resourceDictionary[key] = "";
            //}
            string json = Serializer.Serialize(resourceDictionary);
            string strJavaScript =
                string.Format(
                    "var AnalysisPortal = AnalysisPortal || {{}};" + Environment.NewLine +
                    "AnalysisPortal.Resources = {0};", json);

            string javascriptVsdocOutDir = Server.MapPath("~/Scripts/AnalysisPortal/AnalysisPortal.Resources-vsdoc.js");
            System.IO.File.WriteAllText(javascriptVsdocOutDir, strJavaScript);
            return Content("File was created: " + javascriptVsdocOutDir);            
        }

#endif

    }
}
