using System;
using System.Globalization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information of selected grid cell coordinate system,
    /// gridcell size, centre coodinates (X and Y) for gridcell.
    /// The result i.e. counting/calculation is based on selected 
    /// species observation search criteria and gridcell size.
    /// </summary>
    public class GridCellBase : IGridCellBase
    {
        /// <summary>
        /// Property holding information about what kind of value is returned.
        /// </summary> 
        public GridCellGeometryType GridCellGeometryType
        { get; set; }
        
        /// <summary>
        /// Bounding box for the grid cell in plotting coordinate system, ie CoordinateSystem.
        /// </summary>
        public IPolygon GridCellBoundingBox
        { get; set; }

        /// <summary>
        /// Bounding box for the grid cell in calculated coordinate system, ie GridCellCoordinateSystem.
        /// </summary>
        public IBoundingBox OrginalGridCellBoundingBox
        { get; set; }

        /// <summary>
        /// Centre coordinate for the grid cell in plotting coordinate system, ie CoordinateSystem.
        /// </summary>
        public IPoint GridCellCentreCoordinate
        { get; set; }

        /// <summary>
        /// Centre coordinate for the grid cell in calculated coordinate system, ie GridCellCoordinateSystem.
        /// </summary>
        public IPoint OrginalGridCellCentreCoordinate
        { get; set; }

        /// <summary>
        /// Property holding information om size of the grid cell that has been
        /// used.
        /// </summary>
        public Int32 GridCellSize
        { get; set; }

        /// <summary>
        /// Property contains information about the grid cell coordinate
        /// system that is use for grid calculations and supported (by ArtDatabanken).
        /// </summary>
        public GridCoordinateSystem GridCoordinateSystem
        { get; set; }

        /// <summary>
        /// Property contains information about the coordinate
        /// system that is used for presentation and supported (by ArtDatabanken), when performing grid search.
        /// </summary>
        public ICoordinateSystem CoordinateSystem
        { get; set; }

        /// <summary>
        /// Gets a identifier that looks like this example: "SRID3006SIZE10000E745000N7295000".
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier
        {
            get
            {
                int srid = GridCoordinateSystem.Srid();               
                string easting = "0";
                string northing = "0";
                if (OrginalGridCellCentreCoordinate != null)
                {
                    easting = OrginalGridCellCentreCoordinate.X.ToString(CultureInfo.InvariantCulture.NumberFormat);
                    northing = OrginalGridCellCentreCoordinate.Y.ToString(CultureInfo.InvariantCulture.NumberFormat);
                }

                string identifier = string.Format("SRID{0}SIZE{1}E{2}N{3}", srid, GridCellSize, easting, northing);
                return identifier;                
            }
        }

        /// <summary>
        /// Copies the property values in this instance to the <paramref name="gridCell"/> instance.
        /// </summary>
        /// <param name="gridCell">The grid cell that should be initialized with values from this instance.</param>
        public void CopyPropertiesTo(IGridCellBase gridCell)
        {
            gridCell.CoordinateSystem = CoordinateSystem;
            gridCell.GridCellBoundingBox = GridCellBoundingBox;
            gridCell.GridCellCentreCoordinate = GridCellCentreCoordinate;
            gridCell.GridCellGeometryType = GridCellGeometryType;
            gridCell.GridCellSize = GridCellSize;
            gridCell.GridCoordinateSystem = GridCoordinateSystem;
            gridCell.OrginalGridCellBoundingBox = OrginalGridCellBoundingBox;
            gridCell.OrginalGridCellCentreCoordinate = OrginalGridCellCentreCoordinate;            
        }
    }
}