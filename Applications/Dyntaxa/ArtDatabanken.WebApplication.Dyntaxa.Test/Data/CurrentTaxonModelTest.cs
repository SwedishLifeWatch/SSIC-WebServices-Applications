//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.Dyntaxa.Data;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
//{
//    /// <summary>
//    /// Test parameters for dyntaxa models
//    /// </summary>
//    public struct TestParameters
//    {
//        public const string DEFAULT_TEST_TAXON_ID = "1";
//        public const string DEFAULT_TEST_TAXON_SCIENTIFIC_NAME = "Abortiporus biennis";
//        public const string DEFAULT_TEST_TAXON_COMMON_NAME = "klumpticka";
//        public const string SIBIRIAN_JAY_TAXON_ID = "103031";
//        public const string GAMMARUS_TAXON_ID = "1009327";
//        public const string PROBLEMATC_TEST_TAXON_ID = "2";
//        public const string LUMPED_TEST_TAXON_ID = "3";
//        public const string CARABIDAE_TAXON_ID = "2001007";
//        public const int MISSING_TAXON_ID = 100;
//    }

//    /// <summary>
//    /// Test class for Current taxon model
//    /// </summary>
//    [TestClass]
//    public class CurrentTaxonModelTest : TestBase
//    {
//        private CurrentTaxonModel _model;

//        public CurrentTaxonModelTest()
//        {
//            _model = null;
//        }

//        private CurrentTaxonModel GetModel(Boolean refresh)
//        {
//            if (_model.IsNull() || refresh)
//            {
//                _model = new CurrentTaxonModel(TestParameters.DEFAULT_TEST_TAXON_ID);
//            }
//            return _model;
//        }

//        [TestMethod]
//        public void AuthorTest()
//        {
//            CurrentTaxonModel model = GetModel(false);
//            Assert.AreEqual("(Bull. : Fr.) Singer", model.Author);
//        }

//        [TestMethod]
//        public void CommonNameTest()
//        {
//            CurrentTaxonModel model = GetModel(false);
//            Assert.AreEqual(TestParameters.DEFAULT_TEST_TAXON_COMMON_NAME, model.CommonName);

//            //Common name should be in lower case by default.
//            Assert.AreNotEqual("Klumpticka", model.CommonName);
//        }

//        [TestMethod]
//        public void ScientificNameTest()
//        {
//            CurrentTaxonModel model = GetModel(false);
//            Assert.AreEqual(TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME, model.ScientificName);
//        }

//        [TestMethod]
//        public void TaxonCategoryTest()
//        {
//            CurrentTaxonModel model = GetModel(false);
//            Assert.AreEqual("Art", model.TaxonCategory);
//        }

//        [TestMethod]
//        public void TaxonIdTest()
//        {
//            Assert.AreEqual(TestParameters.DEFAULT_TEST_TAXON_ID, GetModel(true).TaxonId);
//        }

//        [TestMethod]
//        public void InputIdTest()
//        {
//            Assert.AreEqual(TestParameters.DEFAULT_TEST_TAXON_ID, GetModel(true).InputId);
//        }
//    }
//}


