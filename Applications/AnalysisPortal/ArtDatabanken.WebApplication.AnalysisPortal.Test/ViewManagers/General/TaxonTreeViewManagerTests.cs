using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.ViewManagers.General
{
    [TestClass()]
    public class TaxonTreeViewManagerTests : TestBase
    {

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxonTree_CarnivoraTaxon_GetTree()
        {
            const int CarnivoraTaxonId = 3000303;

            LoginApplicationUser();
            TaxonTreeViewManager viewManager = new TaxonTreeViewManager(SessionHandler.UserContext, SessionHandler.MySettings);
            
            viewManager.GetTaxonTree(CarnivoraTaxonId);
        }

    }
}
