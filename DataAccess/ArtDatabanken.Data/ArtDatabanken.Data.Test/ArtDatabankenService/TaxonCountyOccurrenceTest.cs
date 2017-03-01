using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonCountyOccurrenceTest : TestBase
    {
        private TaxonCountyOccurrence _countyOccurrence;

        public TaxonCountyOccurrenceTest()
        {
            _countyOccurrence = null;
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
        public void ArtDataId()
        {
            Int32 artDataId;

            artDataId = GetCountyOccurrence().ArtDataId;
        }

        [TestMethod]
        public void County()
        {
            Assert.IsNotNull(GetCountyOccurrence().County);
        }

        [TestMethod]
        public void CountyOccurrence()
        {
            Assert.IsTrue(GetCountyOccurrence().CountyOccurrence.IsNotEmpty());
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonCountyOccurrence countyOccurrence;

            countyOccurrence = GetCountyOccurrence();
            Assert.IsNotNull(countyOccurrence);
        }

        private TaxonCountyOccurrence GetCountyOccurrence()
        {
            return GetCountyOccurrence(false);
        }

        private TaxonCountyOccurrence GetCountyOccurrence(Boolean refresh)
        {
            if (_countyOccurrence.IsNull() || refresh)
            {
                _countyOccurrence = SpeciesFactManagerTest.GetOneTaxonCountyOccurrence();
            }
            return _countyOccurrence;
        }

        [TestMethod]
        public void HasArtDataId()
        {
            Boolean hasArtDataId;

            hasArtDataId = GetCountyOccurrence().HasArtDataId;
        }

        [TestMethod]
        public void HasSourceId()
        {
            Boolean hasSourceId;

            hasSourceId = GetCountyOccurrence().HasSourceId;
        }

        [TestMethod]
        public void OriginalCountyOccurrence()
        {
            String originalCountyOccurrence;

            originalCountyOccurrence = GetCountyOccurrence().OriginalCountyOccurrence;
        }

        [TestMethod]
        public void Source()
        {
            String source;

            source = GetCountyOccurrence().Source;
        }

        [TestMethod]
        public void SourceId()
        {
            Int32 sourceId;

            sourceId = GetCountyOccurrence().SourceId;
        }

        [TestMethod]
        public void Taxon()
        {
            Assert.IsNotNull(GetCountyOccurrence().Taxon);
        }
    }
}
