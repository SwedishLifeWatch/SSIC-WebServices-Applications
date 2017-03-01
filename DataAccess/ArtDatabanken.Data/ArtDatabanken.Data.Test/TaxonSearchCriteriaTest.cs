using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class TaxonSearchCriteriaTest
    {
        private TaxonSearchCriteria _searchCriteria;

        public TaxonSearchCriteriaTest()
        {
            _searchCriteria = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonSearchCriteria searchCriteria;

            searchCriteria = new TaxonSearchCriteria();
            Assert.IsNotNull(searchCriteria);
        }

        [TestMethod]
        public void TaxonIds()
        {
            List<Int32> taxonIdList;
            taxonIdList = null;
            GetSearchCriteria(true).TaxonIds = taxonIdList;
            Assert.IsNull(GetSearchCriteria().TaxonIds);
            taxonIdList = new List<int>();
            GetSearchCriteria(true).TaxonIds = taxonIdList;
            Assert.AreEqual(GetSearchCriteria().TaxonIds, taxonIdList);
            for (int i = 0; i < 10; i++)
            {
                taxonIdList.Add(i);
            }
            GetSearchCriteria(true).TaxonIds = taxonIdList;
            Assert.AreEqual(GetSearchCriteria().TaxonIds.Count, GetSearchCriteria().TaxonIds.Count);
            Assert.AreEqual(GetSearchCriteria().TaxonIds[4], GetSearchCriteria().TaxonIds[4]);
           
        }

        [TestMethod]
        public void TaxonCategoryIds()
        {
            List<Int32> taxonCategoryIdList;
            taxonCategoryIdList = null;
            GetSearchCriteria(true).TaxonCategoryIds = taxonCategoryIdList;
            Assert.IsNull(GetSearchCriteria().TaxonCategoryIds);
            taxonCategoryIdList = new List<int>();
            GetSearchCriteria(true).TaxonCategoryIds = taxonCategoryIdList;
            Assert.AreEqual(GetSearchCriteria().TaxonCategoryIds, taxonCategoryIdList);
            for (int i = 0; i < 10; i++)
            {
                taxonCategoryIdList.Add(i);
            }
            GetSearchCriteria(true).TaxonCategoryIds = taxonCategoryIdList;
            Assert.AreEqual(GetSearchCriteria().TaxonCategoryIds.Count, GetSearchCriteria().TaxonCategoryIds.Count);
            Assert.AreEqual(GetSearchCriteria().TaxonCategoryIds[4], GetSearchCriteria().TaxonCategoryIds[4]);
        }

    

        [TestMethod]
        public void TaxonName()
        {
            String taxonName;

            taxonName = null;
            GetSearchCriteria(true).TaxonNameSearchString = taxonName;
            Assert.IsNull(GetSearchCriteria().TaxonNameSearchString);
            taxonName = String.Empty;
            GetSearchCriteria().TaxonNameSearchString = taxonName;
            Assert.AreEqual(GetSearchCriteria().TaxonNameSearchString, taxonName);
            taxonName = "test";
            GetSearchCriteria().TaxonNameSearchString = taxonName;
            Assert.AreEqual(GetSearchCriteria().TaxonNameSearchString, taxonName);
            taxonName = "test%";
            GetSearchCriteria().TaxonNameSearchString = taxonName;
            Assert.AreEqual(GetSearchCriteria().TaxonNameSearchString, taxonName);
        }

        private TaxonSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private TaxonSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new TaxonSearchCriteria();
            }
            return _searchCriteria;
        }
    }
}
