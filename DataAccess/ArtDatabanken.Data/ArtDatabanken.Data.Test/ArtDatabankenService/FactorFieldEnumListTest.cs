using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class FactorFieldEnumListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorFieldEnumList _factorFieldEnums;

        public FactorFieldEnumListTest()
        {
            _factorFieldEnums = null;
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
            foreach (FactorFieldEnum factorFieldEnum in GetFactorFieldEnums())
            {
                Assert.AreEqual(factorFieldEnum, GetFactorFieldEnums().Get(factorFieldEnum.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorFieldEnumId;

            factorFieldEnumId = Int32.MinValue;
            GetFactorFieldEnums().Get(factorFieldEnumId);
        }

        private Data.ArtDatabankenService.FactorFieldEnumList GetFactorFieldEnums()
        {
            if (_factorFieldEnums.IsNull())
            {
                _factorFieldEnums = Data.ArtDatabankenService.FactorManager.GetFactorFieldEnums();
            }
            return _factorFieldEnums;
        }
    }
}
