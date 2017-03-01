using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebCountry
    /// </summary>
    [TestClass]
    public class WebCountryTest
    {
        private WebCountry _country;

        public WebCountryTest()
        {
            _country = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebCountry country;

            country = new WebCountry();
            Assert.IsNotNull(country);
        }

        private WebCountry GetCountry()
        {
            return GetCountry(false);
        }

        private WebCountry GetCountry(Boolean refresh)
        {
            if (_country.IsNull() || refresh)
            {
                _country = new WebCountry();
            }
            return _country;
        }


        

        [TestMethod]
        public void Id()
        {
            WebCountry country = GetCountry();
            country.Id = 1;
            Assert.AreEqual(country.Id, 1);
        }

        [TestMethod]
        public void Name()
        {
            WebCountry country = GetCountry();
            country.Name = "Namn";
            Assert.AreEqual(country.Name, "Namn");
        }

        [TestMethod]
        public void NativeName()
        {
            WebCountry country = GetCountry();
            country.NativeName = "Namn";
            Assert.AreEqual(country.NativeName, "Namn");
        }

        [TestMethod]
        public void ISOName()
        {
            WebCountry country = GetCountry();
            country.ISOName = "Namn";
            Assert.AreEqual(country.ISOName, "Namn");
        }

        [TestMethod]
        public void ISOCode()
        {
            WebCountry country = GetCountry();
            country.ISOAlpha2Code = "Namn";
            Assert.AreEqual(country.ISOAlpha2Code, "Namn");
        }

        [TestMethod]
        public void PhoneNumberPrefix()
        {
            WebCountry country = GetCountry();
            country.PhoneNumberPrefix = 46;
            Assert.AreEqual(country.PhoneNumberPrefix, 46);
        }


        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        #endregion


    }
}
