using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    /// <summary>
    /// This test class tests two different TaxonManger classes.
    /// ArtDatabanken.WebService.Data.TaxonManager
    /// ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.TaxonManager
    /// </summary>
    [TestClass]
    public class TaxonManagerTest : TestBase
    {
        private WebService.Data.TaxonManager _taxonManager;

        public TaxonManagerTest()
        {
            _taxonManager = null;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDictionary()
        {
            Dictionary<Int32, WebTaxon> taxonDictionary;
            List<WebTaxon> taxa;
            WebTaxonSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.IsIsValidTaxonSpecified = true;
            searchCriteria.IsValidTaxon = true;
            searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mammals));
            taxa =  GetTaxonManager(true).GetTaxaBySearchCriteria(Context, searchCriteria);
            taxonDictionary = GetTaxonManager().GetDictionary(taxa);
            Assert.IsTrue(taxonDictionary.IsNotEmpty());
            Assert.AreEqual(taxa.Count, taxonDictionary.Count);
            Assert.IsTrue(taxonDictionary.ContainsKey((Int32)(TaxonId.Mammals)));
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxaByAuthority()
        {
            Dictionary<Int32, WebTaxon> taxonDictionary;

            taxonDictionary = GetTaxonManager(true).GetTaxaByAuthority(Context, Context.CurrentRoles[0].Authorities[0]);
            Assert.IsTrue(taxonDictionary.IsEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxonInformation()
        {
            Dictionary<Int32, TaxonInformation> taxonDictionary;

            taxonDictionary = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(Context);
            Assert.IsTrue(taxonDictionary.IsNotEmpty());
        }

        private WebService.Data.TaxonManager GetTaxonManager(Boolean refresh = false)
        {
            if (_taxonManager.IsNull() || refresh)
            {
                _taxonManager = new WebService.Data.TaxonManager();
            }

            return _taxonManager;
        }
    }
}
