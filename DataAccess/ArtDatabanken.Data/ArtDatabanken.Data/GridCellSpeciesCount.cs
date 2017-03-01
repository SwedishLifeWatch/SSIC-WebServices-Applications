using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information on counting number of species and observations, selected grid cell coordinate system,
    /// gridcell size, centre coodinates (X and Y) for gridcell.
    /// The species counting result i.e. counting number of species  is based on selected 
    /// species observation search criteria and gridcell size.
    /// </summary>
    public class GridCellSpeciesCount : GridCellBase, IGridCellSpeciesCount
    {
        /// <summary>
        /// Number of species observations is based on selected 
        /// species observation search criteria and grid cell specifications.
        /// </summary>
        public Int64 ObservationCount
        { get; set; }

        /// <summary>
        /// Number of species is based on selected species
        /// observation search criteria and grid cell specifications.
        /// </summary>
        public Int64 SpeciesCount { get; set; }
    }
}