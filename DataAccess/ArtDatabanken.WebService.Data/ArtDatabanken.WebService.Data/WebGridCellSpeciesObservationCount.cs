using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information on counting number of species
    /// observations, selceted coordinate system, gridcell size,
    /// centre coodinates (X and Y) for gridcell.
    /// The Observation counting result i.e. counting number of
    /// species observations is based on selected 
    /// species observation search criteria and gridcell size.
    /// </summary>
    [DataContract]
    public class WebGridCellSpeciesObservationCount : WebData
    {
        /// <summary>
        /// Bounding box for the grid cell in plotting coordinate system, ie CoordinateSystem.
        /// </summary>
        [DataMember]
        public WebPolygon BoundingBox { get; set; }

        /// <summary>
        /// Centre coordinate for the grid cell in plotting
        /// coordinate system, ie CoordinateSystem.
        /// </summary>
        [DataMember]
        public WebPoint CentreCoordinate { get; set; }

        /// <summary>
        /// Property contains information about the coordinate system requested 
        /// by the user and that is used for presentation of the grid.
        /// </summary>
        [DataMember]
        public WebCoordinateSystem CoordinateSystem { get; set; }

        /// <summary>
        /// Number of species observations is based on selected species
        /// observation search criteria and grid cell specifications.
        /// </summary>
        [DataMember]
        public Int64 Count { get; set; }

        /// <summary>
        /// Property holding information about what kind of grid cell
        /// geometry information that should be returned together with
        /// requested grid statistics.
        /// </summary> 
        public GridCellGeometryType GeometryType
        { get; set; }

        /// <summary>
        /// Property contains information about the grid coordinate system that 
        /// is used for grid calculations and supported (by ArtDatabanken).
        /// </summary>
        [DataMember]
        public GridCoordinateSystem GridCoordinateSystem { get; set; }

        /// <summary>
        /// Bounding box for the grid cell in calculated
        /// coordinate system, ie GridCoordinateSystem.
        /// </summary>
        [DataMember]
        public WebBoundingBox OrginalBoundingBox { get; set; }

        /// <summary>
        /// Centre coordinate for the grid cell in calculated
        /// coordinate system, ie GridCoordinateSystem.
        /// </summary>
        [DataMember]
        public WebPoint OrginalCentreCoordinate { get; set; }

        /// <summary>
        /// Property holding information about size of the grid cell
        /// that has been used.
        /// </summary>
        [DataMember]
        public Int32 Size { get; set; }
    }
}