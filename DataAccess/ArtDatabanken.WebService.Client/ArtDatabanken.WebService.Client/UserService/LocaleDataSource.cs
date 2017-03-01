using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.UserService
{
    /// <summary>
    /// This class is used to retrieve locale related information.
    /// </summary>
    public class LocaleDataSource : UserDataSourceBase, ILocaleDataSource 
    {
        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        public LocaleList GetLocales(IUserContext userContext)
        {
            List<WebLocale> webLocales;

            CheckTransaction(userContext);
            webLocales = WebServiceProxy.UserService.GetLocales(GetClientInformation(userContext));
            return GetLocales(userContext, webLocales);
        }

        /// <summary>
        /// Get locales from web locales.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webLocales">Web locales.</param>
        /// <returns>Locales.</returns>
        private LocaleList GetLocales(IUserContext userContext,
                                      List<WebLocale> webLocales)
        {
            LocaleList locales;

            locales = new LocaleList();
            foreach (WebLocale webLocale in webLocales)
            {
                locales.Add(GetLocale(webLocale));
            }
            return locales;
        }
    }
}
