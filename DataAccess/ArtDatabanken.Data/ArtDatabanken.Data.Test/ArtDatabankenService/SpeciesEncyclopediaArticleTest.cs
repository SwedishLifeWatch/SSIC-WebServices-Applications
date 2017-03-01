using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class SpeciesEncyclopediaArticleTest : TestBase
    {
        private SpeciesEncyclopediaArticle _targetSpeciesEncyclopediaArticle;
        private Data.ArtDatabankenService.SpeciesFactList _targetSpeciesFacts;

        public SpeciesEncyclopediaArticleTest()
        {
            _targetSpeciesFacts = null;
            _targetSpeciesEncyclopediaArticle = null;
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

        private SpeciesEncyclopediaArticle GetTargetSpeciesEncyclopediaArticle()
        {
            Int32 taxonId = 100041; //Hasselsnok
            if (_targetSpeciesEncyclopediaArticle.IsNull())
            {
                _targetSpeciesEncyclopediaArticle = new SpeciesEncyclopediaArticle(taxonId);
            }
            return _targetSpeciesEncyclopediaArticle;
        }

        private Data.ArtDatabankenService.SpeciesFactList GetTargetSpeciesFacts()
        {
            if (_targetSpeciesFacts.IsNull())
            {
                SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
                _targetSpeciesFacts = target.SpeciesFacts;
            }
            return _targetSpeciesFacts;
        }

        /// <summary>
        ///A test for SpeciesEncyclopediaArticle Constructor1
        ///</summary>
        [TestMethod()]
        public void SpeciesEncyclopediaArticleConstructor1Test()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetTargetSpeciesFacts();
            SpeciesEncyclopediaArticle target = new SpeciesEncyclopediaArticle(speciesFacts);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for SpeciesEncyclopediaArticle Constructor2
        ///</summary>
        [TestMethod()]
        public void SpeciesEncyclopediaArticleConstructor2Test()
        {
            SpeciesEncyclopediaArticle target = new SpeciesEncyclopediaArticle(BEAR_TAXON_ID);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for UpdateDateMinValue
        ///</summary>
        [TestMethod()]
        public void UpdateDateMinValueTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
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
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
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
            SpeciesEncyclopediaArticle target = new SpeciesEncyclopediaArticle(speciesFacts);
            string actual = target.ThreatsParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for AutomaticTaxonomicParagraph
        ///</summary>
        [TestMethod()]
        public void AutomaticTaxonomicParagraphTest()
        {
            SpeciesEncyclopediaArticle target = new SpeciesEncyclopediaArticle(100768);
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
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.TaxonomicParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for TaxonomicParagraph
        ///</summary>
        [TestMethod()]
        public void IsPublishable()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            Boolean isPublishable = target.IsPublishable;
            Assert.IsTrue(isPublishable);
        }

        /// <summary>
        ///A test for Taxon
        ///</summary>
        [TestMethod()]
        public void TaxonTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
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
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.ScientificName;
            Assert.IsTrue(actual == "Coronella austriaca");
        }

        /// <summary>
        ///A test for ReferenceParagraph
        ///</summary>
        [TestMethod()]
        public void ReferenceParagraphTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.ReferenceParagraph;
            Assert.IsTrue(actual.Length > 50);
        }

        /// <summary>
        ///A test for RedlistCriteriaString
        ///</summary>
        [TestMethod()]
        public void RedlistCriteriaStringTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.RedlistCriteria;
            Assert.IsTrue(actual.Length > 5);
        }

        /// <summary>
        ///A test for RedlistCategoryShortString
        ///</summary>
        [TestMethod()]
        public void RedlistCategoryShortStringTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
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
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
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
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
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
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.MeasuresParagraph;
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        ///A test for ItalicStringsInText
        ///</summary>
        [TestMethod()]
        public void ItalicStringsInTextTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            List<String> list = target.ItalicStringsInText;
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        ///A test for ItalicStringsInReferences
        ///</summary>
        [TestMethod()]
        public void ItalicStringsInReferencesTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            List<String> list = target.ItalicStringsInReferences;
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        ///A test for ExtraParagraph
        ///</summary>
        [TestMethod()]
        public void ExtraParagraphTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.ExtraParagraph;
            Assert.IsTrue(actual != String.Empty);
        }

        /// <summary>
        ///A test for EcologyParagraph
        ///</summary>
        [TestMethod()]
        public void EcologyParagraphTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.EcologyParagraph;
            Assert.IsTrue(actual.Length > 100);
        }

        /// <summary>
        ///A test for DistributionParagraph
        ///</summary>
        [TestMethod()]
        public void DistributionParagraphTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.DistributionParagraph;
            Assert.IsTrue(actual.Length > 100);
        }

        /// <summary>
        ///A test for DescriptionParagraph
        ///</summary>
        [TestMethod()]
        public void DescriptionParagraphTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.DescriptionParagraph;
            Assert.IsTrue(actual.Length > 100);
        }

        /// <summary>
        ///A test for CommonName
        ///</summary>
        [TestMethod()]
        public void CommonNameTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.CommonName;
            Assert.IsTrue(actual == "hasselsnok");
        }

        /// <summary>
        ///A test for AuthorAndYear
        ///</summary>
        [TestMethod()]
        public void AuthorAndYearTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            String actual = target.AuthorAndYear;
            Assert.IsTrue(actual.Length > 10);
        }

        /// <summary>
        ///A test for AllSpeciesFacts
        ///</summary>
        [TestMethod()]
        public void SpeciesFactsTest()
        {
            SpeciesEncyclopediaArticle target = GetTargetSpeciesEncyclopediaArticle();
            Data.ArtDatabankenService.SpeciesFactList list = target.SpeciesFacts;
            Assert.IsNotNull(list);
        }
    }
}
