using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client.Test.UserService
{
    [TestClass]
    public class CountryDataSourceTest : TestBase
    {
        private CountryDataSource _countryDataSource;

        public CountryDataSourceTest()
        {
            _countryDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            CountryDataSource countryDataSource;

            countryDataSource = new CountryDataSource();
            Assert.IsNotNull(countryDataSource);
        }

        [TestMethod]
        public void GetCountries()
        {
            CountryList countries;

            countries = GetCountryDataSource(true).GetCountries(GetUserContext());
            Assert.IsTrue(countries.IsNotEmpty());
        }

        private CountryDataSource GetCountryDataSource()
        {
            return GetCountryDataSource(false);
        }

        private CountryDataSource GetCountryDataSource(Boolean refresh)
        {
            if (_countryDataSource.IsNull() || refresh)
            {
                _countryDataSource = new CountryDataSource();
            }
            return _countryDataSource;
        }
    }
}
