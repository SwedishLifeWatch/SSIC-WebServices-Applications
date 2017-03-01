using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    
    /// <summary>
    /// This interface contains combined statistics about a grid cell.
    /// </summary>
    public interface IGridCellCombinedStatistics : IGridCellBase
    {

        ///// <summary>
        ///// Bounding box for the grid cell in calculated
        ///// coordinate system, ie GridCoordinateSystem.
        ///// </summary>
        //BoundingBox OriginalBoundingBox { get; set; }

        /// <summary>
        /// Contains information about features in grid cells.
        /// </summary>
        IGridCellFeatureStatistics FeatureStatistics { get; set; }
        
        /// <summary>
        /// Contains information on counting number of species and species observations.
        /// </summary>
        IGridCellSpeciesCount SpeciesCount { get; set; }

    }
}
