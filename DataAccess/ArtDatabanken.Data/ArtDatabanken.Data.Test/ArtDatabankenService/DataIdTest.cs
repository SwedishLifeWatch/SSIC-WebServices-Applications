using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class DataIdTest : TestBase
    {
        private DataId _data;

        public DataIdTest()
        {
            _data = null;
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

        #endregion

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(DataId.AreEqual(null, null));
            Assert.IsFalse(DataId.AreEqual(GetDataId(), null));
            Assert.IsFalse(DataId.AreEqual(null, GetDataId()));
            Assert.IsFalse(DataId.AreEqual(GetDataId(), TaxonManagerTest.GetOneTaxon()));
            Assert.IsTrue(DataId.AreEqual(GetDataId(), GetDataId()));
            Assert.IsFalse(DataId.AreEqual(GetDataId(), ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTypes()[2]));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsFalse(DataId.AreNotEqual(null, null));
            Assert.IsTrue(DataId.AreNotEqual(GetDataId(), null));
            Assert.IsTrue(DataId.AreNotEqual(null, GetDataId()));
            Assert.IsTrue(DataId.AreNotEqual(GetDataId(), TaxonManagerTest.GetOneTaxon()));
            Assert.IsFalse(DataId.AreNotEqual(GetDataId(), GetDataId()));
            Assert.IsTrue(DataId.AreNotEqual(GetDataId(), ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTypes()[2]));
        }

        private DataId GetDataId()
        {
            return GetDataId(false);
        }

        private DataId GetDataId(Boolean refresh)
        {
            if (_data.IsNull() || refresh)
            {
                _data = TaxonManagerTest.GetSpeciesTaxonType();
            }
            return _data;
        }

        [TestMethod]
        public void Id()
        {
            Assert.IsTrue(GetDataId(true).Id > Int32.MinValue);
        }
    }
}
