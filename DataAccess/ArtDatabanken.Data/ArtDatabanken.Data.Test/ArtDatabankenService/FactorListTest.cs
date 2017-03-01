using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorList
    /// </summary>
    [TestClass]
    public class FactorListTest : TestBase
    {
        private Data.ArtDatabankenService.FactorList _factors;

        public FactorListTest()
        {
            _factors = null;
        }

        #region Additional test attributes
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
            foreach (Factor factor in GetFactors())
            {
                Assert.AreEqual(factor, GetFactors().Get(factor.Id));
            }
        }

        [TestMethod]
        public void Exists()
        {
            Data.ArtDatabankenService.FactorList factorList;
            factorList = GetFactors();

            Assert.IsTrue(factorList.Exists(Data.ArtDatabankenService.FactorManager.GetFactor(656)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 factorId;

            factorId = Int32.MinValue;
            GetFactors().Get(factorId);
        }

        private Data.ArtDatabankenService.FactorList GetFactors()
        {
            if (_factors.IsNull())
            {
                _factors = Data.ArtDatabankenService.FactorManager.GetFactors();
            }
            return _factors;
        }

        [TestMethod]
        public void GetFactorsBySearchString()
        {
            Data.ArtDatabankenService.FactorList factors = GetFactors();

            Data.ArtDatabankenService.FactorList subset = factors.GetFactorsBySearchString("A", StringComparison.CurrentCultureIgnoreCase);
            Assert.IsNotNull(subset);
            String firstString = subset[0].Label;
            Data.ArtDatabankenService.FactorList subset1 = factors.GetFactorsBySearchString(firstString, StringComparison.CurrentCultureIgnoreCase);
            Assert.IsNotNull(subset1);
        }
    }
}
