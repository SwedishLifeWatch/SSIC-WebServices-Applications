using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AnalysisPortal.Controllers;
using AnalysisPortal.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MvcContrib.TestHelper;


namespace AnalysisPortal.Tests
{
    [TestClass]
    public class TaxaTest : DBTestControllerBaseTest
    {

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Filter_Data()
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();

            var searchCriteria = new TaxonSearchCriteria();
            searchCriteria.IsValidTaxon = true;
            searchCriteria.TaxonIds = new List<int>();            
            searchCriteria.TaxonIds.Add(1);
            searchCriteria.TaxonIds.Add(2);
            searchCriteria.TaxonIds.Add(3);
            searchCriteria.TaxonIds.Add(4);
            var taxa = CoreData.TaxonManager.GetTaxa(userContext, searchCriteria);

            Assert.AreEqual(4, taxa.Count);

        }

    }
}
