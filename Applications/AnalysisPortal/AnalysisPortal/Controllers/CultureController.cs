using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller have Actions that is used to change current language
    /// </summary>
    public class CultureController : BaseController
    {
        /// <summary>
        /// Sets the current language (culture).
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        public RedirectResult SetCulture(string culture, string returnUrl)
        {            
            IUserContext userContext = GetCurrentUser();
            ILocale locale = CoreData.LocaleManager.GetUsedLocales(userContext).Get(culture);
            Thread.CurrentThread.CurrentCulture = locale.CultureInfo;
            if (locale != null)
            {
                SetLanguage(locale.ISOCode);
                if (userContext.IsAuthenticated())
                {
                    userContext.Locale = locale;
                }                
            }

            returnUrl = RemoveLangQuerystringFromUrl(returnUrl);
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Removes the lang querystring from an URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string RemoveLangQuerystringFromUrl(string url)
        {
            string path;
            string queryString = "";
            int index = url.IndexOf("?");
            if (index >= 0)
            {
                path = url.Substring(0, index);
                queryString = url.Substring(index, url.Length - index);
            }
            else
            {
                path = url;
            }

            var coll = HttpUtility.ParseQueryString(queryString);
            coll.Remove("lang");
            string returnUrl = coll.HasKeys() ? string.Format("{0}?{1}", path, coll) : path;
            return returnUrl;
        }
    }
}
