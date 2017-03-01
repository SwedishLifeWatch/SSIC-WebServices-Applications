using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.Grid;
using ArtDatabanken.GIS.SwedenExtent;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid
{
    public class SpeciesObservationGridCalculator : ResultCalculatorBase
    {
        private const int MAX_NUMBER_OF_GRID_CELLS = 200000;                

        public PresentationMapSetting MapSettings
        {
            get { return MySettings.Presentation.Map; }
        }

        public TaxaSetting TaxaFilterSettings
        {
            get { return MySettings.Filter.Taxa; }
        }

        public SpeciesObservationGridCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public bool IsSearchSettingsDefault()
        {
            return MySettings.IsSettingsDefault();
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            return new QueryComplexityEstimate();
        }

        public SpeciesObservationGridResult GetSpeciesObservationGridResultFromCacheIfAvailableOrElseCalculate()
        {
            SpeciesObservationGridResult result;
            var calculatedDataItemType = CalculatedDataItemType.GridCellObservations;

            // Try get precalculated data.
            if (IsSearchSettingsDefault())
            {
                if (TryGetPrecalculatedResult(calculatedDataItemType, out result))
                {
                    return result;
                }
            }

            // Try get cached data.
            if (TryGetCachedCalculatedResult(calculatedDataItemType, out result))
            {
                return result;
            }

            // Calculate data.
            result = CalculateSpeciesObservationGridResult();
            AddResultToCache(calculatedDataItemType, result);

            return result;
        }
        
        public SpeciesObservationGridResult CalculateSpeciesObservationGridResult()
        {        
            return SpeciesObservationGridResult.Create(CalculateSpeciesObservationGrid());
        }

        public SpeciesObservationGridResult CalculateSpeciesObservationGridResult(MySettings.MySettings mySettings)
        {
            var result = CalculateSpeciesObservationGrid(mySettings.Filter.Taxa.TaxonIds.ToList(), null);
            return SpeciesObservationGridResult.Create(result);
        }

        public IList<IGridCellSpeciesObservationCount> CalculateSpeciesObservationGrid()
        {
            List<int> taxonIds = null;

            if (MySettings.Filter.Taxa.IsActive)
            {
                taxonIds = MySettings.Filter.Taxa.TaxonIds.ToList();
            }

            return CalculateSpeciesObservationGrid(taxonIds, null);
        }

        /// <summary>
        /// Calculates species observation grid result.
        /// </summary>
        /// <param name="taxonid">The taxon id to be use in the calculation. If null, then the MySettings Taxon filter will be used.</param>
        /// <param name="gridCoordinateSystem">The coordinate system the calculation will use.</param>
        /// <returns>Returns a list of grid cell results.</returns>
        public IList<IGridCellSpeciesObservationCount> CalculateSpeciesObservationGrid(int? taxonid, GridCoordinateSystem? gridCoordinateSystem)
        {
            List<int> taxonIds;
            if (!taxonid.HasValue)
            {
                taxonIds = MySettings.Filter.Taxa.TaxonIds.ToList();
            }
            else
            {
                taxonIds = new List<int> { taxonid.Value };
            }

            return CalculateSpeciesObservationGrid(taxonIds, gridCoordinateSystem);
        }

        /// <summary>
        /// Calculates species observation count grid for each selected taxon.
        /// </summary>
        /// <returns></returns>
        public TaxonSpecificGridSpeciesObservationCountResult CalculateMultipleSpeciesObservationGrid()
        {
            return CalculateMultipleSpeciesObservationGrid(MySettings.Filter.Taxa.TaxonIds.ToList(), null);
        }

        /// <summary>
        /// Calculates species observation count grid for each taxon in <paramref name="taxonIds"/>.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>Species observation count for each taxon grouped by grid cells.</returns>
        public TaxonSpecificGridSpeciesObservationCountResult CalculateMultipleSpeciesObservationGrid(
            List<int> taxonIds,
            GridCoordinateSystem? gridCoordinateSystem)
        {
            Dictionary<IGridCellBase, Dictionary<int, IGridCellSpeciesObservationCount>> totalResult;
            IList<IGridCellSpeciesObservationCount> taxonResult;
            List<int> taxonIdList = new List<int>(taxonIds);
            if (taxonIdList.IsEmpty())
            {
                taxonIdList.Add(0);
            }
                            
            totalResult = new Dictionary<IGridCellBase, Dictionary<int, IGridCellSpeciesObservationCount>>(new GridCellBaseCenterPointComparer());
            foreach (int taxonId in taxonIdList)
            {
                taxonResult = CalculateSpeciesObservationGrid(taxonId, gridCoordinateSystem);
                AddTaxonResult(taxonId, taxonResult, totalResult);
            }

            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, taxonIdList);
            List<TaxonViewModel> taxaList = taxonList.GetGenericList().ToTaxonViewModelList();

            TaxonSpecificGridSpeciesObservationCountResult result = new TaxonSpecificGridSpeciesObservationCountResult()
            {                
                Taxa = taxaList,
                GridCells = totalResult
            };
            return result;
        }

        /// <summary>
        /// Adds a grid result for a specific taxon to a total result.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="taxonResult">The taxon result.</param>
        /// <param name="totalResult">The total result.</param>
        private void AddTaxonResult(
            int taxonId,
            IList<IGridCellSpeciesObservationCount> taxonResult,
            Dictionary<IGridCellBase, Dictionary<int, IGridCellSpeciesObservationCount>> totalResult)
        {
            foreach (IGridCellSpeciesObservationCount gridCell in taxonResult)
            {
                if (!totalResult.ContainsKey(gridCell))
                {
                    var newGridCell = new GridCellBase();
                    gridCell.CopyPropertiesTo(newGridCell);
                    totalResult.Add(newGridCell, new Dictionary<int, IGridCellSpeciesObservationCount>());
                }

                totalResult[gridCell].Add(taxonId, gridCell);
            }
        }

        /// <summary>
        /// Calculates the species observation grid.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>List of grid cells data.</returns>
        public IList<IGridCellSpeciesObservationCount> CalculateSpeciesObservationGrid(List<int> taxonIds, GridCoordinateSystem? gridCoordinateSystem)
        {
            var speciesObservationSearchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = speciesObservationSearchCriteriaManager.CreateSearchCriteria(MySettings);
            searchCriteria.TaxonIds = taxonIds;

            CoordinateSystem displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            //CoordinateSystem displayCoordinateSystem = new CoordinateSystem(MapSettings.CoordinateSystemId); // todo - såhär borde det se ut           
            GridStatisticsSetting gridStatisticsSetting = MySettings.Calculation.GridStatistics;
            GridSpecification gridSpecification = new GridSpecification();
            if (gridCoordinateSystem.HasValue)
            {
                gridSpecification.GridCoordinateSystem = gridCoordinateSystem.Value;
            }
            else
            {
                if (gridStatisticsSetting.CoordinateSystemId.HasValue)
                {
                    gridSpecification.GridCoordinateSystem = (GridCoordinateSystem)gridStatisticsSetting.CoordinateSystemId;
                }
                else
                {
                    gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
                }
            }

            if (gridStatisticsSetting.GridSize.HasValue)
            {
                gridSpecification.GridCellSize = gridStatisticsSetting.GridSize.Value;
                gridSpecification.IsGridCellSizeSpecified = true;
            }

            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            // todo - implement check that the number of possible grid cells aren't too large.
            // todo - perhaps this check should be in the service
            ////long numberOfGridCellsThatWillBeGenerated = GisTools.GridCellManager.CalculateNumberOfGridCells(gridSpecification, searchCriteria.Polygons, searchCriteria.RegionGuids);
            ////if (numberOfGridCellsThatWillBeGenerated > MAX_NUMBER_OF_GRID_CELLS)
            ////{
            ////    throw new ArgumentException(string.Format("The cell size is too small, which would have resulted in possibly {0} grid cells. The limit lies at {1}.", numberOfGridCellsThatWillBeGenerated, MAX_NUMBER_OF_GRID_CELLS));
            ////}

            var gridCellObservations = CoreData.AnalysisManager.GetGridSpeciesObservationCounts(UserContext, searchCriteria, gridSpecification, displayCoordinateSystem);

            if (MySettings.Calculation.GridStatistics.GenerateAllGridCells)
            {
                gridCellObservations = AddEmptyGridCells(gridCellObservations, gridSpecification, displayCoordinateSystem);
                gridCellObservations = RemoveGridCellsOutsideBounds(gridCellObservations, gridSpecification);
            }

            gridCellObservations = gridCellObservations.OrderBy(x => x.Identifier).ToList();
            return gridCellObservations;
        }

        public string GetSpeciesObservationAOOEOOAsGeoJson(int? alphaValue = null, bool useCenterPoint = false)
        {
            
            var gridResult = GetSpeciesObservationGridResultFromCacheIfAvailableOrElseCalculate();
            if (gridResult == null)
            {
                return null;
            }

            //Convert SpeciesObservationGridResult to List<IGridCellSpeciesObservationCount>
            //Todo Cache List<IGridCellSpeciesObservationCount> from service and not SpeciesObservationGridResult var gridCellsc = CalculateSpeciesObservationGrid();
            var gridCells = new List<IGridCellSpeciesObservationCount>();

            foreach (var sourceCell in gridResult.Cells)
            {
                var targetCell = new GridCellSpeciesObservationCount();
                
                if (sourceCell.BoundingBox != null)
                {
                    var linearRing = new LinearRing() { Points = new DataId32List<IPoint>() };
                    
                    foreach (var point in sourceCell.BoundingBox)
                    {
                        linearRing.Points.Add(new Data.Point(point[0], point[1]));
                    }
                    var firstPoint = sourceCell.BoundingBox[0]; //Add first point to close ring
                    linearRing.Points.Add(new Data.Point(firstPoint[0], firstPoint[1]));

                    var boundingBoxPolygon = new Data.Polygon()
                    {
                        LinearRings = new DataId32List<ILinearRing>()
                    };
                    boundingBoxPolygon.LinearRings.Add(linearRing);
                    targetCell.GridCellBoundingBox = boundingBoxPolygon;
                }

                if (sourceCell.OriginalBoundingBox != null && sourceCell.OriginalBoundingBox.Length == 4)
                {
                    targetCell.OrginalGridCellBoundingBox = new Data.BoundingBox
                    {
                        Min = new Data.Point(sourceCell.OriginalBoundingBox[0, 0], sourceCell.OriginalBoundingBox[0, 1]),
                        Max = new Data.Point(sourceCell.OriginalBoundingBox[1, 0], sourceCell.OriginalBoundingBox[1, 1])
                    };
                }
                
                if (sourceCell.CentreCoordinate != null)
                {
                    targetCell.GridCellCentreCoordinate = new Data.Point(sourceCell.CentreCoordinateX, sourceCell.CentreCoordinateY);
                }
                targetCell.OrginalGridCellCentreCoordinate = new Data.Point(sourceCell.OriginalCentreCoordinateX, sourceCell.OriginalCentreCoordinateY);
                targetCell.ObservationCount = sourceCell.ObservationCount;
                targetCell.GridCellSize = sourceCell.GridCellSize;
                targetCell.CoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
                targetCell.GridCoordinateSystem = sourceCell.Srid.ToGridCoordinateSystem(); 

                gridCells.Add(targetCell);
            }
          
            return CoreData.AnalysisManager.GetSpeciesObservationAOOEOOAsGeoJson(UserContext, gridCells, alphaValue ?? 0, useCenterPoint);
        }

        /// <summary>
        /// Removes the grid cells outside grid bounds.
        /// </summary>
        /// <param name="gridCellObservations">The grid cell observations.</param>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <returns>A list with all <paramref name="gridCellObservations"/> except the ones outside the grid bounds.</returns>
        private List<IGridCellSpeciesObservationCount> RemoveGridCellsOutsideBounds(IList<IGridCellSpeciesObservationCount> gridCellObservations, GridSpecification gridSpecification)
        {
            List<IGridCellSpeciesObservationCount> result = new List<IGridCellSpeciesObservationCount>();

            IBoundingBox boundingBox = gridSpecification.BoundingBox;
            int cellSize = gridSpecification.GridCellSize;
            double xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;            

            foreach (IGridCellSpeciesObservationCount gridCell in gridCellObservations)
            {
                if (gridCell.OrginalGridCellBoundingBox.Min.X >= xMin &&
                    gridCell.OrginalGridCellBoundingBox.Min.Y >= yMin &&
                    gridCell.OrginalGridCellBoundingBox.Max.X <= xMax &&
                    gridCell.OrginalGridCellBoundingBox.Max.Y <= yMax)
                {
                    result.Add(gridCell);
                }                
            }

            return result;
        }

        private IList<IGridCellSpeciesObservationCount> AddEmptyGridCells(IList<IGridCellSpeciesObservationCount> gridCellObservations, GridSpecification gridSpecification, ICoordinateSystem displayCoordinateSystem)
        {                        
            long numberOfGridCellsThatWillBeGenerated = GisTools.GridCellManager.CalculateNumberOfGridCells(gridSpecification);            
            if (numberOfGridCellsThatWillBeGenerated > MAX_NUMBER_OF_GRID_CELLS)
            {
                throw new ArgumentException(string.Format("The cell size is too small, which would have resulted in the {0} grid cells. The limit lies at {1}.", numberOfGridCellsThatWillBeGenerated, MAX_NUMBER_OF_GRID_CELLS));
            }

            if (gridSpecification.BoundingBox == null)
            {
                gridSpecification.BoundingBox = SwedenExtentManager.GetSwedenExtentBoundingBox(gridSpecification.GridCoordinateSystem.ToCoordinateSystem());
            }            
            
            List<GridCellBase> allGridCells = GisTools.GridCellManager.GenerateGrid(gridSpecification, displayCoordinateSystem);
            List<IGridCellBase> dataGridCells = gridCellObservations.Cast<IGridCellBase>().ToList();
            List<IGridCellBase> missingGridCells = GisTools.GridCellManager.GetMissingGridCells(allGridCells.Cast<IGridCellBase>().ToList(), dataGridCells);                        
            
            foreach (IGridCellBase missingGridCell in missingGridCells)
            {                
                GridCellSpeciesObservationCount gridCellSpeciesObservationCount = new GridCellSpeciesObservationCount();
                gridCellSpeciesObservationCount.ObservationCount = 0;
                missingGridCell.CopyPropertiesTo(gridCellSpeciesObservationCount);
                gridCellObservations.Add(gridCellSpeciesObservationCount);
            }

            return gridCellObservations;            
        }
        
        /// <summary>
        /// Calculates a species observation grid and creates a Feature Collection.
        /// </summary>
        /// <returns>A feature collection containing all grid cells.</returns>
        public FeatureCollection GetSpeciesObservationGridAsFeatureCollection()
        {
            SpeciesObservationGridResult speciesObservationGridResult = GetSpeciesObservationGridResultFromCacheIfAvailableOrElseCalculate();
            
            List<Feature> features = new List<Feature>();
            foreach (SpeciesObservationGridCellResult gridCell in speciesObservationGridResult.Cells)
            {
                Dictionary<string, object> properties = new Dictionary<string, object>();
                properties.Add("Id", gridCell.Identifier);
                properties.Add("ObservationCount", gridCell.ObservationCount);
                properties.Add("CentreCoordinateX", gridCell.CentreCoordinateX);
                properties.Add("CentreCoordinateY", gridCell.CentreCoordinateY);
                properties.Add("OriginalCentreCoordinateX", gridCell.OriginalCentreCoordinateX);
                properties.Add("OriginalCentreCoordinateY", gridCell.OriginalCentreCoordinateY);

                List<GeographicPosition> coordinates = new List<GeographicPosition>();
                coordinates.Add(new GeographicPosition(gridCell.BoundingBox[0][0], gridCell.BoundingBox[0][1]));
                coordinates.Add(new GeographicPosition(gridCell.BoundingBox[1][0], gridCell.BoundingBox[1][1]));
                coordinates.Add(new GeographicPosition(gridCell.BoundingBox[2][0], gridCell.BoundingBox[2][1]));
                coordinates.Add(new GeographicPosition(gridCell.BoundingBox[3][0], gridCell.BoundingBox[3][1]));
                coordinates.Add(new GeographicPosition(gridCell.BoundingBox[0][0], gridCell.BoundingBox[0][1]));

                LineString lineString = new LineString(coordinates);

                List<LineString> linearRings = new List<LineString>();
                linearRings.Add(lineString);
                ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon polygon = new ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon(linearRings);

                Feature feature = new Feature(polygon, properties);
                features.Add(feature);
            }
            
            FeatureCollection featureCollection = new FeatureCollection(features);
            featureCollection.CRS = new NamedCRS(MySettings.Presentation.Map.PresentationCoordinateSystemId.EpsgCode());

            return featureCollection;
        }

        public string GetSpeciesObservationGridAsGeoJson()
        {
            var featureCollection = GetSpeciesObservationGridAsFeatureCollection();
            return JsonConvert.SerializeObject(featureCollection, JsonHelper.GetDefaultJsonSerializerSettings());
        }        

        public SpeciesObservationGridResult CalculateSpeciesObservationGridEx(double bottom, double left, double right, double top, int zoom, int? gridSize)
        {
            List<int> taxonIds = null;
            if (MySettings.Filter.Taxa.IsActive)
            {
                taxonIds = MySettings.Filter.Taxa.TaxonIds.ToList();
            }

            IList<IGridCellSpeciesObservationCount> result = CalculateSpeciesObservationGrid(taxonIds, null, bottom, left, right, top, zoom, gridSize);
            return SpeciesObservationGridResult.Create(result);
        }

        private Data.Polygon GetPolygon(double bottom, double left, double right, double top)
        {
            Data.Polygon polygon = new Data.Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            Data.LinearRing linearRing = new LinearRing();            
            BoundingBox swedenExtentBoundingBox = SwedenExtentManager.GetSwedenExtentBoundingBox(MySettings.Presentation.Map.DisplayCoordinateSystem);

            double limitBottom = bottom;
            double limitLeft = left;
            double limitRight = right;
            double limitTop = top;

            limitBottom = Math.Max(bottom, swedenExtentBoundingBox.Min.Y);
            limitTop = Math.Min(top, swedenExtentBoundingBox.Max.Y);
            limitLeft = Math.Max(left, swedenExtentBoundingBox.Min.X);
            limitRight = Math.Min(right, swedenExtentBoundingBox.Max.X);

            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Data.Point(limitLeft, limitBottom));
            linearRing.Points.Add(new Data.Point(limitLeft, limitTop));
            linearRing.Points.Add(new Data.Point(limitRight, limitTop));
            linearRing.Points.Add(new Data.Point(limitRight, limitBottom));
            linearRing.Points.Add(new Data.Point(limitLeft, limitBottom));            
            polygon.LinearRings.Add(linearRing);
            return polygon;
        }

        /// <summary>
        /// Calculates the species observation grid.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="zoom"></param>
        /// <param name="gridSize"></param>
        /// <returns>List of grid cells data.</returns>
        public IList<IGridCellSpeciesObservationCount> CalculateSpeciesObservationGrid(List<int> taxonIds, GridCoordinateSystem? gridCoordinateSystem, double bottom, double left, double right, double top, int zoom, int? gridSize)
        {
            var speciesObservationSearchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = speciesObservationSearchCriteriaManager.CreateSearchCriteria(MySettings);
            // remove spatial filter?
            searchCriteria.Polygons = null;
            searchCriteria.RegionGuids = null;
            searchCriteria.TaxonIds = taxonIds;

            CoordinateSystem displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;            
            GridStatisticsSetting gridStatisticsSetting = MySettings.Calculation.GridStatistics;
            GridSpecification gridSpecification = new GridSpecification();
            if (gridCoordinateSystem.HasValue)
            {
                gridSpecification.GridCoordinateSystem = gridCoordinateSystem.Value;
            }
            else
            {
                if (gridStatisticsSetting.CoordinateSystemId.HasValue)
                {
                    gridSpecification.GridCoordinateSystem = (GridCoordinateSystem)gridStatisticsSetting.CoordinateSystemId;
                }
                else
                {
                    gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
                }
            }

            gridSpecification.GridCellSize = gridSize.HasValue ? gridSize.Value : ZoomMappings[zoom];
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;            
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(GetPolygon(bottom, left, right, top));
            IList<IGridCellSpeciesObservationCount> gridCellObservations = CoreData.AnalysisManager.GetGridSpeciesObservationCounts(UserContext, searchCriteria, gridSpecification, displayCoordinateSystem);
            gridCellObservations = gridCellObservations.OrderBy(x => x.Identifier).ToList();
            return gridCellObservations;
        }

        private static Dictionary<int, int> _zoomMappings;

        public static Dictionary<int, int> ZoomMappings
        {
            get
            {
                if (_zoomMappings == null)
                {
                    _zoomMappings = new Dictionary<int, int>();
                    _zoomMappings.Add(1, 400000);
                    _zoomMappings.Add(2, 200000);
                    _zoomMappings.Add(3, 100000);
                    _zoomMappings.Add(4, 80000);
                    _zoomMappings.Add(5, 60000);
                    _zoomMappings.Add(6, 40000);
                    _zoomMappings.Add(7, 30000);
                    _zoomMappings.Add(8, 10000);
                    _zoomMappings.Add(9, 5000);

                    _zoomMappings.Add(10, 2500);
                    _zoomMappings.Add(11, 1000);
                    _zoomMappings.Add(12, 600);

                    _zoomMappings.Add(13, 300);
                    _zoomMappings.Add(14, 150);
                    _zoomMappings.Add(15, 75);
                    _zoomMappings.Add(16, 40);
                    _zoomMappings.Add(17, 30);
                    _zoomMappings.Add(18, 20);
                    _zoomMappings.Add(19, 10);
                    _zoomMappings.Add(20, 10);                  
                }
                return _zoomMappings;
            }
        }
    }
}