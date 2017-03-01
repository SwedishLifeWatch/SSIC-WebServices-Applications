using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation
{
    public class SpeciesObservationTaxonTableResultCalculator : ResultCalculatorBase
    {
        public SpeciesObservationTaxonTableResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public List<TaxonViewModel> GetResultFromCacheIfAvailableOrElseCalculate()
        {
            List<TaxonViewModel> result;
            var calculatedDataItemType = CalculatedDataItemType.SpeciesObservationTaxa;

            // Try get cached data.
            if (TryGetCachedCalculatedResult(calculatedDataItemType, out result))
            {
                return result;
            }

            // Calculate data.
            result = CalculateResult();
            AddResultToCache(calculatedDataItemType, result);

            return result;
        }

        public List<TaxonViewModel> CalculateResult()
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (UserContext.CurrentRole.IsNull() && UserContext.CurrentRoles.Count > 0)
            {
                UserContext.CurrentRole = UserContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            
            TaxonList taxonList = CoreData.AnalysisManager.GetTaxaBySearchCriteria(UserContext, searchCriteria, displayCoordinateSystem);
            List<TaxonViewModel> taxaList = taxonList.GetGenericList().ToTaxonViewModelList();

            return taxaList;
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            QueryComplexityEstimate complexityEstimate = new QueryComplexityEstimate();
            int taxonCount = MySettings.Filter.Taxa.TaxonIds.Count;

            if (taxonCount == 0)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Slow;
                complexityEstimate.ComplexityDescription.Text = Resources.Resource.ResultTaxonTableComplexityEstimateAllTaxa; // "Alla taxa är valda";
            }
            else if (taxonCount > 250 && taxonCount < 1000)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Medium;
                complexityEstimate.ComplexityDescription.Text = Resources.Resource.ResultTaxonTableComplexityEstimate250; // "Mer än 250 taxa är valda";
            }
            else if (taxonCount >= 1000)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Slow;
                complexityEstimate.ComplexityDescription.Text = Resources.Resource.ResultTaxonTableComplexityEstimate1000; // "Mer än 1000 taxa är valda";
            }
            else 
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Fast;                
            }
            return complexityEstimate;
        }
     }
}
