using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Database;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    using System;
    using WebService.Data;
    using WebService.Database;
    using TaxonService.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebTaxonNameCategoryExtensionTest:TestBase
    {
        private WebTaxonNameCategory taxonNameCategory;

        public WebTaxonNameCategoryExtensionTest()
        {
            taxonNameCategory = null;
        }


        public WebTaxonNameCategory CreateTaxonNameCategory()
         {
             taxonNameCategory = TaxonService.Data.TaxonManager.CreateTaxonNameCategory(GetContext(), GetReferenceTaxonNameCategory());
             
             return taxonNameCategory;
         }

        [TestMethod]
        public void LoadData()
        {
            WebTaxonNameCategory webTaxonNameCategory;

            using (DataReader dataReader = GetContext().GetTaxonDatabase().GetTaxonNameCategories(Settings.Default.TestLocaleId))
            {
                webTaxonNameCategory = new WebTaxonNameCategory();
                Assert.IsTrue(dataReader.Read());
                webTaxonNameCategory.LoadData(dataReader);
             
                Assert.IsTrue(webTaxonNameCategory.Name.IsNotEmpty());
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
        /// Get reference dat for a taxon name category
        /// </summary>
        /// <returns></returns>
        private WebTaxonNameCategory GetReferenceTaxonNameCategory()
        {
            WebTaxonNameCategory refTaxonNameCategory = new WebTaxonNameCategory();

            string categoryName = "Test namn categori";
            string shortName = "hi";
            var sortOrder = 10;
            refTaxonNameCategory.Name = categoryName;
            refTaxonNameCategory.ShortName = shortName;
            refTaxonNameCategory.SortOrder = sortOrder;
            
            return refTaxonNameCategory;
        }
    #endregion
    }
}

