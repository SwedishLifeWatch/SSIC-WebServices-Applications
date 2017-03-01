using System;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles country related information.
    /// </summary>
    public class CountryManager : ICountryManager
    {
        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        public ICountryDataSource DataSource
        { get; set; }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All countries.</returns>
        public virtual CountryList GetCountries(IUserContext userContext)
        {
            return DataSource.GetCountries(userContext);
        }

        /// <summary>
        /// Get country with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="countryId">Requested country id</param>
        /// <returns>Country with specified id.</returns>
        public virtual ICountry GetCountry(IUserContext userContext,
                                           CountryId countryId)
        {
            return GetCountries(userContext).Get(countryId);
        }

        /// <summary>
        /// Get country with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="countryId">Requested country id</param>
        /// <returns>Country with specified id.</returns>
        public virtual ICountry GetCountry(IUserContext userContext,
                                           Int32 countryId)
        {
            return GetCountries(userContext).Get(countryId);
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }
    }
}
