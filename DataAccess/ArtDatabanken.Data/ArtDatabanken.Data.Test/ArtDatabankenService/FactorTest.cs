using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorTest
    /// </summary>
    [TestClass]
    public class FactorTest : TestBase
    {
        private Data.ArtDatabankenService.Factor _factor;

        public FactorTest()
        {
            _factor = null;
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

        private Data.ArtDatabankenService.Factor GetFactor()
        {
            return GetFactor(false);
        }

        private Data.ArtDatabankenService.Factor GetFactor(Boolean refresh)
        {
            if (_factor.IsNull() || refresh)
            {
                _factor = FactorManagerTest.GetFirstFactor();
            }
            return _factor;
        }

        [TestMethod]
        public void IsLeaf()
        {
            Boolean isLeaf;

            isLeaf = GetFactor(true).IsLeaf;
            Assert.IsTrue(isLeaf || !isLeaf);

            isLeaf = false;
            GetFactor().IsLeaf = isLeaf;
            Assert.AreEqual(GetFactor().IsLeaf, isLeaf);

            isLeaf = true;
            GetFactor().IsLeaf = isLeaf;
            Assert.AreEqual(GetFactor().IsLeaf, isLeaf);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetFactor().Name.IsEmpty());
        }

        [TestMethod]
        public void Label()
        {
            Assert.IsTrue(GetFactor().Label.IsNotEmpty());
        }

        [TestMethod]
        public void Information()
        {
            Assert.IsTrue(GetFactor().Information.IsEmpty());
        }

        [TestMethod]
        public void IsTaxonomic()
        {
            Assert.IsFalse(GetFactor().IsTaxonomic);
        }

        [TestMethod]
        public void IsPeriodic()
        {
            Boolean isPeriodic;

            isPeriodic = GetFactor().IsPeriodic;
        }

        [TestMethod]
        public void HostLabel()
        {
            Assert.IsNull(GetFactor().HostLabel);
        }

        [TestMethod]
        public void DefaultHostParentTaxonId()
        {
            Assert.AreEqual(GetFactor().DefaultHostParentTaxonId, 0);
        }

        [TestMethod]
        public void IsPublic()
        {
            Assert.IsTrue(GetFactor().IsPublic);
        }

        [TestMethod]
        public void FactorUpdateMode()
        {
            Assert.AreEqual(GetFactor().FactorUpdateMode.Name, Data.ArtDatabankenService.FactorManager.GetFactorUpdateMode(0).Name);
        }

        [TestMethod]
        public void FactorDataType()
        {
            Assert.IsNull(GetFactor().FactorDataType);
        }

        [TestMethod]
        public void FactorOrigin()
        {
            Assert.IsNotNull(GetFactor().FactorOrigin);
        }

        [TestMethod]
        public void Tree()
        {
            Data.ArtDatabankenService.FactorTreeNode tree;

            tree = GetFactor().Tree;
            Assert.IsNotNull(tree);
            Assert.AreEqual(GetFactor().Id, tree.Id);
        }
    }
}
