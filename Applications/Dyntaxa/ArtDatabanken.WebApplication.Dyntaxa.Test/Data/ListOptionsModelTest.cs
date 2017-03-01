//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.Dyntaxa.Data;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
//{
//    /// <summary>
//    /// Test class for List options model
//    /// </summary>
//    [TestClass]
//    public class ListOptionsModelTest : TestBase
//    {
//        private ListOptionsModel _model;

//        public ListOptionsModelTest()
//        {
//            _model = null;
//        }

//        private ListOptionsModel GetModel(Boolean refresh)
//        {
//            if (_model.IsNull() || refresh)
//            {
//                _model = new ListOptionsModel(TestParameters.DEFAULT_TEST_TAXON_ID);
//            }
//            return _model;
//        }

//        [TestMethod]
//        public void AuthorTest()
//        {
//            ListOptionsModel model = GetModel(false);
//            Assert.IsTrue(model.Author == "(Bull. : Fr.) Singer");
//        }

//        [TestMethod]
//        public void CommonNameTest()
//        {
//            ListOptionsModel model = GetModel(false);
//            Assert.AreEqual(model.CommonName, "klumpticka");
//            Assert.AreNotEqual(model.CommonName, "Klumpticka");
//        }

//        [TestMethod]
//        public void ScientificNameTest()
//        {
//            ListOptionsModel model = GetModel(false);
//            Assert.AreEqual(model.ScientificName, TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME);
//        }

//        [TestMethod]
//        public void TaxonCategoryTest()
//        {
//            ListOptionsModel model = GetModel(false);
//            Assert.AreEqual(model.TaxonCategory, "Art");
//        }

//        [TestMethod]
//        public void TaxonIdTest()
//        {
//            Assert.IsTrue(GetModel(true).TaxonId == TestParameters.DEFAULT_TEST_TAXON_ID);
//        }

//        [TestMethod]
//        public void ListTypeTest()
//        {
//            ListOptionsModel model = GetModel(true);
//            Assert.AreEqual(TaxonListType.Hierarchical, model.ListType);
//            model.ListType = TaxonListType.TaxonName;
//            Assert.AreEqual(TaxonListType.TaxonName, model.ListType);
//        }

//        [TestMethod]
//        public void ChildTaxonCategoriesTest()
//        {
//            ListOptionsModel model = new ListOptionsModel(TestParameters.CARABIDAE_TAXON_ID);
//            List<ReadTaxonCategoryItem> list = model.RestrictListToTaxonCategories;
//            Assert.IsTrue(list.Count > 4);
//        }

//        [TestMethod]
//        public void ParentTaxonCategoriesTest()
//        {
//            ListOptionsModel model = new ListOptionsModel(TestParameters.CARABIDAE_TAXON_ID);
//            List<ReadTaxonCategoryItem> list = model.RestrictListToParentTaxonCategories;
//            Assert.IsTrue(list.Count > 4);
//        }
//    }
//}

