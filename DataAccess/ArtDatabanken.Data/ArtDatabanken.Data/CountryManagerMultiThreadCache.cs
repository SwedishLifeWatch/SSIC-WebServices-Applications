using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of country related information.
    /// </summary>
    public class CountryManagerMultiThreadCache : CountryManagerSingleThreadCache
    {
        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <returns>All countries.</returns>
        protected override CountryList GetCountries()
        {
            CountryList countries;

            lock (this)
            {
                countries = Countries;
            }
            return countries;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (this)
            {
                Countries = null;
            }
        }

        /// <summary>
        /// Set countries.
        /// </summary>
        /// <param name="countries">Countries.</param>
        protected override void SetCountries(CountryList countries)
        {
            lock (this)
            {
                Countries = countries;
            }
        }
    }
}
