//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.Dyntaxa.Data;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
//{
    

//    /// <summary>
//    /// Summary description for TaxonConceptDescriptionModelTest
//    /// </summary>
//    [TestClass]
//    public class ReadTaxonInformationModelTest : TestBase
//    {
//        private ReadTaxonInformationModel _model;


//        public ReadTaxonInformationModelTest()
//        {
//            _model = null;
//        }

//        private ReadTaxonInformationModel GetModel(Boolean refresh)
//        {
//            if (_model.IsNull() || refresh)
//            {
//                _model = new ReadTaxonInformationModel(TestParameters.DEFAULT_TEST_TAXON_ID);
//            }
//            return _model;
//        }

//        [TestMethod]
//        public void OtherRecommendedNamesTest()
//        {
//            //Lavskrika
//            ReadTaxonInformationModel model = new ReadTaxonInformationModel(TestParameters.SIBIRIAN_JAY_TAXON_ID);
//            Assert.IsTrue(model.OtherRecommendedNames.Count > 3);
//        }

//        [TestMethod]
//        public void ITISLinkTest()
//        {
//            //Gammarus is a taxon with ITIS number
//            ReadTaxonInformationModel model = new ReadTaxonInformationModel(TestParameters.GAMMARUS_TAXON_ID);
//            int count = 0;
//            foreach (LinkItem item in model.RecommendedLinks)
//            {
//                if (item.LinkText == "ITIS") count++;
//            }
//            Assert.IsTrue(count == 1);
//        }

//        [TestMethod]
//        public void NumbersOfChildTaxaTest()
//        {
//            //Gammarus is a taxon with child taxa
//            ReadTaxonInformationModel model = new ReadTaxonInformationModel(TestParameters.GAMMARUS_TAXON_ID);
//            Assert.IsTrue(model.NumbersOfChildTaxa.Count > 1);
//            Assert.IsTrue(model.NumbersOfChildTaxa[0].NationalTaxonCount <= model.NumbersOfChildTaxa[0].TotalTaxonCount);
//        }

//        [TestMethod]
//        public void ParentTaxaTest()
//        {
//            ReadTaxonInformationModel model = GetModel(true);
//            Assert.IsTrue(model.ParentTaxa.Count > 4);
//        }

//        [TestMethod]
//        public void NearesChildTaxaTest()
//        {
//            ReadTaxonInformationModel model = new ReadTaxonInformationModel(TestParameters.GAMMARUS_TAXON_ID);
//            Assert.IsTrue(model.NearestChildTaxa.Count > 1);
//        }


//        [TestMethod]
//        public void AnamorphAuthorTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            model.AnamorphAuthor = "Hej";
//            Assert.IsTrue(model.AnamorphAuthor == "Hej");
//        }

//        [TestMethod]
//        public void AnamorphNameTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            model.AnamorphName = "Hej";
//            Assert.IsTrue(model.AnamorphName == "Hej");
//        }

//        [TestMethod]
//        public void AuthorTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsTrue(model.Author == "(Bull. : Fr.) Singer");
//        }

//        [TestMethod]
//        public void CommonNameTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.AreEqual(model.CommonName, "klumpticka");
//            Assert.AreNotEqual(model.CommonName, "Klumpticka");
//        }

//        [TestMethod]
//        public void ScientificNameTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.AreEqual(model.ScientificName, TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME);
//        }

//        [TestMethod]
//        public void ScientificSynonymsTest()
//        {
//            ReadTaxonInformationModel model = GetModel(true);
//            Assert.IsTrue(model.ScientificSynonyms[0].ValidToDate < DateTime.Now);
//            Assert.IsTrue(model.ScientificSynonyms[0].Reference.Length > 20);
//        }

//        [TestMethod]
//        public void ShowChildTaxonListTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowChildTaxonList);
//            model.ShowChildTaxonList = true;
//            Assert.IsTrue(model.ShowChildTaxonList);
//        }

//        [TestMethod]
//        public void ShowCommentsTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowComments);
//            model.ShowComments = true;
//            Assert.IsTrue(model.ShowComments);
//        }

//        [TestMethod]
//        public void ShowHistoryTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowHistory);
//            model.ShowHistory = true;
//            Assert.IsTrue(model.ShowHistory);
//        }

//        [TestMethod]
//        public void ShowIdentifierListTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowIdentifierList);
//            model.ShowIdentifierList = true;
//            Assert.IsTrue(model.ShowIdentifierList);
//        }

//        [TestMethod]
//        public void ShowInvalidNameTypesTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowInvalidNameTypes);
//            model.ShowInvalidNameTypes = true;
//            Assert.IsTrue(model.ShowInvalidNameTypes);
//        }

//        [TestMethod]
//        public void ShowMapTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowMap);
//            model.ShowMap = true;
//            Assert.IsTrue(model.ShowMap);
//        }

//        [TestMethod]
//        public void ShowNumberOfChildTaxaTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowNumbersOfChildTaxa);
//            model.ShowNumbersOfChildTaxa = true;
//            Assert.IsTrue(model.ShowNumbersOfChildTaxa);
//        }

//        [TestMethod]
//        public void ShowOtherNameTypesTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowOtherNameTypes);
//            model.ShowOtherNameTypes = true;
//            Assert.IsTrue(model.ShowOtherNameTypes);
//        }

//        [TestMethod]
//        public void ShowPhotosTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowPhotos);
//            model.ShowPhotos = true;
//            Assert.IsTrue(model.ShowPhotos);
//        }

//        [TestMethod]
//        public void ShowReferencesTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowReferences);
//            model.ShowReferences = true;
//            Assert.IsTrue(model.ShowReferences);
//        }

//        [TestMethod]
//        public void ShowSynonymsTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowSynonyms);
//            model.ShowSynonyms = true;
//            Assert.IsTrue(model.ShowSynonyms);
//        }

//        [TestMethod]
//        public void ShowTaxonInformationTest()
//        {
//            //Show taxon information should indicate whether or not the taxon information model represents a taxon.
//            ReadTaxonInformationModel model = new ReadTaxonInformationModel("");
//            Assert.IsFalse(model.ShowTaxonInformation); 

//            model = GetModel(true);
//            Assert.IsTrue(model.ShowTaxonInformation);
//        }

//        [TestMethod]
//        public void ShowUpdateInformationTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.IsFalse(model.ShowUpdateInformation);
//            model.ShowUpdateInformation = true;
//            Assert.IsTrue(model.ShowUpdateInformation);
//        }

//        [TestMethod]
//        public void StatusTest()
//        {
//            ReadTaxonInformationModel model = GetModel(true);
//            Assert.AreEqual(model.Status, "");
//            Assert.AreEqual(model.AlertLevel, TaxonAlertLevel.Green);

//            model = new ReadTaxonInformationModel(TestParameters.PROBLEMATC_TEST_TAXON_ID);
//            Assert.IsTrue(model.Status.Length > 10);
//            Assert.AreEqual(model.AlertLevel, TaxonAlertLevel.Yellow);

//            model = new ReadTaxonInformationModel(TestParameters.LUMPED_TEST_TAXON_ID);
//            Assert.IsTrue(model.Status.Length > 10);
//            Assert.AreEqual(model.AlertLevel, TaxonAlertLevel.Red);

//        }

//        [TestMethod]
//        public void TaxonCategoryTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.AreEqual(model.TaxonCategory, "Art");
//        }

//        [TestMethod]
//        public void TaxonId()
//        {
//            Assert.IsTrue(GetModel(true).TaxonId == TestParameters.DEFAULT_TEST_TAXON_ID);
//        }

//        [TestMethod]
//        public void Title()
//        {
//            String title = GetModel(true).Title;
//            Assert.IsTrue(title.IndexOf(TestParameters.DEFAULT_TEST_TAXON_ID) > -1);
//            Assert.IsTrue(title.IndexOf(TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME) > -1);
//            Assert.IsTrue(title.IndexOf(TestParameters.DEFAULT_TEST_TAXON_COMMON_NAME) > -1);
//        }

//        [TestMethod]
//        public void UrlToPhotosTest()
//        {
//            ReadTaxonInformationModel model = GetModel(false);
//            Assert.AreEqual(model.UrlToPhotos, "http://artportalen.se/artportalen/gallery/images.aspx?rappsyst=2&art=1&art_leaf=True&valid=typ1");
//        }
//    }
//}

