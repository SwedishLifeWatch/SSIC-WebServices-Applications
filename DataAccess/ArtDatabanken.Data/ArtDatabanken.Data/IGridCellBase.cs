using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information of selected grid cell coordinate system,
    /// gridcell size, centre coodinates (X and Y) for gridcell.
    /// The result i.e. counting/calculation is based on selected 
    /// species observation search criteria and gridcell size.
    /// </summary>
    public interface IGridCellBase
    {
        /// <summary>
        /// Property holding information about what kind of value is returned.
        /// </summary> 
        GridCellGeometryType GridCellGeometryType { get; set; }

        /// <summary>
        /// Bounding box for the grid cell in plotting coordinate system, ie CoordinateSystem.
        /// </summary>
        IPolygon GridCellBoundingBox { get; set; }

        /// <summary>
        /// Bounding box for the grid cell in calculated coordinate system, ie GridCellCoordinateSystem.
        /// </summary>
        IBoundingBox OrginalGridCellBoundingBox { get; set; }

        /// <summary>
        /// Centre coordinate for the grid cell in plotting coordinate system, ie CoordinateSystem.
        /// </summary>
        IPoint GridCellCentreCoordinate { get; set; }

        /// <summary>
        /// Centre coordinate for the grid cell in calculated coordinate system, ie GridCellCoordinateSystem.
        /// </summary>
        IPoint OrginalGridCellCentreCoordinate { get; set; }

        /// <summary>
        /// Property holding information om size of the grid cell that has been
        /// used.
        /// </summary>
        Int32 GridCellSize { get; set; }

        /// <summary>
        /// Property contains information about the grid cell coordinate
        /// system that is use for grid calculations and supported (by ArtDatabanken).
        /// </summary>
        GridCoordinateSystem GridCoordinateSystem { get; set; }

        /// <summary>
        /// Property contains information about the coordinate
        /// system that is used for presentation and supported (by ArtDatabanken), when performing grid search.
        /// </summary>
        ICoordinateSystem CoordinateSystem { get; set; }

        /// <summary>
        /// Gets a identifier that looks like this example: "SRID3006SIZE10000E745000N7295000".
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        String Identifier { get; }

        /// <summary>
        /// Copies the property values in this instance to the <paramref name="gridCell"/> instance.
        /// </summary>
        /// <param name="gridCell">The grid cell that should be initialized with values from this instance.</param>
        void CopyPropertiesTo(IGridCellBase gridCell);
    }
}