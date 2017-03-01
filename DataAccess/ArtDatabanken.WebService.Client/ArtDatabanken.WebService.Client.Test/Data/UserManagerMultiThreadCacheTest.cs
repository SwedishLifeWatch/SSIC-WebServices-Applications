using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class UserManagerMultiThreadCacheTest : TestBase
    {
        private UserManagerMultiThreadCache _userManager;

        public UserManagerMultiThreadCacheTest()
        {
            _userManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserManagerMultiThreadCache userManager;

            userManager = new UserManagerMultiThreadCache();
            Assert.IsNotNull(userManager);
        }

        [TestMethod]
        public void GetAddressTypes()
        {
            AddressTypeList addressTypes;

            addressTypes = GetUserManager(true).GetAddressTypes(GetUserContext());
            Assert.IsTrue(addressTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetMessageTypes()
        {
            MessageTypeList MessageTypes;

            MessageTypes = GetUserManager(true).GetMessageTypes(GetUserContext());
            Assert.IsTrue(MessageTypes.IsNotEmpty());
        }

        private UserManagerMultiThreadCache GetUserManager()
        {
            return GetUserManager(false);
        }

        private UserManagerMultiThreadCache GetUserManager(Boolean refresh)
        {
            if (_userManager.IsNull() || refresh)
            {
                _userManager = new UserManagerMultiThreadCache();
                _userManager.DataSource = new UserDataSource();
            }
            return _userManager;
        }
    }
}
