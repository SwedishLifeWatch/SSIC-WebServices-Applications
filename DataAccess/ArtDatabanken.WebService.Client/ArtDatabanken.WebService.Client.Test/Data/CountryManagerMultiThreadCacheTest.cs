using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class CountryManagerMultiThreadCacheTest : TestBase
    {
        private CountryManagerMultiThreadCache _countryManager;

        public CountryManagerMultiThreadCacheTest()
        {
            _countryManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            CountryManagerMultiThreadCache countryManager;

            countryManager = new CountryManagerMultiThreadCache();
            Assert.IsNotNull(countryManager);
        }

        private CountryManagerMultiThreadCache GetCountryManager()
        {
            return GetCountryManager(false);
        }

        private CountryManagerMultiThreadCache GetCountryManager(Boolean refresh)
        {
            if (_countryManager.IsNull() || refresh)
            {
                _countryManager = new CountryManagerMultiThreadCache();
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
