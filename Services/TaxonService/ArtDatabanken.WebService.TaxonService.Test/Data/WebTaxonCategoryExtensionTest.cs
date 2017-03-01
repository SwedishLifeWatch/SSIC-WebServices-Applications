using System.Collections.Generic;
using ArtDatabanken.Database;
using TaxonManager = ArtDatabanken.WebService.TaxonService.Data.TaxonManager;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    using System;
    using WebService.Data;
    using WebService.Database;
    using TaxonService.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebTaxonCategoryExtensionTest:TestBase
    {
        private WebTaxonCategory taxonCategory;

        public WebTaxonCategoryExtensionTest()
        {
            taxonCategory = null;
        }


        public WebTaxonCategory CreateTaxonCategory()
         {
             taxonCategory = TaxonService.Data.TaxonManager.CreateTaxonCategory(GetContext(), GetReferenceTaxonCategory());
             
             return taxonCategory;
         }

        [TestMethod]
        public void LoadData()
        {
            WebTaxonCategory webTaxonCategory;

            using (DataReader dataReader = GetContext().GetTaxonDatabase().GetTaxonCategories(Settings.Default.TestLocaleId))
            {
                webTaxonCategory = new WebTaxonCategory();
                Assert.IsTrue(dataReader.Read());
                webTaxonCategory.LoadData(dataReader);
                Assert.IsTrue(webTaxonCategory.Name.IsNotEmpty());
            }
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

        #region Helper functions

        /// <summary>
        /// Creates a taxon category.
        /// </summary>
        /// <returns></returns>
        private WebTaxonCategory GetReferenceTaxonCategory()
        {
            
            WebTaxonCategory refTaxonCategory = new WebTaxonCategory();
            
            string categoryName= "Svenskt";
            var parentCategory = 2;
            var sortOrder = 20;
            bool mainCategory = false;
            bool taxonomic = true;
            refTaxonCategory.Name = categoryName;
            refTaxonCategory.IsMainCategory = mainCategory;
            refTaxonCategory.ParentId = parentCategory;
            refTaxonCategory.SortOrder = sortOrder;
            refTaxonCategory.IsTaxonomic= taxonomic;

            return refTaxonCategory;
        }

        /// <summary>
        /// Creates a taxon category.
        /// </summary>
        /// <param name="infoText"> Additional test to separet data from eachother</param>
        /// <returns></returns>
        private WebTaxonCategory GetReferenceTaxonCategory(string infoText)
        {

            WebTaxonCategory refTaxonCategory = new WebTaxonCategory();

            string categoryName = "Svenskt" +" " + infoText;
            var parentCategory = 2;
            var sortOrder = 20;
            bool mainCategory = false;
            bool taxonomic = true;
            refTaxonCategory.Name = categoryName;
            refTaxonCategory.IsMainCategory = mainCategory;
            refTaxonCategory.ParentId = parentCategory;
            refTaxonCategory.SortOrder = sortOrder;
            refTaxonCategory.IsTaxonomic = taxonomic;

            return refTaxonCategory;
        }
    #endregion
    }
}

