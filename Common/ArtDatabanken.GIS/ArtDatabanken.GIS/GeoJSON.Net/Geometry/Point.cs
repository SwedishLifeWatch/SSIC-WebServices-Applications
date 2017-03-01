// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Point.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the Point type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System;
    using System.Collections.Generic;

    using ArtDatabanken.GIS.GeoJSON.Net.Converters;

    using Newtonsoft.Json;

    /// <summary>
    /// In geography, a point refers to a Position on a map, expressed in latitude and longitude.
    /// </summary>
    /// <seealso>
    ///     <cref>http://geojson.org/geojson-spec.html#point</cref>
    /// </seealso>
    [DataContract]    
    public class Point : GeoJSONObject, IGeometryObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="coordinates">The Position.</param>
        public Point(GeographicPosition coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException("coordinates");
            }

            //this.Coordinates = new List<IPosition> { coordinates };
            this.Coordinates = coordinates;
            this.Type = GeoJSONObjectType.Point;
        }

        /// <summary>
        /// Gets the Coordinate(s).
        /// </summary>
        /// <value>The Coordinates.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(CoordinatesConverter))]
        [DataMember]
        public GeographicPosition Coordinates { get; private set; }


        /// <summary>
        /// Determines whether the specified Point is equal to this instance.
        /// </summary>
        /// <param name="other">The Point to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified Point is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(Point other)
        {
            return base.Equals(other) && Equals(Coordinates, other.Coordinates);
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
            return Equals((Point) obj);
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
