using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about the coordinate systems.
    /// Use either Id or WKT property to define a coordinate system.
    /// Set property Id to value None if the WKT property is used.
    /// </summary>
    public interface ICoordinateSystem
    {
        /// <summary>
        /// Use predefined WKT as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property WKT is used.
        /// </summary>
        CoordinateSystemId Id
        { get; set; }

        /// <summary>
        /// Coordinate system defined as a string according to 
        /// coordinate system WKT as defined by OGC.
        /// Set property Id to value None if property Id is used.
        /// </summary>
        String WKT
        { get; set; }
    }
}
