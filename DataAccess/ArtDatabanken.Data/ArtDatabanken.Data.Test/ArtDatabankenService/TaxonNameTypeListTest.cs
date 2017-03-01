using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonNameTypeListTest : TestBase
    {
        private TaxonNameTypeList _taxonNameTypes;

        public TaxonNameTypeListTest()
        {
            _taxonNameTypes = null;
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
            foreach (TaxonNameType taxonNameType in GetTaxonNameTypes())
            {
                Assert.AreEqual(taxonNameType, GetTaxonNameTypes().Get(taxonNameType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonNameTypeId;

            taxonNameTypeId = Int32.MinValue;
            GetTaxonNameTypes().Get(taxonNameTypeId);
        }

        private TaxonNameTypeList GetTaxonNameTypes()
        {
            if (_taxonNameTypes.IsNull())
            {
                _taxonNameTypes = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameTypes();
            }
            return _taxonNameTypes;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 taxonNameTypeIndex;
            TaxonNameTypeList newTaxonNameTypeList, oldTaxonNameTypeList;

            oldTaxonNameTypeList = GetTaxonNameTypes();
            newTaxonNameTypeList = new TaxonNameTypeList();
            for (taxonNameTypeIndex = 0; taxonNameTypeIndex < oldTaxonNameTypeList.Count; taxonNameTypeIndex++)
            {
                newTaxonNameTypeList.Add(oldTaxonNameTypeList[oldTaxonNameTypeList.Count - taxonNameTypeIndex - 1]);
            }
            for (taxonNameTypeIndex = 0; taxonNameTypeIndex < oldTaxonNameTypeList.Count; taxonNameTypeIndex++)
            {
                Assert.AreEqual(newTaxonNameTypeList[taxonNameTypeIndex], oldTaxonNameTypeList[oldTaxonNameTypeList.Count - taxonNameTypeIndex - 1]);
            }
        }
    }
}
