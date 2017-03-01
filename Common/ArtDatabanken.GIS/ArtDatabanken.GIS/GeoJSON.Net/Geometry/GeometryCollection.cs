// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeometryCollection.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#geometry-collection">GeometryCollection</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System.Collections.Generic;

    using ArtDatabanken.GIS.GeoJSON.Net.Converters;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see>GeometryCollection
    ///         <cref>http://geojson.org/geojson-spec.html#geometry-collection</cref>
    ///     </see> type.
    /// </summary>
    [DataContract]
    public class GeometryCollection : GeoJSONObject, IGeometryObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryCollection"/> class.
        /// </summary>
        /// <param name="geometries">The geometries contained in this GeometryCollection.</param>
        public GeometryCollection(List<IGeometryObject> geometries = null)
        {
            this.Geometries = geometries ?? new List<IGeometryObject>();
            this.Type = GeoJSONObjectType.GeometryCollection;
        }

        /// <summary>
        /// Gets the list of Polygons enclosed in this MultiPolygon.
        /// </summary>
        [JsonProperty(PropertyName = "geometries", Required = Required.Always)]
        [JsonConverter(typeof(GeometryConverter))]
        [DataMember]
        public List<IGeometryObject> Geometries { get; private set; }


        
        /// <summary>
        /// Determines whether the specified GeometryCollection is equal to this instance.
        /// </summary>
        /// <param name="other">The GeometryCollection to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified GeometryCollection is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(GeometryCollection other)
        {
            return base.Equals(other) && Geometries.SequenceEqualEx(other.Geometries);
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
            return Equals((GeometryCollection) obj);
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
                return (base.GetHashCode()*397) ^ (Geometries != null ? Geometries.GetHashCode() : 0);
            }
        }

    }
}
