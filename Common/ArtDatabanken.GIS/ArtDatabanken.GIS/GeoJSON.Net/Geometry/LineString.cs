// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineString.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#linestring">LineString</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System;
    using System.Collections.Generic;

    using ArtDatabanken.GIS.GeoJSON.Net.Converters;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see>LineString
    ///         <cref>http://geojson.org/geojson-spec.html#linestring</cref>
    ///     </see> type.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]    
    public class LineString : GeoJSONObject, IGeometryObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineString"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public LineString(List<GeographicPosition> coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException("coordinates");
            }

            if (coordinates.Count < 2)
            {
                throw new ArgumentOutOfRangeException("coordinates", "According to the GeoJSON v1.0 spec a LineString must have at least two or more positions.");
            }

            this.Coordinates = coordinates;
            this.Type = GeoJSONObjectType.LineString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString"/> class.
        /// </summary>
        public LineString()
        {
            this.Coordinates = new List<GeographicPosition>();
            this.Type = GeoJSONObjectType.LineString;
        }

        /// <summary>
        /// Gets the Positions.
        /// </summary>
        /// <value>The Positions.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(CoordinatesConverter))] // todo - use this one
        [DataMember]
        public List<GeographicPosition> Coordinates { get; private set; }

        /// <summary>
        /// Determines whether this LineString is a <see>LinearRing
        ///         <cref>http://geojson.org/geojson-spec.html#linestring</cref>
        ///     </see>.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if it is a linear ring; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLinearRing()
        {
            return this.Coordinates.Count >= 4 && this.IsClosed();
        }

        /// <summary>
        /// Determines whether this instance has its first and last coordinate at the same position and thereby is closed.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClosed()
        {
            return this.Coordinates[0].Equals(this.Coordinates[this.Coordinates.Count - 1]);
        }

        /// <summary>
        /// Determines whether the specified LineString is equal to this instance.
        /// </summary>
        /// <param name="other">The LineString to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified LineString is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(LineString other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
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
            return Equals((LineString) obj);
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
                int hashCode = HashCodeHelper.GetOrderIndependentHashCode(Coordinates); //todo - ska ej vara orderindependet. Är ju beroende av ordning...
                hashCode = (hashCode * 397) ^ base.GetHashCode();                
                return hashCode;
            }
        }
        
    }

}
 