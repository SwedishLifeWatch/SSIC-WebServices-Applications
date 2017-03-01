using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonCountyOccurrenceListTest : TestBase
    {
        private TaxonCountyOccurrenceList _countyOccurrencies;

        public TaxonCountyOccurrenceListTest()
        {
            _countyOccurrencies = null;
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
        public void GetByCountyIdentifier()
        {
            TaxonCountyOccurrence countyOccurrence;

            foreach (County county in GeographicManager.GetCounties())
            {
                // Kalmar county is divided into "Öland"
                // and "Kalmar fastland".
                if (county.Identifier.ToLower() != "h")
                {
                    countyOccurrence = GetCountyOccurrencies().GetByCountyIdentifier(county.Identifier);
                    Assert.IsNotNull(countyOccurrence);
                    Assert.AreEqual(county, countyOccurrence.County);
                }
            }
        }

        private TaxonCountyOccurrenceList GetCountyOccurrencies()
        {
            return GetCountyOccurrencies(false);
        }

        private TaxonCountyOccurrenceList GetCountyOccurrencies(Boolean refresh)
        {
            if (_countyOccurrencies.IsNull() || refresh)
            {
                _countyOccurrencies = SpeciesFactManagerTest.GetSomeTaxonCountyOccurencies();
            }
            return _countyOccurrencies;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 countyOccurrenceIndex;
            TaxonCountyOccurrenceList newCountyOccurrencies, oldCountyOccurrencies;

            oldCountyOccurrencies = GetCountyOccurrencies();
            newCountyOccurrencies = new TaxonCountyOccurrenceList();
            for (countyOccurrenceIndex = 0; countyOccurrenceIndex < oldCountyOccurrencies.Count; countyOccurrenceIndex++)
            {
                newCountyOccurrencies.Add(oldCountyOccurrencies[oldCountyOccurrencies.Count - countyOccurrenceIndex - 1]);
            }
            for (countyOccurrenceIndex = 0; countyOccurrenceIndex < oldCountyOccurrencies.Count; countyOccurrenceIndex++)
            {
                Assert.AreEqual(newCountyOccurrencies[countyOccurrenceIndex], oldCountyOccurrencies[oldCountyOccurrencies.Count - countyOccurrenceIndex - 1]);
            }
        }
    }
}
