using System;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of the CountryDataSource interface.
    /// This interface is used to retrieve country related information.
    /// </summary>
    public interface ICountryDataSource : IDataSource
    {
        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All countries.</returns>
        CountryList GetCountries(IUserContext userContext);
    }
}
