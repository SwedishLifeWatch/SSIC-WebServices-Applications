using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.ViewManagers.Presentation
{
    [TestClass()]
    public class SpeciesObservationFieldDescriptionViewManagerTests : TestBase
    {
        [TestMethod]
        public void GetTaxonTree_CarnivoraTaxon_GetTree()
        {
            // Arrange            
            LoginApplicationUser();
            var viewManager = new SpeciesObservationFieldDescriptionViewManager(SessionHandler.UserContext, SessionHandler.MySettings);

            // Act
            SpeciesObservationFieldDescriptionsViewModel model = viewManager.CreateSpeciesObservationFieldDescriptionsViewModel();            

            // Assert
            Assert.IsNotNull(model.FieldDescriptionsByProjectName);
        }

    }
}
