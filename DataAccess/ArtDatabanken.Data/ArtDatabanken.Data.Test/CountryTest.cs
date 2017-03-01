using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class CountryTest
    {
        Country _country;

        public CountryTest()
        {
            _country = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Country country;

            country = new Country(Settings.Default.SwedishCountryId,
                                  Settings.Default.SwedishCountryISOCode,
                                  Settings.Default.SwedishCountryISOName,
                                  Settings.Default.SwedishCountryName,
                                  Settings.Default.SwedishCountryNativeName,
                                  Settings.Default.SwedishCountryPhoneNumberPrefix,
                                  DataContextTest.GetOneDataContext());
            Assert.IsNotNull(country);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDataContextNullError()
        {
            Country country;
            IDataContext dataContext;

            dataContext = null;
            country = new Country(Settings.Default.SwedishCountryId,
                                  Settings.Default.SwedishCountryISOCode,
                                  Settings.Default.SwedishCountryISOName,
                                  Settings.Default.SwedishCountryName,
                                  Settings.Default.SwedishCountryNativeName,
                                  Settings.Default.SwedishCountryPhoneNumberPrefix,
                                  dataContext);
            Assert.IsNotNull(country);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetCountry(true).DataContext);
        }

        private Country GetCountry()
        {
            return GetCountry(false);
        }

        private Country GetCountry(Boolean refresh)
        {
            if (_country.IsNull() || refresh)
            {
                _country = new Country(Settings.Default.SwedishCountryId,
                                       Settings.Default.SwedishCountryISOCode,
                                       Settings.Default.SwedishCountryISOName,
                                       Settings.Default.SwedishCountryName,
                                       Settings.Default.SwedishCountryNativeName,
                                       Settings.Default.SwedishCountryPhoneNumberPrefix,
                                       DataContextTest.GetOneDataContext());
            }
            return _country;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = GetCountry(true).Id;
            Assert.AreEqual(id, GetCountry().Id);
        }

        [TestMethod]
        public void ISOCode()
        {
            String ISOCode;

            ISOCode = GetCountry(true).ISOCode;
            Assert.AreEqual(ISOCode, GetCountry().ISOCode);
        }

        [TestMethod]
        public void ISOName()
        {
            String ISOName;

            ISOName = GetCountry(true).ISOName;
            Assert.AreEqual(ISOName, GetCountry().ISOName);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = GetCountry(true).Name;
            Assert.AreEqual(name, GetCountry().Name);
        }

        [TestMethod]
        public void NativeName()
        {
            String nativeName;

            nativeName = GetCountry(true).NativeName;
            Assert.AreEqual(nativeName, GetCountry().NativeName);
        }

        [TestMethod]
        public void PhoneNumberPrefix()
        {
            Int32 phoneNumberPrefix;

            phoneNumberPrefix = GetCountry(true).PhoneNumberPrefix;
            Assert.AreEqual(phoneNumberPrefix, GetCountry().PhoneNumberPrefix);
        }
    }
}
