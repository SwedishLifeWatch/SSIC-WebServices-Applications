using System;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Result.ResultCalculation.SpeciesObservation
{
    [TestClass]
    public class SpeciesObservationDiagramResultCalculatorTests : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_Add1TaxonToFilter_SuccessfullReturnObservationCountForSpecificTaxaPerMounth()
        {
            SpeciesObservationDiagramResultCalculator resultCalculator;
            List<KeyValuePair<string, string>> result;

            LoginApplicationUser();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            resultCalculator = new SpeciesObservationDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            result = resultCalculator.CalculateResult();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            foreach (var keyValuePair in result)
            {
                Assert.IsTrue(Convert.ToInt32(keyValuePair.Value) >= 0);
                Assert.IsTrue(Convert.ToInt32(keyValuePair.Key) > 0);
            }
            
        }



        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_EmptyTaxaFilter_SuccessfullyReturnObservationCountsAllTaxaPerMounth()
        {
            SpeciesObservationDiagramResultCalculator resultCalculator;
            List<KeyValuePair<string, string>> result;

            
            LoginApplicationUser();
         
            resultCalculator = new SpeciesObservationDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            result = resultCalculator.CalculateResult();

            Assert.IsNotNull(result);
            
            Assert.IsTrue(result.Count > 0);
            foreach (var keyValuePair in result)
            {
                Assert.IsNotNull(keyValuePair.Value);
                Assert.IsTrue(Convert.ToInt32(keyValuePair.Key) > 0);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_OneTaxaFilter_ResultCacheIsPopulated()
        {
            SpeciesObservationDiagramResultCalculator resultCalculator;
            List<KeyValuePair<string, string>> result;
            CalculatedDataItem<List<KeyValuePair<string, string>>> calculatedDataItem;

            LoginApplicationUser();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            resultCalculator = new SpeciesObservationDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            //Calculate result and check that it is cashed.
            result = resultCalculator.CalculateResult();

            calculatedDataItem = CalculatedDataItemCacheManager.GetCalculatedDataItem<List<KeyValuePair<string, string>>>(CalculatedDataItemType.SpeciesObservationDiagramData, SessionHandler.MySettings, null);
            Assert.IsTrue(calculatedDataItem.HasData);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_Add1TaxaToFilter_FastQueryEstimation()
        {
            SpeciesObservationDiagramResultCalculator resultCalculator;
            QueryComplexityEstimate estimation;

            LoginApplicationUser();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            resultCalculator = new SpeciesObservationDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            estimation = resultCalculator.GetQueryComplexityEstimate();

            Assert.AreEqual(QueryComplexityExecutionTime.Fast, estimation.QueryComplexityExecutionTime);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_EmptyTaxaFilter_MediumQueryEstimation()
        {
            SpeciesObservationDiagramResultCalculator resultCalculator;
            QueryComplexityEstimate estimation;

            LoginApplicationUser();
            resultCalculator = new SpeciesObservationDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            estimation = resultCalculator.GetQueryComplexityEstimate();

            Assert.AreEqual(QueryComplexityExecutionTime.Medium, estimation.QueryComplexityExecutionTime);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_51TaxaFilter_MediumQueryEstimation()
        {
            SpeciesObservationDiagramResultCalculator resultCalculator;
            QueryComplexityEstimate estimation;

            LoginApplicationUser();
            for (int i = 0; i < 51; i++)
            {
                SessionHandler.MySettings.Filter.Taxa.AddTaxonId(i);
            }

            resultCalculator = new SpeciesObservationDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            estimation = resultCalculator.GetQueryComplexityEstimate();

            Assert.AreEqual(QueryComplexityExecutionTime.Medium, estimation.QueryComplexityExecutionTime);
        }
    }
}
