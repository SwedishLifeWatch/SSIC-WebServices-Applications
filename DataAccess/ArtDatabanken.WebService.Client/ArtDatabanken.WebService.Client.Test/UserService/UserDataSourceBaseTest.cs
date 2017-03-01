using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client.Test.UserService
{
    [TestClass]
    public class UserDataSourceBaseTest
    {
        private UserDataSourceBase _userDataSourceBase;

        public UserDataSourceBaseTest()
        {
            _userDataSourceBase = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserDataSourceBase userDataSourceBase;

            userDataSourceBase = new UserDataSourceBase();
            Assert.IsNotNull(userDataSourceBase);
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            IDataSourceInformation dataSource;

            dataSource = GetUserDataSourceBase(true).GetDataSourceInformation();
            Assert.IsNotNull(dataSource);
        }

        private UserDataSourceBase GetUserDataSourceBase()
        {
            return GetUserDataSourceBase(false);
        }

        private UserDataSourceBase GetUserDataSourceBase(Boolean refresh)
        {
            if (_userDataSourceBase.IsNull() || refresh)
            {
                _userDataSourceBase = new UserDataSourceBase();
            }
            return _userDataSourceBase;
        }

    }
}
