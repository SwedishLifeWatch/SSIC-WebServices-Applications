using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonTreeNodeTest : TestBase
    {
        private Data.ArtDatabankenService.TaxonTreeNode _taxonTree;
        private Data.ArtDatabankenService.TaxonTreeNode _taxonTreeNode;

        public TaxonTreeNodeTest()
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
        public void AddParent()
        {
            Int32 treeIndex;

            // Add no parents.
            Assert.IsNotNull(GetTaxonTreeNode(true).Parents);
            Assert.IsTrue(GetTaxonTreeNode().Parents.IsEmpty());

            // Add one parent.
            GetTaxonTreeNode().AddParent(GetTaxonTree(true).Children[0]);
            Assert.IsNotNull(GetTaxonTreeNode().Parents);
            Assert.AreEqual(GetTaxonTreeNode().Parents.Count, 1);
            Assert.AreEqual(GetTaxonTreeNode().Parents[0], GetTaxonTree().Children[0]);

            // Add many parents.
            for (treeIndex = 1; treeIndex < GetTaxonTree().Children.Count; treeIndex++)
            {
                GetTaxonTreeNode().AddParent(GetTaxonTree().Children[treeIndex]);
            }
            Assert.IsNotNull(GetTaxonTreeNode().Parents);
            Assert.AreEqual(GetTaxonTreeNode().Parents.Count, GetTaxonTree().Children.Count);
            for (treeIndex = 0; treeIndex < GetTaxonTree().Children.Count; treeIndex++)
            {
                Assert.AreEqual(GetTaxonTreeNode().Parents[treeIndex], GetTaxonTree().Children[treeIndex]);
            }
        }

        [TestMethod]
        public void Children()
        {
            Int32 childIndex;

            // Test no children.
            Assert.IsNotNull(GetTaxonTreeNode(true).Children);
            Assert.IsTrue(GetTaxonTreeNode().Children.IsEmpty());

            // Test one child.
            GetTaxonTreeNode().AddChild(GetTaxonTree(true).Children[0]);
            Assert.IsNotNull(GetTaxonTreeNode().Children);
            Assert.AreEqual(GetTaxonTreeNode().Children.Count, 1);
            Assert.AreEqual(GetTaxonTreeNode().Children[0], GetTaxonTree().Children[0]);

            // Test many children.
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

        public Data.ArtDatabankenService.TaxonTreeNode GetTaxonTree(Boolean refresh = false)
        {
            if (_taxonTree.IsNull() || refresh)
            {
                _taxonTree = TaxonManagerTest.GetHawkBirdsTaxonTree();
            }
            return _taxonTree;
        }

        public Data.ArtDatabankenService.TaxonTreeNode GetTaxonTreeNode()
        {
            return GetTaxonTreeNode(false);
        }

        public Data.ArtDatabankenService.TaxonTreeNode GetTaxonTreeNode(Boolean refresh)
        {
            if (_taxonTreeNode.IsNull() || refresh)
            {
                _taxonTreeNode = TaxonManagerTest.GetBearTaxonTreeNode();
            }
            return _taxonTreeNode;
        }

        [TestMethod]
        public void Parents()
        {
            Int32 treeIndex;

            // Test no parents.
            Assert.IsNotNull(GetTaxonTreeNode(true).Parents);
            Assert.IsTrue(GetTaxonTreeNode().Parents.IsEmpty());

            // Test one parent.
            GetTaxonTreeNode().AddParent(GetTaxonTree(true).Children[0]);
            Assert.IsNotNull(GetTaxonTreeNode().Parents);
            Assert.AreEqual(GetTaxonTreeNode().Parents.Count, 1);
            Assert.AreEqual(GetTaxonTreeNode().Parents[0], GetTaxonTree().Children[0]);

            // Test many parents.
            for (treeIndex = 1; treeIndex < GetTaxonTree().Children.Count; treeIndex++)
            {
                GetTaxonTreeNode().AddParent(GetTaxonTree().Children[treeIndex]);
            }
            Assert.IsNotNull(GetTaxonTreeNode().Parents);
            Assert.AreEqual(GetTaxonTreeNode().Parents.Count, GetTaxonTree().Children.Count);
            for (treeIndex = 0; treeIndex < GetTaxonTree().Children.Count; treeIndex++)
            {
                Assert.AreEqual(GetTaxonTreeNode().Parents[treeIndex], GetTaxonTree().Children[treeIndex]);
            }
        }

        [TestMethod]
        public void Taxon()
        {
            Assert.IsNotNull(GetTaxonTreeNode(true).Taxon);
        }
    }
}
