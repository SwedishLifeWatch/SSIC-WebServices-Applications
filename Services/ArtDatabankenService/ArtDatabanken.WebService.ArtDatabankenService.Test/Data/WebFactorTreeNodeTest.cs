using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    /// <summary>
    /// Summary description for WebFactorTreeNodeTest
    /// </summary>
    [TestClass]
    public class WebFactorTreeNodeTest : TestBase
    {
        private WebFactorTreeNode _factorTree;
        private WebFactorTreeNode _factorTreeNode;

        public WebFactorTreeNodeTest()
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
            Int32 childIndex;

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
            }
        }

        [TestMethod]
        public void AddParent()
        {
            Int32 parentIndex;

            // Add one parent.
            GetFactorTreeNode(true).AddParent(GetFactorTree(true).Children[0]);

            // Add many parents.
            for (parentIndex = 1; parentIndex < GetFactorTree().Children.Count; parentIndex++)
            {
                GetFactorTreeNode().AddParent(GetFactorTree().Children[parentIndex]);
            }
        }

        [TestMethod]
        public void Children()
        {
            Int32 childIndex;
            List<WebFactorTreeNode> _children;

            // Test null;
            _children = null;
            GetFactorTreeNode(true).Children = _children;
            Assert.IsNull(GetFactorTreeNode().Children);

            // Test no children.
            _children = new List<WebFactorTreeNode>();
            GetFactorTreeNode().Children = _children;
            Assert.IsNotNull(GetFactorTreeNode().Children);
            Assert.IsTrue(GetFactorTreeNode().Children.IsEmpty());

            // Test one child.
            _children.Add(GetFactorTree(true).Children[0]);
            GetFactorTreeNode().Children = _children;
            Assert.IsNotNull(GetFactorTreeNode().Children);
            Assert.AreEqual(GetFactorTreeNode().Children.Count, 1);
            Assert.AreEqual(GetFactorTreeNode().Children[0], GetFactorTree().Children[0]);

            // Test many children.
            for (childIndex = 1; childIndex < GetFactorTree().Children.Count; childIndex++)
            {
                _children.Add(GetFactorTree().Children[childIndex]);
            }
            GetFactorTreeNode().Children = _children;
            Assert.IsNotNull(GetFactorTreeNode().Children);
            Assert.AreEqual(GetFactorTreeNode().Children.Count, GetFactorTree().Children.Count);
            for (childIndex = 0; childIndex < GetFactorTree().Children.Count; childIndex++)
            {
                Assert.AreEqual(GetFactorTreeNode().Children[childIndex], GetFactorTree().Children[childIndex]);
            }
        }

        public WebFactorTreeNode GetFactorTree()
        {
            return GetFactorTree(false);
        }

        public WebFactorTreeNode GetFactorTree(Boolean refresh)
        {
            if (_factorTree.IsNull() || refresh)
            {
                _factorTree = FactorManagerTest.GetRedlistFactorTreeNode(GetContext());
            }
            return _factorTree;
        }

        public WebFactorTreeNode GetFactorTreeNode()
        {
            return GetFactorTreeNode(false);
        }

        public WebFactorTreeNode GetFactorTreeNode(Boolean refresh)
        {
            if (_factorTreeNode.IsNull() || refresh)
            {
                _factorTreeNode = FactorManagerTest.GetForestFactorTreeNode(GetContext());
            }
            return _factorTreeNode;
        }

        [TestMethod]
        public void HasParent()
        {
            // Test with factor tree node that has no parent.
            Assert.IsFalse(GetFactorTree(true).HasParent(GetFactorTreeNode(true)));
            Assert.IsTrue(GetFactorTreeNode().HasParent(GetFactorTreeNode()));

            // Test with factor tree node that has a parent.
            GetFactorTreeNode().AddParent(GetFactorTree());
            Assert.IsTrue(GetFactorTreeNode().HasParent(GetFactorTree()));
            Assert.IsTrue(GetFactorTreeNode().HasParent(GetFactorTreeNode()));
            Assert.IsFalse(GetFactorTreeNode().HasParent(GetFactorTree().Children[0]));
        }

        [TestMethod]
        public void Factor()
        {
            WebFactor factor;

            factor = null;
            GetFactorTreeNode(true).Factor = factor;
            Assert.IsNull(GetFactorTreeNode().Factor);

            factor = FactorManagerTest.GetForestFactor(GetContext());
            GetFactorTreeNode().Factor = factor;
            Assert.IsNotNull(GetFactorTreeNode().Factor);
            Assert.AreEqual(GetFactorTreeNode().Factor, factor);
        }
    }
}
