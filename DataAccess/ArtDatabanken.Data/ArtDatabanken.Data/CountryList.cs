using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ICountry interface.
    /// </summary>
    [Serializable]
    public class CountryList : DataId32List<ICountry>
    {
        /// <summary>
        /// Get country with specified id.
        /// </summary>
        /// <param name='countryId'>Id of country.</param>
        /// <returns>Requested country.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public ICountry Get(CountryId countryId)
        {
            return Get((Int32)countryId);
        }
    }
}
