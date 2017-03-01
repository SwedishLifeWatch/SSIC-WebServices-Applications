using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using RedListCalculator = ArtDatabanken.Data.ArtDatabankenService.RedListCalculator;

    [TestClass]
    public class RedListCalculatorTest : TestBase
    {
        private const Int64 POPULATION_SIZE_TEST_MAX = 2500;
        private const Int64 POPULATION_SIZE_TEST_INCREASE = 50;

        private RedListCalculator _redListCalculator;

        public RedListCalculatorTest()
        {
            _redListCalculator = null;
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
        public void Category()
        {
            Int64 populationSizeMin, populationSizeMax;

            GetRedListCalculator(true);
            for (populationSizeMin = 0; populationSizeMin < POPULATION_SIZE_TEST_MAX; populationSizeMin += POPULATION_SIZE_TEST_INCREASE)
            {
                for (populationSizeMax = populationSizeMin; populationSizeMax < POPULATION_SIZE_TEST_MAX; populationSizeMax += POPULATION_SIZE_TEST_INCREASE)
                {
                    GetRedListCalculator().SetPopulationSize(true,
                                                             populationSizeMin,
                                                             false,
                                                             0,
                                                             true,
                                                             populationSizeMax,
                                                             null);
                    if ((GetRedListCalculator().CategoryBestCaseGraded == RedListCategory.LC) &&
                         ((GetRedListCalculator().CategoryWorstCaseGraded == RedListCategory.CR) ||
                          (GetRedListCalculator().CategoryWorstCaseGraded == RedListCategory.RE)))
                    {
                        Assert.AreEqual(RedListCategory.DD, GetRedListCalculator().Category);
                    }
                    else
                    {
                        Assert.AreNotEqual(RedListCategory.DD, GetRedListCalculator().Category);
                    }
                }
            }
        }

        private RedListCalculator GetRedListCalculator()
        {
            return GetRedListCalculator(false);
        }

        private RedListCalculator GetRedListCalculator(Boolean refresh)
        {
            if (_redListCalculator.IsNull() || refresh)
            {
                _redListCalculator = new RedListCalculator(null, null);
            }
            return _redListCalculator;
        }
    }
}
