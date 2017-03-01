using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.MySettingsSummary.Improved
{
    [TestClass]
    public class ImprovedMySettingsSummaryTests : TestBase
    {

        [TestMethod]
        public void CanSettingsAffectResult_GridMapAndSettingsSummary_TrueAndFalse()
        {
            LoginApplicationUser();                        
            ImprovedDataProvidersSettingSummary model = new ImprovedDataProvidersSettingSummary(SessionHandler.UserContext, SessionHandler.MySettings);
            List<DataProviderViewModel> dataProviderViewModels = model.GetSettingsSummaryModel();
            bool canSettingAffectResult = model.CanSettingAffectResult(ResultType.SpeciesObservationGridMap);
            Assert.AreEqual(true, canSettingAffectResult);

            canSettingAffectResult = model.CanSettingAffectResult(ResultType.SettingsSummary);
            Assert.AreEqual(false, canSettingAffectResult);
        }


        [TestMethod]
        public void CanSettingsAffectResultAlternative_GridMapAndSettingsSummary_TrueAndFalse()
        {
            LoginApplicationUser();            
            bool canSettingAffectResult = MySettingsSummaryItemIdentifierManager.CanSettingAffectResult(ResultType.SpeciesObservationGridMap, ImprovedDataProvidersSettingSummary.StaticIdentifier);
            Assert.AreEqual(true, canSettingAffectResult);

            canSettingAffectResult = MySettingsSummaryItemIdentifierManager.CanSettingAffectResult(ResultType.SettingsSummary, ImprovedDataProvidersSettingSummary.StaticIdentifier);
            Assert.AreEqual(false, canSettingAffectResult);
        }


        [TestMethod]
        public void CanSettingsAffectResultAlternative2_GridMapAndSettingsSummary_TrueAndFalse()
        {
            LoginApplicationUser();

            List<ResultType> affectedResultTypes = ImprovedGridStatisticsSettingSummary.GetAffectedResultTypes();
            bool canSettingAffectResult = affectedResultTypes.Contains(ResultType.SpeciesObservationGridMap);
            Assert.AreEqual(true, canSettingAffectResult);

            affectedResultTypes = ImprovedGridStatisticsSettingSummary.GetAffectedResultTypes();
            canSettingAffectResult = affectedResultTypes.Contains(ResultType.SpeciesObservationMap);
            Assert.AreEqual(false, canSettingAffectResult);
        }

    }
}
