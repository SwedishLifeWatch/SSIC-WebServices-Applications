using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AnalysisPortal.Controllers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MvcContrib.TestHelper;


namespace AnalysisPortal.Tests
{
    using Microsoft.QualityTools.Testing.Fakes;

    [TestClass]
    public class RegionTest : DBTestControllerBaseTest
    {
        
   
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetRegions()
        {
            using (ShimsContext.Create())
            {
                //Login user
                LoginTestUserAnalyser();
                IUserContext userContext = SessionHandler.UserContext;
                int defaultCountryIsoCode = 752;
                RegionCategoryList regionCategories = CoreData.RegionManager.GetRegionCategories(userContext, defaultCountryIsoCode);
                RegionTypeList regionTypes = CoreData.RegionManager.GetRegionTypes(userContext);

                IRegionCategory regionCategory = CoreData.RegionManager.GetRegionCategory(userContext, 21); //21=län
                RegionList regions = CoreData.RegionManager.GetRegionsByCategory(userContext, regionCategory);
                Region dalarna = regions[1] as Region;


                //var controller = new CultureController();
                //// Mock Controller
                //Builder.InitializeController(controller);
                //Builder.HttpContext.Response.Expect(x => x.Cookies).Return(new HttpCookieCollection());
                //RedirectResult result = controller.SetCulture("en", "home/index");
                //Assert.AreEqual("home/index", result.Url);
            }
        }
    }
}
