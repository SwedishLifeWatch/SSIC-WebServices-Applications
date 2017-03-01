using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Result.ResultCalculation.SpeciesObservation
{
    [TestClass]
    public class SpeciesObservationTableResultCalculatorTests : TestBase
    {

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_Add1TaxonToFilter_SuccessfullReturn()
        {
            PagedSpeciesObservationResultCalculator resultCalculator;
            List<Dictionary<string, string>> result;

            LoginApplicationUser(); // to remove this call, you must implement SpeciesObservationDataSourceTestRepository.GetSpeciesObservationFieldDescriptions
            MockSpeciesObservationManager();

            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            resultCalculator = new PagedSpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            result = resultCalculator.GetTablePagedResult(0, 25);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }



        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_EmptyTaxaFilter_SuccessfullyReturnAllTaxa()
        {
            PagedSpeciesObservationResultCalculator resultCalculator;
            List<Dictionary<string, string>> result;

            LoginApplicationUser(); // to remove this call, you must implement SpeciesObservationDataSourceTestRepository.GetSpeciesObservationFieldDescriptions
            MockSpeciesObservationManager();

            resultCalculator = new PagedSpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            result = resultCalculator.GetTablePagedResult(0, 25);

            Assert.AreEqual(SpeciesObservationDataSourceTestRepository.AllTaxonIds.Count(), result.Count);
        }

      
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_Add1TaxaToFilter_FastQueryEstimation()
        {
            PagedSpeciesObservationResultCalculator resultCalculator;
            QueryComplexityEstimate estimation;

            MockDataSources();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            resultCalculator = new PagedSpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            estimation = resultCalculator.GetQueryComplexityEstimate();

            Assert.AreEqual(QueryComplexityExecutionTime.Fast, estimation.QueryComplexityExecutionTime);            
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetQueryComplexityEstimate_EmptyTaxaFilter_SlowQueryEstimation()
        {
            PagedSpeciesObservationResultCalculator resultCalculator;
            QueryComplexityEstimate estimation;

            MockDataSources();
            resultCalculator = new PagedSpeciesObservationResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);

            estimation = resultCalculator.GetQueryComplexityEstimate();

            Assert.AreEqual(QueryComplexityExecutionTime.Fast, estimation.QueryComplexityExecutionTime);                        
        }

        //[TestMethod]
        //public void GetResult_Add1TaxonToFilter_SuccessfullReturn()
        //{
        //    CoreData.SpeciesObservationManager.DataSource = new SpeciesObservationDataSourceTestRepository();

        //    SpeciesObservationTableResultCalculator resultCalculator;
        //    List<Dictionary<string, string>> result;

        //    LoginTestUser();
        //    SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
        //    resultCalculator = new SpeciesObservationTableResultCalculator(SessionHandler.MySettings);

        //    result = resultCalculator.GetResult(SessionHandler.UserContext);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.Count);
        //    LogoutTestUser();




        //    // Mocking using Rhino Mocks
        //    //var mockDataSource = MockRepository.GenerateMock<SpeciesObservationTableResultCalculator>(SessionHandler.MySettings);
        //    //mockDataSource.Expect(x => x.GetResult(null)).Return(new List<Dictionary<string, string>>());

        //    //result = mockDataSource.GetResult(null);
        //    //mockDataSource.VerifyAllExpectations();

        //}

    }
}
