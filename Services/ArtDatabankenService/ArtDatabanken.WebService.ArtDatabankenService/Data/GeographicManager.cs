using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of geographic information.
    /// </summary>
    public class GeographicManager
    {
        /// <summary>
        /// Get information about cities that matches the search string.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchString">String that city name must match.</param>
        /// <returns>Information about cities.</returns>
        public static List<WebCity> GetCitiesBySearchString(WebServiceContext context,
                                                            String searchString)
        {
            List<WebCity> cities;

            // Check arguments.
            searchString.CheckNotEmpty("searchString");
            searchString = searchString.CheckSqlInjection();

            // Get information from database.
            cities = new List<WebCity>();
            using (DataReader dataReader = DataServer.GetCitiesBySearchString(context, searchString))
            {
                while (dataReader.Read())
                {
                    cities.Add(new WebCity(dataReader));
                }
            }
            return cities;
        }

        /// <summary>
        /// Get information about swedish counties.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about swedish counties.</returns>
        public static List<WebCounty> GetCounties(WebServiceContext context)
        {
            List<WebCounty> counties;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllCounties";
            counties = (List<WebCounty>)context.GetCachedObject(cacheKey);

            if (counties.IsNull())
            {
                // Get information from database.
                counties = new List<WebCounty>();
                using (DataReader dataReader = DataServer.GetCounties(context))
                {
                    while (dataReader.Read())
                    {
                        counties.Add(new WebCounty(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, counties, DateTime.Now + new TimeSpan(24, 0, 0), CacheItemPriority.BelowNormal);
                }
            }
            return counties;
        }

        /// <summary>
        /// Get information about swedish provinces.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about swedish provinces.</returns>
        public static List<WebProvince> GetProvinces(WebServiceContext context)
        {
            List<WebProvince> provinces;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllProvinces";
            provinces = (List<WebProvince>)context.GetCachedObject(cacheKey);

            if (provinces.IsNull())
            {
                // Get information from database.
                provinces = new List<WebProvince>();
                using (DataReader dataReader = DataServer.GetProvinces(context))
                {
                    while (dataReader.Read())
                    {
                        provinces.Add(new WebProvince(dataReader));
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, provinces, DateTime.Now + new TimeSpan(24, 0, 0), CacheItemPriority.BelowNormal);
            }
            return provinces;
        }
    }
}
