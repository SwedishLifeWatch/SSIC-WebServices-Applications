using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  Class that handles cache of species information related information.
    /// </summary>
    public class SpeciesObservationManagerSingleThreadCache : SpeciesObservationManager
    {
        /// <summary>
        /// Create a SpeciesObservationManagerSingleThreadCache instance.
        /// </summary>
        public SpeciesObservationManagerSingleThreadCache()
        {
            BirdNestActivities = new Hashtable();
            SpeciesActivities = new Hashtable();
            SpeciesActivityCategories = new Hashtable();
            SpeciesObservationDataProviders = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Bird nest activity cache.
        /// </summary>
        protected Hashtable BirdNestActivities
        { get; private set; }

        /// <summary>
        /// Species activity cache.
        /// </summary>
        protected Hashtable SpeciesActivities
        { get; private set; }

        /// <summary>
        /// Species activity category cache.
        /// </summary>
        protected Hashtable SpeciesActivityCategories
        { get; private set; }

        /// <summary>
        /// Species observation cache.
        /// </summary>
        protected Hashtable SpeciesObservationDataProviders 
        { get; private set; }

        /// <summary>
        /// Get all bird nest activities for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All bird nest activities for specified locale.</returns>
        protected virtual SpeciesActivityList GetBirdNestActivities(ILocale locale)
        {
            SpeciesActivityList birdNestActivities = null;

            if (BirdNestActivities.ContainsKey(locale.ISOCode))
            {
                birdNestActivities = (SpeciesActivityList)(BirdNestActivities[locale.ISOCode]);
            }
            return birdNestActivities;
        }

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All bird nest activities.</returns>
        public override SpeciesActivityList GetBirdNestActivities(IUserContext userContext)
        {
            SpeciesActivityList birdNestActivities;

            birdNestActivities = GetBirdNestActivities(userContext.Locale);
            if (birdNestActivities.IsNull())
            {
                birdNestActivities = base.GetBirdNestActivities(userContext);
                SetBirdNestActivities(birdNestActivities, userContext.Locale);
            }
            return birdNestActivities;
        }

        /// <summary>
        /// Get all species activities for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All species activities for specified locale.</returns>
        protected virtual SpeciesActivityList GetSpeciesActivities(ILocale locale)
        {
            SpeciesActivityList speciesActivities = null;

            if (SpeciesActivities.ContainsKey(locale.ISOCode))
            {
                speciesActivities = (SpeciesActivityList)(SpeciesActivities[locale.ISOCode]);
            }
            return speciesActivities;
        }

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All species activities.</returns>
        public override SpeciesActivityList GetSpeciesActivities(IUserContext userContext)
        {
            SpeciesActivityList speciesActivities;

            speciesActivities = GetSpeciesActivities(userContext.Locale);
            if (speciesActivities.IsNull())
            {
                speciesActivities = base.GetSpeciesActivities(userContext);
                SetSpeciesActivities(speciesActivities, userContext.Locale);
            }
            return speciesActivities;
        }

        /// <summary>
        /// Get species activity categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Species activity categories for specified locale.</returns>
        protected virtual SpeciesActivityCategoryList GetSpeciesActivityCategories(ILocale locale)
        {
            SpeciesActivityCategoryList speciesActivityCategories = null;

            if (SpeciesActivityCategories.ContainsKey(locale.ISOCode))
            {
                speciesActivityCategories = (SpeciesActivityCategoryList)(SpeciesActivityCategories[locale.ISOCode]);
            }
            return speciesActivityCategories;
        }

        /// <summary>
        /// Get all speceis activity categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All species activity categories.</returns>
        public override SpeciesActivityCategoryList GetSpeciesActivityCategories(IUserContext userContext)
        {
            SpeciesActivityCategoryList speciesActivityCategories;

            speciesActivityCategories = GetSpeciesActivityCategories(userContext.Locale);
            if (speciesActivityCategories.IsNull())
            {
                speciesActivityCategories = base.GetSpeciesActivityCategories(userContext);
                SetSpeciesActivityCategories(speciesActivityCategories, userContext.Locale);
            }
            return speciesActivityCategories;
        }

        /// <summary>
        /// Get data providers for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Data providers for specified locale.</returns>
        protected virtual SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(ILocale locale)
        {
            SpeciesObservationDataProviderList speciesObservationDataProviders = null;

            if (SpeciesObservationDataProviders.ContainsKey(locale.ISOCode))
            {
                speciesObservationDataProviders = (SpeciesObservationDataProviderList)(SpeciesObservationDataProviders[locale.ISOCode]);
            }
            return speciesObservationDataProviders;
        }

        /// <summary>
        /// Get all data providers.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All lump split event types.</returns>
        public override SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(IUserContext userContext)
        {
            SpeciesObservationDataProviderList speciesObservationDataProviders;

            speciesObservationDataProviders = GetSpeciesObservationDataProviders(userContext.Locale);
            if (speciesObservationDataProviders.IsNull())
            {
                speciesObservationDataProviders = base.GetSpeciesObservationDataProviders(userContext);
                SetSpeciesObservationDataProviders(speciesObservationDataProviders, userContext.Locale);
            }
            return speciesObservationDataProviders;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            BirdNestActivities.Clear();
            SpeciesActivities.Clear();
            SpeciesActivityCategories.Clear();
            SpeciesObservationDataProviders.Clear();   
        }

        /// <summary>
        /// Set bird nest activities for specified locale.
        /// </summary>
        /// <param name="birdNestActivities">Bird nest activities.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetBirdNestActivities(SpeciesActivityList birdNestActivities,
                                                     ILocale locale)
        {
            BirdNestActivities[locale.ISOCode] = birdNestActivities;
        }

        /// <summary>
        /// Set species activities for specified locale.
        /// </summary>
        /// <param name="speciesActivities">Species activities.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetSpeciesActivities(SpeciesActivityList speciesActivities,
                                                    ILocale locale)
        {
            SpeciesActivities[locale.ISOCode] = speciesActivities;
        }

        /// <summary>
        /// Set species activity categories for specified locale.
        /// </summary>
        /// <param name="speciesActivityCategories">Species activity categories.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetSpeciesActivityCategories(SpeciesActivityCategoryList speciesActivityCategories,
                                                            ILocale locale)
        {
            SpeciesActivityCategories[locale.ISOCode] = speciesActivityCategories;
        }

        /// <summary>
        /// Set species observation data providers for specified locale.
        /// </summary>
        /// <param name="speciesObservationDataProviders">Species observation data providers.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetSpeciesObservationDataProviders(SpeciesObservationDataProviderList speciesObservationDataProviders,
                                                                  ILocale locale)
        {
            SpeciesObservationDataProviders[locale.ISOCode] = speciesObservationDataProviders;
        }

        /// <summary>
        /// Resets the species observation data providers cache.
        /// </summary>
        public virtual void RefreshSpeciesObservationDataProviders()
        {
            SpeciesObservationDataProviders.Clear();
        }
    }
}
