using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebUserTest
    /// </summary>
    [TestClass]
    public class WebUserTest
    {
        private WebUser _user;

        public WebUserTest()
        {
            _user = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebUser user;

            user = new WebUser();
            Assert.IsNotNull(user);
        }

        private WebUser GetUser()
        {
            return GetUser(false);
        }

        private WebUser GetUser(Boolean refresh)
        {
            if (_user.IsNull() || refresh)
            {
                _user = new WebUser();
            }
            return _user;
        }


        [TestMethod]
        public void ApplicationId()
        {
            GetUser(true).ApplicationId = 10;
            Assert.AreEqual(10, GetUser().ApplicationId);

        }

        [TestMethod]
        public void UserName()
        {
            String userName;

            userName = null;
            GetUser(true).UserName = userName;
            Assert.IsNull(GetUser().UserName);
            userName = String.Empty;
            GetUser().UserName = userName;
            Assert.AreEqual(GetUser().UserName, userName);
            userName = "TestU";
            GetUser().UserName = userName;
            Assert.AreEqual(GetUser().UserName, userName);
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
