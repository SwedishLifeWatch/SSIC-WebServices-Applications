using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class CountryListTest : TestBase
    {
        private CountryList _countries;

        public CountryListTest()
        {
            _countries = null;
        }

        [TestMethod]
        public void Constructor()
        {
            CountryList countries;

            countries = new CountryList();
            Assert.IsNotNull(countries);
        }

        [TestMethod]
        public void Get()
        {
            ICountry country;

            GetCountries(true);
            foreach (CountryId countryId in Enum.GetValues(typeof(CountryId)))
            {
                country = GetCountries().Get(countryId);
                Assert.IsNotNull(country);
            }

            foreach (ICountry tempCountry in GetCountries(true))
            {
                Assert.AreEqual(tempCountry, GetCountries().Get(tempCountry.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 countryId;

            countryId = Int32.MinValue;
            GetCountries(true).Get(countryId);
        }

        private CountryList GetCountries()
        {
            return GetCountries(false);
        }

        private CountryList GetCountries(Boolean refresh)
        {
            if (_countries.IsNull() || refresh)
            {
                _countries = CoreData.CountryManager.GetCountries(GetUserContext());
            }
            return _countries;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            CountryList newCountryList, oldCountryList;
            Int32 countryIndex;

            oldCountryList = GetCountries(true);
            newCountryList = new CountryList();
            for (countryIndex = 0; countryIndex < oldCountryList.Count; countryIndex++)
            {
                newCountryList.Add(oldCountryList[oldCountryList.Count - countryIndex - 1]);
            }
            for (countryIndex = 0; countryIndex < oldCountryList.Count; countryIndex++)
            {
                Assert.AreEqual(newCountryList[countryIndex], oldCountryList[oldCountryList.Count - countryIndex - 1]);
            }
        }
    }
}
