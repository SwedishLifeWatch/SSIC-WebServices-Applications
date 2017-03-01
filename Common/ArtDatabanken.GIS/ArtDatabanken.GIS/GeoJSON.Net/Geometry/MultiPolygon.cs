// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiPolygon.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#multipolygon">MultiPolygon</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System.Collections.Generic;

    using ArtDatabanken.GIS.GeoJSON.Net.Converters;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see>MultiPolygon
    ///         <cref>http://geojson.org/geojson-spec.html#multipolygon</cref>
    ///     </see> type.
    /// </summary>
    [DataContract]
    public class MultiPolygon : GeoJSONObject, IGeometryObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon"/> class.
        /// </summary>
        /// <param name="polygons">The polygons contained in this MultiPolygon.</param>
        public MultiPolygon(List<Polygon> polygons = null)
        {
            this.Coordinates = polygons ?? new List<Polygon>();
            this.Type = GeoJSONObjectType.MultiPolygon;
        }

        /// <summary>
        /// Gets the list of Polygons enclosed in this MultiPolygon.
        /// </summary>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(CoordinatesConverter))]
        [DataMember]
        public List<Polygon> Coordinates { get; private set; }

        /// <summary>
        /// Determines whether the specified MultiPolygon is equal to this instance.
        /// </summary>
        /// <param name="other">The MultiPolygon to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified MultiPolygon is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(MultiPolygon other)
        {
            return base.Equals(other) && Coordinates.SequenceEqualEx(other.Coordinates);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MultiPolygon)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Coordinates != null ? Coordinates.GetHashCode() : 0);
            }
        }
    }
}
