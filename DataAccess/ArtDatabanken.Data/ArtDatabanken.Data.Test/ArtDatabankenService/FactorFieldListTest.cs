using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorFieldListTest
    /// </summary>
    [TestClass]
    public class FactorFieldListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorFieldList _factorFields;

        public FactorFieldListTest()
        {
            _factorFields = null;
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
            foreach (FactorField factorField in GetFactorFields())
            {
                Assert.AreEqual(factorField, GetFactorFields().Get(factorField.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorFieldId;

            factorFieldId = Int32.MinValue;
            GetFactorFields().Get(factorFieldId);
        }

        private Data.ArtDatabankenService.FactorFieldList GetFactorFields()
        {
            if (_factorFields.IsNull())
            {
                _factorFields = Data.ArtDatabankenService.FactorManager.GetFactorDataType(1).Fields;
            }
            return _factorFields;
        }
    }
}
