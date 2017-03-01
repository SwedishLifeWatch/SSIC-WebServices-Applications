using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of spatial reference system in
    /// the format of EPSG:XXXX wcich is very useful
    /// when connectiong to for example wfs servers.
    /// </summary>

    public static class SpatialReferenceSystemType
    {
        /// <summary>
        /// Srs for SWEREF99TM, the planar system all swedish maps are currently published in.
        /// </summary>
        public const string SWEREF99TM = "EPSG:3006";

        /// <summary>
        /// Srs for RT90 2.5 GON V, the planar system all swedish maps were previously published in.
        /// </summary>
        public const string RT9025 = "EPSG:3021";

        /// <summary>
        /// Srs for WGS84, a centric system.
        /// </summary>
        public const string WGS84 = "EPSG:4326";
    }
}


