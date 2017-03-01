using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Result.ResultCalculation.SummaryStatistics
{
    [TestClass]
    public class SummaryStatisticsPerPolygonResultCalculatorTests : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void CalculateSpeciesObservationCountPerPolygonAndTaxa_TwoTaxonAndTwoPolygons_Success()
        {
            // Arrange
            SummaryStatisticsPerPolygonResultCalculator resultCalculator;
            TaxonSpecificSpeciesObservationCountPerPolygonResult result;
            List<int> taxonIds;
                
            LoginApplicationUser();            
            SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
            resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            taxonIds = new List<int> { 1, 2 };

            // Act
            result = resultCalculator.CalculateSpeciesObservationCountPerPolygonAndTaxa(taxonIds);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Taxa.Count > 0);
            Assert.IsTrue(result.SpeciesObservationCountPerPolygon.Count > 0);            
        }

        [TestMethod]
        [TestCategory("TimeoutNightlyTestApp")]
        public void GetSpeciesObservations_And_SpeciesCount_Per_Polygon()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                SummaryStatisticsPerPolygonResultCalculator resultCalculator;
                List<SpeciesObservationsCountPerPolygon> result;

                LoginApplicationUser();
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
                resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

                // Act
                result = resultCalculator.GetSummaryStatisticsPerPolygonFromCacheIfAvailableOrElseCalculate();

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                Assert.IsTrue(result[0].Properties.Split(new[] { '\n' }).Length > 0);
                Assert.IsTrue(Convert.ToInt64(result[0].SpeciesObservationsCount) > -1); // species observation count checked
                Assert.IsTrue(Convert.ToInt64(result[0].SpeciesCount) > -1); // species count checked
            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservations_And_SpeciesCount_Per_Polygon_No_Count_Selected()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                SummaryStatisticsPerPolygonResultCalculator resultCalculator;
                List<SpeciesObservationsCountPerPolygon> result;

                LoginApplicationUser();
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = false;
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = false;
                SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
                resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

                // Act
                result = resultCalculator.GetSummaryStatisticsPerPolygonFromCacheIfAvailableOrElseCalculate();

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                Assert.IsTrue(result[0].Properties.Split(new[] { '\n' }).Length > 0);
                Assert.IsTrue(result[0].SpeciesObservationsCount == "-"); // species observation count not checked
                Assert.IsTrue(result[0].SpeciesCount == "-"); // species count not checked
            }
        }
    }
}