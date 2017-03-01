using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// This class is a Grid Cell result
    /// </summary>
    [DataContract]
    public class WfsStatisticsGridCellResult
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
        public double[][] BoundingBox { get; set; }
        
        [DataMember]
        public Int64 FeatureCount { get; set; }

        [DataMember]
        public Double FeatureLength { get; set; }

        [DataMember]
        public Double FeatureArea { get; set; }        

        /// <summary>
        /// Creates a new grid cell result.
        /// </summary>
        /// <param name="gridCell">The grid cell object returned from web service.</param>        
        public static WfsStatisticsGridCellResult Create(IGridCellFeatureStatistics gridCell)
        {
            var model = new WfsStatisticsGridCellResult();
            //model.WestSouthCoordinate = new double[] { gridCell.GridCellCentreCoordinate.X - gridCell.GridCellSize, gridCell.GridCellCentreCoordinate.Y - gridCell.GridCellSize };
            //model.EastNorthCoordinate = new double[] { gridCell.GridCellCentreCoordinate.X + gridCell.GridCellSize, gridCell.GridCellCentreCoordinate.Y + gridCell.GridCellSize };

            //model.WestSouthCoordinate = new double[] { gridCell.GridCellBoundingBox.LinearRings[0].Points[0].X, gridCell.GridCellBoundingBox.LinearRings[0].Points[0].Y };
            //model.EastNorthCoordinate = new double[] { gridCell.GridCellBoundingBox.LinearRings[0].Points[2].X, gridCell.GridCellBoundingBox.LinearRings[0].Points[2].Y };                        

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
            model.FeatureCount = gridCell.FeatureCount;
            model.FeatureArea = gridCell.FeatureArea;
            model.FeatureLength = gridCell.FeatureLength;
            return model;
        }
    }
}
