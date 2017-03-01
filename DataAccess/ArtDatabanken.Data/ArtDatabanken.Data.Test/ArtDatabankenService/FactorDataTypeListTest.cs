using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorDataTypeListTest
    /// </summary>
    [TestClass]
    public class FactorDataTypeListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorDataTypeList _factorDataTypes;

        public FactorDataTypeListTest()
        {
            _factorDataTypes = null;
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
            foreach (FactorDataType factorDataType in GetFactorDataTypes())
            {
                Assert.AreEqual(factorDataType, GetFactorDataTypes().Get(factorDataType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorDataTypeId;

            factorDataTypeId = Int32.MinValue;
            GetFactorDataTypes().Get(factorDataTypeId);
        }

        private Data.ArtDatabankenService.FactorDataTypeList GetFactorDataTypes()
        {
            if (_factorDataTypes.IsNull())
            {
                _factorDataTypes = Data.ArtDatabankenService.FactorManager.GetFactorDataTypes();
            }
            return _factorDataTypes;
        }
    }
}
