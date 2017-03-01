// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJSONObjectType.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the GeoJSON Objects types as defined in the <see cref="http://geojson.org/geojson-spec.html#geojson-objects">geojson.org v1.0 spec</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ArtDatabanken.GIS.GeoJSON.Net
{
    /// <summary>
    /// Defines the GeoJSON Objects types as defined in the <see>geojson.org v1.0 spec
    ///         <cref>http://geojson.org/geojson-spec.html#geojson-objects</cref>
    ///     </see>.
    /// </summary>
    [DataContract]
    public enum GeoJSONObjectType
    {
        /// <summary>
        /// Defines the <see>Point
        ///         <cref>http://geojson.org/geojson-spec.html#point</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        Point,

        /// <summary>
        /// Defines the <see>MultiPoint
        ///         <cref>http://geojson.org/geojson-spec.html#multipoint</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        MultiPoint,

        /// <summary>
        /// Defines the <see>LineString
        ///         <cref>http://geojson.org/geojson-spec.html#linestring</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        LineString,

        /// <summary>
        /// Defines the <see>MultiLineString
        ///         <cref>http://geojson.org/geojson-spec.html#multilinestring</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        MultiLineString,

        /// <summary>
        /// Defines the <see>Polygon
        ///         <cref>http://geojson.org/geojson-spec.html#polygon</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        Polygon,

        /// <summary>
        /// Defines the <see>MultiPolygon
        ///         <cref>http://geojson.org/geojson-spec.html#multipolygon</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        MultiPolygon,

        /// <summary>
        /// Defines the <see>GeometryCollection
        ///         <cref>http://geojson.org/geojson-spec.html#geometry-collection</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        GeometryCollection,

        /// <summary>
        /// Defines the <see>Feature
        ///         <cref>http://geojson.org/geojson-spec.html#feature-objects</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        Feature,

        /// <summary>
        /// Defines the <see>FeatureCollection
        ///         <cref>http://geojson.org/geojson-spec.html#feature-collection-objects</cref>
        ///     </see> type.
        /// </summary>
        [EnumMember] 
        FeatureCollection
    }
}
