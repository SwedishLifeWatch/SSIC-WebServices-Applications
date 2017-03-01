using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about features in a grid cell.
    /// </summary>
    public class GridCellFeatureStatistics : GridCellBase, IGridCellFeatureStatistics
    {

        /// <summary>
        ///  X-coordinate for grid centre
        /// </summary>
        public decimal Northing { get; set; }

        /// <summary>
        ///  Y-coordinate for grid centre
        /// </summary>
        public decimal Easting { get; set; }

        /// <summary>
        ///  Number of features in grid cell.
        /// </summary>
        public long FeatureCount { get; set; }

        /// <summary>
        ///  Length of line feature.
        /// </summary>
        public double FeatureLength { get; set; }

        /// <summary>
        ///  Area of polygon feature.
        /// </summary>
        public double FeatureArea { get; set; }        

        /// <summary>
        ///  Type of feature.
        /// </summary>
        public FeatureType FeatureType { get; set; }

    }
}
