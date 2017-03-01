using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebTaxonTreeNodeTest : TestBase
    {
        private WebTaxonTreeNode _taxonTree;
        private WebTaxonTreeNode _taxonTreeNode;

        public WebTaxonTreeNodeTest()
            : base (true, 30)
        {
            _taxonTree = null;
            _taxonTreeNode = null;
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
        public void AddChild()
        {
            Int32 childIndex;

            // Add no children.
            Assert.IsNotNull(GetTaxonTreeNode(true).Children);
            Assert.IsTrue(GetTaxonTreeNode().Children.IsEmpty());

            // Add one child.
            GetTaxonTreeNode().AddChild(GetTaxonTree(true).Children[0]);
            Assert.IsNotNull(GetTaxonTreeNode().Children);
            Assert.AreEqual(GetTaxonTreeNode().Children.Count, 1);
            Assert.AreEqual(GetTaxonTreeNode().Children[0], GetTaxonTree().Children[0]);

            // Add many children.
            for (childIndex = 1; childIndex < GetTaxonTree().Children.Count; childIndex++)
            {
                GetTaxonTreeNode().AddChild(GetTaxonTree().Children[childIndex]);
            }
            Assert.IsNotNull(GetTaxonTreeNode().Children);
            Assert.AreEqual(GetTaxonTreeNode().Children.Count, GetTaxonTree().Children.Count);
            for (childIndex = 0; childIndex < GetTaxonTree().Children.Count; childIndex++)
            {
                Assert.AreEqual(GetTaxonTreeNode().Children[childIndex], GetTaxonTree().Children[childIndex]);
            }
        }

        [TestMethod]
        public void Children()
        {
            Int32 childIndex;
            List<WebTaxonTreeNode> _children;

            // Test null;
            _children = null;
            GetTaxonTreeNode(true).Children = _children;
            Assert.IsNull(GetTaxonTreeNode().Children);

            // Test no children.
            _children = new List<WebTaxonTreeNode>();
            GetTaxonTreeNode().Children = _children;
            Assert.IsNotNull(GetTaxonTreeNode().Children);
            Assert.IsTrue(GetTaxonTreeNode().Children.IsEmpty());

            // Test one child.
            _children.Add(GetTaxonTree(true).Children[0]);
            GetTaxonTreeNode().Children = _children;
            Assert.IsNotNull(GetTaxonTreeNode().Children);
            Assert.AreEqual(GetTaxonTreeNode().Children.Count, 1);
            Assert.AreEqual(GetTaxonTreeNode().Children[0], GetTaxonTree().Children[0]);

            // Test many children.
            for (childIndex = 1; childIndex < GetTaxonTree().Children.Count; childIndex++)
            {
                _children.Add(GetTaxonTree().Children[childIndex]);
            }
            GetTaxonTreeNode().Children = _children;
            Assert.IsNotNull(GetTaxonTreeNode().Children);
            Assert.AreEqual(GetTaxonTreeNode().Children.Count, GetTaxonTree().Children.Count);
            for (childIndex = 0; childIndex < GetTaxonTree().Children.Count; childIndex++)
            {
                Assert.AreEqual(GetTaxonTreeNode().Children[childIndex], GetTaxonTree().Children[childIndex]);
            }
        }

        [TestMethod]
        public void Constructor()
        {
            WebTaxonTreeNode taxonTreeNode;

            taxonTreeNode = GetTaxonTreeNode(true);
            Assert.IsNotNull(taxonTreeNode);
        }

        public WebTaxonTreeNode GetTaxonTree()
        {
            return GetTaxonTree(false);
        }

        public WebTaxonTreeNode GetTaxonTree(Boolean refresh)
        {
            if (_taxonTree.IsNull() || refresh)
            {
                _taxonTree = TaxonManagerTest.GetHawkBirdsTaxonTree(GetContext());
            }
            return _taxonTree;
        }

        public WebTaxonTreeNode GetTaxonTreeNode()
        {
            return GetTaxonTreeNode(false);
        }

        public WebTaxonTreeNode GetTaxonTreeNode(Boolean refresh)
        {
            if (_taxonTreeNode.IsNull() || refresh)
            {
                _taxonTreeNode = TaxonManagerTest.GetBearTaxonTreeNode(GetContext());
            }
            return _taxonTreeNode;
        }

        [TestMethod]
        public void IsChild()
        {
            Boolean isChild;
            WebTaxonTreeNode taxonTree;

            // Test value.
            isChild = false;
            GetTaxonTreeNode().IsChild = isChild;
            Assert.AreEqual(isChild, GetTaxonTreeNode().IsChild);
            isChild = true;
            GetTaxonTreeNode().IsChild = isChild;
            Assert.AreEqual(isChild, GetTaxonTreeNode().IsChild);

            // Test with no parent.
            Assert.IsFalse(GetTaxonTreeNode(true).IsChild);

            // Test with parent.
            taxonTree = GetTaxonTree();
            taxonTree.AddChild(GetTaxonTreeNode());
            Assert.IsTrue(GetTaxonTreeNode().IsChild);
        }

        [TestMethod]
        public void RestrictTaxonTypes()
        {
            List<Int32> taxonTypeIds;

            taxonTypeIds = TaxonManagerTest.GetTaxonTypeIds();
            GetTaxonTreeNode(true).RestrictTaxonTypes(taxonTypeIds);
        }

        [TestMethod]
        public void Taxon()
        {
            WebTaxon taxon;

            taxon = null;
            GetTaxonTreeNode(true).Taxon = taxon;
            Assert.IsNull(GetTaxonTreeNode().Taxon);

            taxon = TaxonManagerTest.GetOneTaxon(GetContext());
            GetTaxonTreeNode().Taxon = taxon;
            Assert.IsNotNull(GetTaxonTreeNode().Taxon);
            Assert.AreEqual(GetTaxonTreeNode().Taxon, taxon);
        }
    }
}
