
namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of species information related information.
    /// </summary>
    public class SpeciesObservationManagerMultiThreadCache : SpeciesObservationManagerSingleThreadCache
    {
        /// <summary>
        /// Get all bird nest activities for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All bird nest activities for specified locale.</returns>
        protected override SpeciesActivityList GetBirdNestActivities(ILocale locale)
        {
            SpeciesActivityList birdNestActivities = null;

            lock (BirdNestActivities)
            {
                if (BirdNestActivities.ContainsKey(locale.ISOCode))
                {
                    birdNestActivities = (SpeciesActivityList)(BirdNestActivities[locale.ISOCode]);
                }
            }
            return birdNestActivities;
        }

        /// <summary>
        /// Get all species activities for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All species activities for specified locale.</returns>
        protected override SpeciesActivityList GetSpeciesActivities(ILocale locale)
        {
            SpeciesActivityList speciesActivities = null;

            lock (SpeciesActivities)
            {
                if (SpeciesActivities.ContainsKey(locale.ISOCode))
                {
                    speciesActivities = (SpeciesActivityList)(SpeciesActivities[locale.ISOCode]);
                }
            }
            return speciesActivities;
        }

        /// <summary>
        /// Get species activity categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Species activity categories for specified locale.</returns>
        protected override SpeciesActivityCategoryList GetSpeciesActivityCategories(ILocale locale)
        {
            SpeciesActivityCategoryList speciesActivityCategories = null;

            lock (SpeciesActivityCategories)
            {
                if (SpeciesActivityCategories.ContainsKey(locale.ISOCode))
                {
                    speciesActivityCategories =
                        (SpeciesActivityCategoryList)(SpeciesActivityCategories[locale.ISOCode]);
                }
            }
            return speciesActivityCategories;
        }

        /// <summary>
        /// Get all species observation data providers for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All species observation data providers for specified locale.</returns>
        protected override SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(ILocale locale)
        {
            SpeciesObservationDataProviderList speciesObservationDataProviderList = null;

            lock (SpeciesObservationDataProviders)
            {
                if (SpeciesObservationDataProviders.ContainsKey(locale.ISOCode))
                {
                    speciesObservationDataProviderList = (SpeciesObservationDataProviderList)(SpeciesObservationDataProviders[locale.ISOCode]);
                }
            }
            return speciesObservationDataProviderList;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (BirdNestActivities)
            {
                BirdNestActivities.Clear();
            }
            lock (SpeciesActivities)
            {
                SpeciesActivities.Clear();
            }
            lock (SpeciesActivityCategories)
            {
                SpeciesActivityCategories.Clear();
            }
            lock (SpeciesObservationDataProviders)
            {
                SpeciesObservationDataProviders.Clear();
            }
        }

        /// <summary>
        /// Resets the species observation data providers cache.
        /// </summary>
        public override void RefreshSpeciesObservationDataProviders()
        {
            lock (SpeciesObservationDataProviders)
            {
                SpeciesObservationDataProviders.Clear();
            }
        }

        /// <summary>
        /// Set bird nest activities for specified locale.
        /// </summary>
        /// <param name="birdNestActivities">Bird nest activities.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetBirdNestActivities(SpeciesActivityList birdNestActivities,
                                                      ILocale locale)
        {
            lock (BirdNestActivities)
            {
                BirdNestActivities[locale.ISOCode] = birdNestActivities;
            }
        }

        /// <summary>
        /// Set species activities for specified locale.
        /// </summary>
        /// <param name="speciesActivities">Species activities.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetSpeciesActivities(SpeciesActivityList speciesActivities,
                                                     ILocale locale)
        {
            lock (SpeciesActivities)
            {
                SpeciesActivities[locale.ISOCode] = speciesActivities;
            }
        }

        /// <summary>
        /// Set species activity categories for specified locale.
        /// </summary>
        /// <param name="speciesActivityCategories">Species activity categories.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetSpeciesActivityCategories(SpeciesActivityCategoryList speciesActivityCategories,
                                                             ILocale locale)
        {
            lock (SpeciesActivityCategories)
            {
                SpeciesActivityCategories[locale.ISOCode] = speciesActivityCategories;
            }
        }

        /// <summary>
        /// Set species observation data providers for specified locale.
        /// </summary>
        /// <param name="speciesObservationDataProviders">Species observation data providers.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetSpeciesObservationDataProviders(SpeciesObservationDataProviderList speciesObservationDataProviders,
                                                                   ILocale locale)
        {
            lock (SpeciesObservationDataProviders)
            {
                SpeciesObservationDataProviders[locale.ISOCode] = speciesObservationDataProviders;
            }
        }
    }
}
