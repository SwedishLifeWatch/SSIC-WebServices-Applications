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
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MvcContrib.TestHelper;


namespace AnalysisPortal.Tests
{
    [TestClass]
    public class TableFieldsTest : DBTestControllerBaseTest
    {

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Filter_Data()
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            var viewManager = new SpeciesObservationFieldDescriptionViewManager(userContext, SessionHandler.MySettings);
            SpeciesObservationFieldDescriptionsViewModel viewModel = viewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            var dic = viewModel.FieldDescriptionsByImportance;
            var dic2 = viewModel.FieldDescriptionsByClass;

        }

    }
}
