using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class CountryManagerTest : TestBase
    {
        public CountryManagerTest()
        {
        }

        [TestMethod]
        public void GetCountries()
        {
            List<WebCountry> countries;

            countries = CountryManager.GetCountries(GetContext());
            Assert.IsTrue(countries.IsNotEmpty());
        }

        [TestMethod]
        public void GetCountry()
        {
            WebCountry testCountry;

            foreach (WebCountry country in CountryManager.GetCountries(GetContext()))
            {
                testCountry = CountryManager.GetCountry(GetContext(), country.Id);
                Assert.AreEqual(testCountry.Id, country.Id);
            }
        }
    }
}
