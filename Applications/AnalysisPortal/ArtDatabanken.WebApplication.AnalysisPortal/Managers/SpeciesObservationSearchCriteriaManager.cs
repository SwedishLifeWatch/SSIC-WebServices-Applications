using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.GIS.GisUtils;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class is used to create SpeciesObservationSearchCriteria objects
    /// </summary>
    public class SpeciesObservationSearchCriteriaManager
    {
        private readonly IUserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationSearchCriteriaManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public SpeciesObservationSearchCriteriaManager(IUserContext userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Creates the search criteria based on filter settings.
        /// </summary>
        /// <param name="mySettings">
        /// The my settings object.
        /// </param>
        /// <returns>
        /// The <see cref="SpeciesObservationSearchCriteria"/>.
        /// </returns>
        public SpeciesObservationSearchCriteria CreateSearchCriteria(
            MySettings.MySettings mySettings)
        {            
            return CreateSearchCriteria(
                mySettings,
                mySettings.Presentation.Map.DisplayCoordinateSystem);
        }

        /// <summary>
        /// Creates the search criteria based on filter settings.
        /// </summary>
        /// <param name="mySettings">
        /// The my settings object.
        /// </param>
        /// <returns>
        /// The <see cref="SpeciesObservationSearchCriteria"/>.
        /// </returns>
        public SpeciesObservationSearchCriteria CreateSearchCriteria(
            MySettings.MySettings mySettings,
            CoordinateSystemId coordinateSystemId)
        {
            return CreateSearchCriteria(
                mySettings,
                new CoordinateSystem { Id = coordinateSystemId });
        }

        /// <summary>
        /// Creates the search criteria based on filter settings.
        /// </summary>
        /// <param name="mySettings">
        /// The my settings object.
        /// </param>
        /// <returns>
        /// The <see cref="SpeciesObservationSearchCriteria"/>.
        /// </returns>
        public SpeciesObservationSearchCriteria CreateSearchCriteria(
            MySettings.MySettings mySettings, 
            ICoordinateSystem coordinateSystem)
        {
            var searchCriteria = new SpeciesObservationSearchCriteria();
                        
            // Default settings
            searchCriteria.IncludePositiveObservations = true;            

            // Taxa filter
            if (mySettings.Filter.Taxa.IsActive && mySettings.Filter.Taxa.HasSettings)
            {
                searchCriteria.TaxonIds = mySettings.Filter.Taxa.TaxonIds.ToList();    
            }

            // Temporal filter
            if (mySettings.Filter.Temporal.IsActive)
            {
                // Observation date
                if (mySettings.Filter.Temporal.ObservationDate.UseSetting)
                {
                    searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
                    searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
                    if (mySettings.Filter.Temporal.ObservationDate.Annually)
                    {
                        searchCriteria.ObservationDateTime.Begin = new DateTime(1800, 1, 1);
                        searchCriteria.ObservationDateTime.End = DateTime.Now.AddDays(1);
                        searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
                        IDateTimeInterval dateTimeInterval = new DateTimeInterval();                        
                        dateTimeInterval.Begin = mySettings.Filter.Temporal.ObservationDate.StartDate;
                        dateTimeInterval.End = mySettings.Filter.Temporal.ObservationDate.EndDate;
                        searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
                    }
                    else
                    {
                        searchCriteria.ObservationDateTime.Begin = mySettings.Filter.Temporal.ObservationDate.StartDate;
                        searchCriteria.ObservationDateTime.End = mySettings.Filter.Temporal.ObservationDate.EndDate;    
                    }                    
                }

                // Registration date
                if (mySettings.Filter.Temporal.RegistrationDate.UseSetting)
                {
                    searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
                    searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
                    if (mySettings.Filter.Temporal.RegistrationDate.Annually)
                    {
                        searchCriteria.ReportedDateTime.Begin = new DateTime(1800, 1, 1);
                        searchCriteria.ReportedDateTime.End = DateTime.Now.AddDays(1);
                        searchCriteria.ReportedDateTime.PartOfYear = new List<IDateTimeInterval>();
                        IDateTimeInterval dateTimeInterval = new DateTimeInterval();
                        dateTimeInterval.Begin = mySettings.Filter.Temporal.RegistrationDate.StartDate;
                        dateTimeInterval.End = mySettings.Filter.Temporal.RegistrationDate.EndDate;
                        searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
                    }
                    else
                    {
                        searchCriteria.ReportedDateTime.Begin = mySettings.Filter.Temporal.RegistrationDate.StartDate;
                        searchCriteria.ReportedDateTime.End = mySettings.Filter.Temporal.RegistrationDate.EndDate;
                    }
                }

                // Change date
                if (mySettings.Filter.Temporal.ChangeDate.UseSetting)
                {
                    searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
                    searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
                    if (mySettings.Filter.Temporal.ChangeDate.Annually)
                    {
                        searchCriteria.ChangeDateTime.Begin = new DateTime(1800, 1, 1);
                        searchCriteria.ChangeDateTime.End = DateTime.Now.AddDays(1);
                        searchCriteria.ChangeDateTime.PartOfYear = new List<IDateTimeInterval>();
                        IDateTimeInterval dateTimeInterval = new DateTimeInterval();
                        dateTimeInterval.Begin = mySettings.Filter.Temporal.ChangeDate.StartDate;
                        dateTimeInterval.End = mySettings.Filter.Temporal.ChangeDate.EndDate;
                        searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
                    }
                    else
                    {
                        searchCriteria.ChangeDateTime.Begin = mySettings.Filter.Temporal.ChangeDate.StartDate;
                        searchCriteria.ChangeDateTime.End = mySettings.Filter.Temporal.ChangeDate.EndDate;
                    }
                }
            }

            // Accuracy filter
            if (mySettings.Filter.Accuracy.IsActive)
            {
                if (mySettings.Filter.Accuracy.IsCoordinateAccuracyActive)
                {
                    searchCriteria.Accuracy = mySettings.Filter.Accuracy.MaxCoordinateAccuracy;
                    searchCriteria.IsAccuracyConsidered = mySettings.Filter.Accuracy.Inclusive;
                }
                else
                {
                    searchCriteria.Accuracy = null;
                    searchCriteria.IsAccuracyConsidered = false;
                }
            }
            
            // Spatial filter
            if (mySettings.Filter.Spatial.IsActive)
            {
                if (mySettings.Filter.Spatial.Polygons.Count > 0)
                {
                    var dataContext = new DataContext(_userContext);
                    ICoordinateSystem fromCoordinateSystem = mySettings.Filter.Spatial.PolygonsCoordinateSystem;                    
                    //ICoordinateSystem toCoordinateSystem = mySettings.Presentation.Map.DisplayCoordinateSystem;
                    ICoordinateSystem toCoordinateSystem = coordinateSystem;
                    List<IPolygon> polygons = mySettings.Filter.Spatial.Polygons.ToList().ToPolygons(dataContext);
                    polygons = GisTools.CoordinateConversionManager.GetConvertedPolygons(polygons, fromCoordinateSystem, toCoordinateSystem);

                    GeometryTools geometryTools = new GeometryTools();
                    foreach (IPolygon polygon in polygons)
                    {
                        var polygonStatus = geometryTools.ValidateGeometry(polygon);
                        if (!polygonStatus.IsValid)
                        {
                            throw new Exception(string.Format("Polygon error! {0}", polygonStatus.Description));
                        }
                    }

                    searchCriteria.Polygons = polygons;    
                }

                if (mySettings.Filter.Spatial.RegionIds.Count > 0)
                {
                    searchCriteria.RegionGuids = new List<string>();
                    RegionList regions = CoreData.RegionManager.GetRegionsByIds(_userContext, mySettings.Filter.Spatial.RegionIds.ToList());
                    foreach (IRegion region in regions)
                    {
                        searchCriteria.RegionGuids.Add(region.GUID);    
                    }                               
                }

                // Locality name search
                if (!string.IsNullOrEmpty(mySettings.Filter.Spatial.Locality.LocalityName))
                {
                    searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
                    searchCriteria.LocalityNameSearchString.SearchString = mySettings.Filter.Spatial.Locality.LocalityName;
                    searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
                    searchCriteria.LocalityNameSearchString.CompareOperators.Add(mySettings.Filter.Spatial.Locality.CompareOperator);                    
                }                
            }

            // Occurrence filter
            if (mySettings.Filter.Occurrence.IsActive)
            {
                searchCriteria.IncludeNeverFoundObservations = mySettings.Filter.Occurrence.IncludeNeverFoundObservations;
                searchCriteria.IncludeNotRediscoveredObservations = mySettings.Filter.Occurrence.IncludeNotRediscoveredObservations;
                searchCriteria.IncludePositiveObservations = mySettings.Filter.Occurrence.IncludePositiveObservations;

                if (mySettings.Filter.Occurrence.IsNaturalOccurrence ==
                    mySettings.Filter.Occurrence.IsNotNaturalOccurrence)
                {
                    // TODO: Set ignore IsNaturalOccurence
                }
                else
                {
                    searchCriteria.IsNaturalOccurrence = mySettings.Filter.Occurrence.IsNaturalOccurrence;
                }
            }

            // Data providers
            if (mySettings.DataProvider.DataProviders.IsActive && mySettings.DataProvider.DataProviders.HasSettings)
            {
                //Get all data providers
                bool providersWithObservationDisabled;
                var dataProviders = DataProviderManager.GetAllDataProviders(_userContext, out providersWithObservationDisabled);

                //If all providers are selected and we don't have any providers with observation disabled, we don't set data source guids since it will decrease performance
                //All existing providers will be used if the property is not set and the performance is better compared to setting all providers explicit
                if (providersWithObservationDisabled || dataProviders.Count != mySettings.DataProvider.DataProviders.DataProvidersGuids.Count)
                {
                    searchCriteria.DataSourceGuids = new List<string>();
                    foreach (var guid in mySettings.DataProvider.DataProviders.DataProvidersGuids)
                    {
                        searchCriteria.DataSourceGuids.Add(guid);
                    }
                }
            }            

            // Field filter
            if (mySettings.Filter.Field.IsActive && mySettings.Filter.Field.HasSettings)
            {
                searchCriteria.FieldSearchCriteria = new SpeciesObservationFieldSearchCriteriaList();

                foreach (var fieldFilterExpression in mySettings.Filter.Field.FieldFilterExpressions)
                {
                    searchCriteria.FieldSearchCriteria.Add(fieldFilterExpression);
                }

                searchCriteria.FieldLogicalOperator = mySettings.Filter.Field.FieldLogicalOperator;
            }

            return searchCriteria;            
        }

        ///// <summary>
        ///// Determines whether the specified filter settings is OK and we
        ///// can use them to do a search and get a valid result.
        ///// </summary>
        ///// <param name="filterSettings">The filter settings.</param>
        ///// <returns></returns>
        //public FilterSettingsStatus IsFilterSettingsOK(FilterSettings filterSettings)
        //{
        //    if (!filterSettings.Taxa.IsActive)
        //    {
        //        return FilterSettingsStatus.CreateInvalidStatus(Resources.Resource.FilterSettingsInvalidNoTaxaSelected);
        //    }
        //    if (filterSettings.Taxa.TaxonIds.Count == 0)
        //    {
        //        return FilterSettingsStatus.CreateInvalidStatus(Resources.Resource.FilterSettingsInvalidNoTaxaSelected);
        //    }

        //    return FilterSettingsStatus.CreateValidStatus();
        //}

        // MK 2015-11-24  - This is not needed now. Commented out and saved for future implementation.
        //private SearchViewModel CreateRedListCategorySearchViewModel(IEnumerable<RedListCategory> redListCategories)
        //{
        //    SearchViewModel model = new SearchViewModel();
        //    model.IsRedListCategoriesEnabled = true;
        //    model.RedListCategories = new List<RedListCategoryItemViewModel>();

        //    foreach (RedListCategory redListCategory in redListCategories)
        //    {
        //        RedListCategoryItemViewModel redListCategoryItemViewModel = new RedListCategoryItemViewModel()
        //        {
        //            Id = 0,
        //            OrderNumber = (Int32)redListCategory,
        //            Name = "Test0",
        //            Selected = true
        //        };

        //        model.RedListCategories.Add(redListCategoryItemViewModel);
        //    }

        //    return model;
        //}

        //public List<int> GetTaxaByRedListCategories()
        //{
        //    // If not active or no red list categories set => return empty list.
        //    if (!SessionHandler.MySettings.Filter.RedList.IsActive ||
        //        !SessionHandler.MySettings.Filter.RedList.Categories.Any())
        //    {
        //        return new List<int>();
        //    }

        //    SearchViewModel searchViewModel = CreateRedListCategorySearchViewModel(SessionHandler.MySettings.Filter.RedList.Categories);

        //    var applicationUserContext = CoreData.UserManager.GetApplicationContext();
        //    RedListSearchManager manager = new RedListSearchManager(applicationUserContext);
        //    List<TaxonListInformation> searchResult = manager.SearchTaxa(searchViewModel, null);

        //    return searchResult.Select(item => item.Id).ToList();
        //}
    }
}
