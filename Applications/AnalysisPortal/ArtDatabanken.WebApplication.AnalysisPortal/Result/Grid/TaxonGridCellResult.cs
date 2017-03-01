using System;
using System.Globalization;
using System.Runtime.Serialization;
using ArtDatabanken.Data;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// This class is a Grid Cell result
    /// </summary>
    [DataContract]
    public class TaxonGridCellResult
    {        
        [DataMember]
        public double CentreCoordinateX { get; set; }
        
        [DataMember]
        public double CentreCoordinateY { get; set; }

        [DataMember]
        public double OriginalCentreCoordinateX { get; set; }

        [DataMember]
        public double OriginalCentreCoordinateY { get; set; }

        [DataMember]
        public double[] CentreCoordinate { get; set; }

        [DataMember]
        public double[][] BoundingBox { get; set; }

        /// <summary>
        /// Number of species observations is based on selected 
        /// species observation search criteria and grid cell specifications.
        /// </summary>        
        [DataMember]
        public Int64 ObservationCount { get; set; }

        /// <summary>
        /// Number of species  is based on selected 
        /// species observation search criteria and grid cell specifications.
        /// </summary>        
        [DataMember]
        public Int64 SpeciesCount { get; set; }

        [JsonIgnore]
        public int Srid { get; set; }

        [JsonIgnore]
        public int GridCellSize { get; set; }

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
        public static TaxonGridCellResult Create(IGridCellSpeciesCount gridCell)
        {            
            var model = new TaxonGridCellResult();
            
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
            model.ObservationCount = gridCell.ObservationCount;
            model.SpeciesCount = gridCell.SpeciesCount;
            model.GridCellSize = gridCell.GridCellSize;
            model.Srid = gridCell.GridCoordinateSystem.Srid();
            return model;
        }
    }
}
