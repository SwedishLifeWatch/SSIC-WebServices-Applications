// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CRSType.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the GeoJSON Coordinate Reference System Objects (CRS) types as defined in the <see cref="http://geojson.org/geojson-spec.html#coordinate-reference-system-objects">geojson.org v1.0 spec</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem
{
    using System;

    /// <summary>
    /// Defines the GeoJSON Coordinate Reference System Objects (CRS) types as defined in the <see>geojson.org v1.0 spec
    ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
    ///     </see>.
    /// </summary>
    [Flags]
    [DataContract]
    public enum CRSType
    {
        /// <summary>
        /// Defines the <see>Named
        ///         <cref>http://geojson.org/geojson-spec.html#named-crs</cref>
        ///     </see> CRS type.
        /// </summary>
        [DataMember]        
        Name,

        /// <summary>
        /// Defines the <see>Linked
        ///         <cref>http://geojson.org/geojson-spec.html#linked-crs</cref>
        ///     </see> CRS type.
        /// </summary>
        [DataMember]
        Link,

        /// <summary>
        /// Defines the <see>EPSG
        ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
        ///     </see> CRS type.
        /// </summary>
        [DataMember]
        EPSG

    }
}
