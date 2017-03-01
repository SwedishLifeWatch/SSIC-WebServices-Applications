// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGeometryObject.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the IGeometryObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    /// <summary>
    /// Base Interface for GeometryObject types.
    /// </summary>    
    public interface IGeometryObject
    {
        /// <summary>
        /// Gets the (mandatory) type of the <see>GeoJSON Object
        ///         <cref>http://geojson.org/geojson-spec.html#geometry-objects</cref>
        ///     </see>.
        /// However, for <see>GeoJSON Objects
        ///         <cref>http://geojson.org/geojson-spec.html#geometry-objects</cref>
        ///     </see> only
        /// the 'Point', 'MultiPoint', 'LineString', 'MultiLineString', 'Polygon', 'MultiPolygon', or 'GeometryCollection' types are allowed.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        GeoJSONObjectType Type { get; }
    }
}
