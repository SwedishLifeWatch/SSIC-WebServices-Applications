using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Managers
{
    [TestClass]
    public class QueryComplexityManagerTests : TestBase
    {
        
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_ObservationMapWithoutTaxaFilter_ReturnSlowEstimate()
        {
            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate();
            //var result = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationMap, SessionHandler.UserContext, SessionHandler.MySettings);
            Assert.AreEqual(QueryComplexityExecutionTime.Slow, result.QueryComplexityExecutionTime);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_ObservationMapWithoutTaxaFilter_ReturnGridMapResultViewSuggestion()
        {
            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate(false);
            //QueryComplexityEstimate result;
            //result = QueryComplexityManager.GetQueryComplexityEstimate(
            //    ResultType.SpeciesObservationMap,
            //    SessionHandler.UserContext,
            //    SessionHandler.MySettings);
            Assert.AreEqual(ResultType.SpeciesObservationGridMap, result.ComplexityDescription.SuggestedResultViews[0].ResultType);
        }

        //[TestMethod]
        //public void GetQueryComplexityEstimate_ObservationTableWithoutTaxaFilter_ReturnGridTableResultViewSuggestion()
        //{
        //    SpeciesObservationTableResultCalculator resultCalculator = new SpeciesObservationTableResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
        //    QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate();
        //    //QueryComplexityEstimate result;
        //    //result = QueryComplexityManager.GetQueryComplexityEstimate(
        //    //    ResultType.SpeciesObservationTable,
        //    //    SessionHandler.UserContext,
        //    //    SessionHandler.MySettings);
        //    Assert.AreEqual(ResultType.SpeciesObservationGridTable, result.ComplexityDescription.SuggestedResultViews[0].ResultType);
        //}

        //[TestMethod]
        //public void GetQueryComplexityEstimate_AllResultTypes_TitleAndTextPropertiesHasValues()
        //{
        //    foreach (ResultType resultType in Enum.GetValues(typeof(ResultType)))
        //    {
        //        QueryComplexityEstimate result = QueryComplexityManager.GetQueryComplexityEstimate(resultType, SessionHandler.UserContext, SessionHandler.MySettings);
        //        Assert.IsFalse(string.IsNullOrWhiteSpace(result.ComplexityDescription.Title));
        //        Assert.IsFalse(string.IsNullOrWhiteSpace(result.ComplexityDescription.Text));
        //    }
        //}


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_ObservationMapWithoutTaxaFilterButWithCachedResult_ReturnFastEstimate()
        {            
            SpeciesObservationsData speciesObservationsData;            
            //QueryComplexityEstimate result;
            LoginApplicationUser();

            speciesObservationsData = CreateSpeciesObservationData(SessionHandler.UserContext, SessionHandler.MySettings);
            CalculatedDataItemCacheManager.GetSpeciesObservationData(SessionHandler.MySettings,"").Data = speciesObservationsData;
            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate();

            //result = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationMap, SessionHandler.UserContext, SessionHandler.MySettings);
            Assert.AreEqual(QueryComplexityExecutionTime.Fast, result.QueryComplexityExecutionTime);
        }

        private SpeciesObservationsData CreateSpeciesObservationData(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
        {
            SpeciesObservation speciesObservation = new SpeciesObservation();
            speciesObservation.Taxon = new SpeciesObservationTaxon();
            speciesObservation.Taxon.TaxonID = "1";
            SpeciesObservationList speciesObservationList = new SpeciesObservationList();
            speciesObservationList.Add(speciesObservation);
            var fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(userContext, mySettings);
            var fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            SpeciesObservationsData speciesObservationsData = new SpeciesObservationsData(speciesObservationList, fieldDescriptionsViewModel);
            return speciesObservationsData;
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_ObservationMapWith1Taxa_ReturnFastEstimate()
        {            
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            //var result = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationMap, SessionHandler.UserContext, SessionHandler.MySettings);
            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate();
            Assert.AreEqual(QueryComplexityExecutionTime.Fast, result.QueryComplexityExecutionTime);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_ObservationMapWith100Taxa_ReturnMediumEstimate()
        {            
            SessionHandler.MySettings.Filter.Taxa.AddTaxonIds(Enumerable.Range(1,100));
            //var result = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationMap, SessionHandler.UserContext, SessionHandler.MySettings);
            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate();
            Assert.AreEqual(QueryComplexityExecutionTime.Medium, result.QueryComplexityExecutionTime);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_ObservationMapWith1TaxaWithSpatialFilter_ReturnMediumEstimate()
        {
            DataPolygon dataPolygon = CreateSampleDataPolygon();            
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            SessionHandler.MySettings.Filter.Spatial.Polygons.Add(dataPolygon);
            //var result = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationMap, SessionHandler.UserContext, SessionHandler.MySettings);
            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            QueryComplexityEstimate result = resultCalculator.GetQueryComplexityEstimate();
            Assert.AreEqual(QueryComplexityExecutionTime.Medium, result.QueryComplexityExecutionTime);
        }


        private DataPolygon CreateSampleDataPolygon()
        {         
            DataLinearRing dataLinearRing1 = new DataLinearRing();
            dataLinearRing1.Points = new List<DataPoint>();
            dataLinearRing1.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing1.Points.Add(new DataPoint(2.0, 3.0, null));
            dataLinearRing1.Points.Add(new DataPoint(3.0, 4.0, null));

            DataPolygon dataPolygon1 = new DataPolygon();
            dataPolygon1.LinearRings = new List<DataLinearRing>();
            dataPolygon1.LinearRings.Add(dataLinearRing1);

            return dataPolygon1;
        }

        //LoginTestUser();
        //IUserContext userContext = SessionHandler.UserContext;            

    }
}
