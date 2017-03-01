using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// This class contains handling of species activity information.
    /// </summary>
    public static class SpeciesActivityManager
    {
        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All bird nest activities.</returns>
        public static List<WebSpeciesActivity> GetBirdNestActivities(WebServiceContext context)
        {
            List<WebSpeciesActivity> birdNestActivities, speciesActivities;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.BirdNestActivitiesCacheKey + context.Locale.ISOCode;
            birdNestActivities = (List<WebSpeciesActivity>)(context.GetCachedObject(cacheKey));

            if (birdNestActivities.IsEmpty())
            {
                // Data not in cache.
                speciesActivities = GetSpeciesActivities(context);
                birdNestActivities = new List<WebSpeciesActivity>();

                foreach (WebSpeciesActivity speciesActivity in speciesActivities)
                {
                    if ((Settings.Default.BirdNestActivityIdMin <= speciesActivity.Id) &&
                        (speciesActivity.Id <= Settings.Default.BirdNestActivityIdMax))
                    {
                        birdNestActivities.Add(speciesActivity);
                    }
                }

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
        public static List<WebSpeciesActivity> GetSpeciesActivities(WebServiceContext context)
        {
            List<WebSpeciesActivity> speciesActivities;
            String cacheKey;
            WebSpeciesActivity speciesActivity;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesActivitiesCacheKey + ":" + context.Locale.ISOCode;
            speciesActivities = (List<WebSpeciesActivity>)(context.GetCachedObject(cacheKey));

            if (speciesActivities.IsEmpty())
            {
                // Data not in cache. Get information from database.
                speciesActivities = new List<WebSpeciesActivity>();

                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesActivities(context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        speciesActivity = new WebSpeciesActivity();
                        speciesActivity.LoadData(dataReader);
                        speciesActivities.Add(speciesActivity);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        speciesActivities,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return speciesActivities;
        }

        /// <summary>
        /// Get all species activity categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species activity categories.</returns>
        public static List<WebSpeciesActivityCategory> GetSpeciesActivityCategories(WebServiceContext context)
        {
            String cacheKey;
            List<WebSpeciesActivityCategory> speciesActivityCategories;
            WebSpeciesActivityCategory speciesActivityCategory;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesActivityCategoriesCacheKey + ":" + context.Locale.ISOCode;
            speciesActivityCategories = (List<WebSpeciesActivityCategory>)(context.GetCachedObject(cacheKey));

            if (speciesActivityCategories.IsEmpty())
            {
                // Data not in cache. Get information from database.
                speciesActivityCategories = new List<WebSpeciesActivityCategory>();

                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesActivityCategories(context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        speciesActivityCategory = new WebSpeciesActivityCategory();
                        speciesActivityCategory.LoadData(dataReader);
                        speciesActivityCategories.Add(speciesActivityCategory);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        speciesActivityCategories,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return speciesActivityCategories;
        }
    }
}
