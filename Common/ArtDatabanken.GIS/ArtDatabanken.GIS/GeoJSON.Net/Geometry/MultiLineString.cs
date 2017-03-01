// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiLineString.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#multilinestring">MultiLineString</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see>MultiLineString
    ///         <cref>http://geojson.org/geojson-spec.html#multilinestring</cref>
    ///     </see> type.
    /// </summary>
    [DataContract]
    public class MultiLineString : GeoJSONObject, IGeometryObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineString"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public MultiLineString(List<LineString> coordinates)
        {
            this.Coordinates = coordinates ?? new List<LineString>();
            this.Type = GeoJSONObjectType.MultiLineString;
        }
        
        /// <summary>
        /// Gets the Coordinates.
        /// </summary>
        /// <value>The Coordinates.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(CoordinatesConverter))]
        [DataMember]
        public List<LineString> Coordinates { get; private set; }


        /// <summary>
        /// Determines whether the specified MultiLineString is equal to this instance.
        /// </summary>
        /// <param name="other">The MultiLineString to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified MultiLineString is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(MultiLineString other)
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
            return Equals((MultiLineString) obj);
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
                return (base.GetHashCode()*397) ^ (Coordinates != null ? Coordinates.GetHashCode() : 0);
            }
        }
     
    }
}
