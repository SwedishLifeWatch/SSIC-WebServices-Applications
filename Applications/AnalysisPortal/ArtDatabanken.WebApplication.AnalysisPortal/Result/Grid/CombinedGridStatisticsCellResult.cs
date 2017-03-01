using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ArtDatabanken.Data;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// Combined grid statistics cell result.
    /// </summary>
    public class CombinedGridStatisticsCellResult : GridCellResultBase
    {
        public Int64 FeatureCount { get; set; }
        public Double FeatureLength { get; set; }
        public Double FeatureArea { get; set; }        

        /// <summary>
        /// Number of species observations is based on selected 
        /// species observation search criteria and grid cell specifications.
        /// </summary>        
        public Int64 ObservationCount { get; set; }

        /// <summary>
        /// Number of species  is based on selected 
        /// species observation search criteria and grid cell specifications.
        /// </summary>        
        public Int64 SpeciesCount { get; set; }

        [JsonIgnore]
        public int Srid { get; set; }

        [JsonIgnore]
        public int GridCellSize { get; set; }

        /// <summary>
        /// Gets the grid cell identifier.
        /// </summary>        
        public string Identifier
        {
            get
            {
                string easting = OriginalCentreCoordinateX.ToString(CultureInfo.InvariantCulture.NumberFormat);
                string northing = OriginalCentreCoordinateY.ToString(CultureInfo.InvariantCulture.NumberFormat);
                string identifier = string.Format("SRID{0}SIZE{1}E{2}N{3}", Srid, GridCellSize, easting, northing);
                return identifier;
            }
        }

        /// <summary>
        /// Creates a new grid cell result.
        /// </summary>
        /// <param name="gridCell">The grid cell object returned from web service.</param>                
        public static CombinedGridStatisticsCellResult Create(IGridCellCombinedStatistics gridCell)
        {
            CombinedGridStatisticsCellResult model = new CombinedGridStatisticsCellResult();            
            model.BoundingBox = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                IPoint point = gridCell.GridCellBoundingBox.LinearRings[0].Points[i];
                model.BoundingBox[i] = new[] { point.X, point.Y };
            }

            model.CentreCoordinateX = gridCell.GridCellCentreCoordinate.X;
            model.CentreCoordinateY = gridCell.GridCellCentreCoordinate.Y;
            model.OriginalCentreCoordinateX = gridCell.OrginalGridCellCentreCoordinate.X;
            model.OriginalCentreCoordinateY = gridCell.OrginalGridCellCentreCoordinate.Y;

            if (gridCell.SpeciesCount != null)
            {
                model.ObservationCount = gridCell.SpeciesCount.ObservationCount;
                model.SpeciesCount = gridCell.SpeciesCount.SpeciesCount;
            }

            if (gridCell.FeatureStatistics != null)
            {
                model.FeatureCount = gridCell.FeatureStatistics.FeatureCount;
                model.FeatureArea = gridCell.FeatureStatistics.FeatureArea;
                model.FeatureLength = gridCell.FeatureStatistics.FeatureLength;
            }

            model.GridCellSize = gridCell.GridCellSize;
            model.Srid = gridCell.GridCoordinateSystem.Srid();

            return model;
        }
    }
}
