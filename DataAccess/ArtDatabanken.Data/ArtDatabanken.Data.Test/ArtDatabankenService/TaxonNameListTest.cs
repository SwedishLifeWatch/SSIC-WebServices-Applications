using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonNameListTest : TestBase
    {
        private ArtDatabanken.Data.ArtDatabankenService.TaxonNameList _taxonNames;

        public TaxonNameListTest()
        {
            _taxonNames = null;
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
            foreach (TaxonName taxonName in GetTaxonNames())
            {
                Assert.AreEqual(taxonName, GetTaxonNames().Get(taxonName.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonNameId;

            taxonNameId = Int32.MinValue;
            GetTaxonNames().Get(taxonNameId);
        }

        private ArtDatabanken.Data.ArtDatabankenService.TaxonNameList GetTaxonNames()
        {
            if (_taxonNames.IsNull())
            {
                _taxonNames = TaxonManagerTest.GetSomeTaxonNames();
            }
            return _taxonNames;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 taxonNameIndex;
            ArtDatabanken.Data.ArtDatabankenService.TaxonNameList newTaxonNameList, oldTaxonNameList;

            oldTaxonNameList = GetTaxonNames();
            newTaxonNameList = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameList();
            for (taxonNameIndex = 0; taxonNameIndex < oldTaxonNameList.Count; taxonNameIndex++)
            {
                newTaxonNameList.Add(oldTaxonNameList[oldTaxonNameList.Count - taxonNameIndex - 1]);
            }
            for (taxonNameIndex = 0; taxonNameIndex < oldTaxonNameList.Count; taxonNameIndex++)
            {
                Assert.AreEqual(newTaxonNameList[taxonNameIndex], oldTaxonNameList[oldTaxonNameList.Count - taxonNameIndex - 1]);
            }
        }
    }
}
