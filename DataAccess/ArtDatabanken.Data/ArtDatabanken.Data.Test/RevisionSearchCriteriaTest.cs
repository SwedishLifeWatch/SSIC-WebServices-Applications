using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class RevisionSearchCriteriaTest
    {
        private TaxonRevisionSearchCriteria _searchCriteria;

        public RevisionSearchCriteriaTest()
        {
            _searchCriteria = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonRevisionSearchCriteria searchCriteria;

            searchCriteria = new TaxonRevisionSearchCriteria();
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
        public void RevisionStateIds()
        {
            List<Int32> revisionStateIdList;
            revisionStateIdList = null;
            GetSearchCriteria(true).StateIds = revisionStateIdList;
            Assert.IsNull(GetSearchCriteria().StateIds);
            revisionStateIdList = new List<int>();
            GetSearchCriteria(true).StateIds = revisionStateIdList;
            Assert.AreEqual(GetSearchCriteria().StateIds, revisionStateIdList);
            for (int i = 0; i < 10; i++)
            {
                revisionStateIdList.Add(i);
            }
            GetSearchCriteria(true).StateIds = revisionStateIdList;
            Assert.AreEqual(GetSearchCriteria().StateIds.Count, GetSearchCriteria().StateIds.Count);
            Assert.AreEqual(GetSearchCriteria().StateIds[4], GetSearchCriteria().StateIds[4]);
        }





        private TaxonRevisionSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private TaxonRevisionSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new TaxonRevisionSearchCriteria();
            }
            return _searchCriteria;
        }
    }
}
