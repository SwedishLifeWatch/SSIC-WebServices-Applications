using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass()]
    public class SpeciesInformationDocumentTest : TestBase
    {
        private SpeciesInformationDocument _targetSpeciesInformationDocument;
        private Data.ArtDatabankenService.SpeciesFactList _targetSpeciesFacts;

        public SpeciesInformationDocumentTest()
        {
            _targetSpeciesFacts = null;
            _targetSpeciesInformationDocument = null;
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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private SpeciesInformationDocument GetTargetSpeciesInformationDocument()
        {
            Int32 taxonId = 100041; //Hasselsnok
            
            if (_targetSpeciesInformationDocument.IsNull())
            {
                _targetSpeciesInformationDocument = new SpeciesInformationDocument(taxonId);
            }
            return _targetSpeciesInformationDocument;
        }

        private Data.ArtDatabankenService.SpeciesFactList GetTargetSpeciesFacts()
        {
            if (_targetSpeciesFacts.IsNull())
            {
                SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
                _targetSpeciesFacts = target.SpeciesFacts;
            }
            return _targetSpeciesFacts;
        }

        /// <summary>
        ///A test for SpeciesInformationDocument Constructor1
        ///</summary>
        [TestMethod()]
        public void SpeciesInformationDocumentConstructor1Test()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetTargetSpeciesFacts();
            SpeciesInformationDocument target = new SpeciesInformationDocument(speciesFacts);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for SpeciesInformationDocument Constructor2
        ///</summary>
        [TestMethod()]
        public void SpeciesInformationDocumentConstructor2Test()
        {
            SpeciesInformationDocument target = new SpeciesInformationDocument(BEAR_TAXON_ID);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for UpdateDateMinValue
        ///</summary>
        [TestMethod()]
        public void UpdateDateMinValueTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            DateTime today = DateTime.Today;
            DateTime actual = target.UpdateDateMinValue;
            Assert.IsTrue(actual < today);
        }

        /// <summary>
        ///A test for UpdateDateMaxValue
        ///</summary>
        [TestMethod()]
        public void UpdateDateMaxValueTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            DateTime test = DateTime.MinValue;
            DateTime actual = target.UpdateDateMaxValue;
            Assert.IsTrue(actual > test);
        }

        /// <summary>
        ///A test for ThreatsParagraph
        ///</summary>
        [TestMethod()]
        public void ThreatsParagraphTest()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetTargetSpeciesFacts();
            SpeciesInformationDocument target = new SpeciesInformationDocument(speciesFacts);
            string actual = target.ThreatsParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for AutomaticTaxonomicParagraph
        ///</summary>
        [TestMethod()]
        public void AutomaticTaxonomicParagraphTest()
        {
            SpeciesInformationDocument target = new SpeciesInformationDocument(100768);
            String actual = target.AutomaticTaxonomicParagraph;
            Assert.IsTrue(actual.Length > 50);
            Int32 pos = actual.IndexOf("Cucullia scrophulariae");
            Assert.IsTrue(pos > 0);
        }

        /// <summary>
        ///A test for TaxonomicParagraph
        ///</summary>
        [TestMethod()]
        public void TaxonomicParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.TaxonomicParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for PreambleParagraph
        ///</summary>
        [TestMethod()]
        public void PreambleParagraphTest()
        {
            SpeciesInformationDocument target = new SpeciesInformationDocument(100046); //vitryggig hackspett
            String actual = target.PreambleParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for TaxonomicParagraph
        ///</summary>
        [TestMethod()]
        public void IsPublishable()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            Boolean isPublishable = target.IsPublishable;
            Assert.IsTrue(isPublishable);
        }

        /// <summary>
        ///A test for Taxon
        ///</summary>
        [TestMethod()]
        public void TaxonTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            ArtDatabanken.Data.ArtDatabankenService.Taxon actual = target.Taxon;
            Assert.IsTrue(actual.ScientificName == target.ScientificName);
            Assert.IsTrue(actual.CommonName == target.CommonName);
        }

        /// <summary>
        ///A test for ScientificName
        ///</summary>
        [TestMethod()]
        public void ScientificNameTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.ScientificName;
            Assert.IsTrue(actual == "Coronella austriaca");
        }

        /// <summary>
        ///A test for ReferenceParagraph
        ///</summary>
        [TestMethod()]
        public void ReferenceParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.ReferenceParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for RedlistCriteriaString
        ///</summary>
        [TestMethod()]
        public void RedlistCriteriaStringTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.RedlistCriteria;
            Assert.IsTrue(actual.Length > 5);
        }

        /// <summary>
        ///A test for RedlistCategoryShortString
        ///</summary>
        [TestMethod()]
        public void RedlistCategoryShortStringTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.RedlistCategoryShortString;
            Assert.IsTrue(actual.Length > 0);
            Assert.IsTrue(actual.Length < 4);

        }

        /// <summary>
        ///A test for RedlistCategoryName
        ///</summary>
        [TestMethod()]
        public void RedlistCategoryNameTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.RedlistCategoryName;
            Assert.IsTrue(actual.Length > 0);
            Assert.IsTrue(actual.Length < 200);
        }

        /// <summary>
        ///A test for OrganismGroup
        ///</summary>
        [TestMethod()]
        public void OrganismGroupTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.OrganismGroup;
            String expected = "Grod- och kräldjur";
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        ///A test for MeasuresParagraph
        ///</summary>
        [TestMethod()]
        public void MeasuresParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.MeasuresParagraph;
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        ///A test for ItalicStringsInText
        ///</summary>
        [TestMethod()]
        public void ItalicStringsInTextTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            List<String> list = target.ItalicStringsInText;
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        ///A test for ItalicStringsInReferences
        ///</summary>
        [TestMethod()]
        public void ItalicStringsInReferencesTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            List<String> list = target.ItalicStringsInReferences;
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        ///A test for ExtraParagraph
        ///</summary>
        [TestMethod()]
        public void ExtraParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.ExtraParagraph;
            Assert.IsTrue(actual != String.Empty);
        }

        /// <summary>
        ///A test for EcologyParagraph
        ///</summary>
        [TestMethod()]
        public void EcologyParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.EcologyParagraph;
            Assert.IsTrue(actual.Length > 100);
        }

        /// <summary>
        ///A test for DistributionParagraph
        ///</summary>
        [TestMethod()]
        public void DistributionParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.DistributionParagraph;
            Assert.IsTrue(actual.Length > 100);
        }

        /// <summary>
        ///A test for DescriptionParagraph
        ///</summary>
        [TestMethod()]
        public void DescriptionParagraphTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.DescriptionParagraph;
            Assert.IsTrue(actual.Length > 100);
        }

        /// <summary>
        ///A test for CommonName
        ///</summary>
        [TestMethod()]
        public void CommonNameTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.CommonName;
            Assert.IsTrue(actual == "hasselsnok");
        }

        /// <summary>
        ///A test for AuthorAndYear
        ///</summary>
        [TestMethod()]
        public void AuthorAndYearTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            String actual = target.AuthorAndYear;
            Assert.IsTrue(actual.Length > 10);
        }

        /// <summary>
        ///A test for AllSpeciesFacts
        ///</summary>
        [TestMethod()]
        public void SpeciesFactsTest()
        {
            SpeciesInformationDocument target = GetTargetSpeciesInformationDocument();
            Data.ArtDatabankenService.SpeciesFactList list = target.SpeciesFacts;
            Assert.IsNotNull(list);
        }
    }
}
