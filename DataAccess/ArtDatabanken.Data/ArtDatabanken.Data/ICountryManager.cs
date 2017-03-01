using System;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the CountryManager interface.
    /// </summary>
    public interface ICountryManager : IManager
    {
        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        ICountryDataSource DataSource
        { get; set; }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All countries.</returns>
        CountryList GetCountries(IUserContext userContext);

        /// <summary>
        /// Get country with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="countryId">Requested country id</param>
        /// <returns>Country with specified id.</returns>
        ICountry GetCountry(IUserContext userContext,
                            CountryId countryId);

        /// <summary>
        /// Get country with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="countryId">Requested country id</param>
        /// <returns>Country with specified id.</returns>
        ICountry GetCountry(IUserContext userContext, Int32 countryId);
    }
}
