using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class FactorFactorFieldEnumValueListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorFieldEnumValueList _factorFieldEnumValues;

        public FactorFactorFieldEnumValueListTest()
        {
            _factorFieldEnumValues = null;
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
            foreach (FactorFieldEnumValue factorFieldEnumValue in GetFactorFieldEnumValues())
            {
                Assert.AreEqual(factorFieldEnumValue, GetFactorFieldEnumValues().Get(factorFieldEnumValue.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorFieldEnumValueId;

            factorFieldEnumValueId = Int32.MinValue;
            GetFactorFieldEnumValues().Get(factorFieldEnumValueId);
        }

        private Data.ArtDatabankenService.FactorFieldEnumValueList GetFactorFieldEnumValues()
        {
            if (_factorFieldEnumValues.IsNull())
            {
                _factorFieldEnumValues = Data.ArtDatabankenService.FactorManager.GetFactorFieldEnums()[0].Values;
            }
            return _factorFieldEnumValues;
        }
    }
}
