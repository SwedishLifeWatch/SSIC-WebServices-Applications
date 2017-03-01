using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handels information about features in a grid cell.
    /// </summary>
    public interface IGridCellFeatureStatistics : IGridCellBase
    {
        /// <summary>
        ///  X-coordinate for grid centre
        /// </summary>
        decimal Northing { get; set; }

        /// <summary>
        ///  Y-coordinate for grid centre
        /// </summary>
        decimal Easting { get; set; }

        /// <summary>
        ///  Number of features in grid cell.
        /// </summary>
        long FeatureCount { get; set; }

        /// <summary>
        ///  Length of line feature.
        /// </summary>
        double FeatureLength { get; set; }

        /// <summary>
        ///  Area of polygon feature.
        /// </summary>
        double FeatureArea { get; set; }        

        /// <summary>
        ///  Type of feature.
        /// </summary>
        FeatureType FeatureType { get; set; }
    }
}