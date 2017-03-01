using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using Newtonsoft.Json;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation
{
    public class SpeciesObservationResultCalculator : ResultCalculatorBase
    {
        public SpeciesObservationResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {            
        }

        public SpeciesObservationsData GetSpeciesObservations(CoordinateSystemId displayCoordinateSystemId)
        {
            return CalculateSpeciesObservations(displayCoordinateSystemId);
            // Cache can get very large => Memory Exception
            //return base.GetResultFromCacheOrElseCalculate(CalculateSpeciesObservations);
        }

        public long GetSpeciesObservationsCount()
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            var searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (UserContext.CurrentRole.IsNull() && UserContext.CurrentRoles.Count > 0)
            {
                UserContext.CurrentRole = UserContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;

            AnalysisManager am = new AnalysisManager();
            return am.GetSpeciesObservationCountBySearchCriteria(UserContext, searchCriteria, displayCoordinateSystem);
        }

        public string GetSpeciesObservationsAsGeoJson(CoordinateSystemId displayCoordinateSystem)
        {
            var data = CalculateSpeciesObservations(displayCoordinateSystem);
            var model = SpeciesObservationsGeoJsonModel.CreateResult(data.SpeciesObservationList);
            return JsonConvert.SerializeObject(model.Points, JsonHelper.GetDefaultJsonSerializerSettings());
        }

        public SpeciesObservationsData CalculateSpeciesObservations(CoordinateSystemId displayCoordinateSystemId)
        {
            var displayCoordinateSystem = new CoordinateSystem(displayCoordinateSystemId);
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            var searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings, displayCoordinateSystem);
            if (UserContext.CurrentRole.IsNull() && UserContext.CurrentRoles.Count > 0)
            {
                UserContext.CurrentRole = UserContext.CurrentRoles[0];
            }
            
            var speciesObservationList = CoreData.SpeciesObservationManager.GetSpeciesObservations(UserContext, searchCriteria, displayCoordinateSystem);
            var fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(UserContext, MySettings);
            var fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            var speciesObservationsData = new SpeciesObservationsData(speciesObservationList, fieldDescriptionsViewModel);
            return speciesObservationsData;
        }

        public List<Dictionary<ViewTableField, string>> GetTableResult(
            CoordinateSystemId coordinateSystemId,
            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId)
        {
            SpeciesObservationsData speciesObservationsData = GetSpeciesObservations(coordinateSystemId);
            var viewManager = new SpeciesObservationTableViewManager(UserContext, MySettings);
            ViewTableViewModel viewModel = viewManager.CreateViewTableViewModel(speciesObservationTableColumnsSetId);
            var speciesObservationDataManager = new SpeciesObservationDataManager(UserContext, MySettings);
            List<Dictionary<ViewTableField, string>> dicResult = speciesObservationDataManager.GetObservationsListDictionary(speciesObservationsData.SpeciesObservationViewModels, viewModel.TableFields);
            return dicResult;
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate(bool tableResult = true)
        {
            var complexityEstimate = new QueryComplexityEstimate
            {
                QueryComplexityExecutionTime = QueryComplexityExecutionTime.Fast
            };

            if (!MySettings.Filter.Spatial.IsSettingsDefault())
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Medium;
                complexityEstimate.ComplexityDescription.Text = Resource.QueryComplexitySpatialFilterIsUsed;
            }
            if (MySettings.Filter.Taxa.TaxonIds.Count == 0)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Slow;
                complexityEstimate.ComplexityDescription.Text = Resource.QueryComplexityAllTaxaSelected;
                if (tableResult)
                {
                    complexityEstimate.ComplexityDescription.SuggestedResultViews.Add(new SpeciesObservationGridTableResultView());
                }
                else
                {
                    complexityEstimate.ComplexityDescription.SuggestedResultViews.Add(new SpeciesObservationGridMapResultView());
                }
            }
            else if (MySettings.Filter.Taxa.TaxonIds.Count > 50)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Medium;
                complexityEstimate.ComplexityDescription.Text = string.Format(Resource.QueryComplexityManyTaxaIsUsedTemplate, MySettings.Filter.Taxa.TaxonIds.Count);
            }

            return complexityEstimate;
        }
    }
}
