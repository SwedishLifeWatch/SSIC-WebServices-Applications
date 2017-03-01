using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation
{
    public class PagedSpeciesObservationResultCalculator : ResultCalculatorBase
    {
        public PagedSpeciesObservationResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }          

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            return new QueryComplexityEstimate();
        }

        private Data.Polygon GetPolygon(double bottom, double left, double right, double top)
        {
            Data.Polygon polygon = new Data.Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            Data.LinearRing linearRing = new LinearRing();

            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Data.Point(left, bottom));
            linearRing.Points.Add(new Data.Point(left, top));
            linearRing.Points.Add(new Data.Point(right, top));
            linearRing.Points.Add(new Data.Point(right, bottom));
            linearRing.Points.Add(new Data.Point(left, bottom));
            
            polygon.LinearRings.Add(linearRing);
            
            return polygon;
        }

        public long GetTotalBboxCount(double bottom, double left, double right, double top)
        {
            return CalculateTotalCount(null, GetPolygon(bottom, left, right, top));            
        }

        public long GetTotalCount(int? taxonId)
        {
            if (taxonId.HasValue) // If it's a specific taxon, don't use cache
            {
                return CalculateTotalCount(taxonId, null);
            }

            return GetTotalCount();
        }

        public long GetTotalCount()
        {
            var cacheCalculatedData = GetCacheCalculatedDataItem<SpeciesObservationList>(CalculatedDataItemType.PagedSpeciesObservation);
            if (!cacheCalculatedData.TotalCount.HasValue)
            {
                cacheCalculatedData.TotalCount = CalculateTotalCount(null, null);
            }

            return cacheCalculatedData.TotalCount.Value;
        }

        protected long CalculateTotalCount(int? taxonId, Polygon bbox)
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (taxonId.HasValue)
            {
                searchCriteria.TaxonIds = new List<int> { taxonId.Value };
            }
            if (bbox != null)
            {
                searchCriteria.Polygons = new List<IPolygon> { bbox };
            }

            if (UserContext.CurrentRole.IsNull() && UserContext.CurrentRoles.Count > 0)
            {
                UserContext.CurrentRole = UserContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            long speciesObservationCount = CoreData.AnalysisManager.GetSpeciesObservationCountBySearchCriteria(UserContext, searchCriteria, displayCoordinateSystem);

            return speciesObservationCount;
        }

        public List<Dictionary<string, string>> GetTablePagedResult(int start, int pageSize)
        {
            SpeciesObservationFieldDescriptionViewManager fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(UserContext, MySettings);
            SpeciesObservationFieldDescriptionsViewModel fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            SpeciesObservationList speciesObservationList = GetPagedSpeciesObservationList(start, pageSize, null, null);
            SpeciesObservationsData speciesObservationsData = new SpeciesObservationsData(speciesObservationList, fieldDescriptionsViewModel);
            var viewManager = new SpeciesObservationTableViewManager(UserContext, MySettings);
            ViewTableViewModel viewModel = viewManager.CreateViewTableViewModel();
            var speciesObservationDataManager = new SpeciesObservationDataManager(UserContext, MySettings);
            List<Dictionary<string, string>> dicResult = speciesObservationDataManager.GetObservationsDataList(speciesObservationsData.SpeciesObservationViewModels, viewModel.TableFields);
            return dicResult;
        }

        public SpeciesObservationsGeoJsonModel GetMapPagedResult(int start, int pageSize, int? taxonId)
        {
            SpeciesObservationList speciesObservationList = GetPagedSpeciesObservationList(start, pageSize, taxonId, null);
            if (speciesObservationList == null)
            {
                throw new Exception(Resource.InformationExceptionNoObservationsFound);
            }
            return SpeciesObservationsGeoJsonModel.CreateResult(speciesObservationList, null);
        }

        protected SpeciesObservationList GetPagedSpeciesObservationList(int start, int pageSize, int? taxonId, Polygon bbox)
        {
            SpeciesObservationPageSpecification pageSpecification = new SpeciesObservationPageSpecification();
            pageSpecification.Size = pageSize;
            pageSpecification.Start = start;
            pageSpecification.SortOrder = new SpeciesObservationFieldSortOrderList();
            SpeciesObservationFieldSortOrder sortOrderOne = new SpeciesObservationFieldSortOrder();
            sortOrderOne.SortOrder = SortOrder.Descending;
            sortOrderOne.Class = new SpeciesObservationClass();
            sortOrderOne.Class.Id = SpeciesObservationClassId.Event;
            sortOrderOne.Property = new SpeciesObservationProperty();
            sortOrderOne.Property.Id = SpeciesObservationPropertyId.Start;
            pageSpecification.SortOrder.Add(sortOrderOne);

            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (taxonId.HasValue)
            {
                searchCriteria.TaxonIds = new List<int> { taxonId.Value };
            }
            if (bbox != null)
            {
                searchCriteria.Polygons = new List<IPolygon> { bbox };
            }

            if (UserContext.CurrentRole.IsNull() && UserContext.CurrentRoles.Count > 0)
            {
                UserContext.CurrentRole = UserContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            // Todo Add more pecifice page information

            //SpeciesObservationList speciesObservationList = CoreData.SpeciesObservationManager.GetSpeciesObservations(UserContext, searchCriteria, displayCoordinateSystem);
            SpeciesObservationList speciesObservationList = CoreData.SpeciesObservationManager.GetSpeciesObservations(UserContext, searchCriteria, displayCoordinateSystem, pageSpecification);
            return speciesObservationList;
        }

        public List<Dictionary<string, string>> GetTablePagedResultInBbox(int start, int pageSize, double bottom, double left, double right, double top)
        {
            Polygon bbox = GetPolygon(bottom, left, right, top);
            SpeciesObservationList speciesObservationList = GetPagedSpeciesObservationList(start, pageSize, null, bbox);
            var fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(UserContext, MySettings);
            var fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            SpeciesObservationsData speciesObservationsData = new SpeciesObservationsData(speciesObservationList, fieldDescriptionsViewModel);
            var viewManager = new SpeciesObservationTableViewManager(UserContext, MySettings);
            ViewTableViewModel viewModel = viewManager.CreateViewTableViewModel();
            var speciesObservationDataManager = new SpeciesObservationDataManager(UserContext, MySettings);
            List<Dictionary<string, string>> dicResult = speciesObservationDataManager.GetObservationsDataList(speciesObservationsData.SpeciesObservationViewModels, viewModel.TableFields);
            return dicResult;
        }

        public List<ResultObservationsListItem> GetTablePagedResultInBboxAsItems(int start, int pageSize, double bottom, double left, double right, double top)
        {
            Polygon bbox = GetPolygon(bottom, left, right, top);
            SpeciesObservationList speciesObservationList = GetPagedSpeciesObservationList(start, pageSize, null, bbox);
            var fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(UserContext, MySettings);
            var fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            SpeciesObservationsData speciesObservationsData = new SpeciesObservationsData(speciesObservationList, fieldDescriptionsViewModel);

            List<ResultObservationsListItem> list = new List<ResultObservationsListItem>();
            foreach (SpeciesObservationViewModel speciesObservationViewModel in speciesObservationsData.SpeciesObservationViewModels)
            {                
                list.Add(ResultObservationsListItem.Create(speciesObservationViewModel));
            }

            return list;
            //speciesObservationsData.SpeciesObservationViewModels
            //var viewManager = new SpeciesObservationTableViewManager(UserContext, MySettings);
            //ViewTableViewModel viewModel = viewManager.CreateViewTableViewModel();
            //var speciesObservationDataManager = new SpeciesObservationDataManager(UserContext, MySettings);
            //List<Dictionary<string, string>> dicResult = speciesObservationDataManager.GetObservationsDataList(speciesObservationsData.SpeciesObservationViewModels, viewModel.TableFields);
            //return dicResult;
        }
    }
}
