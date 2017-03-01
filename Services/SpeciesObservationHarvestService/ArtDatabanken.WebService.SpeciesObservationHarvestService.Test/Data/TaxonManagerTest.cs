using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class TaxonManagerTest : TestBase
    {
        [TestMethod]
        public void GetCurrentTaxonId()
        {
            Int32 currentTaxonId, oldTaxonId;

            oldTaxonId = 155;
            currentTaxonId = TaxonManager.GetCurrentTaxonId(GetContext(), oldTaxonId);
            Assert.AreNotEqual(oldTaxonId, currentTaxonId);
        }


        [TestMethod]
        public void GetTaxonNameDictionaries()
        {
            TaxonNameDictionaries taxonNameDictionaries;

            taxonNameDictionaries = TaxonManager.GetTaxonNameDictionaries(GetContext());
            Assert.IsNotNull(taxonNameDictionaries);
            Assert.IsTrue(taxonNameDictionaries.Genus.IsNotEmpty());
            Assert.IsTrue(taxonNameDictionaries.ScientificNameAndAuthor.IsNotEmpty());
            Assert.IsTrue(taxonNameDictionaries.ScientificNames.IsNotEmpty());
        }

        [TestMethod]
        public void GetNewTaxonId()
        {
            Int32 newTaxonId, oldTaxonId;

            oldTaxonId = 377;
            newTaxonId = TaxonManager.GetNewTaxonId(GetContext(), oldTaxonId);
            Assert.AreNotEqual(oldTaxonId, newTaxonId);
        }

        [TestMethod]
        public void GetTaxonRemarks()
        {
            Dictionary<Int32, String> taxonRemarks;

            taxonRemarks = TaxonManager.GetTaxonRemarks(GetContext());
            Assert.IsNotNull(taxonRemarks);
        }
    }
}
