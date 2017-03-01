using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class FactorUpdateModeListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorUpdateModeList _factorUpdateModes;

        public FactorUpdateModeListTest()
        {
            _factorUpdateModes = null;
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
            foreach (FactorUpdateMode factorUpdateMode in GetFactorUpdateModes())
            {
                Assert.AreEqual(factorUpdateMode, GetFactorUpdateModes().Get(factorUpdateMode.Id));
            }
        }

        [TestMethod]
        public void GetSortOrder()
        {
            foreach (Data.ArtDatabankenService.FactorUpdateMode factorUpdateMode in GetFactorUpdateModes())
            {
                Assert.AreEqual(factorUpdateMode.Id, GetFactorUpdateModes().Get(factorUpdateMode.Id).SortOrder);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorUpdateModeId;

            factorUpdateModeId = Int32.MinValue;
            GetFactorUpdateModes().Get(factorUpdateModeId);
        }

        private Data.ArtDatabankenService.FactorUpdateModeList GetFactorUpdateModes()
        {
            if (_factorUpdateModes.IsNull())
            {
                _factorUpdateModes = Data.ArtDatabankenService.FactorManager.GetFactorUpdateModes();
            }
            return _factorUpdateModes;
        }

    }
}
