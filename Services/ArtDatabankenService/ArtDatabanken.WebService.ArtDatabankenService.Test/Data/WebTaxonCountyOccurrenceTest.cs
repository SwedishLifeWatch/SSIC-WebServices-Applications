using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebTaxonCountyOccurrenceTest : TestBase
    {
        private WebTaxonCountyOccurrence _countyOccurrence;

        public WebTaxonCountyOccurrenceTest()
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

            artDataId = 42;
            GetCountyOccurrence(true).ArtDataId = artDataId;
            Assert.AreEqual(artDataId, GetCountyOccurrence().ArtDataId);
        }

        [TestMethod]
        public void CountyId()
        {
            Int32 countyId;

            countyId = 42;
            GetCountyOccurrence(true).CountyId = countyId;
            Assert.AreEqual(countyId, GetCountyOccurrence().CountyId);
        }

        [TestMethod]
        public void CountyOccurrence()
        {
            String countyOccurrence;

            countyOccurrence = null;
            GetCountyOccurrence(true).CountyOccurrence = countyOccurrence;
            Assert.IsNull(GetCountyOccurrence().CountyOccurrence);
            countyOccurrence = "";
            GetCountyOccurrence().CountyOccurrence = countyOccurrence;
            Assert.IsTrue(GetCountyOccurrence().CountyOccurrence.IsEmpty());
            countyOccurrence = "Test county occurrence";
            GetCountyOccurrence().CountyOccurrence = countyOccurrence;
            Assert.AreEqual(countyOccurrence, GetCountyOccurrence().CountyOccurrence);
        }

        private WebTaxonCountyOccurrence GetCountyOccurrence()
        {
            return GetCountyOccurrence(false);
        }

        private WebTaxonCountyOccurrence GetCountyOccurrence(Boolean refresh)
        {
            if (_countyOccurrence.IsNull() || refresh)
            {
                _countyOccurrence = SpeciesFactManagerTest.GetOneTaxonCountyOccurrence(GetContext());
            }
            return _countyOccurrence;
        }

        [TestMethod]
        public void HasArtDataId()
        {
            Boolean hasArtDataId;

            hasArtDataId = false;
            GetCountyOccurrence(true).IsArtDataIdSpecified = hasArtDataId;
            Assert.AreEqual(hasArtDataId, GetCountyOccurrence().IsArtDataIdSpecified);
            hasArtDataId = true;
            GetCountyOccurrence().IsArtDataIdSpecified = hasArtDataId;
            Assert.AreEqual(hasArtDataId, GetCountyOccurrence().IsArtDataIdSpecified);
        }

        [TestMethod]
        public void HasSourceId()
        {
            Boolean hasSourceId;

            hasSourceId = false;
            GetCountyOccurrence(true).IsSourceIdSpecified = hasSourceId;
            Assert.AreEqual(hasSourceId, GetCountyOccurrence().IsSourceIdSpecified);
            hasSourceId = true;
            GetCountyOccurrence().IsSourceIdSpecified = hasSourceId;
            Assert.AreEqual(hasSourceId, GetCountyOccurrence().IsSourceIdSpecified);
        }

        [TestMethod]
        public void OriginalCountyOccurrence()
        {
            String originalCountyOccurrence;

            originalCountyOccurrence = null;
            GetCountyOccurrence(true).OriginalCountyOccurrence = originalCountyOccurrence;
            Assert.IsNull(GetCountyOccurrence().OriginalCountyOccurrence);
            originalCountyOccurrence = "";
            GetCountyOccurrence().OriginalCountyOccurrence = originalCountyOccurrence;
            Assert.IsTrue(GetCountyOccurrence().OriginalCountyOccurrence.IsEmpty());
            originalCountyOccurrence = "Test original county occurrence";
            GetCountyOccurrence().OriginalCountyOccurrence = originalCountyOccurrence;
            Assert.AreEqual(originalCountyOccurrence, GetCountyOccurrence().OriginalCountyOccurrence);
        }

        [TestMethod]
        public void Source()
        {
            String source;

            source = null;
            GetCountyOccurrence(true).Source = source;
            Assert.IsNull(GetCountyOccurrence().Source);
            source = "";
            GetCountyOccurrence().Source = source;
            Assert.IsTrue(GetCountyOccurrence().Source.IsEmpty());
            source = "Test source";
            GetCountyOccurrence().Source = source;
            Assert.AreEqual(source, GetCountyOccurrence().Source);
        }

        [TestMethod]
        public void SourceId()
        {
            Int32 sourceId;

            sourceId = 42;
            GetCountyOccurrence(true).SourceId = sourceId;
            Assert.AreEqual(sourceId, GetCountyOccurrence().SourceId);
        }

        [TestMethod]
        public void TaxonId()
        {
            Int32 taxonId;

            taxonId = 42;
            GetCountyOccurrence(true).TaxonId = taxonId;
            Assert.AreEqual(taxonId, GetCountyOccurrence().TaxonId);
        }
    }
}
