using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebAuthorityType
    /// </summary>
    [TestClass]
    public class WebAuthorityTypeTest
    {
        private WebAuthorityType _authorityType;

        public WebAuthorityTypeTest()
        {
            _authorityType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebAuthorityType authorityType;

            authorityType = new WebAuthorityType();

            Assert.IsNotNull(authorityType);
        }

        private WebAuthorityType GetAuthorityType()
        {
            if (_authorityType.IsNull())
            {
                _authorityType = new WebAuthorityType();
            }
            return _authorityType;
        }

        

        [TestMethod]
        public void Id()
        {

            GetAuthorityType().Id = 2;
            Assert.AreEqual(GetAuthorityType().Id, 2);

        }


        [TestMethod]
        public void AuthorityTypeName()
        {

            GetAuthorityType().AuthorityTypeName= "Test";
            Assert.AreEqual(GetAuthorityType().AuthorityTypeName, "Test");

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

