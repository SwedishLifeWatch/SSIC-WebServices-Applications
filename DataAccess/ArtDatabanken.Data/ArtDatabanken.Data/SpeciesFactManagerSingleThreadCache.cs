using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of species fact information.
    /// </summary>
    public class SpeciesFactManagerSingleThreadCache : SpeciesFactManager
    {
        /// <summary>
        /// Create a SpeciesFactManagerSingleThreadCache instance.
        /// </summary>
        public SpeciesFactManagerSingleThreadCache()
        {
            SpeciesFactQualities = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Species fact qualities cache.
        /// </summary>
        protected Hashtable SpeciesFactQualities { get; private set; }

        /// <summary>
        /// Get species fact qualities for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>Species fact qualities for specified locale.</returns>
        protected virtual SpeciesFactQualityList GetSpeciesFactQualities(ILocale locale)
        {
            SpeciesFactQualityList speciesFactQualities = null;

            if (SpeciesFactQualities.ContainsKey(locale.ISOCode))
            {
                speciesFactQualities = (SpeciesFactQualityList)(SpeciesFactQualities[locale.ISOCode]);
            }

            return speciesFactQualities;
        }

        /// <summary>
        /// Get all species fact qualities.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All species fact qualities.</returns>
        public override SpeciesFactQualityList GetSpeciesFactQualities(IUserContext userContext)
        {
            SpeciesFactQualityList speciesFactQualities;

            speciesFactQualities = GetSpeciesFactQualities(userContext.Locale);
            if (speciesFactQualities.IsNull())
            {
                speciesFactQualities = base.GetSpeciesFactQualities(userContext);
                SetSpeciesFactQualities(speciesFactQualities, userContext.Locale);
            }

            return speciesFactQualities;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            SpeciesFactQualities.Clear();
        }

        /// <summary>
        /// Set species fact qualities for specified locale.
        /// </summary>
        /// <param name="speciesFactQualities">Species fact qualities.</param>
        /// <param name="locale">Currently used locale.</param>
        protected virtual void SetSpeciesFactQualities(SpeciesFactQualityList speciesFactQualities,
                                                       ILocale locale)
        {
            SpeciesFactQualities[locale.ISOCode] = speciesFactQualities;
        }
    }
}