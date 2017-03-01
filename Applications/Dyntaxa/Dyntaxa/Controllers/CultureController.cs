using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace Dyntaxa.Controllers
{
    public class CultureController : DyntaxaBaseController
    {
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

        // GET: /Culture/
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            IUserContext userContext = GetCurrentUser();
            ILocale locale = CoreData.LocaleManager.GetUsedLocales(userContext).Get(culture);
            if (locale != null)
            {
                SetLanguage(locale.ISOCode);
                if (userContext.IsAuthenticated())
                {
                    userContext.Locale = locale;
                }

                const string recreateTreeKey = "RecreateTree";
                if (TempData.ContainsKey(recreateTreeKey))
                {
                    TempData[recreateTreeKey] = true;
                }
                else
                {
                    TempData.Add(recreateTreeKey, true);
                }
                
            }

            returnUrl = RemoveLangQuerystringFromUrl(returnUrl);
            return Redirect(returnUrl);
        }
    }
}
