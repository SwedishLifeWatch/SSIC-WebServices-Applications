using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonCategoryTest : TestBase
    {
        TaxonCategory _taxonCategory;

        public TaxonCategoryTest()
        {
            _taxonCategory = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonCategory taxonCategory;

            taxonCategory = new TaxonCategory();
            Assert.IsNotNull(taxonCategory);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetTaxonCategory(true).DataContext);
        }

        [TestMethod]
        public void GetParent()
        {
            ITaxonCategory parent;

            parent = GetTaxonCategory(true).GetParent(GetUserContext());
            Assert.IsNotNull(parent);
            Assert.AreEqual(GetTaxonCategory().ParentId, parent.Id);
        }

        public static TaxonCategory GetTaxonCategory(IUserContext userContext)
        {
            TaxonCategory taxonCategory;

            taxonCategory = new TaxonCategory();
            taxonCategory.DataContext = new DataContext(userContext);
            taxonCategory.Id = Int32.MinValue;
            taxonCategory.IsMainCategory = false;
            taxonCategory.IsTaxonomic = false;
            taxonCategory.Name = String.Empty;
            taxonCategory.ParentId = Int32.MinValue;
            taxonCategory.SortOrder = Int32.MinValue;
            return taxonCategory;
        }

        private TaxonCategory GetTaxonCategory()
        {
            return GetTaxonCategory(false);
        }

        private TaxonCategory GetTaxonCategory(Boolean refresh)
        {
            if (_taxonCategory.IsNull() || refresh)
            {
                _taxonCategory = (TaxonCategory)(CoreData.TaxonManager.GetTaxonCategories(GetUserContext())[20]);
            }
            return _taxonCategory;
        }

        /// <summary>
        /// Test some of the taxon category properties
        /// </summary>
        [TestMethod]
        public void TaxonProperties()
        {
            string categoryName = "Hej666";
            GetTaxonCategory(true).Name = categoryName;
            Assert.AreEqual("Hej666",GetTaxonCategory().Name);

            
            int id = 77;
            GetTaxonCategory(true).Id = id;
            Assert.AreEqual(77, GetTaxonCategory().Id);


            GetTaxonCategory(true).IsMainCategory = true;
            Assert.AreEqual(true, GetTaxonCategory().IsMainCategory);

            int parentCategory = -11;
            GetTaxonCategory(true).ParentId = parentCategory;
            Assert.AreEqual(parentCategory, GetTaxonCategory().ParentId);

            int sortOrder = 33;
            GetTaxonCategory(true).SortOrder = sortOrder;
            Assert.AreEqual(sortOrder, GetTaxonCategory().SortOrder);

            GetTaxonCategory(true).IsTaxonomic = false;
            Assert.AreEqual(false, GetTaxonCategory().IsTaxonomic);

        }
    }
}
