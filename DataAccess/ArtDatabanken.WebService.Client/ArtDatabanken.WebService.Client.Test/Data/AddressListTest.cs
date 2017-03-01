using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class AddressListTest : TestBase
    {
        private AddressList _addresses;

        public AddressListTest()
        {
            _addresses = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AddressList address;

            address = new AddressList();
            Assert.IsNotNull(address);
        }

        [TestMethod]
        public void Get()
        {
            GetAddresses(true);
            foreach (IAddress address in GetAddresses())
            {
                Assert.AreEqual(address, GetAddresses().Get(address.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 addressId;

            addressId = Int32.MaxValue;
            GetAddresses(true).Get(addressId);
        }

        private AddressList GetAddresses()
        {
            return GetAddresses(false);
        }

        private AddressList GetAddresses(Boolean refresh)
        {
            if (_addresses.IsNull() || refresh)
            {
                _addresses = new AddressList();
                _addresses.Add(AddressTest.GetOneAddress(GetUserContext()));
            }
            return _addresses;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            AddressList newAddressList, oldAddressList;
            Int32 addressIndex;

            oldAddressList = GetAddresses(true);
            newAddressList = new AddressList();
            for (addressIndex = 0; addressIndex < oldAddressList.Count; addressIndex++)
            {
                newAddressList.Add(oldAddressList[oldAddressList.Count - addressIndex - 1]);
            }
            for (addressIndex = 0; addressIndex < oldAddressList.Count; addressIndex++)
            {
                Assert.AreEqual(newAddressList[addressIndex], oldAddressList[oldAddressList.Count - addressIndex - 1]);
            }
        }
    }
}
