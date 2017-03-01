using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.SwedenExtent;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid
{
    public class TaxonGridCalculator : ResultCalculatorBase
    {
        private const int MAX_NUMBER_OF_GRID_CELLS = 200000;

        public TaxonGridCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }
     
        private bool IsSearchSettingsDefault()
        {
            return MySettings.IsSettingsDefault();
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            return new QueryComplexityEstimate();
        }          

        public TaxonGridResult GetTaxonGridResultFromCacheIfAvailableOrElseCalculate()
        {
            TaxonGridResult result;
            var calculatedDataItemType = CalculatedDataItemType.GridCellTaxa;

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
            result = CalculateTaxonGrid();
            AddResultToCache(calculatedDataItemType, result);

            return result;
        }

        public TaxonGridResult CalculateTaxonGrid()
        {
            List<int> taxonIds = null;

            if (MySettings.Filter.Taxa.IsActive)
            {
                taxonIds = MySettings.Filter.Taxa.TaxonIds.ToList();
            }

            IList<IGridCellSpeciesCount> result = CalculateTaxonGrid(taxonIds, null);
            return TaxonGridResult.Create(result);
        }

        public TaxonGridResult CalculateTaxonGrid(MySettings.MySettings mySettings)
        {
            IList<IGridCellSpeciesCount> result = CalculateTaxonGrid(mySettings.Filter.Taxa.TaxonIds.ToList(), null);
            return TaxonGridResult.Create(result);
        }

        /// <summary>
        /// Calculates species richness grid result.
        /// </summary>
        /// <param name="taxonid">The taxon id to be use in the calculation. If null, then the MySettings Taxon filter will be used.</param>
        /// <param name="gridCoordinateSystem">The coordinate system the calculation will use.</param>
        /// <returns>Returns a list of grid cell results.</returns>
        public IList<IGridCellSpeciesCount> CalculateTaxonGrid(int? taxonid, GridCoordinateSystem? gridCoordinateSystem)
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

            return CalculateTaxonGrid(taxonIds, gridCoordinateSystem);
        }

        public IList<IGridCellSpeciesCount> CalculateTaxonGrid(List<int> taxonIds, GridCoordinateSystem? gridCoordinateSystem)
        {
            var speciesObservationSearchCriteriaManager = new SpeciesObservationSearchCriteriaManager(UserContext);
            SpeciesObservationSearchCriteria searchCriteria = speciesObservationSearchCriteriaManager.CreateSearchCriteria(MySettings);
            searchCriteria.TaxonIds = taxonIds;
            ICoordinateSystem displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
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

            IList<IGridCellSpeciesCount> gridCellTaxa = CoreData.AnalysisManager.GetGridSpeciesCounts(UserContext, searchCriteria, gridSpecification, displayCoordinateSystem);

            if (MySettings.Calculation.GridStatistics.GenerateAllGridCells)
            {
                gridCellTaxa = AddEmptyGridCells(gridCellTaxa, gridSpecification, displayCoordinateSystem);
                gridCellTaxa = RemoveGridCellsOutsideBounds(gridCellTaxa, gridSpecification);
            }

            gridCellTaxa = gridCellTaxa.OrderBy(x => x.Identifier).ToList();
            return gridCellTaxa;
        }

        /// <summary>
        /// Removes the grid cells outside grid bounds.
        /// </summary>
        /// <param name="gridCellObservations">The grid cell observations.</param>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <returns>A list with all <paramref name="gridCellObservations"/> except the ones outside the grid bounds.</returns>
        private List<IGridCellSpeciesCount> RemoveGridCellsOutsideBounds(IList<IGridCellSpeciesCount> gridCellObservations, GridSpecification gridSpecification)
        {
            List<IGridCellSpeciesCount> result = new List<IGridCellSpeciesCount>();

            IBoundingBox boundingBox = gridSpecification.BoundingBox;
            int cellSize = gridSpecification.GridCellSize;
            double xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;

            foreach (IGridCellSpeciesCount gridCell in gridCellObservations)
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

        private IList<IGridCellSpeciesCount> AddEmptyGridCells(IList<IGridCellSpeciesCount> gridCellSpeciesCounts, GridSpecification gridSpecification, ICoordinateSystem displayCoordinateSystem)
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
            List<IGridCellBase> dataGridCells = gridCellSpeciesCounts.Cast<IGridCellBase>().ToList();
            List<IGridCellBase> missingGridCells = GisTools.GridCellManager.GetMissingGridCells(allGridCells.Cast<IGridCellBase>().ToList(), dataGridCells);            

            foreach (IGridCellBase missingGridCell in missingGridCells)
            {
                GridCellSpeciesCount gridCellSpeciesCount = new GridCellSpeciesCount();
                gridCellSpeciesCount.ObservationCount = 0;
                gridCellSpeciesCount.SpeciesCount = 0;
                missingGridCell.CopyPropertiesTo(gridCellSpeciesCount);
                gridCellSpeciesCounts.Add(gridCellSpeciesCount);
            }
            
            return gridCellSpeciesCounts;
        }

        /// <summary>
        /// Calculates a species richness grid and creates a feature collection.
        /// </summary>
        /// <returns>A feature collection containing all grid cells.</returns>
        public FeatureCollection GetTaxonGridAsFeatureCollection()
        {
            TaxonGridResult taxonGridResult = GetTaxonGridResultFromCacheIfAvailableOrElseCalculate();
            
            List<Feature> features = new List<Feature>();
            foreach (TaxonGridCellResult gridCell in taxonGridResult.Cells)
            {
                Dictionary<string, object> properties = new Dictionary<string, object>();
                properties.Add("Id", gridCell.Identifier);
                properties.Add("SpeciesCount", gridCell.SpeciesCount);
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

        public string GetTaxonGridAsGeoJson()
        {
            var featureCollection = GetTaxonGridAsFeatureCollection();
            return JsonConvert.SerializeObject(featureCollection, JsonHelper.GetDefaultJsonSerializerSettings());
        }
    }
}
