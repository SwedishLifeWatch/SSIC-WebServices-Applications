using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebTaxonSearchCriteriaTest: WebDomainTestBase<WebTaxonSearchCriteria>
    {
        [TestMethod]
        public void Constructor()
        {
            WebTaxonSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonSearchCriteria();
            Assert.IsNotNull(searchCriteria);
        }

        [TestMethod]
        public void TaxonCategoryIds()
        {
            List<Int32> taxonCategoryIds = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                taxonCategoryIds.Add(i+100);
            }

            GetObject(true).TaxonCategoryIds = taxonCategoryIds;
            Assert.AreEqual(taxonCategoryIds, GetObject().TaxonCategoryIds);
        }

        [TestMethod]
        public void TaxonIds()
        {
            List<Int32> taxonIds = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                taxonIds.Add(i);
            }

            GetObject(true).TaxonIds = taxonIds;
            Assert.AreEqual(taxonIds, GetObject().TaxonIds);
        }


        [TestMethod]
        public void TaxonName()
        {
            const string taxonName = "Taxon name";
            GetObject(true).TaxonNameSearchString = new WebStringSearchCriteria();
            GetObject().TaxonNameSearchString.SearchString = taxonName;
            Assert.AreEqual(GetObject().TaxonNameSearchString.SearchString, taxonName);
        }

    }
}
