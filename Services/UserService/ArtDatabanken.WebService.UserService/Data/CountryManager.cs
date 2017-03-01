using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Manager of country related information.
    /// </summary>
    public class CountryManager
    {
        /// <summary>
        /// Get cached countries.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Cached countries.</returns>
        private static Hashtable GetCachedCountries(WebServiceContext context)
        {
            Hashtable countries;
            WebCountry country;

            // Get cached information.
            countries = (Hashtable)context.GetCachedObject(Settings.Default.CountryCacheKey);

            // Data not in cache - store it in the cache.
            if (countries.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetCountries())
                {
                    countries = new Hashtable();
                    while (dataReader.Read())
                    {
                        country = new WebCountry();
                        country.LoadData(dataReader);

                        // Add object to Hashtable.
                        countries.Add(country.Id, country);
                    }
                    // Add information to cache.
                    context.AddCachedObject(Settings.Default.CountryCacheKey,
                                            countries,
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.High);
                }
            }
            return countries;
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All countries.</returns>
        public static List<WebCountry> GetCountries(WebServiceContext context)
        {
            Hashtable countryTable;
            List<WebCountry> countries;

            countries = new List<WebCountry>();
            countryTable = GetCachedCountries(context);
            foreach (WebCountry country in countryTable.Values)
            {
                countries.Add(country);
            }
            return countries;
        }

        /// <summary>
        /// Get country with specified id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="countryId">Requested country id</param>
        /// <returns>Country with specified id.</returns>
        public static WebCountry GetCountry(WebServiceContext context,
                                            Int32 countryId)
        {
            WebCountry webCountry = (WebCountry)(GetCachedCountries(context)[countryId]);
            if (webCountry.IsNull())
            {
                throw new ArgumentException("Country not found. CountryId = " + countryId);
            }
            return webCountry;
        }
    }
}
