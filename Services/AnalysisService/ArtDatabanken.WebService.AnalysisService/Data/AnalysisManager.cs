using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;
using ArtDatabanken.Database;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using ArtDatabanken.GIS.WFS;
using ArtDatabanken.WebService.AnalysisService.Database;
using ArtDatabanken.WebService.Data;
using Microsoft.SqlServer.Types;
using WebSpeciesFact = ArtDatabanken.WebService.Data.WebSpeciesFact;
using WebSpeciesObservationSearchCriteria = ArtDatabanken.WebService.Data.WebSpeciesObservationSearchCriteria;
using WebTaxon = ArtDatabanken.WebService.Data.WebTaxon;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using Newtonsoft.Json;
using ArtDatabanken.GIS.FormatConversion;
using ArtDatabanken.GIS.Extensions;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using WebDataField = ArtDatabanken.WebService.Data.WebDataField;

namespace ArtDatabanken.WebService.AnalysisService.Data
{
    /// <summary>
    /// Manager class for Analysis Service, handling species observation calculations etc. 
    /// </summary>
    public static class AnalysisManager
    {
        #region Public Methods

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>        
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>        
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature as json.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public static List<WebGridCellCombinedStatistics>
            GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
            WebServiceContext context,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebGridSpecification gridSpecification,
            WebCoordinateSystem coordinateSystem,
            String featuresUrl,
            String featureCollectionJson = null)
        {
            List<WebGridCellSpeciesCount> speciesObservationResult = GetGridSpeciesCounts(context, searchCriteria, gridSpecification, coordinateSystem);
            var featuresUri = string.IsNullOrEmpty(featuresUrl) ? null : new Uri(featuresUrl);
            List<WebGridCellFeatureStatistics> featureStatisticsResult = GetGridCellFeatureStatistics(context, featuresUri, featureCollectionJson, gridSpecification, coordinateSystem);
            List<WebGridCellCombinedStatistics> mergedResult = MergeFeatureStatisticsWithSpeciesObservationCounts(featureStatisticsResult, speciesObservationResult);
            return mergedResult;
        }

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>        
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>        
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature as json</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public static List<WebGridCellCombinedStatistics>
            GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCountsElasticsearch(
            WebServiceContext context,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebGridSpecification gridSpecification,
            WebCoordinateSystem coordinateSystem,
            String featuresUrl,
            String featureCollectionJson = null)
        {
            var featuresUri = string.IsNullOrEmpty(featuresUrl) ? null : new Uri(featuresUrl);
            var speciesObservationResult = GetGridSpeciesCountsElasticsearch(context, searchCriteria, gridSpecification, coordinateSystem);
            var featureStatisticsResult = GetGridCellFeatureStatistics(context, featuresUri, featureCollectionJson, gridSpecification, coordinateSystem);

            return MergeFeatureStatisticsWithSpeciesObservationCounts(featureStatisticsResult, speciesObservationResult);
        }

        /// <summary>
        /// Get grid cell species observation results from species observation search criteria - WebSpeciesObservationSearchCriteria,
        /// grid cell specifications - WebGridSpecifications and
        /// and specified what coordinate system to use in WebCoordinateSystem.
        /// A list of grid cell species observation results is returned.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestsId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc. GridCellSize and GridCoordinateSystem are the
        /// only properties that is used in this method. If Bounding Box is set then WebSpeciesObservationSearchCriteria.BoundingBox
        /// is overriding with WebGridSpecifications.Box.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns> A list of WebGridCellSpeciesObservationCount i.e grid cell result class.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo",
            MessageId = "System.String.ToUpper"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static List<WebGridCellSpeciesObservationCount> GetGridSpeciesObservationCounts(
            WebServiceContext context,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebGridSpecification gridSpecification,
            WebCoordinateSystem coordinateSystem)
        {
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context, false, null, false);
            coordinateSystem.CheckData();
            if (searchCriteria.BoundingBox != null && gridSpecification.IsNotNull())
            {
                gridSpecification.CheckData();
                gridSpecification.CheckGridSpecificationsForSpeciesCount(searchCriteria.BoundingBox);
            }

            // If bounding box is set in grid specifications then overriding species observation search criteria bounding box.
            if (gridSpecification != null &&
                (gridSpecification.IsNotNull() && gridSpecification.BoundingBox.IsNotNull()))
            {
                WebCoordinateSystem gridCellCoordinateSystemAsWebCoordinateSystem =
                    gridSpecification.GetWebCoordinateSystem();
                searchCriteria.BoundingBox =
                    WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(gridSpecification.BoundingBox,
                        gridCellCoordinateSystemAsWebCoordinateSystem,
                        coordinateSystem).GetBoundingBox();
            }

            // Get grid specific properties
            Int32? gridCellSize = null;
            GridCoordinateSystem gridCellCoordinateSystem;

            if (gridSpecification != null && gridSpecification.GridCoordinateSystem.IsNotNull())
            {
                gridCellCoordinateSystem = gridSpecification.GridCoordinateSystem;
            }
            else
            {
                // Default GridCell coordinate system is RT 90 2.5 gon Väst
                gridCellCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            }

            if (gridSpecification != null && gridSpecification.IsGridCellSizeSpecified)
            {
                gridCellSize = gridSpecification.GridCellSize;
            }


            // Check grid coordinate system.
            String gridCoordinateSystemTemp = null;
            if (gridSpecification != null)
            {
                switch (gridSpecification.GridCoordinateSystem)
                {
                    case GridCoordinateSystem.Rt90_25_gon_v:
                        gridCoordinateSystemTemp = "RT90";
                        break;
                    case GridCoordinateSystem.SWEREF99_TM:
                        gridCoordinateSystemTemp = "SWEREF99";
                        break;
                    default:
                        throw new ArgumentException("Not handled GridCoordinateSystem specified: " +
                                                    gridSpecification.GridCoordinateSystem);
                }
            }

            // Set Sweden extent if there are no polygons
            if (searchCriteria.Polygons.IsEmpty() && searchCriteria.RegionGuids.IsEmpty())
            {
                WebPolygon swedenExtentBoundingBox = GeometryManager.GetSwedenExtentBoundingBoxPolygon(coordinateSystem);
                searchCriteria.Polygons = new List<WebPolygon> { swedenExtentBoundingBox };
            }

            // Check if too many grid cells will be generated.
            searchCriteria.ValidateGridCellSize(coordinateSystem, gridSpecification);

            // Build up all data required to perform/build up db search.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                searchCriteria,
                coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);

            // Create result list.
            List<WebGridCellSpeciesObservationCount> gridList = new List<WebGridCellSpeciesObservationCount>();
            using (DataReader dataReader = context.GetAnalysisDatabase().GetGridCellSpeciesObservationCounts(polygons,
                regionIds,
                taxonIds,
                joinCondition,
                whereCondition,
                gridCoordinateSystemTemp,
                gridCellSize,
                speciesObservationIds))
            {
                while (dataReader.Read())
                {
                    WebGridCellSpeciesObservationCount observationCount = new WebGridCellSpeciesObservationCount();
                    observationCount.LoadData(dataReader);
                    observationCount.GridCoordinateSystem = gridCellCoordinateSystem;
                    observationCount.CoordinateSystem = coordinateSystem;
                    gridList.Add(observationCount);
                }
            }

            if (gridSpecification != null)
            {
                WebCoordinateSystem gridCellCoordinateSystemAsWebCoordinateSystemTemp =
                    gridSpecification.GetWebCoordinateSystem();
                foreach (WebGridCellSpeciesObservationCount gridCell in gridList)
                {
                    if (gridSpecification.GridCellGeometryType.Equals(GridCellGeometryType.Polygon))
                    {
                        gridCell.BoundingBox = GetConvertedBoundingBoxCoordinates(coordinateSystem,
                            gridCellCoordinateSystemAsWebCoordinateSystemTemp,
                            gridCell.OrginalBoundingBox);
                    }
                    else
                    {
                        gridCell.BoundingBox = null;
                    }

                    // Must always return the centrepoint
                    gridCell.CentreCoordinate = GetConvertedPointCoordinates(coordinateSystem,
                        gridCellCoordinateSystemAsWebCoordinateSystemTemp,
                        gridCell.OrginalCentreCoordinate);
                    gridCell.GeometryType = gridSpecification.GridCellGeometryType;
                }
            }

            if (gridSpecification.BoundingBox.IsNotNull())
            {
                gridList = RemoveGridCellSpeciesObservationCountsOutsideBounds(gridList, gridSpecification);
            }

            return gridList;
        }

        public static List<WebGridCellSpeciesObservationCount> GetGridSpeciesObservationCountsElasticsearch(
            WebServiceContext context,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebGridSpecification gridSpecification,
            WebCoordinateSystem coordinateSystem)
        {
            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");

            var gridList = new List<WebGridCellSpeciesObservationCount>();

            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                foreach (var gridSpecieCount in elastisearch.GetGridSpeciesCounts(filter.ToString(), gridSpecification))
                {
                    var speciesCount = new WebGridCellSpeciesObservationCount();
                    speciesCount.LoadData(gridSpecieCount, gridSpecification, coordinateSystem);
                    gridList.Add(speciesCount);
                }
            }

            if (gridSpecification != null)
            {
                var gridCellCoordinateSystemAsWebCoordinateSystemTemp = gridSpecification.GetWebCoordinateSystem();
                foreach (var gridCell in gridList)
                {
                    if (gridSpecification.GridCellGeometryType.Equals(GridCellGeometryType.Polygon))
                    {
                        gridCell.BoundingBox = GetConvertedBoundingBoxCoordinates(coordinateSystem, gridCellCoordinateSystemAsWebCoordinateSystemTemp, gridCell.OrginalBoundingBox);
                    }
                    else
                    {
                        gridCell.BoundingBox = null;
                    }

                    // Must always return the centrepoint
                    gridCell.CentreCoordinate = GetConvertedPointCoordinates(coordinateSystem, gridCellCoordinateSystemAsWebCoordinateSystemTemp, gridCell.OrginalCentreCoordinate);
                    gridCell.GeometryType = gridSpecification.GridCellGeometryType;
                }

                if (gridSpecification.BoundingBox.IsNotNull())
                {
                    gridList = RemoveGridCellSpeciesObservationCountsOutsideBounds(gridList, gridSpecification);
                }
            }

            return gridList;
        }

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        public static List<WebTaxon> GetHostsBySpeciesFactSearchCriteria(WebServiceContext context,
            WebSpeciesFactSearchCriteria searchCriteria)
        {
            List<Int32> hostIds;
            List<WebSpeciesFact> speciesFacts;
            List<WebTaxon> hosts;

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get species facts from web service.
            // TODO: Check users access rights for species facts.
            speciesFacts = WebServiceData.SpeciesFactManager.GetSpeciesFactsBySearchCriteria(context,
                searchCriteria);

            // Get host ids.
            hostIds = new List<Int32>();
            if (speciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.IsHostSpecified &&
                        (speciesFact.HostId > 0) &&
                        !hostIds.Contains(speciesFact.HostId))
                    {
                        hostIds.Add(speciesFact.HostId);
                    }
                }
            }

            // Get hosts.
            hosts = null;
            if (hostIds.IsNotEmpty())
            {
                hosts = WebServiceData.TaxonManager.GetTaxaByIds(context,
                    hostIds);
            }

            return hosts;
        }

        /// <summary>
        /// Removes the grid cells outside grid bounds.
        /// </summary>
        /// <param name="gridCellObservations">The grid cell observations.</param>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <returns>A list with all <paramref name="gridCellObservations"/> except the ones outside the grid bounds.</returns>
        private static List<WebGridCellSpeciesObservationCount> RemoveGridCellSpeciesObservationCountsOutsideBounds(
            List<WebGridCellSpeciesObservationCount> gridCellObservations, WebGridSpecification gridSpecification)
        {
            List<WebGridCellSpeciesObservationCount> result = new List<WebGridCellSpeciesObservationCount>();

            WebBoundingBox boundingBox = gridSpecification.BoundingBox;
            int cellSize = gridSpecification.GridCellSize;
            double xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;

            foreach (WebGridCellSpeciesObservationCount gridCell in gridCellObservations)
            {
                if (gridCell.OrginalBoundingBox.Min.X >= xMin &&
                    gridCell.OrginalBoundingBox.Min.Y >= yMin &&
                    gridCell.OrginalBoundingBox.Max.X <= xMax &&
                    gridCell.OrginalBoundingBox.Max.Y <= yMax)
                {
                    result.Add(gridCell);
                }
            }

            return result;
        }


        /// <summary>
        /// Get grid species (Taxa of category species) results from species observation search criteria - WebSpeciesObservationSearchCriteria,
        /// grid cell specifications - WebGridSpecifications and
        /// and specified what coordinate system to use in WebCoordinateSystem.
        /// A list of grid cell selected species results is returned.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc. GridCellSize and GridCoordinateSystem are the
        /// only properties that is used in this method. If Bounding Box is set then WebSpeciesObservationSearchCriteria.BoundingBox
        /// is overriding with WebGridSpecifications.Box.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns> A list of WebGridCellSpeciesCount i.e grid cell result class.</returns>
        public static List<WebGridCellSpeciesCount> GetGridSpeciesCounts(WebServiceContext context,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebGridSpecification gridSpecification,
            WebCoordinateSystem coordinateSystem)
        {
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context, false, null, false);
            coordinateSystem.CheckData();

            // Check if too many grid cells will be generated.
            if (gridSpecification.IsGridCellSizeSpecified && gridSpecification.GridCellSize < 100)
            {
                throw new ArgumentException(string.Format("Grid cell size is too small. Cell Size: {0}",
                    gridSpecification.GridCellSize));
            }

            if (searchCriteria.BoundingBox != null && gridSpecification.IsNotNull())
            {
                gridSpecification.CheckData();
                gridSpecification.CheckGridSpecifications(searchCriteria.BoundingBox);
            }

            // If bounding box is set in grid specifications then override species observation search criteria bounding box.
            if (gridSpecification.IsNotNull() && gridSpecification.BoundingBox.IsNotNull())
            {
                WebCoordinateSystem gridCellCoordinateSystemAsWebCoordinateSystem =
                    gridSpecification.GetWebCoordinateSystem();
                searchCriteria.BoundingBox =
                    WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(gridSpecification.BoundingBox,
                        gridCellCoordinateSystemAsWebCoordinateSystem,
                        coordinateSystem).GetBoundingBox();
            }

            // Get grid specific properties
            Int32? gridCellSize = null;
            GridCoordinateSystem gridCellCoordinateSystem;

            if (gridSpecification != null && gridSpecification.GridCoordinateSystem.IsNotNull())
            {
                gridCellCoordinateSystem = gridSpecification.GridCoordinateSystem;
            }
            else
            {
                // Default GridCell coordinate system is RT 90 2.5 g v
                gridCellCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            }

            if (gridSpecification != null && gridSpecification.IsGridCellSizeSpecified)
            {
                gridCellSize = gridSpecification.GridCellSize;
            }


            // Check grid coordinate system.
            String gridCoordinateSystemTemp = null;
            if (gridSpecification != null)
            {
                switch (gridSpecification.GridCoordinateSystem)
                {
                    case GridCoordinateSystem.Rt90_25_gon_v:
                        gridCoordinateSystemTemp = "RT90";
                        break;
                    case GridCoordinateSystem.SWEREF99_TM:
                        gridCoordinateSystemTemp = "SWEREF99";
                        break;
                    default:
                        throw new ArgumentException("Not handled GridCoordinateSystem specified: " +
                                                    gridSpecification.GridCoordinateSystem);
                }
            }

            // Set Sweden extent if there are no polygons or regions specified.
            if (searchCriteria.Polygons.IsEmpty() && searchCriteria.RegionGuids.IsEmpty())
            {
                WebPolygon swedenExtentBoundingBox = GeometryManager.GetSwedenExtentBoundingBoxPolygon(coordinateSystem);
                searchCriteria.Polygons = new List<WebPolygon> { swedenExtentBoundingBox };
            }

            // Check if too many grid cells will be generated.
            searchCriteria.ValidateGridCellSize(coordinateSystem, gridSpecification);

            // Build upp all data required to performe/bulid up db search.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                searchCriteria,
                coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);


            // Create result list.
            List<WebGridCellSpeciesCount> gridList = new List<WebGridCellSpeciesCount>();
            using (DataReader dataReader = context.GetAnalysisDatabase().GetGridCellSpeciesCounts(polygons,
                regionIds,
                taxonIds,
                joinCondition,
                whereCondition,
                gridCoordinateSystemTemp,
                gridCellSize,
                speciesObservationIds))
            {
                while (dataReader.Read())
                {
                    WebGridCellSpeciesCount speciesCount = new WebGridCellSpeciesCount();
                    speciesCount.LoadData(dataReader);
                    speciesCount.GridCoordinateSystem = gridCellCoordinateSystem;
                    speciesCount.CoordinateSystem = coordinateSystem;
                    gridList.Add(speciesCount);
                }
            }

            string gridCoordinateName = Enum.GetName(typeof(GridCoordinateSystem), gridCellCoordinateSystem);
            if (gridCoordinateName == null || gridSpecification == null)
            {
                return gridList;
            }

            WebCoordinateSystem gridCellCoordinateSystemAsWebCoordinateSystemTemp =
                gridSpecification.GetWebCoordinateSystem();
            foreach (WebGridCellSpeciesCount gridCell in gridList)
            {
                gridCell.BoundingBox = null;
                gridCell.CentreCoordinate = null;
                gridCell.GeometryType = GridCellGeometryType.CentrePoint;
                if (gridSpecification.GridCellGeometryType.Equals(GridCellGeometryType.Polygon))
                {
                    WebPolygon webBoundingBox = GetConvertedBoundingBoxCoordinates(
                        coordinateSystem,
                        gridCellCoordinateSystemAsWebCoordinateSystemTemp,
                        gridCell.OrginalBoundingBox);

                    gridCell.BoundingBox = webBoundingBox;
                    gridCell.GeometryType = GridCellGeometryType.Polygon;
                }

                //// Must always return centre point
                gridCell.CentreCoordinate = GetConvertedPointCoordinates(coordinateSystem,
                    gridCellCoordinateSystemAsWebCoordinateSystemTemp,
                    gridCell.OrginalCentreCoordinate);
            }

            if (gridSpecification.BoundingBox.IsNotNull())
            {
                gridList = RemoveGridCellSpeciesCountsOutsideBounds(gridList, gridSpecification);
            }

            return gridList;
        }

        /// <summary>
        /// Get grid species (Taxa of category species) results from species observation search criteria - WebSpeciesObservationSearchCriteria,
        /// grid cell specifications - WebGridSpecifications and
        /// and specified what coordinate system to use in WebCoordinateSystem.
        /// A list of grid cell selected species results is returned.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc. GridCellSize and GridCoordinateSystem are the
        /// only properties that is used in this method. If Bounding Box is set then WebSpeciesObservationSearchCriteria.BoundingBox
        /// is overriding with WebGridSpecifications.Box.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns> A list of WebGridCellSpeciesCount i.e grid cell result class.</returns>
        public static List<WebGridCellSpeciesCount> GetGridSpeciesCountsElasticsearch(WebServiceContext context,
                                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                      WebGridSpecification gridSpecification,
                                                                                      WebCoordinateSystem coordinateSystem)
        {
            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");

            var gridList = new List<WebGridCellSpeciesCount>();

            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                foreach (var gridSpecieCount in elastisearch.GetGridSpeciesCounts(filter.ToString(), gridSpecification))
                {
                    var speciesCount = new WebGridCellSpeciesCount();
                    speciesCount.LoadData(gridSpecieCount, gridSpecification, coordinateSystem);
                    gridList.Add(speciesCount);
                }
            }

            //TODO: Convert this conversion logic to a function (it's duplicated in several 'grid-functions')
            var gridCellCoordinateSystemAsWebCoordinateSystemTemp = gridSpecification.GetWebCoordinateSystem();
            foreach (var gridCell in gridList)
            {
                gridCell.BoundingBox = null;
                gridCell.CentreCoordinate = null;
                gridCell.GeometryType = GridCellGeometryType.CentrePoint;
                if (gridSpecification.GridCellGeometryType.Equals(GridCellGeometryType.Polygon))
                {
                    var webBoundingBox = GetConvertedBoundingBoxCoordinates(coordinateSystem, gridCellCoordinateSystemAsWebCoordinateSystemTemp, gridCell.OrginalBoundingBox);
                    gridCell.BoundingBox = webBoundingBox;
                    gridCell.GeometryType = GridCellGeometryType.Polygon;
                }
                //// Must always return centre point
                gridCell.CentreCoordinate = GetConvertedPointCoordinates(coordinateSystem, gridCellCoordinateSystemAsWebCoordinateSystemTemp, gridCell.OrginalCentreCoordinate);
            }

            if (gridSpecification.BoundingBox.IsNotNull())
            {
                gridList = RemoveGridCellSpeciesCountsOutsideBounds(gridList, gridSpecification);
            }

            return gridList;
        }

        /// <summary>
        /// Removes the grid cells outside grid bounds.
        /// </summary>
        /// <param name="gridCellObservations">The grid cell observations.</param>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <returns>A list with all <paramref name="gridCellObservations"/> except the ones outside the grid bounds.</returns>
        private static List<WebGridCellSpeciesCount> RemoveGridCellSpeciesCountsOutsideBounds(List<WebGridCellSpeciesCount> gridCellObservations, WebGridSpecification gridSpecification)
        {
            List<WebGridCellSpeciesCount> result = new List<WebGridCellSpeciesCount>();

            WebBoundingBox boundingBox = gridSpecification.BoundingBox;
            int cellSize = gridSpecification.GridCellSize;
            double xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;

            foreach (WebGridCellSpeciesCount gridCell in gridCellObservations)
            {
                if (gridCell.OrginalBoundingBox.Min.X >= xMin &&
                    gridCell.OrginalBoundingBox.Min.Y >= yMin &&
                    gridCell.OrginalBoundingBox.Max.X <= xMax &&
                    gridCell.OrginalBoundingBox.Max.Y <= yMax)
                {
                    result.Add(gridCell);
                }
            }

            return result;
        }

        /// <summary>
        /// Get information about spatial features in a grid representation inside a user supplied bounding box.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="featuresUri">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc..</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>Statistical measurements on spatial features in grid format.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context")]
        public static List<WebGridCellFeatureStatistics> GetGridCellFeatureStatistics(
            WebServiceContext context,
            Uri featuresUri,
            String featureCollectionJson,
            WebGridSpecification gridSpecification,
            WebCoordinateSystem coordinateSystem)
        {
            FeatureCollection featureCollection = null;
            WfsBoundingBox wfsBoundingBox = null;

            coordinateSystem.CheckData();
            gridSpecification.CheckData();
            var srid = GeometryManager.GetSridFromGridCoordinateSystem(gridSpecification.GridCoordinateSystem);
            var sridName = "EPSG:" + srid;

            // If no BoundingBox is choosen select Sweden ar reference BoundingBox to start from.
            if (gridSpecification.BoundingBox.IsNull())
            {
                // Set default bounding box to Sweden extents
                gridSpecification.BoundingBox = GeometryManager.GetSwedenExtentBoundingBox(gridSpecification.GridCoordinateSystem.ToWebCoordinateSystem());
            }
            else
            {
                wfsBoundingBox = new WfsBoundingBox(gridSpecification.BoundingBox.Min.X, gridSpecification.BoundingBox.Min.Y, gridSpecification.BoundingBox.Max.X, gridSpecification.BoundingBox.Max.Y, sridName);
            }

            if (featuresUri != null)
            {
                featuresUri.ToString().CheckNotEmpty("featuresUrl");

                // Get features to retrive enviromental data from.
                featuresUri = new Uri(WFSManager.AddOrReplaceSrsInUrl(featuresUri.ToString(), sridName));

                // featureCollection = WFSManager.GetWfsFeatures(featuresUrl, WFSVersion.Ver110);
                featureCollection = WFSManager.GetWfsFeaturesUsingHttpPost(featuresUri.ToString(), wfsBoundingBox);
            }
            else if (!string.IsNullOrEmpty(featureCollectionJson))
            {
                featureCollection = JsonConvert.DeserializeObject(featureCollectionJson, typeof(FeatureCollection)) as FeatureCollection;
            }

            // If no features found return empty feature statistics.
            if (featureCollection == null || featureCollection.Features.IsEmpty())
            {
                return new List<WebGridCellFeatureStatistics>();
            }

            // Convert features to sqlgeometies
            var sqlGeometryObjectList = ConvertFeatureCollectionToSqlGeometries(featureCollection, srid);

            // Get features modified ie calucalted result data etc adjusted and added into List<WebGridCellFeatureStatistics>
            var webGridCellFeatureStatisticsList = GeometryManager.GetFeaturesBasedOnGridCells(sqlGeometryObjectList, gridSpecification, srid);

            // Convert coordinates if needed
            if (coordinateSystem.Id == gridSpecification.GridCoordinateSystem.ToWebCoordinateSystem().Id)
            {
                return webGridCellFeatureStatisticsList;
            }

            WebCoordinateSystem fromCoordinateSystem = gridSpecification.GridCoordinateSystem.ToWebCoordinateSystem();
            ConvertWebFeatureStatisticsCoordinates(webGridCellFeatureStatisticsList, fromCoordinateSystem, coordinateSystem);
            return webGridCellFeatureStatisticsList;
        }

        /// <summary>
        /// Get no of species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public static Int64 GetSpeciesCountBySearchCriteria(WebServiceContext context,
                                                            WebSpeciesObservationSearchCriteria searchCriteria,
                                                            WebCoordinateSystem coordinateSystem)
        {
            Int64 speciesObservationCount;
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<int> regionIds, taxonIds;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get all data required to performe/build up a db search.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);
            AnalysisServer analysisServer = context.GetAnalysisDatabase();

            speciesObservationCount = analysisServer.GetSpeciesCountBySearchCriteria(polygons,
                                                                                     regionIds,
                                                                                     taxonIds,
                                                                                     joinCondition,
                                                                                     whereCondition,
                                                                                     speciesObservationIds);
            return speciesObservationCount;
        }

        /// <summary>
        /// Get no of species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public static Int64 GetSpeciesCountBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                         WebSpeciesObservationSearchCriteria searchCriteria,
                                                                         WebCoordinateSystem coordinateSystem)
        {
            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");

            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                return elastisearch.GetSpeciesCount(filter.ToString());
            }
        }

        /// <summary>
        /// Get no of species observations that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>No of observations that matches search criteria.</returns>
        /// 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static Int64 GetSpeciesObservationCountBySearchCriteria(WebServiceContext context,
                                                                       WebSpeciesObservationSearchCriteria searchCriteria,
                                                                       WebCoordinateSystem coordinateSystem)
        {
            Int64 speciesObservationCount;
            String geometryWhereCondition, joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get species observation ids.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);

            // If speciesObservationIds is empty return o otherwise return number of ids.
            speciesObservationCount = speciesObservationIds.IsEmpty() ? 0 : speciesObservationIds.Count;

            // Get all data required to performe/build up a db search.
            regionIds = searchCriteria.GetRegionIds(context);
            geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);
            speciesObservationCount += context.GetAnalysisDatabase().GetSpeciesObservationCountBySearchCriteria(polygons,
                                                                                                                regionIds,
                                                                                                                taxonIds,
                                                                                                                joinCondition,
                                                                                                                whereCondition,
                                                                                                                geometryWhereCondition);
            return speciesObservationCount;
        }

        /// <summary>
        /// Get number of species observations that matches
        /// provided species observation search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// Number of species observations that matches
        /// provided species observation search criteria.
        /// </returns>
        public static Int64 GetSpeciesObservationCountBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                                    WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                    WebCoordinateSystem coordinateSystem)
        {
            return WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(context,
                                                                                                                                      searchCriteria,
                                                                                                                                      coordinateSystem);
        }

        /// <summary>
        /// Get species observation ids for
        /// user with complex access rights.
        /// Returned species observations ids are limited to
        /// protected species observations.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Species observations ids for protected species observations.</returns>
        private static List<Int64> GetSpeciesObservationIdsAccessRights(WebServiceContext context,
                                                                        WebSpeciesObservationSearchCriteria searchCriteria,
                                                                        WebCoordinateSystem coordinateSystem)
        {
            Boolean isMinProtectionLevelSpecified;
            Int32 minProtectionLevel;
            List<Int64> speciesObservationIds;
            List<WebRole> currentRoles;
            SpeciesObservationAccessRights speciesObservation;
            String geometryWhereCondition, joinCondition, whereCondition;

            speciesObservationIds = new List<Int64>();
            currentRoles = context.CurrentRoles;
            if (!currentRoles.IsSimpleSpeciesObservationAccessRights())
            {
                // Save user specified min protection level.
                isMinProtectionLevelSpecified = searchCriteria.IsMinProtectionLevelSpecified;
                minProtectionLevel = searchCriteria.MinProtectionLevel;

                // Set temporary min protection level. 
                searchCriteria.IsMinProtectionLevelSpecified = true;
                if (isMinProtectionLevelSpecified)
                {
                    searchCriteria.MinProtectionLevel = Math.Max(2,
                                                                 searchCriteria.MinProtectionLevel);
                }
                else
                {
                    searchCriteria.MinProtectionLevel = 2;
                }

                // Get species observation ids.
                geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
                joinCondition = searchCriteria.GetJoinCondition();
                whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);
                using (DataReader dataReader = context.GetAnalysisDatabase().GetSpeciesObservationsAccessRights(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                searchCriteria.GetRegionIds(context),
                                                                                                                WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false),
                                                                                                                joinCondition,
                                                                                                                whereCondition,
                                                                                                                geometryWhereCondition))
                {
                    while (dataReader.Read())
                    {
                        speciesObservation = new SpeciesObservationAccessRights();
                        speciesObservation.Load(dataReader);
                        if (speciesObservation.CheckAccessRights(context))
                        {
                            speciesObservationIds.Add(speciesObservation.Id);
                        }
                    }
                }

                // Adjust protection levels for search on public
                // species observations.
                searchCriteria.MaxProtectionLevel = 1;
                searchCriteria.IsMinProtectionLevelSpecified = isMinProtectionLevelSpecified;
                searchCriteria.MinProtectionLevel = minProtectionLevel;
            }

            return speciesObservationIds;
        }

        /// <summary>
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        public static List<WebTaxon> GetTaxaBySpeciesFactSearchCriteria(WebServiceContext context,
                                                                        WebSpeciesFactSearchCriteria searchCriteria)
        {
            // Check that data is valid
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get taxa
            return WebServiceData.SpeciesFactManager.GetTaxaBySearchCriteria(context, searchCriteria);
        }

        /// <summary>
        /// Get no of species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public static IList<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context,
                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem)
        {
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get all data required to performe/build up a db search.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);

            List<int> taxonIdList = new List<int>();
            List<WebTaxon> taxonList;
            using (DataReader dataReader = context.GetAnalysisDatabase().GetTaxonIdsBySearchCriteria(polygons,
                                                                                                     regionIds,
                                                                                                     taxonIds,
                                                                                                     joinCondition,
                                                                                                     whereCondition,
                                                                                                     speciesObservationIds))
            {
                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(TaxonIdsSearchCriteria.TAXON_ID);
                    taxonIdList.Add(id);
                }
            }

            // Get taxa from cache or from TaxonService.
            taxonList = GetCachedTaxa(context, taxonIdList);
            return taxonList;
        }
        /// <summary>
        /// Get all unique taxonId's (dyntaxaTaxonId) that matches WebSpeciesObservationSearchCriteria and the specified coordinate system.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is using.</param>
        /// <returns>A list of unique taxonId's (dyntaxaTaxonId) that matches search criteria.</returns>
        public static IList<WebTaxon> GetTaxaBySearchCriteriaElasticsearch(WebServiceContext context,
                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem)
        {
            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");

            DocumentUniqueValuesResponse taxonIdUniqueValues;
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                taxonIdUniqueValues = elastisearch.GetTaxonIdUniqueValues(filter.ToString());
            }

            // Get taxa from cache or from TaxonService.
            return GetCachedTaxa(context, taxonIdUniqueValues.UniqueValues.Select(uniqueValue => int.Parse(uniqueValue.Key)).ToList());
        }

        /// <summary>
        /// Get no of species and number of related observed species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public static IList<WebTaxonSpeciesObservationCount> GetTaxaWithSpeciesObservationCountsBySearchCriteria(WebServiceContext context,
                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem)
        {
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get all data required to performe/build up a db search.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);

            Dictionary<int, int> taxonIdsWithSpeciesObservationCountsDictionary = new Dictionary<int, int>();
            List<WebTaxonSpeciesObservationCount> taxonList;
            using (DataReader dataReader = context.GetAnalysisDatabase().GetTaxonIdsWithSpeciesObservationCountsBySearchCriteria(polygons,
                                                                                                     regionIds,
                                                                                                     taxonIds,
                                                                                                     joinCondition,
                                                                                                     whereCondition,
                                                                                                     speciesObservationIds))
            {
                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(TaxonIdsSearchCriteria.TAXON_ID);
                    int speciesObservationCount = dataReader.GetInt32(TaxonIdsSearchCriteria.SPECIES_OBSERVATION_COUNT);
                    taxonIdsWithSpeciesObservationCountsDictionary.Add(id, speciesObservationCount);
                }
            }

            // Get taxa from cache or from TaxonService.
            taxonList = GetCachedTaxaWithSpeciesObservationCounts(context, taxonIdsWithSpeciesObservationCountsDictionary);
            return taxonList;
        }

        /// <summary>
        /// Get no of species and number of related observed species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public static IList<WebTaxonSpeciesObservationCount> GetTaxaWithSpeciesObservationCountsBySearchCriteriaElasticsearch(WebServiceContext context,
                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem)
        {
            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");

            DocumentUniqueValuesResponse taxonIdUniqueValues;
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                taxonIdUniqueValues = elastisearch.GetTaxonIdUniqueValues(filter.ToString());
            }

            // Get taxa from cache or from TaxonService.
            return GetCachedTaxaWithSpeciesObservationCounts(context, taxonIdUniqueValues.UniqueValues.ToDictionary(i => int.Parse(i.Key), i => Convert.ToInt32(i.Value)));
        }

        /// <summary>
        /// Get time step specific species observation counts for a specific set of species observation search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>A list of time step specific species observation counts.</returns>
        public static List<WebTimeStepSpeciesObservationCount> GetTimeSpeciesObservationCountsBySearchCriteria(
                                                               WebServiceContext context,
                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                               Periodicity periodicity,
                                                               WebCoordinateSystem coordinateSystem)
        {
            List<WebTimeStepSpeciesObservationCount> timeSerie;

            timeSerie = GetTimeSpeciesObservationCountsBySearchCriteriaInternal(context, searchCriteria, periodicity, coordinateSystem);
            timeSerie = ComplementTimeSerieWithSpeciesObservationZeroCounts(context, periodicity, timeSerie);
            return timeSerie;
        }

        /// <summary>
        /// Get time step specific species observation counts for a specific set of species observation search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>A list of time step specific species observation counts.</returns>
        public static List<WebTimeStepSpeciesObservationCount> GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(
                                                               WebServiceContext context,
                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                               Periodicity periodicity,
                                                               WebCoordinateSystem coordinateSystem)
        {
            WebDataField webDataField;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            webDataField = new WebDataField();
            webDataField.Name = "Periodicity";
            webDataField.Type = WebService.Data.WebDataType.String;
            webDataField.Value = periodicity.ToString();
            if (searchCriteria.DataFields.IsNull())
            {
                searchCriteria.DataFields = new List<WebDataField>();
            }

            searchCriteria.DataFields.Add(webDataField);
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");
            //filter.Append("{ \"aggs\" : { \"GetTimeSpeciesObservationCounts\": {");
            //filter.Append(searchCriteria.GetFilter(context, false));
            //filter.Append("}}}");

            Dictionary<String, Int64> uniqueValues;
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                uniqueValues = elastisearch.GetTimeSpeciesObservationCountsBySearchCriteria(filter.ToString(), periodicity);
            }

            var timeSeries = new List<WebTimeStepSpeciesObservationCount>();
            var timeStepId = 1;

            foreach (var uniqueValue in uniqueValues)
            {
                var item = new WebTimeStepSpeciesObservationCount();
                item.LoadData(uniqueValue, periodicity, new CultureInfo(context.Locale.ISOCode));
                item.Id = timeStepId++;
                timeSeries.Add(item);
            }

            return ComplementTimeSerieWithSpeciesObservationZeroCounts(context, periodicity, timeSeries);
        }

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name="alphaValue">If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        public static string GetSpeciesObservationAOOEOOAsGeoJson(List<WebGridCellSpeciesObservationCount> gridCells, int alphaValue = 0, bool useCenterPoint = true)
        {
            if (gridCells == null || !gridCells.Any())
            {
                return null;
            }

            var eooGeometry = alphaValue == 0 ? gridCells.ConvexHull() : gridCells.ConcaveHull(alphaValue, useCenterPoint);

            if (eooGeometry == null)
            {
                return null;
            }

            var gridCellCount = gridCells.Count;
            var gridCellSize = gridCells.First().Size;
            var gridCellArea = gridCellSize*gridCellSize/1000000; //Calculate area in km2

            var area = eooGeometry.Area / 1000000; //Calculate area in km2
            var eoo = Math.Round(area, 0).ToString("## ### ### ##0");
            var aoo = Math.Round((double)gridCellCount * gridCellArea, 0).ToString("## ### ### ##0");

            var sourceCoordinateSystem = gridCells.First().GridCoordinateSystem.GetCoordinateSystemId();
            var targetCoordinateSystem = gridCells.First().CoordinateSystem.Id;

            //Make sure we have a planar projection (SWEREF99_TM) when we get the area in square meters
            eooGeometry = eooGeometry.ReProject(sourceCoordinateSystem, targetCoordinateSystem);

            return eooGeometry.ToGeoJson(targetCoordinateSystem.Srid(), new[]
            {
                new KeyValuePair<string, object>("EOO", string.Format("{0} km2", eoo)), 
                new KeyValuePair<string, object>("AOO", string.Format("{0} km2", aoo)),
                new KeyValuePair<string, object>("Grid cell area", string.Format("{0} km2", gridCellArea.ToString("## ### ### ##0")))
            });
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Clear cached taxon information.
        /// This must be done since WebTaxon contains 
        /// property IsInRevision that indicates if the
        /// taxon is in an ongoing revision or not.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)")]
        public static void ClearCachedTaxa()
        {
            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    // WebServiceData.LogManager.Log(context, "Start of clearing cached taxa in CheckOutTaxonRevision.",
                    //                              LogType.Information, null);
                    String cacheKeyStart = Settings.Default.TaxaCacheKey + ":";
                    IDictionaryEnumerator cacheEnum = HttpContext.Current.Cache.GetEnumerator();
                    List<String> cacheKeys = new List<String>();
                    while (cacheEnum.MoveNext())
                    {
                        if (((String)(cacheEnum.Key)).StartsWith(cacheKeyStart))
                        {
                            cacheKeys.Add((String)(cacheEnum.Key));
                        }
                    }

                    foreach (String cacheKey in cacheKeys)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);

                        // WebServiceData.LogManager.Log(context, "Clear taxa with key: " + cacheKey, LogType.Information,
                        //                              null);
                    }

                    // WebServiceData.LogManager.Log(context, "End of clearing cached taxa in CheckOutTaxonRevision.",
                    //                              LogType.Information, null);
                }
            }
        }

        /// <summary>
        /// Complements time series with species observations zero counts.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="periodicity">Periodicity of the time series.</param>
        /// <param name="timeSerie">Time series to be used.</param>
        /// <returns>A list of time series species observation counts.</returns>
        private static List<WebTimeStepSpeciesObservationCount> ComplementTimeSerieWithSpeciesObservationZeroCounts(WebServiceContext context,
                                                                                                                    Periodicity periodicity,
                                                                                                                    List<WebTimeStepSpeciesObservationCount> timeSerie)
        {
            CultureInfo culture = new CultureInfo(context.Locale.ISOCode);
            List<WebTimeStepSpeciesObservationCount> timeSerieComplemented = new List<WebTimeStepSpeciesObservationCount>();

            // Complement list with zero counts if nessesary.
            if (timeSerie.IsNotEmpty())
            {
                if (periodicity == Periodicity.DayOfTheYear || periodicity == Periodicity.MonthOfTheYear || periodicity == Periodicity.WeekOfTheYear || periodicity == Periodicity.Yearly)
                {
                    Int32 min = Int32.Parse(timeSerie[0].Name);
                    Int32 max = Int32.Parse(timeSerie[timeSerie.Count - 1].Name);

                    Int32 id = 1;
                    Int32 currentIndex = 0;
                    for (Int32 step = min; step < max + 1; step++)
                    {
                        WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount = new WebTimeStepSpeciesObservationCount();
                        timeStepSpeciesObservationCount.Id = id++;
                        timeStepSpeciesObservationCount.Periodicity = periodicity;
                        timeStepSpeciesObservationCount.IsDateSpecified = false;
                        // ReSharper disable SpecifyACultureInStringConversionExplicitly
                        timeStepSpeciesObservationCount.Name = step.ToString();
                        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                        if (timeSerie[currentIndex].Name == step.ToString())
                        // ReSharper restore SpecifyACultureInStringConversionExplicitly
                        {
                            timeStepSpeciesObservationCount.Count = timeSerie[currentIndex++].Count;
                        }
                        else
                        {
                            timeStepSpeciesObservationCount.Count = 0;
                        }

                        // Renaming time steps
                        if (periodicity == Periodicity.MonthOfTheYear)
                        {
                            timeStepSpeciesObservationCount.Name = culture.DateTimeFormat.GetMonthName(step);
                            timeStepSpeciesObservationCount.Name = timeStepSpeciesObservationCount.Name.Substring(0, 3);
                        }

                        timeSerieComplemented.Add(timeStepSpeciesObservationCount);
                    }

                    return timeSerieComplemented;
                }

                if (periodicity == Periodicity.Monthly)
                {
                    DateTime minStepDate = timeSerie[0].Date;
                    DateTime maxStepDate = timeSerie[timeSerie.Count - 1].Date;
                    DateTime currentDate = minStepDate;
                    Int32 id = 1;
                    Int32 currentIndex = 0;
                    while (currentDate <= maxStepDate)
                    {
                        WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount = new WebTimeStepSpeciesObservationCount();
                        timeStepSpeciesObservationCount.Id = id++;
                        timeStepSpeciesObservationCount.Periodicity = periodicity;
                        timeStepSpeciesObservationCount.Date = currentDate;
                        timeStepSpeciesObservationCount.IsDateSpecified = true;

                        Int32 year = currentDate.Year;
                        Int32 month = currentDate.Month;
                        String monthName = culture.DateTimeFormat.GetMonthName(month);
                        monthName = monthName.Substring(0, 3);
                        monthName = year + " " + monthName;
                        timeStepSpeciesObservationCount.Name = monthName;

                        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                        if (timeSerie[currentIndex].Name == monthName)
                        {
                            timeStepSpeciesObservationCount.Count = timeSerie[currentIndex++].Count;
                        }
                        else
                        {
                            timeStepSpeciesObservationCount.Count = 0;
                        }

                        timeSerieComplemented.Add(timeStepSpeciesObservationCount);
                        currentDate = currentDate.AddMonths(1);
                    }

                    return timeSerieComplemented;
                }

                if (periodicity == Periodicity.Weekly)
                {
                    DateTime minStepDate = timeSerie[0].Date;
                    DateTime maxStepDate = timeSerie[timeSerie.Count - 1].Date;
                    DateTime currentDate = minStepDate;
                    // ReSharper disable once RedundantAssignment
                    DateTime tempDate = currentDate;
                    Int32 id = 1;
                    Int32 currentIndex = 0;
                    while (currentDate <= maxStepDate)
                    {
                        // ReSharper disable once UseObjectOrCollectionInitializer
                        WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount = new WebTimeStepSpeciesObservationCount();
                        timeStepSpeciesObservationCount.Id = id++;
                        timeStepSpeciesObservationCount.Periodicity = periodicity;
                        timeStepSpeciesObservationCount.Date = currentDate;
                        timeStepSpeciesObservationCount.IsDateSpecified = true;
                        Int32 year = currentDate.Year;
                        Int32 week = GetWeekOfYear(currentDate);
                        String name = year + ":" + week;
                        timeStepSpeciesObservationCount.Name = name;
                        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                        if (timeSerie[currentIndex].Date == currentDate)
                        {
                            timeStepSpeciesObservationCount.Count = timeSerie[currentIndex++].Count;
                        }
                        else
                        {
                            timeStepSpeciesObservationCount.Count = 0;
                        }

                        timeSerieComplemented.Add(timeStepSpeciesObservationCount);
                        currentDate = currentDate.AddDays(7);
                        tempDate = GetFirstDateOfWeek(currentDate.Year, GetWeekOfYear(currentDate));
                        // ReSharper disable once RedundantCheckBeforeAssignment
                        if (tempDate != currentDate)
                        {
                            currentDate = tempDate;
                        }
                    }

                    return timeSerieComplemented;
                }

                if (periodicity == Periodicity.Daily)
                {
                    DateTime minStepDate = timeSerie[0].Date;
                    DateTime maxStepDate = timeSerie[timeSerie.Count - 1].Date;
                    DateTime currentDate = minStepDate;
                    Int32 id = 1;
                    Int32 currentIndex = 0;
                    while (currentDate <= maxStepDate)
                    {
                        WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount = new WebTimeStepSpeciesObservationCount();
                        timeStepSpeciesObservationCount.Id = id++;
                        timeStepSpeciesObservationCount.Periodicity = periodicity;
                        timeStepSpeciesObservationCount.Date = currentDate;
                        timeStepSpeciesObservationCount.IsDateSpecified = true;
                        String name = currentDate.ToString("d", culture.DateTimeFormat);
                        timeStepSpeciesObservationCount.Name = name;
                        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                        if (timeSerie[currentIndex].Name == name)
                        {
                            timeStepSpeciesObservationCount.Count = timeSerie[currentIndex++].Count;
                        }
                        else
                        {
                            timeStepSpeciesObservationCount.Count = 0;
                        }

                        timeSerieComplemented.Add(timeStepSpeciesObservationCount);
                        currentDate = currentDate.AddDays(1);
                    }

                    return timeSerieComplemented;
                }
            }

            return timeSerie;
        }

        /// <summary>
        /// Converts a feature collection to SQL geometries.
        /// </summary>
        /// <param name="featureCollection">The feature collection.</param>
        /// <param name="srid">The srid that is used. For example: 900913.</param>
        /// <returns>A list of SqlGeometry objects.</returns>        
        private static List<SqlGeometry> ConvertFeatureCollectionToSqlGeometries(FeatureCollection featureCollection, int srid)
        {
            SqlGeometry sqlPolygon;
            SqlGeometry sqlGeomertyObject;
            List<SqlGeometry> sqlGeomertyObjectList;
            List<SqlGeometry> sqlPolygonList;
            List<SqlGeometry> sqlLineStringList;
            List<SqlGeometry> sqlPointList;

            sqlGeomertyObjectList = new List<SqlGeometry>();
            if (featureCollection.Features.IsEmpty())
            {
                return sqlGeomertyObjectList;
            }

            switch (featureCollection.Features[0].Geometry.Type)
            {
                case GeoJSONObjectType.Polygon:
                    // This is almost the same code as for multi polygon.
                    // ReSharper disable ForCanBeConvertedToForeach
                    for (int featureIdx = 0; featureIdx < featureCollection.Features.Count; featureIdx++)
                    {
                        ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon polygon =
                            (ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon)featureCollection.Features[featureIdx].Geometry;
                        sqlPolygonList = new List<SqlGeometry>();

                        List<LineString> lineStrings = polygon.Coordinates;
                        for (int lineStringIdx = 0; lineStringIdx < lineStrings.Count; lineStringIdx++)
                        {
                            LineString lineString = lineStrings[lineStringIdx];
                            sqlPointList = ConvertGeoJsonLineStringToSqlPointList(srid, lineString);
                            sqlPolygon = GeometryManager.CreatePolygon(sqlPointList);

                            sqlPolygonList.Add(sqlPolygon);
                        }

                        sqlGeomertyObject = GeometryManager.CreateMultiGeometry(sqlPolygonList);
                        sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    }

                    break;

                case GeoJSONObjectType.MultiPolygon:
                    // Extract all the GeoJSON point features from the geometry and add them to a geometrylist.
                    for (int featureIdx = 0; featureIdx < featureCollection.Features.Count; featureIdx++)
                    {
                        MultiPolygon multiPolygon = (MultiPolygon)featureCollection.Features[featureIdx].Geometry;
                        sqlPolygonList = new List<SqlGeometry>();
                        for (int polygonIdx = 0; polygonIdx < multiPolygon.Coordinates.Count; polygonIdx++)
                        {
                            ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon polygon = multiPolygon.Coordinates[polygonIdx];
                            for (int lineStringIdx = 0; lineStringIdx < polygon.Coordinates.Count; lineStringIdx++)
                            {
                                LineString lineString = polygon.Coordinates[lineStringIdx];

                                sqlPointList = ConvertGeoJsonLineStringToSqlPointList(srid, lineString);
                                sqlPolygon = GeometryManager.CreatePolygon(sqlPointList);
                                sqlPolygonList.Add(sqlPolygon);
                            }
                        }

                        sqlGeomertyObject = GeometryManager.CreateMultiGeometry(sqlPolygonList);
                        sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    }

                    break;

                case GeoJSONObjectType.Point:
                    List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point> geoJsonPointList = new List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point>();
                    for (int featureIdx = 0; featureIdx < featureCollection.Features.Count; featureIdx++)
                    {
                        ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point geoJsonPnt = (ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point)featureCollection.Features[featureIdx].Geometry;
                        geoJsonPointList.Add(geoJsonPnt);
                    }

                    // Convert the GeoJSON point features to sql geometries
                    sqlGeomertyObject = GeometryManager.CreateMultiPointGeometry(geoJsonPointList, Convert.ToInt32(srid));
                    //sqlPointList = GeometryManager.ConvertGeoJsonPointToSqlGeometry(geoJsonPointList, Convert.ToInt32(srid));
                    //sqlGeomertyObject = GeometryManager.CreateMultiGeometry(sqlPointList);
                    sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    break;

                case GeoJSONObjectType.MultiPoint:
                    List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point> geoJsonMultiPointList = new List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point>();
                    for (int featureIdx = 0; featureIdx < featureCollection.Features.Count; featureIdx++)
                    {
                        MultiPoint geoJsonMultiPoint = (MultiPoint)featureCollection.Features[featureIdx].Geometry;
                        // ReSharper disable once LoopCanBeConvertedToQuery
                        for (int pointIdx = 0; pointIdx < geoJsonMultiPoint.Coordinates.Count; pointIdx++)
                        {
                            ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point geoJsonPnt = geoJsonMultiPoint.Coordinates[pointIdx];
                            geoJsonMultiPointList.Add(geoJsonPnt);
                        }
                        sqlGeomertyObject = GeometryManager.CreateMultiPointGeometry(geoJsonMultiPointList, Convert.ToInt32(srid));
                        sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    }

                    // Convert the GeoJSON point features to sql geometries
                    //sqlPointList = GeometryManager.ConvertGeoJsonPointToSqlGeometry(geoJsonMultiPointList, Convert.ToInt32(srid));
                    //sqlGeomertyObject = GeometryManager.CreateMultiGeometry(sqlPointList);
                    // sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    break;

                case GeoJSONObjectType.GeometryCollection:
                    break;

                case GeoJSONObjectType.LineString:
                    sqlLineStringList = new List<SqlGeometry>();
                    for (int featureIdx = 0; featureIdx < featureCollection.Features.Count; featureIdx++)
                    {
                        LineString lineString = (LineString)featureCollection.Features[featureIdx].Geometry;
                        sqlPointList = ConvertGeoJsonLineStringToSqlPointList(srid, lineString);
                        SqlGeometry sqlLineString = GeometryManager.CreateLineString(sqlPointList);
                        sqlLineStringList.Add(sqlLineString);
                    }

                    sqlGeomertyObject = GeometryManager.CreateMultiGeometry(sqlLineStringList);
                    sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    break;

                case GeoJSONObjectType.MultiLineString:
                    sqlLineStringList = new List<SqlGeometry>();

                    for (int featureIdx = 0; featureIdx < featureCollection.Features.Count; featureIdx++)
                    {
                        MultiLineString multiLineString = (MultiLineString)featureCollection.Features[featureIdx].Geometry;
                        for (int lineIdx = 0; lineIdx < multiLineString.Coordinates.Count; lineIdx++)
                        {
                            LineString lineString = (LineString)featureCollection.Features[lineIdx].Geometry;
                            sqlPointList = ConvertGeoJsonLineStringToSqlPointList(srid, lineString);
                            SqlGeometry sqlLineString = GeometryManager.CreateLineString(sqlPointList);
                            sqlLineStringList.Add(sqlLineString);
                        }
                    }

                    sqlGeomertyObject = GeometryManager.CreateMultiGeometry(sqlLineStringList);
                    sqlGeomertyObjectList.Add(sqlGeomertyObject);
                    break;
            }
            // ReSharper restore ForCanBeConvertedToForeach
            return sqlGeomertyObjectList;
        }

        /// <summary>
        /// Converts a GeoJSON line string to SQLPointList. 
        /// </summary>
        /// <param name="srid">The SRID that is used. For example: 900913.</param>
        /// <param name="lineString">GeoJSON line string to be converted.</param>
        /// <returns>A list of SQL geometry objects.</returns>
        private static List<SqlGeometry> ConvertGeoJsonLineStringToSqlPointList(int srid, LineString lineString)
        {
            List<SqlGeometry> sqlPointList;
            List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point> geoJsonPointList = new List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (int pointIdx = 0; pointIdx < lineString.Coordinates.Count; pointIdx++)
            {
                GeographicPosition position = lineString.Coordinates[pointIdx];
                ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point geoJsonPnt = new ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point(position);
                geoJsonPointList.Add(geoJsonPnt);
            }

            // Convert the GeoJSON point features to sql geometries
            sqlPointList = GeometryManager.ConvertGeoJsonPointToSqlGeometry(geoJsonPointList, Convert.ToInt32(srid));
            return sqlPointList;
        }

        /// <summary>
        /// Converts the web feature statistics coordinates from a coordinate system to another .
        /// </summary>
        /// <param name="webGridCellFeatureStatisticsList">The web grid cell feature statistics list containing coordinates to be converted.</param>
        /// <param name="fromCoordinateSystem">Coordinate system to convert from.</param>
        /// <param name="toCoordinateSystem">Coordinate system to convert to.</param>
        private static void ConvertWebFeatureStatisticsCoordinates(List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList, WebCoordinateSystem fromCoordinateSystem, WebCoordinateSystem toCoordinateSystem)
        {
            foreach (WebGridCellFeatureStatistics cell in webGridCellFeatureStatisticsList)
            {
                WebPolygon gridCellBoundingPolygon = WebServiceData.CoordinateConversionManager.GetConvertedPolygon(
                    cell.BoundingBox, fromCoordinateSystem, toCoordinateSystem);
                WebPoint centrePoint = WebServiceData.CoordinateConversionManager.GetConvertedPoint(
                    cell.CentreCoordinate, fromCoordinateSystem, toCoordinateSystem);
                cell.BoundingBox = gridCellBoundingPolygon;
                cell.CentreCoordinate = centrePoint;
            }
        }

        /// <summary>
        /// Get converted Bounding box from one coordinate system to another coordinate system. 
        /// </summary>
        /// <param name="toCoordinateSystem">Coordinate system to convert to.</param>
        /// <param name="fromCoordinateSystem">Coordinate system to convert from.</param>
        /// <param name="boundingBoxToBeConverted">Bounding box to be converted.</param>
        /// <returns>Converted bounding box.</returns>
        private static WebPolygon GetConvertedBoundingBoxCoordinates(WebCoordinateSystem toCoordinateSystem,
                                                                     WebCoordinateSystem fromCoordinateSystem,
                                                                     WebBoundingBox boundingBoxToBeConverted)
        {
            // Convert coordinates if needed 
            if (toCoordinateSystem.GetWkt().ToUpper() != fromCoordinateSystem.GetWkt().ToUpper())
            {
                WebPolygon gridCellBoundingBox = WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(
                                                                                                            boundingBoxToBeConverted,
                                                                                                            fromCoordinateSystem,
                                                                                                            toCoordinateSystem);
                return gridCellBoundingBox;
            }

            return boundingBoxToBeConverted.GetPolygon();
        }

        /// <summary>
        /// Gets the first date of the week.
        /// </summary>
        /// <param name="year">Selected year.</param>
        /// <param name="weekOfYear">Selected week of the year.</param>
        /// <returns>First date of the week.</returns>
        private static DateTime GetFirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        /// <summary>
        /// Get all taxa that exist in Taxon service or if already searched for available in cash.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>All taxa that has id in taxonIds.</returns>
        public static List<WebTaxon> GetCachedTaxa(WebServiceContext context,
                                                     List<Int32> taxonIds)
        {
            Hashtable cachedTaxa;
            String cacheKey;
            List<WebTaxon> taxa = new List<WebTaxon>();
            List<Int32> notCachedTaxonIds = new List<Int32>();

            // Get cached information.
            cacheKey = Settings.Default.TaxaCacheKey + ":" + context.Locale.ISOCode;
            cachedTaxa = (Hashtable)(context.GetCachedObject(cacheKey));

            // Find taxa that already has been saved to cash
            if (cachedTaxa.IsNotNull())
            {
                foreach (Int32 taxonId in taxonIds)
                {
                    WebTaxon taxon = (WebTaxon)cachedTaxa[taxonId];
                    if (taxon.IsNull())
                    {
                        notCachedTaxonIds.Add(taxonId);
                    }
                    else
                    {
                        taxa.Add(taxon);
                    }
                }
            }
            else
            {
                notCachedTaxonIds = taxonIds;
            }

            if (notCachedTaxonIds.IsNotEmpty())
            {
                // Data not in cache. Get information from database.
                List<WebTaxon> taxonList = WebServiceData.TaxonManager.GetTaxaByIds(context, notCachedTaxonIds);

                if (cachedTaxa.IsNull())
                {
                    cachedTaxa = new Hashtable();

                    // Add information to cache.
                    context.AddCachedObject(cacheKey,
                                            cachedTaxa,
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.AboveNormal);
                }

                foreach (WebTaxon webTaxon in taxonList)
                {
                    if (webTaxon != null)
                    {
                        taxa.Add(webTaxon);

                        // Add to hash table for saving to cash.
                        cachedTaxa[webTaxon.Id] = webTaxon;
                    }
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get all taxa, with related number of observed species, that exist in Taxon service or if already searched for available in cash.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="taxonIdsWithSpeciesObservationCounts">Taxon ids with related number of observed species.</param>
        /// <returns>All taxa that has id in taxonIds.</returns>
        internal static List<WebTaxonSpeciesObservationCount> GetCachedTaxaWithSpeciesObservationCounts(WebServiceContext context,
                                                     Dictionary<Int32, Int32> taxonIdsWithSpeciesObservationCounts)
        {
            List<int> taxonIdsList = new List<int>();
            List<WebTaxon> taxonList;
            List<WebTaxonSpeciesObservationCount> taxa = new List<WebTaxonSpeciesObservationCount>();

            foreach (Int32 taxonId in taxonIdsWithSpeciesObservationCounts.Keys)
            {
                taxonIdsList.Add(taxonId);
            }

            taxonList = GetCachedTaxa(context, taxonIdsList);
            if (taxonList.IsNotEmpty())
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (WebTaxon webTaxon in taxonList)
                {
                    if (webTaxon != null)
                    {
                        // ReSharper disable once RedundantEmptyObjectCreationArgumentList
                        WebTaxonSpeciesObservationCount webTaxonSpeciesObservationCount = new WebTaxonSpeciesObservationCount()
                        {
                            SpeciesObservationCount = taxonIdsWithSpeciesObservationCounts[webTaxon.Id],
                            Taxon = webTaxon
                        };
                        taxa.Add(webTaxonSpeciesObservationCount);
                    }
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get list of species observations provenances that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is using.</param>
        /// <returns>List of species observation's provenance that matches search criteria.</returns>
        public static List<WebSpeciesObservationProvenance> GetProvenancesBySearchCriteria(WebServiceContext context,
                                                                                           WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                           WebCoordinateSystem coordinateSystem)
        {
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;
            List<WebSpeciesObservationProvenance> webSpeciesObservationProvenance = new List<WebSpeciesObservationProvenance>();
            WebSpeciesObservationProvenance provenance;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get all data required to build up a db query.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);

            using (DataReader dataReader = context.GetAnalysisDatabase().GetProvenanceDarwinCoreObservationsBySearchCriteria(polygons,
                                                                                                                             regionIds,
                                                                                                                             taxonIds,
                                                                                                                             joinCondition,
                                                                                                                             whereCondition,
                                                                                                                             speciesObservationIds))
            {
                provenance = new WebSpeciesObservationProvenance();
                provenance.Values = new List<WebSpeciesObservationProvenanceValue>();
                while (dataReader.Read())
                {
                    // Create new provenance object when name changes; owner, observer, reporter etc.
                    if (provenance.Name != null && provenance.Name != dataReader.GetString(SpeciesObservationProvenanceSearchCriteriaData.NAME))
                    {
                        // Add provenance group to main list
                        webSpeciesObservationProvenance.Add(provenance);
                        // Create new provenance group
                        provenance = new WebSpeciesObservationProvenance();
                        provenance.Values = new List<WebSpeciesObservationProvenanceValue>();
                    }
                    provenance.LoadData(dataReader);
                }

                // Add the last valid provenance group to main list
                if (provenance.Name != null)
                {
                    webSpeciesObservationProvenance.Add(provenance);
                }
            }

            using (DataReader dataReader = context.GetAnalysisDatabase().GetProvenanceDataProvidersBySearchCriteria(polygons,
                                                                                                                    regionIds,
                                                                                                                    taxonIds,
                                                                                                                    whereCondition,
                                                                                                                    joinCondition,
                                                                                                                    context.Locale.Id,
                                                                                                                    speciesObservationIds))
            {
                provenance = new WebSpeciesObservationProvenance();
                provenance.Values = new List<WebSpeciesObservationProvenanceValue>();
                while (dataReader.Read())
                {
                    provenance.LoadData(dataReader);
                }

                // Add the last valid provenance group to main list
                if (provenance.Name != null)
                {
                    webSpeciesObservationProvenance.Add(provenance);
                }
            }

            return webSpeciesObservationProvenance;
        }


        /// <summary>
        /// Get list of species observations provenances that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is using.</param>
        /// <returns>List of species observation's provenance that matches search criteria.</returns>
        public static List<WebSpeciesObservationProvenance> GetProvenancesBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                                           WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                           WebCoordinateSystem coordinateSystem)
        {
            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid and convert some properties to Elasticsearch specific formats
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            var filter = new StringBuilder();
            filter.Append("{ \"query\" : { \"filtered\": {\"query\": {\"match_all\": {}}, ");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}}}");

            var webSpeciesObservationProvenances = new List<WebSpeciesObservationProvenance>();

            var provenance = new WebSpeciesObservationProvenance { Name = "Owner", Values = new List<WebSpeciesObservationProvenanceValue>() };
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                foreach (var uniqueValue in elastisearch.GetProvenanceUniqueOwners(filter.ToString()).UniqueValues)
                {
                    provenance.Values.Add(new WebSpeciesObservationProvenanceValue
                    {
                        SpeciesObservationCount = uniqueValue.Value,
                        Id = null,
                        Value = uniqueValue.Key
                    });
                }
            }

            webSpeciesObservationProvenances.Add(provenance);

            provenance = new WebSpeciesObservationProvenance { Name = "Observer", Values = new List<WebSpeciesObservationProvenanceValue>() };
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                foreach (var uniqueValue in elastisearch.GetProvenanceUniqueObservers(filter.ToString()).UniqueValues)
                {
                    provenance.Values.Add(new WebSpeciesObservationProvenanceValue
                    {
                        SpeciesObservationCount = uniqueValue.Value,
                        Id = null,
                        Value = uniqueValue.Key
                    });
                }
            }

            webSpeciesObservationProvenances.Add(provenance);

            provenance = new WebSpeciesObservationProvenance { Name = "Reporter", Values = new List<WebSpeciesObservationProvenanceValue>() };
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                foreach (var uniqueValue in elastisearch.GetProvenanceUniqueReporters(filter.ToString()).UniqueValues)
                {
                    provenance.Values.Add(new WebSpeciesObservationProvenanceValue
                    {
                        SpeciesObservationCount = uniqueValue.Value,
                        Id = null,
                        Value = uniqueValue.Key
                    });
                }
            }

            webSpeciesObservationProvenances.Add(provenance);

            provenance = new WebSpeciesObservationProvenance { Name = "DataProvider", Values = new List<WebSpeciesObservationProvenanceValue>() };
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                foreach (var uniqueValue in elastisearch.GetProvenanceUniqueDataProviders(filter.ToString()).UniqueValues)
                {
                    provenance.Values.Add(new WebSpeciesObservationProvenanceValue
                    {
                        SpeciesObservationCount = uniqueValue.Value,
                        Id = null,
                        Value = uniqueValue.Key
                        //Id = uniqueValue.Key
                        //Value =GetDataProvider( uniqueValue.Key, locale ).Name;
                    });
                }
            }

            webSpeciesObservationProvenances.Add(provenance);

            return webSpeciesObservationProvenances;
        }


        /// <summary>
        /// Get information about red listed taxa.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <returns>Information about red listed taxa.</returns>
        private static Hashtable GetRedlistedTaxa(WebServiceContext context)
        {
            ArtDatabanken.Data.ArtDatabankenService.Factor factor;
            Hashtable redlistedTaxaInformation;
            List<Int32> allRedlistedTaxonIds, redlistedTaxonIds;
            ArtDatabanken.Data.ArtDatabankenService.TaxonList taxa;
            RedListCategory redListCategory;
            SpeciesFactCondition speciesFactCondition;
            SpeciesFactFieldCondition speciesFactFieldCondition;

            // Get data from cache.
            redlistedTaxaInformation = (Hashtable)context.GetCachedObject(GetRedlistedTaxaCacheKey());

            if (redlistedTaxaInformation.IsNull())
            {
                // Data not in cache - store it in the cache
                allRedlistedTaxonIds = new List<Int32>();
                redlistedTaxaInformation = new Hashtable();
                speciesFactCondition = new SpeciesFactCondition();
                factor = ArtDatabanken.Data.ArtDatabankenService.FactorManager.GetFactor(ArtDatabanken.Data.ArtDatabankenService.FactorId.RedlistCategory);
                speciesFactCondition.Factors.Add(factor);
                speciesFactCondition.IndividualCategories.Add(IndividualCategoryManager.GetDefaultIndividualCategory());
                speciesFactCondition.Periods.Add(PeriodManager.GetCurrentPublicPeriod());
                for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
                {
                    speciesFactFieldCondition = new SpeciesFactFieldCondition();
                    speciesFactFieldCondition.FactorField = factor.FactorDataType.Field1;
                    speciesFactFieldCondition.SetValue((Int32)redListCategory);
                    speciesFactCondition.SpeciesFactFieldConditions.Clear();
                    speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition,
                                                                                               TaxonInformationType.Basic);
                    redlistedTaxonIds = new List<Int32>();
                    foreach (ArtDatabanken.Data.ArtDatabankenService.Taxon taxon in taxa)
                    {
                        redlistedTaxonIds.Add(taxon.Id);
                    }

                    allRedlistedTaxonIds.AddRange(redlistedTaxonIds);
                    redlistedTaxaInformation[GetRedlistedTaxaCacheKey(redListCategory)] = redlistedTaxonIds;
                }

                redlistedTaxaInformation[GetRedlistedTaxaCacheKey()] = allRedlistedTaxonIds;

                // Store data in cache.
                context.AddCachedObject(GetRedlistedTaxaCacheKey(),
                                        redlistedTaxaInformation,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return redlistedTaxaInformation;
        }

        /// <summary>
        /// Get cache key for all red listed taxa.
        /// </summary>
        /// <returns>Cache key for all red listed taxa.</returns>
        private static String GetRedlistedTaxaCacheKey()
        {
            return Settings.Default.RedlistedTaxaCacheKey;
        }

        /// <summary>
        /// Get cache key for taxa that is red listed
        /// in specified red list category.
        /// </summary>
        /// <param name="redlistCategory">Cache key for taxa belonging to specified red list category should be returned.</param>
        /// <returns>Cache key for red listed taxa.</returns>
        private static String GetRedlistedTaxaCacheKey(RedListCategory redlistCategory)
        {
            return Settings.Default.RedlistedTaxaCacheKey +
                   WebService.Settings.Default.CacheKeyDelimiter +
                   redlistCategory;
        }

        ///// <summary>
        ///// Get taxon ids for red listed taxa.
        ///// </summary>
        ///// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        ///// <param name="includeRedlistedTaxa">If true all red listed taxa should be returned.</param>
        ///// <param name="redlistCategories">Taxa belonging to specified red list categories should be returned.</param>
        ///// <returns>Requested red listed taxa.</returns>
        //private static List<Int32> GetRedlistedTaxonIds(WebServiceContext context,
        //                                                Boolean includeRedlistedTaxa,
        //                                                List<RedListCategory> redlistCategories)
        //{
        //    Hashtable redlistedTaxaInformation;
        //    List<Int32> redlistedTaxonIds;

        //    // Get cached information.
        //    redlistedTaxaInformation = GetRedlistedTaxa(context);

        //    if (includeRedlistedTaxa)
        //    {
        //        redlistedTaxonIds = (List<Int32>)(redlistedTaxaInformation[GetRedlistedTaxaCacheKey()]);
        //    }
        //    else
        //    {
        //        redlistedTaxonIds = new List<Int32>();
        //        if (redlistCategories.IsNotEmpty())
        //        {
        //            foreach (RedListCategory redListCategory in redlistCategories)
        //            {
        //                redlistedTaxonIds.AddRange((List<Int32>)(redlistedTaxaInformation[GetRedlistedTaxaCacheKey(redListCategory)]));
        //            }
        //        }
        //    }

        //    return redlistedTaxonIds;
        //}

        ///// <summary>
        ///// Get taxon ids for red listed taxa.
        ///// </summary>
        ///// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        ///// <param name="searchCriteria">The species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        ///// <returns>Requested red listed taxa.</returns>
        //private static List<Int32> GetTaxonIds(WebServiceContext context,
        //                                       WebSpeciesObservationSearchCriteria searchCriteria)
        //{
        //    List<Int32> redlistedTaxonIds, taxonIds;

        //    taxonIds = searchCriteria.TaxonIds;
        //    if (searchCriteria.IncludeRedlistedTaxa ||
        //        searchCriteria.IncludeRedListCategories.IsNotEmpty())
        //    {
        //        // Get all redlisted taxa from ArtDatabankenService.
        //        redlistedTaxonIds =WebSpeciesObservationServiceData.TaxonManager.GetRedlistedTaxonIds(context,
        //                                                 searchCriteria.IncludeRedlistedTaxa,
        //                                                 searchCriteria.IncludeRedListCategories);
        //        if (redlistedTaxonIds.IsNotEmpty())
        //        {
        //            if (taxonIds.IsEmpty())
        //            {
        //                taxonIds = redlistedTaxonIds;
        //            }
        //            else
        //            {
        //                foreach (Int32 taxonId in redlistedTaxonIds)
        //                {
        //                    if (!taxonIds.Contains(taxonId))
        //                    {
        //                        taxonIds.Add(taxonId);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return taxonIds;
        //}

        /// <summary>
        /// Get time step specific species observation counts for a specific set of species observation search criteria from database.
        /// Only counts greater than zero is included in the resulting list from the database.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">WebCoordinateSystem i.e coordinate system which WebSpeciesObservationSearchCriteria is 
        /// using.</param>
        /// <returns>A list of time step specific species observation counts.</returns>
        private static List<WebTimeStepSpeciesObservationCount> GetTimeSpeciesObservationCountsBySearchCriteriaInternal(WebServiceContext context,
            WebSpeciesObservationSearchCriteria searchCriteria,
            Periodicity periodicity,
            WebCoordinateSystem coordinateSystem)
        {
            List<Int64> speciesObservationIds;
            CultureInfo culture = new CultureInfo(context.Locale.ISOCode);
            List<WebTimeStepSpeciesObservationCount> timeSerie;
            WebTimeStepSpeciesObservationCount timeStep;
            Int32 timeStepId = 1;
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;
            List<int> regionIds, taxonIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get all data required to performe/build up a db search.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            joinCondition = searchCriteria.GetJoinCondition();
            polygons = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            whereCondition = searchCriteria.GetWhereCondition(context, coordinateSystem);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);

            // Create result list from database
            timeSerie = new List<WebTimeStepSpeciesObservationCount>();

            // Retrieve counts greater than zero from database.
            using (DataReader dataReader = context.GetAnalysisDatabase().GetTimeSpeciesObservationCountsBySearchCriteria(polygons,
                                                                                                                         regionIds,
                                                                                                                         taxonIds,
                                                                                                                         joinCondition,
                                                                                                                         whereCondition,
                                                                                                                         periodicity.ToString(),
                                                                                                                         speciesObservationIds))
            {
                while (dataReader.Read())
                {
                    timeStep = new WebTimeStepSpeciesObservationCount();
                    timeStep.LoadData(dataReader, culture);
                    timeStep.Id = timeStepId++;
                    timeSerie.Add(timeStep);
                }
            }

            return timeSerie;
        }

        /// <summary>
        /// Get converted point.
        /// </summary>
        /// <param name="toCoordinateSystem">Coordinate system to convert to.</param>
        /// <param name="fromCoordinateSystem">Coordinate system to convert from.</param>
        /// <param name="pointToBeConverted">Point to be converted.</param>
        /// <returns>Converted web point.</returns>
        private static WebPoint GetConvertedPointCoordinates(WebCoordinateSystem toCoordinateSystem,
                                                             WebCoordinateSystem fromCoordinateSystem,
                                                             WebPoint pointToBeConverted)
        {
            // Convert coordinates if needed 
            if (toCoordinateSystem.GetWkt().ToUpper() != fromCoordinateSystem.GetWkt().ToUpper())
            {
                WebPoint gridCellCoordinate = WebServiceData.CoordinateConversionManager.GetConvertedPoint(pointToBeConverted,
                                                                                                           fromCoordinateSystem,
                                                                                                           toCoordinateSystem);
                return gridCellCoordinate;
            }

            return pointToBeConverted;
        }

        /// <summary>
        /// Get Corresponding WebCoordinateSystem from GridCellCoordinatedSystem.
        /// </summary>
        /// <param name="gridCellCoordinateSystem">Coordinate system used in grid cells.</param>
        /// <returns>Matching coordinate system.</returns>
        private static WebCoordinateSystem GetWebCoordinateSystemFromGridCellCoordinateSystem(GridCoordinateSystem gridCellCoordinateSystem)
        {
            WebCoordinateSystem gridCellCoordinateSystemAsWebCoordinateSystem = null;
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                if (coordinateSystemId.ToString().Equals(gridCellCoordinateSystem.ToString()))
                {
                    gridCellCoordinateSystemAsWebCoordinateSystem = new WebCoordinateSystem();
                    gridCellCoordinateSystemAsWebCoordinateSystem.Id = coordinateSystemId;
                    break;
                }
            }

            if (gridCellCoordinateSystemAsWebCoordinateSystem.IsNull())
            {
                throw new ArgumentException("GridCellCoordinateSystem don't match any existing CoordinateSystem. " +
                                gridCellCoordinateSystem.ToString() +
                                " don't exsist in CoordinateSystem as enum value.");
            }

            return gridCellCoordinateSystemAsWebCoordinateSystem;
        }

        /// <summary>
        /// Extract the srid number from the featuresUrl.
        /// </summary>
        /// <param name="featuresUri">Url for selected feature.</param>
        /// <returns>Matching coordinate system.</returns>
        public static string GetCoordinateSystemFromFeaturesUrlAsSrsName(Uri featuresUri)
        {
            string srsName = string.Empty;
            int index = 0;
            if (featuresUri != null && !featuresUri.ToString().IsNotEmpty())
            {
                index = featuresUri.ToString().IndexOf("EPSG:", index, StringComparison.Ordinal);
                index = index + 5;
                int endIndex = featuresUri.ToString().IndexOf("&", index, StringComparison.Ordinal);
                if (endIndex < 0)
                {
                    endIndex = featuresUri.ToString().Length;
                }

                srsName = featuresUri.ToString().Substring(index, endIndex - index).Trim();
            }

            return srsName;
        }

        /// <summary>
        /// Gets the week of the year.
        /// </summary>
        /// <param name="date">Date to get week from.</param>
        /// <returns>Week number.</returns>
        private static int GetWeekOfYear(DateTime date)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            return weekNum;
        }

        /// <summary>
        /// Merges the feature statistics with species observation counts.
        /// </summary>
        /// <param name="featureStatistics">The feature statistics.</param>
        /// <param name="speciesObservationCounts">The species observation counts.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        private static List<WebGridCellCombinedStatistics> MergeFeatureStatisticsWithSpeciesObservationCounts(
                                                                                                            List<WebGridCellFeatureStatistics> featureStatistics,
                                                                                                            List<WebGridCellSpeciesCount> speciesObservationCounts)
        {
            Dictionary<WebBoundingBox, WebGridCellCombinedStatistics> cellDictionary;

            cellDictionary = new Dictionary<WebBoundingBox, WebGridCellCombinedStatistics>(new WebBoundingBoxEqualityComparer());
            foreach (WebGridCellSpeciesCount observationCount in speciesObservationCounts)
            {
                if (!cellDictionary.ContainsKey(observationCount.OrginalBoundingBox))
                {
                    WebGridCellCombinedStatistics statisticsItem = new WebGridCellCombinedStatistics();
                    statisticsItem.OriginalBoundingBox = observationCount.OrginalBoundingBox;
                    statisticsItem.SpeciesCount = observationCount;
                    cellDictionary.Add(observationCount.OrginalBoundingBox, statisticsItem);
                }
            }

            foreach (WebGridCellFeatureStatistics featureItem in featureStatistics)
            {
                if (!cellDictionary.ContainsKey(featureItem.OrginalBoundingBox))
                {
                    WebGridCellCombinedStatistics statisticsItem = new WebGridCellCombinedStatistics();
                    statisticsItem.OriginalBoundingBox = featureItem.OrginalBoundingBox;
                    statisticsItem.FeatureStatistics = featureItem;
                    cellDictionary.Add(featureItem.OrginalBoundingBox, statisticsItem);
                }
                else
                {
                    WebGridCellCombinedStatistics statisticsItem = cellDictionary[featureItem.OrginalBoundingBox];
                    statisticsItem.FeatureStatistics = featureItem;
                }
            }

            return cellDictionary.Values.ToList();
        }

        #endregion
    }
}
