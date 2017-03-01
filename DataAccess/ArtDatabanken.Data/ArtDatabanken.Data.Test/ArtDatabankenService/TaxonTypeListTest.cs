using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonTypeListTest : TestBase
    {
        private TaxonTypeList _taxonTypes;

        public TaxonTypeListTest()
        {
            _taxonTypes = null;
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
            foreach (TaxonType taxonType in GetTaxonTypes())
            {
                Assert.AreEqual(taxonType, GetTaxonTypes().Get(taxonType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonTypeId;

            taxonTypeId = Int32.MinValue;
            GetTaxonTypes().Get(taxonTypeId);
        }

        private TaxonTypeList GetTaxonTypes()
        {
            if (_taxonTypes.IsNull())
            {
                _taxonTypes = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTypes();
            }
            return _taxonTypes;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 taxonTypeIndex;
            TaxonTypeList newTaxonTypeList, oldTaxonTypeList;

            oldTaxonTypeList = GetTaxonTypes();
            newTaxonTypeList = new TaxonTypeList();
            for (taxonTypeIndex = 0; taxonTypeIndex < oldTaxonTypeList.Count; taxonTypeIndex++)
            {
                newTaxonTypeList.Add(oldTaxonTypeList[oldTaxonTypeList.Count - taxonTypeIndex - 1]);
            }
            for (taxonTypeIndex = 0; taxonTypeIndex < oldTaxonTypeList.Count; taxonTypeIndex++)
            {
                Assert.AreEqual(newTaxonTypeList[taxonTypeIndex], oldTaxonTypeList[oldTaxonTypeList.Count - taxonTypeIndex - 1]);
            }
        }
    }
}
