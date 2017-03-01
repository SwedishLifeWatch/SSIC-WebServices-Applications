using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.SwedenExtent;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Newtonsoft.Json.Linq;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid
{
    /// <summary>
    /// This class is used for calculating combined grid statistics.
    /// </summary>
    public class CombinedGridStatisticsCalculator : ResultCalculatorBase
    {
        private const int MAX_NUMBER_OF_GRID_CELLS = 200000;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedGridStatisticsCalculator"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">My settings.</param>
        public CombinedGridStatisticsCalculator(IUserContext userContext, MySettings.MySettings mySettings) 
            : base(userContext, mySettings)
        {
        }        

        /// <summary>
        /// Calculates the combined grid result.
        /// </summary>
        /// <param name="calculationCoordinateSystemId">The calculation coordinate system id.</param>
        /// <param name="gridCellSize">Size of the grid cell.</param>
        /// <param name="wfsLayerId">The WFS layer id.</param>
        /// <returns>Combined grid cell statistics.</returns>
        public CombinedGridStatisticsResult CalculateCombinedGridResult(int calculationCoordinateSystemId, int gridCellSize, int wfsLayerId)
        {
            GridSpecification gridSpecification = new GridSpecification();
            CoordinateSystem displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            gridSpecification.GridCoordinateSystem = (GridCoordinateSystem)calculationCoordinateSystemId;
            gridSpecification.GridCellSize = gridCellSize;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            if (MySettings.Filter.Spatial.IsActive)
            {
                // Create bounding box from spatial filter
                ObservableCollection<DataPolygon> polygons = MySettings.Filter.Spatial.Polygons;
                if (polygons.Count > 0)
                {
                    BoundingBox boundingBox = polygons.GetBoundingBox();
                    CoordinateSystem toCoordinateSystem = CoordinateSystemHelper.GetCoordinateSystemFromGridCoordinateSystem(gridSpecification.GridCoordinateSystem);
                    IPolygon convertedBoundingBoxPolygon = GisTools.CoordinateConversionManager.GetConvertedBoundingBox(boundingBox, MySettings.Filter.Spatial.PolygonsCoordinateSystem, toCoordinateSystem);
                    BoundingBox convertedBoundingBox = convertedBoundingBoxPolygon.GetBoundingBox();
                    gridSpecification.BoundingBox = convertedBoundingBox;
                }
            }
            
            WfsLayerSetting wfsLayer = SessionHandler.MySettings.DataProvider.MapLayers.WfsLayers.FirstOrDefault(l => l.Id == wfsLayerId);
            string featuresUrl = "";
            string featureCollectionJson = null;
            if (wfsLayer.IsFile)
            {
                featureCollectionJson = JObject.FromObject(MySettingsManager.GetMapDataFeatureCollection(UserContext, wfsLayer.GeometryName, gridSpecification.GridCoordinateSystem.GetCoordinateSystemId())).ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(wfsLayer.Filter))
                {
                    featuresUrl = string.Format("{0}?service=wfs&version=1.1.0&request=GetFeature&typeName={1}",
                        wfsLayer.ServerUrl, wfsLayer.TypeName);
                }
                else
                {
                    featuresUrl =
                        string.Format("{0}?service=wfs&version=1.1.0&request=GetFeature&typeName={1}&filter={2}",
                            wfsLayer.ServerUrl, wfsLayer.TypeName, wfsLayer.Filter);
                }
            }
            var speciesObservationSearchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = speciesObservationSearchCriteriaManager.CreateSearchCriteria(MySettings);
            IList<IGridCellCombinedStatistics> gridCells = CoreData.AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(UserContext, gridSpecification, searchCriteria, null, featuresUrl, featureCollectionJson, displayCoordinateSystem);
            if (MySettings.Calculation.GridStatistics.GenerateAllGridCells)
            {
                gridCells = AddEmptyGridCells(gridCells, gridSpecification, displayCoordinateSystem);
                gridCells = RemoveGridCellsOutsideBounds(gridCells, gridSpecification);
            }

            gridCells = gridCells.OrderBy(x => x.Identifier).ToList();
            return CombinedGridStatisticsResult.Create(
                (CoordinateSystemId)calculationCoordinateSystemId, displayCoordinateSystem.Id, gridCellSize, gridCells);
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            return new QueryComplexityEstimate();
        }

        /// <summary>
        /// Removes the grid cells outside grid bounds.
        /// </summary>
        /// <param name="gridCellObservations">The grid cell observations.</param>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <returns>A list with all <paramref name="gridCellObservations"/> except the ones outside the grid bounds.</returns>
        private List<IGridCellCombinedStatistics> RemoveGridCellsOutsideBounds(IList<IGridCellCombinedStatistics> gridCellObservations, GridSpecification gridSpecification)
        {
            List<IGridCellCombinedStatistics> result = new List<IGridCellCombinedStatistics>();

            IBoundingBox boundingBox = gridSpecification.BoundingBox;
            int cellSize = gridSpecification.GridCellSize;
            double xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;

            foreach (IGridCellCombinedStatistics gridCell in gridCellObservations)
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

        /// <summary>
        /// Adds empty grid cells if they doesn't exist (inside Sweden bounds).
        /// </summary>
        /// <param name="gridCells">The grid cells.</param>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <param name="displayCoordinateSystem">The display coordinate system.</param>
        /// <returns>The grid cells with missing grid cells appended.</returns>
        private IList<IGridCellCombinedStatistics> AddEmptyGridCells(IList<IGridCellCombinedStatistics> gridCells, GridSpecification gridSpecification, ICoordinateSystem displayCoordinateSystem)
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
            List<IGridCellBase> dataGridCells = gridCells.Cast<IGridCellBase>().ToList();
            List<IGridCellBase> missingGridCells = GisTools.GridCellManager.GetMissingGridCells(allGridCells.Cast<IGridCellBase>().ToList(), dataGridCells);

            foreach (IGridCellBase missingGridCell in missingGridCells)
            {
                GridCellCombinedStatistics gridCellSpeciesObservationCount = new GridCellCombinedStatistics();
                gridCellSpeciesObservationCount.SpeciesCount = new GridCellSpeciesCount();
                gridCellSpeciesObservationCount.FeatureStatistics = new GridCellFeatureStatistics();                
                missingGridCell.CopyPropertiesTo(gridCellSpeciesObservationCount);
                gridCells.Add(gridCellSpeciesObservationCount);
            }

            return gridCells;
        }
    }
}