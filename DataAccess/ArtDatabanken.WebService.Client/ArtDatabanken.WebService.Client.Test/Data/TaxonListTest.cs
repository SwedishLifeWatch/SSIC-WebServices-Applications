using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonListTest : TestBase
    {
        private TaxonList _taxon;

        public TaxonListTest()
        {
            _taxon = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonList taxa;

            taxa = new TaxonList();
            Assert.IsNotNull(taxa);
        }

        [TestMethod]
        public void Get()
        {
            GetTaxa(true);
            foreach (ITaxon taxon in GetTaxa())
            {
                Assert.AreEqual(taxon, GetTaxa().Get(taxon.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonId;

            taxonId = Int32.MaxValue;
            GetTaxa(true).Get(taxonId);
        }

        private TaxonList GetTaxa()
        {
            return GetTaxa(false);
        }

        private TaxonList GetTaxa(Boolean refresh)
        {
            if (_taxon.IsNull() || refresh)
            {
                _taxon = new TaxonList();
                _taxon.Add(TaxonTest.GetTaxon(GetUserContext()));
            }
            return _taxon;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            TaxonList newTaxonList, oldTaxonList;
            Int32 taxonIndex;

            oldTaxonList = GetTaxa(true);
            newTaxonList = new TaxonList();
            for (taxonIndex = 0; taxonIndex < oldTaxonList.Count; taxonIndex++)
            {
                newTaxonList.Add(oldTaxonList[oldTaxonList.Count - taxonIndex - 1]);
            }
            for (taxonIndex = 0; taxonIndex < oldTaxonList.Count; taxonIndex++)
            {
                Assert.AreEqual(newTaxonList[taxonIndex], oldTaxonList[oldTaxonList.Count - taxonIndex - 1]);
            }
        }
    }
}
