using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebService.Client.TaxonService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data.Extensions
{
    /// <summary>
    /// Summary description for ITaxonManagerExtensionTest
    /// </summary>
    [TestClass]
    public class ITaxonManagerExtensionTest : TestBase
    {
        ITaxonManager _taxonManager;

        [TestMethod]
        public void GetPossibleTaxonCategories()
        {
            IList<ITaxonCategory> possibleCategories;
            ITaxon taxon;

            taxon = GetTaxonManager().GetTaxon(GetUserContext(), 248239);
            possibleCategories = GetTaxonManager().GetPossibleTaxonCategories(GetRevisionUserContext(), taxon);
            Assert.IsTrue(possibleCategories.Count > 0);
        }

        private ITaxonManager GetTaxonManager(Boolean refresh = false)
        {
            if (_taxonManager.IsNull() || refresh)
            {
                _taxonManager = new TaxonManager();
                _taxonManager.DataSource = new TaxonDataSource();
            }
            return _taxonManager;
        }

    }
}