using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class AddressTypeListTest : TestBase
    {
        private AddressTypeList _addressTypes;

        public AddressTypeListTest()
        {
            _addressTypes = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AddressTypeList addressTypes;

            addressTypes = new AddressTypeList();
            Assert.IsNotNull(addressTypes);
        }

        [TestMethod]
        public void Get()
        {
            IAddressType addressType;

            GetAddressTypes(true);
            foreach (AddressTypeId addressTypeId in Enum.GetValues(typeof(AddressTypeId)))
            {
                addressType = GetAddressTypes().Get(addressTypeId);
                Assert.IsNotNull(addressType);
            }

            foreach (IAddressType tempAddressType in GetAddressTypes())
            {
                Assert.AreEqual(tempAddressType, GetAddressTypes().Get(tempAddressType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 addressTypeId;

            addressTypeId = Int32.MinValue;
            GetAddressTypes(true).Get(addressTypeId);
        }

        private AddressTypeList GetAddressTypes()
        {
            return GetAddressTypes(false);
        }

        private AddressTypeList GetAddressTypes(Boolean refresh)
        {
            if (_addressTypes.IsNull() || refresh)
            {
                _addressTypes = CoreData.UserManager.GetAddressTypes(GetUserContext());
            }
            return _addressTypes;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            AddressTypeList newAddressTypeList, oldAddressTypeList;
            Int32 addressTypeIndex;

            oldAddressTypeList = GetAddressTypes(true);
            newAddressTypeList = new AddressTypeList();
            for (addressTypeIndex = 0; addressTypeIndex < oldAddressTypeList.Count; addressTypeIndex++)
            {
                newAddressTypeList.Add(oldAddressTypeList[oldAddressTypeList.Count - addressTypeIndex - 1]);
            }
            for (addressTypeIndex = 0; addressTypeIndex < oldAddressTypeList.Count; addressTypeIndex++)
            {
                Assert.AreEqual(newAddressTypeList[addressTypeIndex], oldAddressTypeList[oldAddressTypeList.Count - addressTypeIndex - 1]);
            }
        }
    }
}
