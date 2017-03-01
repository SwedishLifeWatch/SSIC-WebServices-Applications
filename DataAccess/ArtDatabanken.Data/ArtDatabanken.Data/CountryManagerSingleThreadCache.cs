using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of country related information.
    /// </summary>
    public class CountryManagerSingleThreadCache : CountryManager
    {
        /// <summary>
        /// Create a CountryManagerSingleThreadCache instance.
        /// </summary>
        public CountryManagerSingleThreadCache()
        {
            Countries = null;
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Countries cache.
        /// </summary>
        protected CountryList Countries
        { get; set; }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All countries.</returns>
        public override CountryList GetCountries(IUserContext userContext)
        {
            CountryList countries;

            countries = GetCountries();
            if (countries.IsNull())
            {
                countries = base.GetCountries(userContext);
                SetCountries(countries);
            }
            return countries;
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <returns>All countries.</returns>
        protected virtual CountryList GetCountries()
        {
            return Countries;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            Countries = null;
        }

        /// <summary>
        /// Set countries.
        /// </summary>
        /// <param name="countries">Countries.</param>
        protected virtual void SetCountries(CountryList countries)
        {
            Countries = countries;
        }
    }
}
