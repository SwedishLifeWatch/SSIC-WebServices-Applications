using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using IAnalysisDataSource = ArtDatabanken.Data.DataSource.IAnalysisDataSource;

namespace ArtDatabanken.WebService.Client.AnalysisService
{
    /// <summary>
    /// This class is used to retrieve species observation related information.
    /// </summary>
    public class AnalysisDataSource : AnalysisDataSourceBase, IAnalysisDataSource
    {
        /// <summary>
        /// Get number of species  that matches the search criteria.
        /// Scope is restricted to those species that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Number of species observations.</returns>
        public Int64 GetSpeciesCountBySearchCriteria(IUserContext userContext,
                                              ISpeciesObservationSearchCriteria searchCriteria,
                                              ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;


            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesCountBySearchCriteria(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webCoordinateSystem);


            return noOfObservations;
        }
        
        
        /// <summary>
        /// Get number of species observations that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Number of species observations.</returns>
        public Int64 GetSpeciesObservationCountBySearchCriteria(IUserContext userContext,
                                                              ISpeciesObservationSearchCriteria searchCriteria,
                                                              ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;
            

            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webCoordinateSystem);


            return noOfObservations;
        }

        /// <summary>
        /// Get species observation provenances that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>List of species observation provenances.</returns>
        public List<SpeciesObservationProvenance> GetSpeciesObservationProvenancesBySearchCriteria(IUserContext userContext,
                                                                      ISpeciesObservationSearchCriteria searchCriteria,
                                                                      ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;
            List<SpeciesObservationProvenance> speciesObservationProvenances;

            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);

            // Map calling classes
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            List<WebSpeciesObservationProvenance> webProvenances = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webCoordinateSystem);

            // Convert to onion data from web data.
            speciesObservationProvenances = null;
            if (webProvenances.IsNotNull())
            {
                speciesObservationProvenances = new List<SpeciesObservationProvenance>();
                
                for (int i = 0; i < webProvenances.Count; i++)
                {
                    WebSpeciesObservationProvenance webProvenance = webProvenances[i];
                    SpeciesObservationProvenance speciesObservationProvenance = ConvertSpeciesObservationProvenance(webProvenance);
                    speciesObservationProvenances.Add(speciesObservationProvenance);
                }
            }

            return speciesObservationProvenances;
        }

        ///// <summary>
        ///// TODO Inplemet code
        ///// </summary>
        ///// <param name="userContext"></param>
        ///// <param name="featureStatistics"></param>
        ///// <param name="resourceAdresse"></param>
        ///// <param name="gridSpecification"></param>
        ///// <param name="coordinateSystem"></param>
        ///// <returns></returns>
        //public List<IGridCellFeatureStatistics> GetGridCellFeatureStatistics(IUserContext userContext, FeatureStatisticsSummary featureStatistics,
        //                                     ResourceAdresse resourceAdresse, IGridSpecification gridSpecification,
        //                                     ICoordinateSystem coordinateSystem)
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>        
        /// <param name="userContext">User context.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="featureStatistics">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection as json.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public IList<IGridCellCombinedStatistics> GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
             IUserContext userContext,
             IGridSpecification gridSpecification, 
             ISpeciesObservationSearchCriteria searchCriteria,
             FeatureStatisticsSummary featureStatistics,
             String featuresUrl,
             String featureCollectionJson,
             ICoordinateSystem coordinateSystem
             )
         {
             List<WebGridCellCombinedStatistics> webGridCellCombinedStatistics;             
             WebGridSpecification webGridSpecification;
             WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
             WebFeatureStatisticsSpecification webFeatureStatisticsSpecification;
             WebCoordinateSystem webCoordinateSystem;
             
             // Map calling classes
             webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
             webFeatureStatisticsSpecification = new WebFeatureStatisticsSpecification();
             if (featureStatistics != null)
             {
                 webFeatureStatisticsSpecification.FeatureType = featureStatistics.FeatureType;
             }

             webGridSpecification = new WebGridSpecification();
             if (gridSpecification.IsNotNull())
             {
                 webGridSpecification = new WebGridSpecification();
                 webGridSpecification.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                 webGridSpecification.IsGridCellSizeSpecified = gridSpecification.IsGridCellSizeSpecified;
                 if (gridSpecification.BoundingBox.IsNotNull())
                 {
                     webGridSpecification.BoundingBox = GetBoundingBox(gridSpecification.BoundingBox);
                 }
                 if (gridSpecification.IsGridCellSizeSpecified)
                 {
                     webGridSpecification.GridCellSize = gridSpecification.GridCellSize;
                 }
                 webGridSpecification.GridCellGeometryType = gridSpecification.GridCellGeometryType;
             }
             webCoordinateSystem = GetCoordinateSystem(coordinateSystem);


             // Execute method             
             webGridCellCombinedStatistics = WebServiceProxy.AnalysisService.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
                 GetClientInformation(userContext), 
                 webGridSpecification, 
                 webSpeciesObservationSearchCriteria, 
                 webFeatureStatisticsSpecification, 
                 featuresUrl,
                 featureCollectionJson,
                 webCoordinateSystem);
        
             // Convert to onion data from web data.
            IList<IGridCellCombinedStatistics> resultList = null;
            if (webGridCellCombinedStatistics.IsNotNull())
            {
                resultList = new List<IGridCellCombinedStatistics>();
                for (int i = 0; i < webGridCellCombinedStatistics.Count; i++)
                {                    
                    WebGridCellCombinedStatistics webCellStatistics = webGridCellCombinedStatistics[i];
                    IGridCellCombinedStatistics gridCellCombinedStatistics = ConvertGridCellCombinedStatistics(webCellStatistics);
                    resultList.Add(gridCellCombinedStatistics);
                }
            }

            return resultList; 
         }


        /// <summary>
        /// Get information about spatial features in a grid representation inside a user supplied bounding box.
        /// </summary>
        /// /// <param name="userContext">User context.</param>
        /// <param name="featureStatistics">Information about what statistics are requested from a web feature 
        /// service and wich spatial feature type that is to be measured</param>
        /// <param name="featuresUrl">Resource address.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">Specifications of requested grid cell size, requested coordinate system 
        /// and user supplied bounding box.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Statistical measurements on spatial features in grid format.</returns>
        public List<IGridCellFeatureStatistics> GetGridCellFeatureStatistics(IUserContext userContext,
                                                                  FeatureStatisticsSummary featureStatistics,
                                                                  String featuresUrl,
                                                                  String featureCollectionJson,
                                                                  IGridSpecification gridSpecification, 
                                                                  ICoordinateSystem coordinateSystem)
        {
            List<IGridCellFeatureStatistics> gridCellFeatureStatisticsList;
            gridCellFeatureStatisticsList = new List<IGridCellFeatureStatistics>();

            WebGridSpecification webGridSpecification;
            webGridSpecification = new WebGridSpecification();
            
            WebCoordinateSystem webCoordinateSystem;
            webCoordinateSystem = new WebCoordinateSystem();
            
            WebFeatureStatisticsSpecification webFeatureStatistics;
            webFeatureStatistics = new WebFeatureStatisticsSpecification();

            //Map calling classes
            if (featureStatistics != null)
            {
                webFeatureStatistics.FeatureType = featureStatistics.FeatureType;
            }
            if (gridSpecification.IsNotNull())
            {
                webGridSpecification = new WebGridSpecification();
                webGridSpecification.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                webGridSpecification.IsGridCellSizeSpecified = gridSpecification.IsGridCellSizeSpecified;
                if (gridSpecification.BoundingBox.IsNotNull())
                {
                    webGridSpecification.BoundingBox = GetBoundingBox(gridSpecification.BoundingBox);
                }
                if (gridSpecification.IsGridCellSizeSpecified)
                {
                    webGridSpecification.GridCellSize = gridSpecification.GridCellSize;
                }
                webGridSpecification.GridCellGeometryType = gridSpecification.GridCellGeometryType;
            }
            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);

            // Execute method
            List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList =
                WebServiceProxy.AnalysisService.GetGridCellFeatureStatistics(GetClientInformation(userContext),
                                                                         webFeatureStatistics, featuresUrl, featureCollectionJson,
                                                                         webGridSpecification, webCoordinateSystem);
            // Convert to onion data from web data.
            if (webGridCellFeatureStatisticsList.IsNotNull())
            {
                foreach (WebGridCellFeatureStatistics webGridCellFeatureStatistics in webGridCellFeatureStatisticsList)
                {
                    IGridCellFeatureStatistics gridCellFeatureStatistics = ConvertGridCellFeatureStatistics(webGridCellFeatureStatistics);
                    gridCellFeatureStatisticsList.Add(gridCellFeatureStatistics);
                }
            }

            return gridCellFeatureStatisticsList;
        }

        /// <summary>
        /// Gets no of species observations
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="gridSpecification"> Specifications used for the grid in grid search.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criterias.</param>
        /// <returns>Information about changed species observations.</returns>
        public IList<IGridCellSpeciesObservationCount> GetGridSpeciesObservationCounts(IUserContext userContext,
                                                              ISpeciesObservationSearchCriteria searchCriteria,
                                                              IGridSpecification gridSpecification,
                                                              ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;
            WebGridSpecification webGridSpecification = null;

            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            // Note! If Bounding Box is set then it is used in SpeciesObservationSearchCriteria.BoundingBox.
            if(gridSpecification.IsNotNull())
            {
                webGridSpecification = new WebGridSpecification();
                webGridSpecification.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                webGridSpecification.IsGridCellSizeSpecified = gridSpecification.IsGridCellSizeSpecified;
                if (gridSpecification.IsGridCellSizeSpecified)
                {
                    webGridSpecification.GridCellSize = gridSpecification.GridCellSize;
                 }
                webGridSpecification.GridCellGeometryType = gridSpecification.GridCellGeometryType;
            }
           
            // Execute method
            IList<WebGridCellSpeciesObservationCount> webGridCellSpeciesObservationCountsList = WebServiceProxy.AnalysisService.GetGridSpeciesObservationCounts(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webGridSpecification, webCoordinateSystem);
            
            // Convert to onion data from web data.
            IList<IGridCellSpeciesObservationCount> gridCellSpeciesObservationCountsList = null;
            
            if ((webGridCellSpeciesObservationCountsList as List<WebGridCellSpeciesObservationCount>).IsNotNull())
            {
                gridCellSpeciesObservationCountsList = new List<IGridCellSpeciesObservationCount>();
                foreach (var webGridCellSpeciesObservationCount in webGridCellSpeciesObservationCountsList)
                {
                    var gridCellSpeciesObservationCount = ConvertGridCellSpeciesObservationCount(webGridCellSpeciesObservationCount);
                    gridCellSpeciesObservationCountsList.Add(gridCellSpeciesObservationCount);
                }
            }

           return gridCellSpeciesObservationCountsList;                                               
        }

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name = "alphaValue" > If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        public string GetSpeciesObservationAOOEOOAsGeoJson(IUserContext userContext, IList<IGridCellSpeciesObservationCount> gridCells, int alphaValue = 0, bool useCenterPoint = true)
        {
            if (gridCells == null || gridCells.Count == 0)
            {
                return null;
            }
            var webGridCells = (
                from 
                    gc 
                in 
                    gridCells
                select 
                    ConvertWebGridCellSpeciesObservationCount(gc)).ToList();
            
            // Execute method
            return WebServiceProxy.AnalysisService.GetSpeciesObservationAOOEOOAsGeoJson(GetClientInformation(userContext), webGridCells, alphaValue, useCenterPoint);
        }
        
        /// <summary>
        /// Gets GridCellInformations 
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="gridSpecification">The species observation search criteria, GridCellSize and GridCoordinateSyatem are the
        /// only properties that is used in this method.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criterias.</param>
        /// <returns>Information about species.</returns>
        public IList<IGridCellSpeciesCount> GetGridSpeciesCounts(IUserContext userContext,
                                                                 ISpeciesObservationSearchCriteria searchCriteria,
                                                                 IGridSpecification gridSpecification,
                                                                 ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;
            WebGridSpecification webGridSpecification = null;

            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            // Note! If Bounding Box is set then it is used in SpeciesObservationSearchCriteria.BoundingBox.
            if (gridSpecification.IsNotNull())
            {
                webGridSpecification = new WebGridSpecification();
                webGridSpecification.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                webGridSpecification.IsGridCellSizeSpecified = gridSpecification.IsGridCellSizeSpecified;
                if (gridSpecification.IsGridCellSizeSpecified)
                {
                    webGridSpecification.GridCellSize = gridSpecification.GridCellSize;
                }
                webGridSpecification.GridCellGeometryType = gridSpecification.GridCellGeometryType;
            }



            // Execute method
            IList<WebGridCellSpeciesCount> webGridCellSpeciesCountsList = WebServiceProxy.AnalysisService.GetGridSpeciesCounts(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webGridSpecification, webCoordinateSystem);

            // Convert to onion data from web data.
            IList<IGridCellSpeciesCount> gridCellSpeciesCountsList = null;

            if ((webGridCellSpeciesCountsList as List<WebGridCellSpeciesCount>).IsNotNull())
            {
                gridCellSpeciesCountsList = new List<IGridCellSpeciesCount>();
                foreach (WebGridCellSpeciesCount webGridCellSpeciesCount in webGridCellSpeciesCountsList)
                {
                    IGridCellSpeciesCount gridCellSpeciesCount = ConvertGridCellSpeciesCount(webGridCellSpeciesCount);
                    gridCellSpeciesCountsList.Add(gridCellSpeciesCount);
                }
            }

            return gridCellSpeciesCountsList;                          
        }

        /// <summary>
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         ISpeciesFactSearchCriteria searchCriteria)
        {
            List<WebTaxon> taxa;
            WebSpeciesFactSearchCriteria webSpeciesFactSearchCriteria;

            webSpeciesFactSearchCriteria = GetSpeciesFactSearchCriteria(searchCriteria);
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(userContext),
                                                                                      webSpeciesFactSearchCriteria);

            return GetTaxa(userContext, taxa);
        }

        /// <summary>
        /// Gets taxa 
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criterias.</param>
        /// <returns>Information about taxa.</returns>
        public TaxonList GetTaxaBySearchCriteria(IUserContext userContext,
                                                 ISpeciesObservationSearchCriteria searchCriteria,
                                                 ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;


            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            List<WebTaxon> webTaxa = WebServiceProxy.AnalysisService.GetTaxaBySearchCriteria(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webCoordinateSystem);

            return GetTaxa(userContext, webTaxa);
        }

        /// <summary>
        /// Gets taxa, with related number of observed species,
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criterias.</param>
        /// <returns>Information about taxa.</returns>
        public TaxonSpeciesObservationCountList GetTaxaWithSpeciesObservationCountsBySearchCriteria(IUserContext userContext,
                                                 ISpeciesObservationSearchCriteria searchCriteria,
                                                 ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;


            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            List<WebTaxonSpeciesObservationCount> webTaxaSpeciesObservationCount = WebServiceProxy.AnalysisService.GetTaxaWithSpeciesObservationCountsBySearchCriteria(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webCoordinateSystem);

            return GetTaxaSpeciesObservationCount(userContext, webTaxaSpeciesObservationCount); ;
        }

        /// <summary>
        /// Convert one WebGridCellSpeciesObservationCount to one GridCellSpeciesObservationCount.
        /// </summary>
        private GridCellSpeciesObservationCount ConvertGridCellSpeciesObservationCount(WebGridCellSpeciesObservationCount webGridCellSpeciesObservationCount)
        {
            GridCellSpeciesObservationCount gridCellSpeciesObservationCount;
            gridCellSpeciesObservationCount = new GridCellSpeciesObservationCount();
            gridCellSpeciesObservationCount.GridCoordinateSystem = webGridCellSpeciesObservationCount.GridCoordinateSystem;
            gridCellSpeciesObservationCount.GridCellSize = webGridCellSpeciesObservationCount.Size;
            gridCellSpeciesObservationCount.ObservationCount = webGridCellSpeciesObservationCount.Count;
            gridCellSpeciesObservationCount.CoordinateSystem = GetCoordinateSystem(webGridCellSpeciesObservationCount.CoordinateSystem);
            // Get points
            gridCellSpeciesObservationCount.GridCellCentreCoordinate = ConvertPoint(webGridCellSpeciesObservationCount.CentreCoordinate);
            gridCellSpeciesObservationCount.OrginalGridCellCentreCoordinate = ConvertPoint(webGridCellSpeciesObservationCount.OrginalCentreCoordinate);
            // Get bounding boxes
            gridCellSpeciesObservationCount.GridCellBoundingBox = ConvertBoundingPolygon(webGridCellSpeciesObservationCount.BoundingBox);
            gridCellSpeciesObservationCount.OrginalGridCellBoundingBox = ConvertBoundingBox(webGridCellSpeciesObservationCount.OrginalBoundingBox);
             return gridCellSpeciesObservationCount;
        }

        /// <summary>
        /// Convert one WebGridCellSpeciesObservationCount to one GridCellSpeciesObservationCount.
        /// </summary>
        private WebGridCellSpeciesObservationCount ConvertWebGridCellSpeciesObservationCount(IGridCellSpeciesObservationCount gridCell)
        {
            return new WebGridCellSpeciesObservationCount()
            {
                GridCoordinateSystem = gridCell.GridCoordinateSystem,
                Size = gridCell.GridCellSize,
                Count = gridCell.ObservationCount,
                CoordinateSystem = GetCoordinateSystem(gridCell.CoordinateSystem),
                // Get points
                CentreCoordinate = ConvertWebPoint(gridCell.GridCellCentreCoordinate),
                OrginalCentreCoordinate = ConvertWebPoint(gridCell.OrginalGridCellCentreCoordinate),
                // Get bounding boxes
                BoundingBox = GetPolygon(gridCell.GridCellBoundingBox),
                OrginalBoundingBox = GetBoundingBox(gridCell.OrginalGridCellBoundingBox)
            };
        }

        /// <summary>
        /// Convert one WebGridCellSpeciesObservationCount to one GridCellSpeciesObservationCount.
        /// </summary>
        private IGridCellFeatureStatistics ConvertGridCellFeatureStatistics(WebGridCellFeatureStatistics webGridCellFeatureStatistics)
        {
            IGridCellFeatureStatistics gridCellFeatureStatistics;
            gridCellFeatureStatistics = new GridCellFeatureStatistics();

            //gridCellFeatureStatistics.Easting = webGridCellFeatureStatistics.Easting; //deleted from data contract?
            //gridCellFeatureStatistics.Northing = webGridCellFeatureStatistics.Northing; //deleted from data contract?
            //gridCellFeatureStatistics.FeatureSize = webGridCellFeatureStatistics.FeatureSize; //deleted from data contract?
            gridCellFeatureStatistics.FeatureArea = webGridCellFeatureStatistics.FeatureArea; 
            gridCellFeatureStatistics.FeatureCount = webGridCellFeatureStatistics.FeatureCount; 
            gridCellFeatureStatistics.FeatureLength = webGridCellFeatureStatistics.FeatureLength;
            gridCellFeatureStatistics.FeatureType = webGridCellFeatureStatistics.FeatureType;
            
            gridCellFeatureStatistics.GridCoordinateSystem = webGridCellFeatureStatistics.GridCoordinateSystem;
            gridCellFeatureStatistics.GridCellSize = webGridCellFeatureStatistics.Size;
            gridCellFeatureStatistics.CoordinateSystem = GetCoordinateSystem(webGridCellFeatureStatistics.CoordinateSystem);
            // Get points
            gridCellFeatureStatistics.GridCellCentreCoordinate = ConvertPoint(webGridCellFeatureStatistics.CentreCoordinate);
            gridCellFeatureStatistics.OrginalGridCellCentreCoordinate = ConvertPoint(webGridCellFeatureStatistics.OrginalCentreCoordinate);
            // Get bounding boxes
            gridCellFeatureStatistics.GridCellBoundingBox = ConvertBoundingPolygon(webGridCellFeatureStatistics.BoundingBox);
            gridCellFeatureStatistics.OrginalGridCellBoundingBox = ConvertBoundingBox(webGridCellFeatureStatistics.OrginalBoundingBox);

            return gridCellFeatureStatistics;
        }


        private IGridCellCombinedStatistics ConvertGridCellCombinedStatistics(WebGridCellCombinedStatistics webCellStatistics)
        {
            GridCellCombinedStatistics gridCellCombinedStatistics;
            IGridCellBase gridCellBase = null;

            gridCellCombinedStatistics = new GridCellCombinedStatistics();

            if (webCellStatistics.FeatureStatistics.IsNotNull())
            {
                gridCellCombinedStatistics.FeatureStatistics = ConvertGridCellFeatureStatistics(webCellStatistics.FeatureStatistics);
                gridCellBase = gridCellCombinedStatistics.FeatureStatistics;
            }
            if (webCellStatistics.SpeciesCount.IsNotNull())
            {
                gridCellCombinedStatistics.SpeciesCount = ConvertGridCellSpeciesCount(webCellStatistics.SpeciesCount);
                gridCellBase = gridCellCombinedStatistics.SpeciesCount;
            }

            //webCellStatistics.OriginalBoundingBox
            if (gridCellBase != null)
            {
                gridCellCombinedStatistics.GridCellGeometryType = gridCellBase.GridCellGeometryType;
                gridCellCombinedStatistics.GridCoordinateSystem = gridCellBase.GridCoordinateSystem;
                gridCellCombinedStatistics.GridCellSize = gridCellBase.GridCellSize;
                gridCellCombinedStatistics.CoordinateSystem = gridCellBase.CoordinateSystem;
                // Get points
                gridCellCombinedStatistics.GridCellCentreCoordinate = gridCellBase.GridCellCentreCoordinate;
                gridCellCombinedStatistics.OrginalGridCellCentreCoordinate = gridCellBase.OrginalGridCellCentreCoordinate;
                // Get bounding boxes
                gridCellCombinedStatistics.GridCellBoundingBox = gridCellBase.GridCellBoundingBox;
                gridCellCombinedStatistics.OrginalGridCellBoundingBox = gridCellBase.OrginalGridCellBoundingBox;
            }
            return gridCellCombinedStatistics;
        }

        /// <summary>
        /// Convert one WebGridCellSpeciesCount to one GridCellSpeciesCount.
        /// </summary>
        private GridCellSpeciesCount ConvertGridCellSpeciesCount(WebGridCellSpeciesCount webGridCellSpeciesCount)
        {
            GridCellSpeciesCount gridCellSpeciesCount;
            gridCellSpeciesCount = new GridCellSpeciesCount();
            gridCellSpeciesCount.GridCoordinateSystem = webGridCellSpeciesCount.GridCoordinateSystem;
            gridCellSpeciesCount.GridCellSize = webGridCellSpeciesCount.Size;
            gridCellSpeciesCount.ObservationCount = webGridCellSpeciesCount.SpeciesObservationCount;
            gridCellSpeciesCount.SpeciesCount = webGridCellSpeciesCount.SpeciesCount;
            gridCellSpeciesCount.CoordinateSystem = GetCoordinateSystem(webGridCellSpeciesCount.CoordinateSystem);
            // Get points
            gridCellSpeciesCount.GridCellCentreCoordinate = ConvertPoint(webGridCellSpeciesCount.CentreCoordinate);
            gridCellSpeciesCount.OrginalGridCellCentreCoordinate = ConvertPoint(webGridCellSpeciesCount.OrginalCentreCoordinate);
            // Get bounding boxes
            gridCellSpeciesCount.GridCellBoundingBox = ConvertBoundingPolygon(webGridCellSpeciesCount.BoundingBox);
            gridCellSpeciesCount.OrginalGridCellBoundingBox = ConvertBoundingBox(webGridCellSpeciesCount.OrginalBoundingBox);
            return gridCellSpeciesCount;
        }

        private static Point ConvertPoint(WebPoint webCoordinatePoint)
        {
            Point coordinatePoint = null;
            if (webCoordinatePoint.IsNotNull())
            {
                coordinatePoint = new Point();
                coordinatePoint.X = webCoordinatePoint.X;
                coordinatePoint.Y = webCoordinatePoint.Y;
                }
            return coordinatePoint;
        }

        private static WebPoint ConvertWebPoint(IPoint coordinatePoint)
        {
            return coordinatePoint == null ? null : new WebPoint(coordinatePoint.X, coordinatePoint.Y);
        }

        private static BoundingBox ConvertBoundingBox(WebBoundingBox webBoundingBox)
        {
            BoundingBox boundingBox = null;
            if (webBoundingBox.IsNotNull())
            {
                boundingBox = new BoundingBox();
                WebPoint webMaxPoint = webBoundingBox.Max;
                WebPoint webMinPoint = webBoundingBox.Min;
                boundingBox.Max = ConvertPoint(webMaxPoint);
                boundingBox.Min = ConvertPoint(webMinPoint);
            }
            return boundingBox;
        }

        private static IPolygon ConvertBoundingPolygon(WebPolygon webBoundingBox)
        {
            IPolygon boundingPolygon = null;
            if (webBoundingBox.IsNotNull())
            {
                boundingPolygon = new Polygon();
                boundingPolygon.LinearRings = new List<ILinearRing>();
                
                foreach (WebLinearRing webRing in webBoundingBox.LinearRings)
                {
                   ILinearRing ring = new LinearRing();
                   ring.Points = new List<IPoint>();
                   
                    foreach (WebPoint webPoint in webRing.Points)
                    {
                        IPoint point = new Point();
                        point.X = webPoint.X;
                        point.Y = webPoint.Y;
                        ring.Points.Add(point);

                    }
                    boundingPolygon.LinearRings.Add(ring);
                }
               
                //boundingPolygon.LinearRings.Add(new LinearRing());
                //boundingPolygon.LinearRings[0].Points = new List<IPoint>();

                //boundingPolygon.LinearRings[0].Points[0].X = webBoundingBox.LinearRings[0].Points[0].X;
                //boundingPolygon.LinearRings[0].Points[0].Y = webBoundingBox.LinearRings[0].Points[0].Y;
                //boundingPolygon.LinearRings[0].Points[1].X = webBoundingBox.LinearRings[0].Points[1].X;
                //boundingPolygon.LinearRings[0].Points[1].Y = webBoundingBox.LinearRings[0].Points[1].Y;
                //boundingPolygon.LinearRings[0].Points[2].X = webBoundingBox.LinearRings[0].Points[2].X;
                //boundingPolygon.LinearRings[0].Points[2].Y = webBoundingBox.LinearRings[0].Points[2].Y;
                //boundingPolygon.LinearRings[0].Points[3].X = webBoundingBox.LinearRings[0].Points[3].X;
                //boundingPolygon.LinearRings[0].Points[3].Y = webBoundingBox.LinearRings[0].Points[3].Y;
                //boundingPolygon.LinearRings[0].Points[4].X = webBoundingBox.LinearRings[0].Points[4].X;
                //boundingPolygon.LinearRings[0].Points[4].Y = webBoundingBox.LinearRings[0].Points[4].Y;
               }
            return boundingPolygon;
        }

        private SpeciesObservationProvenance ConvertSpeciesObservationProvenance(WebSpeciesObservationProvenance webSpeciesObservationProvenance)
        {
            SpeciesObservationProvenance speciesObservationProvenance;
            SpeciesObservationProvenanceValue speciesObservationProvenanceValue;

            speciesObservationProvenance = new SpeciesObservationProvenance();

            if (webSpeciesObservationProvenance.Name.IsNotNull())
            {
                speciesObservationProvenance.Name = webSpeciesObservationProvenance.Name;
            }

            if (webSpeciesObservationProvenance.Values.IsNotNull())
            {
                foreach (var webSpeciesObservationProvenanceValue in webSpeciesObservationProvenance.Values)
                {
                    speciesObservationProvenanceValue = new SpeciesObservationProvenanceValue()
                                                            {
                                                                Id = webSpeciesObservationProvenanceValue.Id,
                                                                SpeciesObservationCount = webSpeciesObservationProvenanceValue.SpeciesObservationCount,
                                                                Value = webSpeciesObservationProvenanceValue.Value
                                                            };
                    speciesObservationProvenance.Values.Add(speciesObservationProvenanceValue);
                }
            }

            return speciesObservationProvenance;
        }
        
        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        /// <returns>User context or null if login failed.</returns>
        public void Login(IUserContext userContext,
                          String userName,
                          String password,
                          String applicationIdentifier,
                          Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;


            loginResponse = WebServiceProxy.AnalysisService.Login(userName,
                                                               password,
                                                               applicationIdentifier,
                                                               isActivationRequired);
            if (loginResponse.IsNotNull())
            {
                SetToken(userContext, loginResponse.Token);
            }
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void Logout(IUserContext userContext)
        {

            WebServiceProxy.AnalysisService.Logout(GetClientInformation(userContext));
            SetToken(userContext, null);
        }

        /// <summary>
        /// Set AnalysisService as data source in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            AnalysisDataSource analysisDataSource;

            analysisDataSource = new AnalysisDataSource();
            CoreData.AnalysisManager.DataSource = analysisDataSource;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += new UserSOALoggedInEventHandler(analysisDataSource.Login);
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += new UserSOALoggedOutEventHandler(analysisDataSource.Logout);

        }

        /// <summary>
        /// Convert a list of WebTaxon instances to a TaxonList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxa">List of WebTaxon instances.</param>
        /// <returns>Taxa.</returns>
        private TaxonList GetTaxa(IUserContext userContext,
                                  List<WebTaxon> webTaxa)
        {
            TaxonList taxa;

            taxa = new TaxonList();
            if (webTaxa.IsNotEmpty())
            {
                foreach (WebTaxon webTaxon in webTaxa)
                {
                    if (Configuration.Debug && webTaxon.IsNull())
                    {
                        // This may happend in test since species fact
                        // data base has been updated with latest from
                        // production but taxon data base has
                        // not been updated.
                        continue;
                    }

                    taxa.Add(GetTaxon(userContext, webTaxon));
                }
            }
            return taxa;
        }

        /// <summary>
        /// Convert a list of WebTaxonSpeciesObservationCount instances to a TaxonSpeciesObservationCountList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxaSpeciesObservationCount">List of WebTaxonSpeciesObservationCount instances.</param>
        /// <returns>Taxa.</returns>
        private TaxonSpeciesObservationCountList GetTaxaSpeciesObservationCount(IUserContext userContext,
                                  List<WebTaxonSpeciesObservationCount> webTaxaSpeciesObservationCount)
        {
            TaxonSpeciesObservationCountList taxaSpeciesObservationCount;

            taxaSpeciesObservationCount = new TaxonSpeciesObservationCountList();
            if (webTaxaSpeciesObservationCount.IsNotEmpty())
            {
                foreach (WebTaxonSpeciesObservationCount webTaxonSpeciesObservationCount in webTaxaSpeciesObservationCount)
                {
                    taxaSpeciesObservationCount.Add(GetTaxonSpeciesObservationCount(userContext, webTaxonSpeciesObservationCount));
                }
            }
            return taxaSpeciesObservationCount;
        }

        /// <summary>
        /// Convert a WebTaxon instance to an ITaxon instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxon">An WebTaxon object.</param>
        /// <returns>An ITaxon object.</returns>
        private ITaxon GetTaxon(IUserContext userContext,
                                WebTaxon webTaxon)
        {
            ITaxon taxon;

            taxon = new Taxon();
            UpdateTaxon(userContext, taxon, webTaxon);
            return taxon;
        }

        /// <summary>
        /// Convert a WebTaxonSpeciesObservationCount instance to an ITaxonSpeciesObservationCount instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonSpeciesObservationCount">A WebTaxonSpeciesObservationCount object.</param>
        /// <returns>An ITaxon object.</returns>
        private ITaxonSpeciesObservationCount GetTaxonSpeciesObservationCount(IUserContext userContext,
                                WebTaxonSpeciesObservationCount webTaxonSpeciesObservationCount)
        {
            ITaxonSpeciesObservationCount taxonSpeciesObservationCount;

            taxonSpeciesObservationCount = new TaxonSpeciesObservationCount();
            UpdateTaxonSpeciesObservationCount(userContext, taxonSpeciesObservationCount, webTaxonSpeciesObservationCount);
            return taxonSpeciesObservationCount;
        }

        /// <summary>
        /// Update taxon object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="webTaxon">Web taxon.</param>
        private void UpdateTaxon(IUserContext userContext,
                                 ITaxon taxon,
                                 WebTaxon webTaxon)
        {
            taxon.AlertStatus = CoreData.TaxonManager.GetTaxonAlertStatus(userContext, webTaxon.AlertStatusId);
            taxon.Author = webTaxon.Author;
            taxon.Category = CoreData.TaxonManager.GetTaxonCategory(userContext, webTaxon.CategoryId);
            taxon.ChangeStatus = CoreData.TaxonManager.GetTaxonChangeStatus(userContext, webTaxon.ChangeStatusId);
            taxon.CommonName = webTaxon.CommonName;
            taxon.CreatedBy = webTaxon.CreatedBy;
            taxon.CreatedDate = webTaxon.CreatedDate;
            taxon.DataContext = GetDataContext(userContext);
            taxon.Guid = webTaxon.Guid;
            taxon.Id = webTaxon.Id;
            taxon.IsInRevision = webTaxon.IsInRevision;
            taxon.IsPublished = webTaxon.IsPublished;
            taxon.IsValid = webTaxon.IsValid;
            taxon.ModifiedBy = webTaxon.ModifiedBy;
            taxon.ModifiedByPerson =GetModifiedByPerson(webTaxon); //taxon.GetModifiedByPersonFullname(userContext);
            taxon.ModifiedDate = webTaxon.ModifiedDate;
            taxon.PartOfConceptDefinition = webTaxon.PartOfConceptDefinition;
            taxon.ScientificName = webTaxon.ScientificName;
            taxon.SortOrder = webTaxon.SortOrder;
            taxon.ValidFromDate = webTaxon.ValidFromDate;
            taxon.ValidToDate = webTaxon.ValidToDate;
        }

        /// <summary>
        /// Update taxon with observation count object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonSpeciesObservationCount">Taxon.</param>
        /// <param name="webTaxonSpeciesObservationCount">Web taxon.</param>
        private void UpdateTaxonSpeciesObservationCount(IUserContext userContext,
                                 ITaxonSpeciesObservationCount taxonSpeciesObservationCount,
                                 WebTaxonSpeciesObservationCount webTaxonSpeciesObservationCount)
        {
            taxonSpeciesObservationCount.AlertStatus = CoreData.TaxonManager.GetTaxonAlertStatus(userContext, webTaxonSpeciesObservationCount.Taxon.AlertStatusId);
            taxonSpeciesObservationCount.Author = webTaxonSpeciesObservationCount.Taxon.Author;
            taxonSpeciesObservationCount.Category = CoreData.TaxonManager.GetTaxonCategory(userContext, webTaxonSpeciesObservationCount.Taxon.CategoryId);
            taxonSpeciesObservationCount.ChangeStatus = CoreData.TaxonManager.GetTaxonChangeStatus(userContext, webTaxonSpeciesObservationCount.Taxon.ChangeStatusId);
            taxonSpeciesObservationCount.CommonName = webTaxonSpeciesObservationCount.Taxon.CommonName;
            taxonSpeciesObservationCount.CreatedBy = webTaxonSpeciesObservationCount.Taxon.CreatedBy;
            taxonSpeciesObservationCount.CreatedDate = webTaxonSpeciesObservationCount.Taxon.CreatedDate;
            taxonSpeciesObservationCount.DataContext = GetDataContext(userContext);
            taxonSpeciesObservationCount.Guid = webTaxonSpeciesObservationCount.Taxon.Guid;
            taxonSpeciesObservationCount.Id = webTaxonSpeciesObservationCount.Taxon.Id;
            taxonSpeciesObservationCount.IsInRevision = webTaxonSpeciesObservationCount.Taxon.IsInRevision;
            taxonSpeciesObservationCount.IsPublished = webTaxonSpeciesObservationCount.Taxon.IsPublished;
            taxonSpeciesObservationCount.IsValid = webTaxonSpeciesObservationCount.Taxon.IsValid;
            taxonSpeciesObservationCount.ModifiedBy = webTaxonSpeciesObservationCount.Taxon.ModifiedBy;
            taxonSpeciesObservationCount.ModifiedByPerson = GetModifiedByPerson(webTaxonSpeciesObservationCount.Taxon);
            taxonSpeciesObservationCount.ModifiedDate = webTaxonSpeciesObservationCount.Taxon.ModifiedDate;
            taxonSpeciesObservationCount.PartOfConceptDefinition = webTaxonSpeciesObservationCount.Taxon.PartOfConceptDefinition;
            taxonSpeciesObservationCount.ScientificName = webTaxonSpeciesObservationCount.Taxon.ScientificName;
            taxonSpeciesObservationCount.SortOrder = webTaxonSpeciesObservationCount.Taxon.SortOrder;
            taxonSpeciesObservationCount.ValidFromDate = webTaxonSpeciesObservationCount.Taxon.ValidFromDate;
            taxonSpeciesObservationCount.ValidToDate = webTaxonSpeciesObservationCount.Taxon.ValidToDate;
            taxonSpeciesObservationCount.SpeciesObservationCount = webTaxonSpeciesObservationCount.SpeciesObservationCount;
        }

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        public virtual TaxonList GetHosts(IUserContext userContext,
                                          ISpeciesFactSearchCriteria searchCriteria)
        {
            List<WebTaxon> hosts;
            WebSpeciesFactSearchCriteria webSpeciesFactSearchCriteria;

            webSpeciesFactSearchCriteria = GetSpeciesFactSearchCriteria(searchCriteria);
            hosts = WebServiceProxy.AnalysisService.GetHostsBySpeciesFactSearchCriteria(GetClientInformation(userContext),
                                                                                        webSpeciesFactSearchCriteria);

            return GetTaxa(userContext, hosts);
        }

        /// <summary>
        /// Get name of person that made the last
        /// modification this piece of data.
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <returns>Name of person that made the last modification this piece of data.</returns>
        private String GetModifiedByPerson(WebData webData)
        {
            if (webData.DataFields.IsDataFieldSpecified(Settings.Default.WebDataModifiedByPerson))
            {
                return webData.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
            }
            else
            {
                // This will happen when ModifiedByPerson has been
                // replaced by ModifiedBy in TaxonService.
                return null;
            }
        }

        /// <summary>
        /// Get a time serie with species observation counts.
        /// It retrieves a shortlist of counts from the Analysis Service and complements it with zero counts for missing time steps.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="periodicity">Enum determining the time step length and periodicity.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criterias.</param>
        /// <returns>A list of time step specific specis observation counts.</returns>
        public TimeStepSpeciesObservationCountList GetTimeSpeciesObservationCounts(IUserContext userContext,
                                                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                                                Periodicity periodicity,
                                                                                ICoordinateSystem coordinateSystem)
        {
            TimeStepSpeciesObservationCountList list;
            List<WebTimeStepSpeciesObservationCount> webList;
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;

            //Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            //Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            // Get list from web service
            webList = WebServiceProxy.AnalysisService.GetTimeSpeciesObservationCounts(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, periodicity, webCoordinateSystem);

            // Convert to onion data from web data.
            list = new TimeStepSpeciesObservationCountList();
            foreach (WebTimeStepSpeciesObservationCount item in webList)
            {
                list.Add(getTimeStepSpecificSpeciesObservationCount(item));
            }
            return list;          
        }

        /// <summary>
        /// Converts a WebTimeStepSpeciesObservationCount object to a TimeStepSpeciesObservationCount.
        /// </summary>
        /// <param name="webTimeStepSpeciesObservationCount">The WebTimeStepSpeciesObservationCount object.</param>
        /// <returns>A TimeStepSpeciesObservationCount object.</returns>
        private ITimeStepSpeciesObservationCount getTimeStepSpecificSpeciesObservationCount(WebTimeStepSpeciesObservationCount webTimeStepSpeciesObservationCount)
        {
            ITimeStepSpeciesObservationCount timeStepSpeciesObservationCount = new TimeStepSpeciesObservationCount();
            timeStepSpeciesObservationCount.Id = webTimeStepSpeciesObservationCount.Id;
            if (webTimeStepSpeciesObservationCount.IsDateSpecified)
            {
                timeStepSpeciesObservationCount.Date = webTimeStepSpeciesObservationCount.Date;
            }
            timeStepSpeciesObservationCount.Name = webTimeStepSpeciesObservationCount.Name;
            timeStepSpeciesObservationCount.Periodicity = webTimeStepSpeciesObservationCount.Periodicity;
            timeStepSpeciesObservationCount.ObservationCount = webTimeStepSpeciesObservationCount.Count;
            return timeStepSpeciesObservationCount;
        }

        /// <summary>
        /// Get Bounding Polygon from Bounding Box;
        /// </summary>
        /// <param name="boundingBoxToBeConverted"></param>
        private static IPolygon GetBoundingPolygon(BoundingBox boundingBoxToBeConverted)
        {
            IPolygon gridCellBoundingPolygon;
            gridCellBoundingPolygon = new Polygon();
            
            gridCellBoundingPolygon.LinearRings[0] = new LinearRing();
            gridCellBoundingPolygon.LinearRings[0].Points[0] = new Point();
            gridCellBoundingPolygon.LinearRings[0].Points[1] = new Point();
            gridCellBoundingPolygon.LinearRings[0].Points[2] = new Point();
            gridCellBoundingPolygon.LinearRings[0].Points[3] = new Point();
            gridCellBoundingPolygon.LinearRings[0].Points[4] = new Point();
            //Create the linear ring that is the "bounding polygon".
            gridCellBoundingPolygon.LinearRings[0].Points[0].X = boundingBoxToBeConverted.Min.X;
            gridCellBoundingPolygon.LinearRings[0].Points[0].Y = boundingBoxToBeConverted.Min.Y;
            gridCellBoundingPolygon.LinearRings[0].Points[1].X = boundingBoxToBeConverted.Min.X;
            gridCellBoundingPolygon.LinearRings[0].Points[1].Y = boundingBoxToBeConverted.Max.Y;
            gridCellBoundingPolygon.LinearRings[0].Points[2].X = boundingBoxToBeConverted.Max.X;
            gridCellBoundingPolygon.LinearRings[0].Points[2].Y = boundingBoxToBeConverted.Max.Y;
            gridCellBoundingPolygon.LinearRings[0].Points[3].X = boundingBoxToBeConverted.Max.X;
            gridCellBoundingPolygon.LinearRings[0].Points[3].Y = boundingBoxToBeConverted.Min.Y;
            gridCellBoundingPolygon.LinearRings[0].Points[4].X = boundingBoxToBeConverted.Min.X;
            gridCellBoundingPolygon.LinearRings[0].Points[4].Y = boundingBoxToBeConverted.Min.Y;

            return gridCellBoundingPolygon;


        }
    }
}
