// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Feature.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the Feature type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArtDatabanken.GIS.Helpers;
using WFS_Schemas.Ver1_1_0;

namespace ArtDatabanken.GIS.GeoJSON.Net.Feature
{
    using System.Collections.Generic;

    using ArtDatabanken.GIS.GeoJSON.Net.Converters;
    using ArtDatabanken.GIS.GeoJSON.Net.Geometry;

    using Newtonsoft.Json;

    /// <summary>
    /// A GeoJSON <see>Feature Object
    ///         <cref>http://geojson.org/geojson-spec.html#feature-objects</cref>
    ///     </see>.
    /// </summary>
    [DataContract]
    public class Feature : GeoJSONObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        /// <param name="geometry">The Geometry Object.</param>
        /// <param name="properties">The properties.</param>
        public Feature(IGeometryObject geometry, Dictionary<string, object> properties = null)
        {
            this.Geometry = geometry;
            this.Properties = properties;

            this.Type = GeoJSONObjectType.Feature;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The handle.</value>
        [JsonProperty(PropertyName = "id")]
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        [JsonProperty(PropertyName = "geometry", Required = Required.Default)]
        [JsonConverter(typeof(GeometryConverter))]
        [DataMember]
        public IGeometryObject Geometry { get; set; }
        
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        [JsonProperty(PropertyName = "properties", Required = Required.Default)]
        [DataMember]
        public Dictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// Determines whether the specified Feature is equal to this instance.
        /// </summary>
        /// <param name="other">The Feature to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified Feature is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(Feature other)
        {            
            return base.Equals(other) && string.Equals(Id, other.Id) && Equals(Geometry, other.Geometry) && Properties.SequenceEqualEx(other.Properties);
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
            return Equals((Feature) obj);
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
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Geometry != null ? Geometry.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Properties != null ? Properties.GetHashCode() : 0);
                return hashCode;
            }
        }
     
    }
}
