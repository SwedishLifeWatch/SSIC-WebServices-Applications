using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebApplicationTest
    /// </summary>
    [TestClass]
    public class WebApplicationTest
    {
        private WebApplication _application;

        public WebApplicationTest()
        {
            _application = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebApplication application;

            application = new WebApplication();
            Assert.IsNotNull(application);
        }

        private WebApplication GetApplication()
        {
            return GetApplication(false);
        }

        private WebApplication GetApplication(Boolean refresh)
        {
            if (_application.IsNull() || refresh)
            {
                _application = new WebApplication();
            }
            return _application;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id = 100;
            GetApplication(true).Id = id;
            Assert.AreEqual(id, GetApplication().Id);
        }

        [TestMethod]
        public void IsContactPersonIdSpecified()
        {
            Boolean isContactPersonIdSpecified;

            isContactPersonIdSpecified = false;
            GetApplication(true).IsContactPersonIdSpecified = isContactPersonIdSpecified;
            Assert.AreEqual(isContactPersonIdSpecified, GetApplication().IsContactPersonIdSpecified);

            isContactPersonIdSpecified = true;
            GetApplication().IsContactPersonIdSpecified = isContactPersonIdSpecified;
            Assert.AreEqual(isContactPersonIdSpecified, GetApplication().IsContactPersonIdSpecified);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsNull(GetApplication().Name);
            String name = "TestName";
            GetApplication(true).Name = name;
            Assert.AreEqual(name, GetApplication().Name);
        }

        [TestMethod]
        public void GUID()
        {
            Assert.IsNull(GetApplication().GUID);
            String GUID = "TestGUID:1234:artdatabanken.slu.se";
            GetApplication(true).GUID = GUID;
            Assert.AreEqual(GUID, GetApplication().GUID);
        }

        [TestMethod]
        public void AdministrationRoleId()
        {
            Int32 administrationRoleId = 99;
            GetApplication(true).AdministrationRoleId = administrationRoleId;
            Assert.AreEqual(administrationRoleId, GetApplication().AdministrationRoleId);
        }

        [TestMethod]
        public void Description()
        {
            Assert.IsNull(GetApplication().Description);
            String value = "DescriptionTest " +
            "DescriptionTest2";
            GetApplication(true).Description = value;
            Assert.AreEqual(value, GetApplication().Description);
        }

        [TestMethod]
        public void ContactPersonId()
        {
            Int32 contactPersonId = 99;
            GetApplication(true).ContactPersonId = contactPersonId;
            Assert.AreEqual(contactPersonId, GetApplication().ContactPersonId);
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
