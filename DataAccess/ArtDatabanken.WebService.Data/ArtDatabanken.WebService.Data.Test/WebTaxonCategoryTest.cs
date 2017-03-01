using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Test all properties and constructor for WebTaxonCategory class.
    /// </summary>
    [TestClass]
    public class WebTaxonCategoryTest : WebDomainTestBase<WebTaxonCategory>
    {
        [TestMethod]
        public void Constructor()
        {
            WebTaxonCategory webTaxonCategory = new WebTaxonCategory();
            Assert.IsNotNull(webTaxonCategory);
        }

        [TestMethod]
        public void CategoryName()
        {
            const string value = "TEST";
            GetObject(true).Name = value;
            Assert.AreEqual(GetObject().Name, value);
        }


        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void MainCategory()
        {
           bool mainCategory = true;
           GetObject(true).IsMainCategory = mainCategory;
           Assert.AreEqual(GetObject().IsMainCategory, mainCategory);

           mainCategory = false;
           GetObject(true).IsMainCategory = mainCategory;
           Assert.AreEqual(GetObject().IsMainCategory, mainCategory);
        }   

        [TestMethod]
        public void ParentCategory()
        {
            int parentCategory = 10;
            GetObject(true).ParentId = parentCategory;
            Assert.AreEqual(GetObject().ParentId, parentCategory);

            parentCategory = -6;
            GetObject(true).ParentId = parentCategory;
            Assert.AreEqual(GetObject().ParentId, parentCategory);
        }

        [TestMethod]
        public void SortOrder()
        {
            int sortOrder = 11;
            GetObject(true).SortOrder = sortOrder;
            Assert.AreEqual(GetObject().SortOrder, sortOrder);

            sortOrder = -6;
            GetObject(true).SortOrder = sortOrder;
            Assert.AreEqual(GetObject().SortOrder, sortOrder);
        }

        [TestMethod]
        public void Taxonomic()
        {
            bool taxonomic = true;
            GetObject(true).IsTaxonomic = taxonomic;
            Assert.AreEqual(GetObject().IsTaxonomic, taxonomic);

            taxonomic = false;
            GetObject(true).IsTaxonomic = taxonomic;
            Assert.AreEqual(GetObject().IsTaxonomic, taxonomic);
        }
    }
}
