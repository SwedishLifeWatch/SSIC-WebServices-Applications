using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebCountry class.
    /// </summary>
    public static class WebCountryExtension
    {
        private const String DEFAULT_NAME = "MISSING_NAME";
        private const Int32 _int32Default = Int32.MinValue;

        /// <summary>
        /// Load data into the WebCountry instance.
        /// </summary>
        /// <param name="country">This country.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebCountry country,
                                    DataReader dataReader)
        {
            country.Id = dataReader.GetInt32(CountryData.ID);
            country.Name = dataReader.GetString(CountryData.NAME);
            country.NativeName = dataReader.GetString(CountryData.NATIVE_NAME, DEFAULT_NAME);
            country.ISOName = dataReader.GetString(CountryData.ISO_NAME);
            country.ISOAlpha2Code = dataReader.GetString(CountryData.ISO_CODE);
            country.PhoneNumberPrefix = dataReader.GetInt32(CountryData.PHONE_NUMBER_PREFIX, _int32Default);
        }
    }
}
