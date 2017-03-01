using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class AddressTypeTest
    {
        AddressType _addressType;

        public AddressTypeTest()
        {
            _addressType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AddressType addressType;

            addressType = new AddressType(0,
                                          AddressTypeId.Home.ToString(),
                                          0,
                                          DataContextTest.GetOneDataContext());
            Assert.IsNotNull(addressType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDataContextNullError()
        {
            AddressType addressType;
            IDataContext dataContext;

            dataContext = null;
            addressType = new AddressType(0,
                                          AddressTypeId.Home.ToString(),
                                          0,
                                          dataContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameEmptyError()
        {
            AddressType addressType;

            addressType = new AddressType(0,
                                          " ",
                                          0,
                                          DataContextTest.GetOneDataContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNullError()
        {
            AddressType addressType;

            addressType = new AddressType(0,
                                          null,
                                          0,
                                          DataContextTest.GetOneDataContext());
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetAddressType(true).DataContext);
        }

        private AddressType GetAddressType()
        {
            return GetAddressType(false);
        }

        private AddressType GetAddressType(Boolean refresh)
        {
            if (_addressType.IsNull() || refresh)
            {
                _addressType = new AddressType(0,
                                               AddressTypeId.Home.ToString(),
                                               0,
                                               DataContextTest.GetOneDataContext());
            }
            return _addressType;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = GetAddressType(true).Id;
            Assert.AreEqual(id, GetAddressType().Id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = AddressTypeId.Billing.ToString();
            GetAddressType(true).Name = name;
            Assert.AreEqual(name, GetAddressType().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameEmptyError()
        {
            String name;

            name = String.Empty;
            GetAddressType(true).Name = name;
            Assert.AreEqual(name, GetAddressType().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameNullError()
        {
            String name;

            name = null;
            GetAddressType(true).Name = name;
            Assert.AreEqual(name, GetAddressType().Name);
        }

        [TestMethod]
        public void NameStringId()
        {
            Int32 nameStringId;

            nameStringId = GetAddressType(true).NameStringId;
            Assert.AreEqual(nameStringId, GetAddressType().NameStringId);
        }
    }
}
