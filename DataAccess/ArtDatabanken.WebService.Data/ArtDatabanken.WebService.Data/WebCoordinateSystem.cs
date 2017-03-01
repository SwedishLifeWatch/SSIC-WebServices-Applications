using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about the coordinate
    /// system that is provided or requested.
    /// Use either Id or WKT property to define a coordinate system.
    /// Set property Id to value None if the WKT property is used.
    /// </summary>
    [DataContract]
    public class WebCoordinateSystem : WebData
    {
        /// <summary>
        /// Use predefined WKT as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property WKT is used.
        /// </summary>
        [DataMember]
        public CoordinateSystemId Id
        { get; set; }

        /// <summary>
        /// Coordinate system defined as a string according to 
        /// coordinate system WKT as defined by OGC.
        /// Set property Id to value None if property WKT is used.
        /// </summary>
        [DataMember]
        public String WKT
        { get; set; }
    }
}
