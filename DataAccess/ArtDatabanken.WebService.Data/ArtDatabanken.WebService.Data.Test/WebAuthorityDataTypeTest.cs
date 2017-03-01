using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebAuthorityDataType
    /// </summary>
    [TestClass]
    public class WebAuthorityDataTypeTest
    {
        private WebAuthorityDataType _authorityDataType;

        public WebAuthorityDataTypeTest()
        {
            _authorityDataType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebAuthorityDataType authorityType;

            authorityType = new WebAuthorityDataType();

            Assert.IsNotNull(authorityType);
        }

        private WebAuthorityDataType GetAuthorityDataType()
        {
            if (_authorityDataType.IsNull())
            {
                _authorityDataType = new WebAuthorityDataType();
            }
            return _authorityDataType;
        }

        

        [TestMethod]
        public void Id()
        {

            GetAuthorityDataType().Id = 2;
            Assert.AreEqual(GetAuthorityDataType().Id, 2);

        }


        [TestMethod]
        public void Identifier()
        {

            GetAuthorityDataType().Identifier= "Test";
            Assert.AreEqual(GetAuthorityDataType().Identifier, "Test");

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

