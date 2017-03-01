using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebApplicationVersionTest
    /// </summary>
    [TestClass]
    public class WebApplicationVersionTest
    {
        private WebApplicationVersion _applicationVersion;

        public WebApplicationVersionTest()
        {
            _applicationVersion = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebApplicationVersion applicationVersion;

            applicationVersion = new WebApplicationVersion();
            Assert.IsNotNull(applicationVersion);
        }

        private WebApplicationVersion GetApplicationVersion()
        {
            return GetApplicationVersion(false);
        }

        private WebApplicationVersion GetApplicationVersion(Boolean refresh)
        {
            if (_applicationVersion.IsNull() || refresh)
            {
                _applicationVersion = new WebApplicationVersion();
            }
            return _applicationVersion;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id = 100;
            GetApplicationVersion(true).Id = id;
            Assert.AreEqual(id, GetApplicationVersion().Id);
        }

        [TestMethod]
        public void ApplicationId()
        {
            Int32 applicationId = 100;
            GetApplicationVersion(true).ApplicationId = applicationId;
            Assert.AreEqual(applicationId, GetApplicationVersion().ApplicationId);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsNull(GetApplicationVersion().Version);
            String version = "1.0.0-100";
            GetApplicationVersion(true).Version = version;
            Assert.AreEqual(version, GetApplicationVersion().Version);
        }

        [TestMethod]
        public void IsRecommended()
        {
            Boolean isRecommended = true;
            GetApplicationVersion(true).IsRecommended = isRecommended;
            Assert.IsTrue(GetApplicationVersion().IsRecommended);
        }

        [TestMethod]
        public void IsValid()
        {
            Boolean isValid = true;
            GetApplicationVersion(true).IsValid = isValid;
            Assert.IsTrue(GetApplicationVersion().IsValid);
        }

      
        [TestMethod]
        public void Description()
        {
            Assert.IsNull(GetApplicationVersion().Description);
            String value = "DescriptionTest " +
            "DescriptionTest2";
            GetApplicationVersion(true).Description = value;
            Assert.AreEqual(value, GetApplicationVersion().Description);
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

      
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


    }
}
