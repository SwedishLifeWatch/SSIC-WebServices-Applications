using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorDataTypeTest
    /// </summary>
    [TestClass]
    public class FactorDataTypeTest : TestBase
    {
        public FactorDataTypeTest()
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

        private Data.ArtDatabankenService.FactorDataType GetFactorDataType()
        {
            return FactorManagerTest.GetFirstFactorDataType();
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetFactorDataType().Name.IsNotEmpty());
        }

        [TestMethod]
        public void Definition()
        {
            Assert.IsFalse(GetFactorDataType().Definition.IsNotEmpty());
        }

        [TestMethod]
        public void Fields()
        {
            Assert.IsTrue(GetFactorDataType().Fields.Count > 1);
        }

        [TestMethod]
        public void SubstantialFields()
        {
            Assert.IsTrue(GetFactorDataType().SubstantialFields.Count < GetFactorDataType().Fields.Count);
        }

        [TestMethod]
        public void MainField()
        {
            Assert.AreEqual(GetFactorDataType().MainField.Id, GetFactorDataType().SubstantialFields[0].Id);
        }

    }
}
