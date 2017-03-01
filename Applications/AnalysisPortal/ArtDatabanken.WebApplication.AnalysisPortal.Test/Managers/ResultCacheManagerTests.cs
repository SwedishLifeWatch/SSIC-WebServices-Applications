using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Managers
{
    using ArtDatabanken.Data;
    using AnalysisPortal.Managers;
    
    using TestModels;
    using Utils.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResultCacheManagerTests : TestBase
    {

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationData_AddSpeciesObservationDataBeforeGet_ResultCacheItemReturned()
        {
            SpeciesObservationsData speciesObservationsData;            
            speciesObservationsData = CreateSpeciesObservationDataSample();

            CalculatedDataItemCacheManager.GetSpeciesObservationData(SessionHandler.MySettings,"sv").Data = speciesObservationsData;            
            CalculatedDataItem<SpeciesObservationsData> calculatedDataItem = CalculatedDataItemCacheManager.GetSpeciesObservationData(SessionHandler.MySettings,"sv");
            SessionHandler.MySettings.ResultCacheNeedsRefresh = false;

            Assert.IsTrue(calculatedDataItem.HasData);
            Assert.IsTrue(calculatedDataItem.HasFreshData);            
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationData_AddSpeciesObservationDataAndChangeMySettingsAfterwards_ResultCacheItemIsNotUpToDate()
        {
            SpeciesObservationsData speciesObservationsData;
            speciesObservationsData = CreateSpeciesObservationDataSample();

            CalculatedDataItemCacheManager.GetSpeciesObservationData(SessionHandler.MySettings,"").Data = speciesObservationsData;            
            CalculatedDataItem<SpeciesObservationsData> calculatedDataItem = CalculatedDataItemCacheManager.GetSpeciesObservationData(SessionHandler.MySettings,"");
            SessionHandler.MySettings.ResultCacheNeedsRefresh = false;
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(1);
            
            Assert.IsFalse(calculatedDataItem.HasFreshData);                        
        }        

        private static SpeciesObservationsData CreateSpeciesObservationDataSample()
        {
            SpeciesObservation speciesObservation;
            SpeciesObservationList speciesObservationList;
            SpeciesObservationsData speciesObservationsData;
            speciesObservation = new SpeciesObservation();
            speciesObservation.Taxon = new SpeciesObservationTaxon();
            speciesObservation.Taxon.TaxonID = "27";
            speciesObservationList = new SpeciesObservationList();
            speciesObservationList.Add(speciesObservation);
            var fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(SessionHandler.UserContext, SessionHandler.MySettings);
            var fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            speciesObservationsData = new SpeciesObservationsData(speciesObservationList, fieldDescriptionsViewModel);
            return speciesObservationsData;
        }


    }
}
