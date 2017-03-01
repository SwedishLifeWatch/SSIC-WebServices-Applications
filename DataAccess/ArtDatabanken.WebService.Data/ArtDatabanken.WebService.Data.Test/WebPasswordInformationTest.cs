using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebPasswordInformation
    /// </summary>
    [TestClass]
    public class WebPasswordInformationTest
    {
        private WebPasswordInformation _webPasswordInformation;

        public WebPasswordInformationTest()
        {
            _webPasswordInformation = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebPasswordInformation webPasswordInformation;

            webPasswordInformation = new WebPasswordInformation();
            Assert.IsNotNull(webPasswordInformation);
        }

        private WebPasswordInformation GetPasswordInformation()
        {
            return GetPasswordInformation(false);
        }

        private WebPasswordInformation GetPasswordInformation(Boolean refresh)
        {
            if (_webPasswordInformation.IsNull() || refresh)
            {
                _webPasswordInformation = new WebPasswordInformation();
            }
            return _webPasswordInformation;
        }


        [TestMethod]
        public void EmailAddress()
        {
            Assert.IsNull(GetPasswordInformation().EmailAddress);
            String value = "test@artdata.se";
            Assert.IsTrue(value.IsValidEmail());
            GetPasswordInformation(true).EmailAddress = value;
            Assert.AreEqual(value, GetPasswordInformation().EmailAddress);
        }

        [TestMethod]
        public void UserName()
        {
            String userName = "userName";
            GetPasswordInformation().UserName = userName;
            Assert.AreEqual(GetPasswordInformation().UserName, userName);
        }

        [TestMethod]
        public void Password()
        {
            Assert.IsNull(GetPasswordInformation().Password);
            String value = "password";
            GetPasswordInformation(true).Password = value;
            Assert.AreEqual(value, GetPasswordInformation().Password);
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
