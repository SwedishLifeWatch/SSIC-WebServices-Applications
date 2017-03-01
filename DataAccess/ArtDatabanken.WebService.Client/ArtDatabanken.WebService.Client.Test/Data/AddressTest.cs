using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class AddressTest : TestBase
    {
        Address _address;

        public AddressTest()
        {
            _address = null;
        }

        [TestMethod]
        public void City()
        {
            String city;

            city = null;
            GetAddress(true).City = city;
            Assert.AreEqual(city, GetAddress().City);
            city = "";
            GetAddress().City = city;
            Assert.AreEqual(city, GetAddress().City);
            city = "Uppsala";
            GetAddress().City = city;
            Assert.AreEqual(city, GetAddress().City);
        }

        [TestMethod]
        public void Country()
        {
            ICountry country;

            country = null;
            GetAddress(true).Country = country;
            Assert.IsNotNull(GetAddress().Country);
            country = CoreData.CountryManager.GetCountries(GetUserContext())[0];
            GetAddress().Country = country;
            Assert.AreEqual(country, GetAddress().Country);
        }

        [TestMethod]
        public void Constructor()
        {
            Address address;

            address = new Address(GetUserContext());
            Assert.IsNotNull(address);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetAddress(true).DataContext);
        }

        private Address GetAddress()
        {
            return GetAddress(false);
        }

        private Address GetAddress(Boolean refresh)
        {
            if (_address.IsNull() || refresh)
            {
                _address = new Address(GetUserContext());
            }
            return _address;
        }

        public static Address GetOneAddress(IUserContext userContext)
        {
            return new Address(userContext);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetAddress(true).Id = id;
            Assert.AreEqual(id, GetAddress().Id);
        }

        [TestMethod]
        public void PostalAddress1()
        {
            String postalAddress1;

            postalAddress1 = null;
            GetAddress(true).PostalAddress1 = postalAddress1;
            Assert.AreEqual(postalAddress1, GetAddress().PostalAddress1);
            postalAddress1 = "  ";
            GetAddress().PostalAddress1 = postalAddress1;
            Assert.AreEqual(postalAddress1, GetAddress().PostalAddress1);
            postalAddress1 = "ArtDatabanken";
            GetAddress().PostalAddress1 = postalAddress1;
            Assert.AreEqual(postalAddress1, GetAddress().PostalAddress1);
        }

        [TestMethod]
        public void PostalAddress2()
        {
            String postalAddress2;

            postalAddress2 = null;
            GetAddress(true).PostalAddress2 = postalAddress2;
            Assert.AreEqual(postalAddress2, GetAddress().PostalAddress2);
            postalAddress2 = "  ";
            GetAddress().PostalAddress2 = postalAddress2;
            Assert.AreEqual(postalAddress2, GetAddress().PostalAddress2);
            postalAddress2 = "ArtDatabanken";
            GetAddress().PostalAddress2 = postalAddress2;
            Assert.AreEqual(postalAddress2, GetAddress().PostalAddress2);
        }

        [TestMethod]
        public void Type()
        {
            IAddressType type;

            type = null;
            GetAddress(true).Type = type;
            Assert.IsNotNull(GetAddress().Type);
            type = CoreData.UserManager.GetAddressTypes(GetUserContext())[0];
            GetAddress().Type = type;
            Assert.AreEqual(type, GetAddress().Type);
        }

        [TestMethod]
        public void ZipCode()
        {
            String zipCode;

            zipCode = null;
            GetAddress(true).ZipCode = zipCode;
            Assert.AreEqual(zipCode, GetAddress().ZipCode);
            zipCode = "  ";
            GetAddress().ZipCode = zipCode;
            Assert.AreEqual(zipCode, GetAddress().ZipCode);
            zipCode = "752 52";
            GetAddress().ZipCode = zipCode;
            Assert.AreEqual(zipCode, GetAddress().ZipCode);
        }
    }
}
