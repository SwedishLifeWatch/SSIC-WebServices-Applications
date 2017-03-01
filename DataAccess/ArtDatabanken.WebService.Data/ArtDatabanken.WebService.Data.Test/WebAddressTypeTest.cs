using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebAddressType
    /// </summary>
    [TestClass]
    public class WebAddressTypeTest
    {
        private WebAddressType _addressType;

        public WebAddressTypeTest()
        {
            _addressType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebAddressType addressType;

            addressType = new WebAddressType();

            Assert.IsNotNull(addressType);
        }

        private WebAddressType GetAddressType()
        {
            if (_addressType.IsNull())
            {
                _addressType = new WebAddressType();
            }
            return _addressType;
        }

        

        [TestMethod]
        public void Id()
        {

            GetAddressType().Id = 2;
            Assert.AreEqual(GetAddressType().Id, 2);

        }

        [TestMethod]
        public void NameStringId()
        {

            GetAddressType().NameStringId = 2;
            Assert.AreEqual(GetAddressType().NameStringId, 2);

        }

        [TestMethod]
        public void Name()
        {

            GetAddressType().Name = "Hem";
            Assert.AreEqual(GetAddressType().Name, "Hem");

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

