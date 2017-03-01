using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorTreeNodeTest
    /// </summary>
    [TestClass]
    public class FactorTreeNodeTest : TestBase
    {
        private Data.ArtDatabankenService.FactorTreeNode _factorTree;
        private Data.ArtDatabankenService.FactorTreeNode _factorTreeNode;

        public FactorTreeNodeTest()
        {
            _factorTree = null;
            _factorTreeNode = null;
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
            /*            Int32 childIndex;

                        // Add no children.
                        Assert.IsNotNull(GetFactorTreeNode(true).Children);
                        Assert.IsTrue(GetFactorTreeNode().Children.IsEmpty());

                        // Add one child.
                        GetFactorTreeNode().AddChild(GetFactorTree(true).Children[0]);
                        Assert.IsNotNull(GetFactorTreeNode().Children);
                        Assert.AreEqual(GetFactorTreeNode().Children.Count, 1);
                        Assert.AreEqual(GetFactorTreeNode().Children[0], GetFactorTree().Children[0]);

                        // Add many children.
                        for (childIndex = 1; childIndex < GetFactorTree().Children.Count; childIndex++)
                        {
                            GetFactorTreeNode().AddChild(GetFactorTree().Children[childIndex]);
                        }
                        Assert.IsNotNull(GetFactorTreeNode().Children);
                        Assert.AreEqual(GetFactorTreeNode().Children.Count, GetFactorTree().Children.Count);
                        for (childIndex = 0; childIndex < GetFactorTree().Children.Count; childIndex++)
                        {
                            Assert.AreEqual(GetFactorTreeNode().Children[childIndex], GetFactorTree().Children[childIndex]);
                        }*/
        }

        [TestMethod]
        public void AddParent()
        {
            /*            Int32 treeIndex;

                        // Add no parents.
                        Assert.IsNotNull(GetFactorTreeNode(true).Parents);
                        Assert.IsTrue(GetFactorTreeNode().Parents.IsEmpty());

                        // Add one parent.
                        GetFactorTreeNode().AddParent(GetFactorTree(true).Children[0]);
                        Assert.IsNotNull(GetFactorTreeNode().Parents);
                        Assert.AreEqual(GetFactorTreeNode().Parents.Count, 1);
                        Assert.AreEqual(GetFactorTreeNode().Parents[0], GetFactorTree().Children[0]);

                        // Add many parents.
                        for (treeIndex = 1; treeIndex < GetFactorTree().Children.Count; treeIndex++)
                        {
                            GetFactorTreeNode().AddParent(GetFactorTree().Children[treeIndex]);
                        }
                        Assert.IsNotNull(GetFactorTreeNode().Parents);
                        Assert.AreEqual(GetFactorTreeNode().Parents.Count, GetFactorTree().Children.Count);
                        for (treeIndex = 0; treeIndex < GetFactorTree().Children.Count; treeIndex++)
                        {
                            Assert.AreEqual(GetFactorTreeNode().Parents[treeIndex], GetFactorTree().Children[treeIndex]);
                        }*/
        }

        [TestMethod]
        public void Children()
        {
            /*            Int32 childIndex;

                        // Test no children.
                        Assert.IsNotNull(GetFactorTreeNode(true).Children);
                        Assert.IsTrue(GetFactorTreeNode().Children.IsEmpty());

                        // Test one child.
                        GetFactorTreeNode().AddChild(GetFactorTree(true).Children[0]);
                        Assert.IsNotNull(GetFactorTreeNode().Children);
                        Assert.AreEqual(GetFactorTreeNode().Children.Count, 1);
                        Assert.AreEqual(GetFactorTreeNode().Children[0], GetFactorTree().Children[0]);

                        // Test many children.
                        for (childIndex = 1; childIndex < GetFactorTree().Children.Count; childIndex++)
                        {
                            GetFactorTreeNode().AddChild(GetFactorTree().Children[childIndex]);
                        }
                        Assert.IsNotNull(GetFactorTreeNode().Children);
                        Assert.AreEqual(GetFactorTreeNode().Children.Count, GetFactorTree().Children.Count);
                        for (childIndex = 0; childIndex < GetFactorTree().Children.Count; childIndex++)
                        {
                            Assert.AreEqual(GetFactorTreeNode().Children[childIndex], GetFactorTree().Children[childIndex]);
                        }*/
        }

        public Data.ArtDatabankenService.FactorTreeNode GetFactorTree()
        {
            return GetFactorTree(false);
        }

        public Data.ArtDatabankenService.FactorTreeNode GetFactorTree(Boolean refresh)
        {
            if (_factorTree.IsNull() || refresh)
            {
                _factorTree = FactorManagerTest.GetForestFactorTreeNode();
            }
            return _factorTree;
        }

        public Data.ArtDatabankenService.FactorTreeNode GetFactorTreeNode()
        {
            return GetFactorTreeNode(false);
        }

        public Data.ArtDatabankenService.FactorTreeNode GetFactorTreeNode(Boolean refresh)
        {
            if (_factorTreeNode.IsNull() || refresh)
            {
                _factorTreeNode = FactorManagerTest.GetLandscapeFactorTreeNode();
            }
            return _factorTreeNode;
        }

        [TestMethod]
        public void Parents()
        {
            /*            Int32 treeIndex;

                        // Test no parents.
                        Assert.IsNotNull(GetFactorTree(true).Parents);
                        Assert.IsTrue(GetFactorTree().Parents.IsEmpty());

                        // Test one parent.
                        GetFactorTreeNode().AddParent(GetFactorTree(true).Children[0]);
                        Assert.IsNotNull(GetFactorTreeNode().Parents);
                        Assert.AreEqual(GetFactorTreeNode().Parents.Count, 1);
                        Assert.AreEqual(GetFactorTreeNode().Parents[0], GetFactorTree().Children[0]);

                        // Test many parents.
                        for (treeIndex = 1; treeIndex < GetFactorTree().Children.Count; treeIndex++)
                        {
                            GetFactorTreeNode().AddParent(GetFactorTree().Children[treeIndex]);
                        }
                        Assert.IsNotNull(GetFactorTreeNode().Parents);
                        Assert.AreEqual(GetFactorTreeNode().Parents.Count, GetFactorTree().Children.Count);
                        for (treeIndex = 0; treeIndex < GetFactorTree().Children.Count; treeIndex++)
                        {
                            Assert.AreEqual(GetFactorTreeNode().Parents[treeIndex], GetFactorTree().Children[treeIndex]);
                        }*/
        }

        [TestMethod]
        public void Factor()
        {
            Assert.IsNotNull(GetFactorTreeNode(true).Factor);
        }

        [TestMethod]
        public void GetAllChildNodes()
        {
            Assert.IsNotNull(GetFactorTreeNode(true).GetAllChildTreeNodes());
            Assert.AreEqual(GetFactorTreeNode().Children.Count, (GetFactorTreeNode().GetAllChildTreeNodes().Count - 1));
        }
    }
}
