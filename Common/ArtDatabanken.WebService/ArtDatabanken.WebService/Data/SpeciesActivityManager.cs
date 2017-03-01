using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains handling of species activity objects.
    /// </summary>
    public class SpeciesActivityManager : ManagerBase, ISpeciesActivityManager
    {
        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All bird nest activities.</returns>
        public virtual List<WebSpeciesActivity> GetBirdNestActivities(WebServiceContext context)
        {
            List<WebSpeciesActivity> birdNestActivities;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.BirdNestActivityCacheKey + context.Locale.ISOCode;
            birdNestActivities = (List<WebSpeciesActivity>)(context.GetCachedObject(cacheKey));

            // Data not in cache - store it in the cache.
            if (birdNestActivities.IsNull())
            {
                birdNestActivities = WebServiceProxy.SwedishSpeciesObservationService.GetBirdNestActivities(GetClientInformation(context, WebServiceId.SwedishSpeciesObservationService));

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        birdNestActivities,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return birdNestActivities;
        }

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species activities.</returns>
        public virtual List<WebSpeciesActivity> GetSpeciesActivities(WebServiceContext context)
        {
            List<WebSpeciesActivity> speciesActivities;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesActivityCacheKey + context.Locale.ISOCode;
            speciesActivities = (List<WebSpeciesActivity>)(context.GetCachedObject(cacheKey));

            // Data not in cache - store it in the cache.
            if (speciesActivities.IsNull())
            {
                speciesActivities = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivities(GetClientInformation(context, WebServiceId.SwedishSpeciesObservationService));

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        speciesActivities,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return speciesActivities;
        }
    }
}
