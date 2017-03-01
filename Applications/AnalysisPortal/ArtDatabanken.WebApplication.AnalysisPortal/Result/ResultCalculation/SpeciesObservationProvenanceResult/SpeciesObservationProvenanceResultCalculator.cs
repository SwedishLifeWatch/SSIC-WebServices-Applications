using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservationProvenanceResult
{
    public class SpeciesObservationProvenanceResultCalculator : ResultCalculatorBase
    {
        public SpeciesObservationProvenanceResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {            
        }

        public List<SpeciesObservationProvenance> GetSpeciesObservationProvenances()
        {
            return CalculateProvenances();
        }

        /// <summary>
        /// Get the species observation provenances from web service.
        /// </summary>
        /// <returns>List of provenances that matches my settings.</returns>
        private List<SpeciesObservationProvenance> CalculateProvenances()
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (UserContext.CurrentRole.IsNull() && UserContext.CurrentRoles.Count > 0)
            {
                UserContext.CurrentRole = UserContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            var speciesObservationProvenanceList = CoreData.AnalysisManager.GetSpeciesObservationProvenancesBySearchCriteria(UserContext, searchCriteria, displayCoordinateSystem);
            speciesObservationProvenanceList = CreateOrderedSpeciesObservationProvenanceList(speciesObservationProvenanceList);
            return speciesObservationProvenanceList;
        }

        private List<SpeciesObservationProvenance> CreateOrderedSpeciesObservationProvenanceList(List<SpeciesObservationProvenance> speciesObservationProvenanceList)
        {
            const string OwnerIdentifier = "Owner";
            const string ObserverIdentifier = "Observer";
            const string ReporterIdentifier = "Reporter";
            const string DataProviderIdentifier = "DataProvider";
            SpeciesObservationProvenance ownerProvenance = null;
            SpeciesObservationProvenance observerProvenance = null;
            SpeciesObservationProvenance reporterProvenance = null;
            SpeciesObservationProvenance dataProviderProvenance = null;

            foreach (var speciesObservationProvenance in speciesObservationProvenanceList)
            {
                switch (speciesObservationProvenance.Name)
                {
                    case OwnerIdentifier:
                        ownerProvenance = speciesObservationProvenance;
                        break;
                    case ObserverIdentifier:
                        observerProvenance = speciesObservationProvenance;
                        break;
                    case ReporterIdentifier:
                        reporterProvenance = speciesObservationProvenance;
                        break;
                    case DataProviderIdentifier:
                        dataProviderProvenance = speciesObservationProvenance;
                        break;
                }                
            }

            List<SpeciesObservationProvenance> orderedProvenances = new List<SpeciesObservationProvenance>();
            if (dataProviderProvenance != null) orderedProvenances.Add(dataProviderProvenance);
            if (ownerProvenance != null) orderedProvenances.Add(ownerProvenance);
            if (observerProvenance != null) orderedProvenances.Add(observerProvenance);
            if (reporterProvenance != null) orderedProvenances.Add(reporterProvenance);

            return orderedProvenances;            
        }
    }
}