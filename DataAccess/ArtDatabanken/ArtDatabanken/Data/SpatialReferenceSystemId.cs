using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Constants for spatial reference system (SRID)in
    /// the format of EPSG:XXXX wcich is very useful
    /// when connectiong to for example wfs servers.
    /// </summary>
    public class SpatialReferenceSystemId
    {
        /// <summary>
        /// Srid for SWEREF99TM, the planar system all swedish maps are currently published in.
        /// </summary>
// ReSharper disable InconsistentNaming
        public const string SWEREF99TM = "EPSG:3006";

// ReSharper restore InconsistentNaming
        /// <summary>
        /// Srid for RT90 2.5 GON V, the planar system all swedish maps were previously published in.
        /// </summary>
// ReSharper disable InconsistentNaming
        public const string RT9025 = "EPSG:3021";

// ReSharper restore InconsistentNaming
        /// <summary>
        /// Srid for WGS84, a centric (loat/long) system.
        /// </summary>
// ReSharper disable InconsistentNaming
        public const string WGS84 = "EPSG:4326";
// ReSharper restore InconsistentNaming
    
    }

}