using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorFieldTypeListTest
    /// </summary>
    [TestClass]
    public class FactorFieldTypeListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorFieldTypeList _factorFieldTypes;

        public FactorFieldTypeListTest()
        {
            _factorFieldTypes = null;
        }

        #region Additional test attributes
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

        [TestMethod]
        public void Get()
        {
            foreach (FactorFieldType factorFieldType in GetFactorFieldTypes())
            {
                Assert.AreEqual(factorFieldType, GetFactorFieldTypes().Get(factorFieldType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 FactorFieldTypeId;

            FactorFieldTypeId = Int32.MinValue;
            GetFactorFieldTypes().Get(FactorFieldTypeId);
        }

        private Data.ArtDatabankenService.FactorFieldTypeList GetFactorFieldTypes()
        {
            if (_factorFieldTypes.IsNull())
            {
                _factorFieldTypes = Data.ArtDatabankenService.FactorManager.GetFactorFieldTypes();
            }
            return _factorFieldTypes;
        }
    }
}
