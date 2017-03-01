using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class CountryManagerSingleThreadCacheTest : TestBase
    {
        private CountryManagerSingleThreadCache _countryManager;

        public CountryManagerSingleThreadCacheTest()
        {
            _countryManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            CountryManagerSingleThreadCache countryManager;

            countryManager = new CountryManagerSingleThreadCache();
            Assert.IsNotNull(countryManager);
        }

        private CountryManagerSingleThreadCache GetCountryManager()
        {
            return GetCountryManager(false);
        }

        private CountryManagerSingleThreadCache GetCountryManager(Boolean refresh)
        {
            if (_countryManager.IsNull() || refresh)
            {
                _countryManager = new CountryManagerSingleThreadCache();
                _countryManager.DataSource = new CountryDataSource();
            }
            return _countryManager;
        }

        [TestMethod]
        public void GetCountries()
        {
            CountryList countries;

            countries = GetCountryManager(true).GetCountries(GetUserContext());
            Assert.IsTrue(countries.IsNotEmpty());
        }
    }
}
