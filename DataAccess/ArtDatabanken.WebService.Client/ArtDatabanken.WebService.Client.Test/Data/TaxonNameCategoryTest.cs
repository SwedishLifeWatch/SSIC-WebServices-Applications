using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    // TODO should implement template class for this test....
    [TestClass]
    public class TaxonNameCategoryTest : TestBase
    {
        TaxonNameCategory _taxonNameCategory;

        public TaxonNameCategoryTest()
        {
            _taxonNameCategory = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonNameCategory taxonNameCategory;

            taxonNameCategory = new TaxonNameCategory();
            Assert.IsNotNull(taxonNameCategory);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetTaxonNameCategory(true).DataContext);
        }

        private TaxonNameCategory GetTaxonNameCategory(Boolean refresh = false)
        {
            if (_taxonNameCategory.IsNull() || refresh)
            {
                _taxonNameCategory = new TaxonNameCategory();
                _taxonNameCategory.DataContext = new DataContext(GetUserContext());
            }
            return _taxonNameCategory;
        }

        /// <summary>
        /// Test some of the taxon category properties
        /// </summary>
        [TestMethod]
        public void TaxonProperties()
        {
            string categoryName = "Hej666";
            GetTaxonNameCategory(true).Name = categoryName;
            Assert.AreEqual("Hej666", GetTaxonNameCategory().Name);

            
            int id = 77;
            GetTaxonNameCategory(true).Id = id;
            Assert.AreEqual(77, GetTaxonNameCategory().Id);

            string shortName = "KORT";
            GetTaxonNameCategory(true).ShortName = shortName;
            Assert.AreEqual("KORT", GetTaxonNameCategory().ShortName);
          
            int sortOrder = 33;
            GetTaxonNameCategory(true).SortOrder = sortOrder;
            Assert.AreEqual(sortOrder, GetTaxonNameCategory().SortOrder);

        }
    }
}
