using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class CountryManagerTest : TestBase
    {
        private CountryManager _countryManager;

        public CountryManagerTest()
        {
            _countryManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            CountryManager countryManager;

            countryManager = new CountryManager();
            Assert.IsNotNull(countryManager);
        }

        [TestMethod]
        public void DataSource()
        {
            ICountryDataSource dataSource;

            dataSource = null;
            GetCountryManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetCountryManager().DataSource);

            dataSource = new CountryDataSource();
            GetCountryManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetCountryManager().DataSource);
        }

        [TestMethod]
        public void GetCountry()
        {
            ICountry country;

            foreach (CountryId countryId in Enum.GetValues(typeof(CountryId)))
            {
                country = GetCountryManager().GetCountry(GetUserContext(), countryId);
                Assert.IsNotNull(country);
            }

            foreach (ICountry testCountry in GetCountryManager(true).GetCountries(GetUserContext()))
            {
                country = GetCountryManager().GetCountry(GetUserContext(), testCountry.Id);
                Assert.AreEqual(testCountry.Id, country.Id);
            }
        }

        private CountryManager GetCountryManager()
        {
            return GetCountryManager(false);
        }

        private CountryManager GetCountryManager(Boolean refresh)
        {
            if (_countryManager.IsNull() || refresh)
            {
                _countryManager = new CountryManager();
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

        [TestMethod]
        public void GetDataSourceInformation()
        {
            Assert.IsNotNull(GetCountryManager(true).GetDataSourceInformation());
        }
    }
}
