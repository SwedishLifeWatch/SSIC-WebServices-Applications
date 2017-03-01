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
    /// Summary description for FactorTreeNodeListTest
    /// </summary>
    [TestClass]
    public class FactorTreeNodeListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorTreeNodeList _factorTrees;

        public FactorTreeNodeListTest()
        {
            _factorTrees = null;
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
            foreach (FactorTreeNode factorTree in GetFactorTrees())
            {
                Assert.AreEqual(factorTree, GetFactorTrees().Get(factorTree.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorId;

            factorId = Int32.MinValue;
            GetFactorTrees().Get(factorId);
        }

        private Data.ArtDatabankenService.FactorTreeNodeList GetFactorTrees()
        {
            if (_factorTrees.IsNull())
            {
                _factorTrees = FactorManagerTest.GetForestFactorTreeNode().Children;
            }
            return _factorTrees;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 factorTreeIndex;
            Data.ArtDatabankenService.FactorTreeNodeList newFactorTreeList, oldFactorTreeList;

            oldFactorTreeList = GetFactorTrees();
            newFactorTreeList = new Data.ArtDatabankenService.FactorTreeNodeList();
            for (factorTreeIndex = 0; factorTreeIndex < oldFactorTreeList.Count; factorTreeIndex++)
            {
                newFactorTreeList.Add(oldFactorTreeList[oldFactorTreeList.Count - factorTreeIndex - 1]);
            }
            for (factorTreeIndex = 0; factorTreeIndex < oldFactorTreeList.Count; factorTreeIndex++)
            {
                Assert.AreEqual(newFactorTreeList[factorTreeIndex], oldFactorTreeList[oldFactorTreeList.Count - factorTreeIndex - 1]);
            }
        }
    }
}
