using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.WFS;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Converters;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using WmsCapabilites_1_3_0;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics
{
    using System.Text;

    using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
    using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;

    using ArtDatabanken.GIS.GeoJSON.Net;
    using ArtDatabanken.GIS.GeoJSON.Net.Geometry;

    /// <summary>
    /// Calculator class for summary statistics per polygon.
    /// </summary>
    public class SummaryStatisticsPerPolygonResultCalculator : ResultCalculatorBase
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="mySettings">
        /// The user settings.
        /// </param>
        public SummaryStatisticsPerPolygonResultCalculator(IUserContext userContext, MySettings mySettings)
            : base(userContext, mySettings)
        {            
        }

        /// <summary>
        /// Gets the number of observations and species per polygon.
        /// </summary>
        /// <returns>
        /// A list of counts and features.
        /// </returns>
        public List<SpeciesObservationsCountPerPolygon> GetSummaryStatisticsPerPolygonFromCacheIfAvailableOrElseCalculate()
        {
            List<SpeciesObservationsCountPerPolygon> result;
            var calculatedDataItemType = CalculatedDataItemType.SummaryStatisticsPerPolygon;
            string localeISOCode = UserContext.Locale.ISOCode;

            // Try get cached data.
            if (TryGetCachedCalculatedResult(calculatedDataItemType, localeISOCode, out result))
            {
                return result;
            }

            // Calculate data.
            result = CalculateSummaryStatisticsPerPolygon();
            AddResultToCache(calculatedDataItemType, localeISOCode, result);

            return result;
        }

        /// <summary>
        /// Calculates summary statistics per polygon (sets culture).
        /// </summary>
        /// <param name="locale">
        /// The current locale.
        /// </param>
        /// <returns>
        /// A list of statistics in form of pairs (calls a private method with same name and different signature).
        /// </returns>
        public List<SpeciesObservationsCountPerPolygon> CalculateSummaryStatisticsPerPolygon(ILocale locale)
        {
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentUICulture = locale.CultureInfo;
                Thread.CurrentThread.CurrentCulture = locale.CultureInfo;
                return this.CalculateSummaryStatisticsPerPolygon();
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        /// <summary>
        /// Estimates the complexity of the query being executed.
        /// </summary>
        /// <returns>
        /// The estimated complexity of the query.
        /// </returns>
        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            QueryComplexityEstimate complexityEstimate = new QueryComplexityEstimate();
            var calculatedDataItemType = CalculatedDataItemType.SummaryStatisticsPerPolygon;
            string localeISOCode = UserContext.Locale.ISOCode;
            CalculatedDataItem<List<SpeciesObservationsCountPerPolygon>> calculatedDataItem;

            calculatedDataItem = this.GetCacheCalculatedDataItem<List<SpeciesObservationsCountPerPolygon>>(calculatedDataItemType, localeISOCode);                
            if (calculatedDataItem.HasData)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Fast;
                complexityEstimate.EstimatedProcessTime = TimeSpan.FromSeconds(0);
                complexityEstimate.ComplexityDescription.Text = "Result stored in cache";
            }
            else if (MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData && MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Slow;
                complexityEstimate.ComplexityDescription.Text = Resources.Resource.ResultSummaryStatisticsPerPolygonComplexityEstimateBothCounts; // both number of observations and number of species selected
            }
            else if (MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData || MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Medium;
                complexityEstimate.ComplexityDescription.Text = MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData
                        ? Resources.Resource.ResultSummaryStatisticsPerPolygonComplexityEstimateObservationCount
                        : Resources.Resource.ResultSummaryStatisticsPerPolygonComplexityEstimateSpeciesCount;
            }

            return complexityEstimate;
        }

        /// <summary>
        /// Calculates summary statistics per polygon.
        /// </summary>
        /// <returns>
        /// A list of statistics in form of pairs.
        /// </returns>
        private List<SpeciesObservationsCountPerPolygon> CalculateSummaryStatisticsPerPolygon()
        {
            List<SpeciesObservationsCountPerPolygon> result = new List<SpeciesObservationsCountPerPolygon>();

            if (MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId.HasValue)
            {
                var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
                SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
                FeatureCollection featureCollection = GetFeatureCollection();               
                DataContext dataContext = new DataContext(this.UserContext);
                CoordinateSystem displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;

                searchCriteria.Polygons = new List<IPolygon>();
                foreach (Feature feature in featureCollection.Features)
                {
                    long speciesObservationCount = -1; // -1 means user did not choose to display the number of species observation
                    long speciesCount = -1; // -1 means user did not choose to display the number of species
                    string featureDescription = GetFeatureDescription(feature);
                    List<DataPolygon> dataPolygons = GetDataPolygons(feature);
                    searchCriteria.Polygons = dataPolygons.ToPolygons(dataContext);                

                    if (MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData)
                    {
                        speciesObservationCount = CoreData.AnalysisManager.GetSpeciesObservationCountBySearchCriteria(this.UserContext, searchCriteria, displayCoordinateSystem);
                    }

                    if (MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData)
                    {
                        speciesCount = CoreData.AnalysisManager.GetSpeciesCountBySearchCriteria(this.UserContext, searchCriteria, displayCoordinateSystem);
                    }

                    result.Add(new SpeciesObservationsCountPerPolygon
                    {
                        Properties = featureDescription,
                        SpeciesObservationsCount = speciesObservationCount == -1 ? "-" : Convert.ToString(speciesObservationCount),
                        SpeciesCount = speciesCount == -1 ? "-" : Convert.ToString(speciesCount)
                    });
                }                
            }

            return result;
        }

        /// <summary>
        /// Calculates species observation count per polygon and taxa.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <returns>
        /// A dictionary where the key is a polygon description. 
        /// The value is a dictionary where the key is TaxonId and the value is species observation count.
        /// </returns>
        public TaxonSpecificSpeciesObservationCountPerPolygonResult CalculateSpeciesObservationCountPerPolygonAndTaxa(List<int> taxonIds)
        {
            Dictionary<string, Dictionary<int, long>> speciesObservationData = new Dictionary<string, Dictionary<int, long>>();
            FeatureCollection featureCollection = GetFeatureCollection();            
            List<int> taxonIdList = new List<int>(taxonIds);
            if (taxonIdList.IsEmpty())
            {
                taxonIdList.Add(0);
            }
            
            if (MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId.HasValue &&
                featureCollection != null && featureCollection.Features.Count > 0)
            {
                SpeciesObservationSearchCriteriaManager searchCriteriaManager =
                    new SpeciesObservationSearchCriteriaManager(UserContext);
                SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
                DataContext dataContext = new DataContext(UserContext);
                CoordinateSystem displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;

                foreach (Feature feature in featureCollection.Features)
                {
                    string featureDescription = GetFeatureDescription(feature);
                    List<DataPolygon> dataPolygons = GetDataPolygons(feature);
                    searchCriteria.Polygons = dataPolygons.ToPolygons(dataContext);

                    foreach (int taxonId in taxonIdList)
                    {
                        searchCriteria.TaxonIds = new List<int>();
                        searchCriteria.TaxonIds.Add(taxonId);

                        long speciesObservationCount = CoreData.AnalysisManager.GetSpeciesObservationCountBySearchCriteria(UserContext, searchCriteria, displayCoordinateSystem);
                        if (!speciesObservationData.ContainsKey(featureDescription))
                        {
                            speciesObservationData.Add(featureDescription, new Dictionary<int, long>());
                        }

                        speciesObservationData[featureDescription].Add(taxonId, speciesObservationCount);
                    }
                }
            }

            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, taxonIdList);
            List<TaxonViewModel> taxaList = taxonList.GetGenericList().ToTaxonViewModelList();
            TaxonSpecificSpeciesObservationCountPerPolygonResult result = new TaxonSpecificSpeciesObservationCountPerPolygonResult()
            {
                Taxa = taxaList,
                SpeciesObservationCountPerPolygon = speciesObservationData
            };

            return result;
        }

        /// <summary>
        /// Gets feature collection based on selected WFS layer in SummaryStatistics.
        /// </summary>
        /// <returns>A feature collection or null.</returns>
        private FeatureCollection GetFeatureCollection()
        {
            if (!MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId.HasValue)
            {
                return null;
            }

            int wfsLayerId = MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId.Value;            
            WfsLayerSetting wfsLayer = MySettings.DataProvider.MapLayers.WfsLayers.FirstOrDefault(l => l.Id == wfsLayerId);
            string featuresUrl;
            string srsName = MySettings.Presentation.Map.PresentationCoordinateSystemId.EpsgCode();
            FeatureCollection featureCollection = null;            
            if (wfsLayer.IsFile)
            {
                featureCollection = MySettingsManager.GetMapDataFeatureCollection(
                    UserContext,
                    wfsLayer.GeometryName,
                    MySettings.Presentation.Map.PresentationCoordinateSystemId);
            }
            else
            {
                if (string.IsNullOrEmpty(wfsLayer.Filter))
                {
                    featuresUrl =
                        string.Format("{0}?service=wfs&version=1.1.0&request=GetFeature&typeName={1}&srsName={2}",
                            wfsLayer.ServerUrl, wfsLayer.TypeName, srsName);
                }
                else
                {
                    featuresUrl =
                        string.Format(
                            "{0}?service=wfs&version=1.1.0&request=GetFeature&typeName={1}&filter={2}&srsName={3}",
                            wfsLayer.ServerUrl, wfsLayer.TypeName, wfsLayer.Filter, srsName);
                }
                featureCollection = WFSManager.GetWfsFeaturesUsingHttpPost(featuresUrl);
            }
            
            return featureCollection;
        }

        /// <summary>
        /// Gets the data polygons from a feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>List with DataPolygons.</returns>
        private List<DataPolygon> GetDataPolygons(Feature feature)
        {
            GeoJSONObjectType polygonType = feature.Geometry.Type;
            List<DataPolygon> polygons = new List<DataPolygon>();
            if (!(polygonType == GeoJSONObjectType.MultiPolygon || polygonType == GeoJSONObjectType.Polygon))
            {
                return polygons;
            }

            DataPolygon dataPolygon;            
            if (polygonType == GeoJSONObjectType.MultiPolygon)
            {
                MultiPolygon multiPolygon = (MultiPolygon)feature.Geometry;
                foreach (Polygon polygon in multiPolygon.Coordinates)
                {
                    dataPolygon = DataPolygonConverter.ConvertToDataPolygon(polygon);
                    polygons.Add(dataPolygon);
                }                    
            }

            if (polygonType == GeoJSONObjectType.Polygon)
            {
                dataPolygon = DataPolygonConverter.ConvertToDataPolygon((Polygon)feature.Geometry);
                polygons.Add(dataPolygon);
            }

            return polygons;
        }

        /// <summary>
        /// Gets a description of the feature based on its properties.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>A description of the feature.</returns>
        private string GetFeatureDescription(Feature feature)
        {
            if (feature == null || feature.Properties == null || feature.Properties.Count == 0)
            {
                return "";
            }

            StringBuilder flattenedProperties = new StringBuilder();
            foreach (KeyValuePair<string, object> property in feature.Properties)
            {
                flattenedProperties.Append(property.Key + " = " + Convert.ToString(property.Value) + "<br />");
            }

            return flattenedProperties.ToString(0, flattenedProperties.Length - 6);            
        }
    }
}