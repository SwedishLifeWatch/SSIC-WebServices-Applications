using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class UserTest : TestBase
    {
        User _user;

        public UserTest()
        {
            _user = null;
        }

        [TestMethod]
        public void Constructor()
        {
            User user;

            user = new User(GetUserContext());
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetUser(true).DataContext);
        }

        private User GetUser()
        {
            return GetUser(false);
        }

        private User GetUser(Boolean refresh)
        {
            if (_user.IsNull() || refresh)
            {
                _user = new User(GetUserContext());
            }
            return _user;
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetUser(true).UserName = name;
            Assert.IsNull(GetUser().UserName);

            name = "";
            GetUser().UserName = name;
            Assert.AreEqual(GetUser().UserName, name);

            name = Settings.Default.TestUserName;
            GetUser().UserName = name;
            Assert.AreEqual(GetUser().UserName, name);
        }
    }
}
