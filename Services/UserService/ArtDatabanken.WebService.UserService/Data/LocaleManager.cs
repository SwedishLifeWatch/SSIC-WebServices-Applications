using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Manager of locale information.
    /// </summary>
    public class LocaleManager
    {
        /// <summary>
        /// Get cached locales.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Cached locales.</returns>
        private static Hashtable GetCachedLocales(WebServiceContext context)
        {
            Hashtable localeTable;
            WebLocale locale;

            // Get cached information.
            localeTable = (Hashtable)context.GetCachedObject(Settings.Default.LocaleCacheKey);

            // Data not in cache - store it in the cache.
            if (localeTable.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetLocales())
                {
                    localeTable = new Hashtable();
                    while (dataReader.Read())
                    {
                        locale = new WebLocale();
                        locale.LoadData(dataReader);

                        // Add object to Hashtable.
                        localeTable.Add(locale.Id, locale);
                        localeTable.Add(locale.ISOCode.ToUpper(), locale);
                    }
                    // Add information to cache.
                    context.AddCachedObject(Settings.Default.LocaleCacheKey,
                                            localeTable,
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.High);
                }
            }
            return localeTable;
        }

        /// <summary>
        /// Get a locale object by id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="localeId">Requested locale id</param>
        /// <returns>Requested locale object</returns>
        public static WebLocale GetLocale(WebServiceContext context,
                                          Int32 localeId)
        {
            return (WebLocale)(GetCachedLocales(context)[localeId]);
        }

        /// <summary>
        /// Get a locale object by ISO code.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="ISOCode">ISO code for requested locale.</param>
        /// <returns>Requested locale object</returns>
        public static WebLocale GetLocale(WebServiceContext context,
                                          String ISOCode)
        {
            ISOCode = ISOCode.CheckInjection();
            return (WebLocale)(GetCachedLocales(context)[ISOCode.ToUpper()]);
        }

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All active locales.</returns>
        public static List<WebLocale> GetLocales(WebServiceContext context)
        {
            Hashtable localeTable;
            List<WebLocale> locales;
            WebLocale locale;

            locales = new List<WebLocale>();
            localeTable = GetCachedLocales(context);
            foreach (Object localeKey in localeTable.Keys)
            {
                // Each locale is stored twice in the cache
                // with different cache keys.
                // Filter on Int32 which is used for locale id.
                if (localeKey is Int32)
                {
                    locale = (WebLocale)(localeTable[localeKey]);
                    locales.Add(locale);
                }
            }
            return locales;
        }
    }
}
