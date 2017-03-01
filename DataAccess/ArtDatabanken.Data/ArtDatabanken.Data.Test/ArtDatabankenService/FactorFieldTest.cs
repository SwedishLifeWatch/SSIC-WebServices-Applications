using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorFieldTest
    /// </summary>
    [TestClass]
    public class FactorFieldTest : TestBase
    {
        public FactorFieldTest()
        {
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

        private Data.ArtDatabankenService.FactorField GetFactorField()
        {
            return FactorManagerTest.GetOneFactorField();
        }

        [TestMethod]
        public void Label()
        {
            Assert.IsTrue(GetFactorField().Label.IsNotEmpty());
        }

        [TestMethod]
        public void Information()
        {
            Assert.IsTrue(GetFactorField().Information.IsNotEmpty());
        }

        [TestMethod]
        public void UnitLabel()
        {
            Assert.IsTrue(GetFactorField().UnitLabel.IsEmpty());
        }

        [TestMethod]
        public void IsMain()
        {
            Assert.IsTrue(GetFactorField().IsMain);
        }

        [TestMethod]
        public void IsSubstantial()
        {
            Assert.IsTrue(GetFactorField().IsSubstantial);
        }

        [TestMethod]
        public void TypeIsString()
        {
            Assert.AreEqual(GetFactorField().Type, Data.ArtDatabankenService.FactorManager.GetFactorFieldType(2));
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(GetFactorField().Size > -99);
        }

        [TestMethod]
        public void FactorFieldEnum()
        {
            Assert.IsNull(GetFactorField().FactorFieldEnum);
        }

    }
}
