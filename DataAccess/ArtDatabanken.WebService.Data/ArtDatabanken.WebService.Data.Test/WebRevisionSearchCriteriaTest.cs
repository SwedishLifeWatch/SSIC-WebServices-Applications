using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebRevisionSearchCriteriaTest: WebDomainTestBase<WebTaxonRevisionSearchCriteria>
    {
        [TestMethod]
        public void Constructor()
        {
            WebTaxonRevisionSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonRevisionSearchCriteria();
            Assert.IsNotNull(searchCriteria);
        }

        [TestMethod]
        public void RevisionStateIds()
        {
            List<Int32> revisionStateIds = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                revisionStateIds.Add(i+100);
            }

            GetObject(true).StateIds = revisionStateIds;
            Assert.AreEqual(revisionStateIds, GetObject().StateIds);
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


        
    }
}
