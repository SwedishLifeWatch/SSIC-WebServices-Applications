using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonTreeNodeListTest : TestBase
    {
        private Data.ArtDatabankenService.TaxonTreeNodeList _taxonTrees;

        public TaxonTreeNodeListTest()
        {
            _taxonTrees = null;
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
            foreach (Data.ArtDatabankenService.TaxonTreeNode taxonTree in GetTaxonTrees())
            {
                Assert.AreEqual(taxonTree, GetTaxonTrees().Get(taxonTree.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonId;

            taxonId = Int32.MinValue;
            GetTaxonTrees().Get(taxonId);
        }

        private Data.ArtDatabankenService.TaxonTreeNodeList GetTaxonTrees()
        {
            if (_taxonTrees.IsNull())
            {
                _taxonTrees = TaxonManagerTest.GetHawkBirdsTaxonTree().Children;
            }
            return _taxonTrees;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 taxonTreeIndex;
            Data.ArtDatabankenService.TaxonTreeNodeList newTaxonTreeList, oldTaxonTreeList;

            oldTaxonTreeList = GetTaxonTrees();
            newTaxonTreeList = new Data.ArtDatabankenService.TaxonTreeNodeList();
            for (taxonTreeIndex = 0; taxonTreeIndex < oldTaxonTreeList.Count; taxonTreeIndex++)
            {
                newTaxonTreeList.Add(oldTaxonTreeList[oldTaxonTreeList.Count - taxonTreeIndex - 1]);
            }
            for (taxonTreeIndex = 0; taxonTreeIndex < oldTaxonTreeList.Count; taxonTreeIndex++)
            {
                Assert.AreEqual(newTaxonTreeList[taxonTreeIndex], oldTaxonTreeList[oldTaxonTreeList.Count - taxonTreeIndex - 1]);
            }
        }
    }
}
