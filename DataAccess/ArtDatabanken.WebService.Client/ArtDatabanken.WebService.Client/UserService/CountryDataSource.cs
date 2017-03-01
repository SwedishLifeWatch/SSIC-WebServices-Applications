using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.UserService
{
    /// <summary>
    /// This class is used to retrieve country related information.
    /// </summary>
    public class CountryDataSource : UserDataSourceBase, ICountryDataSource
    {
        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All countries.</returns>
        public CountryList GetCountries(IUserContext userContext)
        {
            List<WebCountry> webCountries;

            CheckTransaction(userContext);
            webCountries = WebServiceProxy.UserService.GetCountries(GetClientInformation(userContext));
            return GetCountries(userContext, webCountries);
        }

        /// <summary>
        /// Get countries from web countries.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webCountries">Web countries.</param>
        /// <returns>Countries.</returns>
        private CountryList GetCountries(IUserContext userContext,
                                         List<WebCountry> webCountries)
        {
            CountryList countries;

            countries = new CountryList();
            foreach (WebCountry webCountry in webCountries)
            {
                countries.Add(GetCountry(userContext, webCountry));
            }
            return countries;
        }
    }
}
