using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonNameUseTypeListTest : TestBase
    {
        private TaxonNameUseTypeList _taxonNameUseTypes;

        public TaxonNameUseTypeListTest()
        {
            _taxonNameUseTypes = null;
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

        [TestMethod]
        public void Get()
        {
            foreach (TaxonNameUseType taxonNameUseType in GetTaxonNameUseTypes())
            {
                Assert.AreEqual(taxonNameUseType, GetTaxonNameUseTypes().Get(taxonNameUseType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonNameUseTypeId;

            taxonNameUseTypeId = Int32.MinValue;
            GetTaxonNameUseTypes().Get(taxonNameUseTypeId);
        }

        private TaxonNameUseTypeList GetTaxonNameUseTypes()
        {
            if (_taxonNameUseTypes.IsNull())
            {
                _taxonNameUseTypes = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameUseTypes();
            }
            return _taxonNameUseTypes;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 taxonNameUseTypeIndex;
            TaxonNameUseTypeList newTaxonNameUseTypeList, oldTaxonNameUseTypeList;

            oldTaxonNameUseTypeList = GetTaxonNameUseTypes();
            newTaxonNameUseTypeList = new TaxonNameUseTypeList();
            for (taxonNameUseTypeIndex = 0; taxonNameUseTypeIndex < oldTaxonNameUseTypeList.Count; taxonNameUseTypeIndex++)
            {
                newTaxonNameUseTypeList.Add(oldTaxonNameUseTypeList[oldTaxonNameUseTypeList.Count - taxonNameUseTypeIndex - 1]);
            }
            for (taxonNameUseTypeIndex = 0; taxonNameUseTypeIndex < oldTaxonNameUseTypeList.Count; taxonNameUseTypeIndex++)
            {
                Assert.AreEqual(newTaxonNameUseTypeList[taxonNameUseTypeIndex], oldTaxonNameUseTypeList[oldTaxonNameUseTypeList.Count - taxonNameUseTypeIndex - 1]);
            }
        }
    }
}
