using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains combined statistics about a grid cell.
    /// </summary>
    public class GridCellCombinedStatistics : GridCellBase, IGridCellCombinedStatistics
    {
        public IGridCellFeatureStatistics FeatureStatistics { get; set; }
        public IGridCellSpeciesCount SpeciesCount { get; set; }
    }
}
