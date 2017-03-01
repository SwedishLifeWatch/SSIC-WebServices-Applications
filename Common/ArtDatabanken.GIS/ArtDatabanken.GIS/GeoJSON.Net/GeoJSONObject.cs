// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoJSONObject.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the GeoJSONObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net
{
    using Newtonsoft.Json;

    /// <summary>
    /// Base class for all IGeometryObject implementing types
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    public abstract class GeoJSONObject : IGeoJSONObject
    {
        /// <summary>
        /// Gets the (mandatory) type of the <see>GeoJSON Object
        ///         <cref>http://geojson.org/geojson-spec.html#geojson-objects</cref>
        ///     </see>.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        [JsonProperty(PropertyName = "type", Required = Required.Always, Order = 0)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [DataMember]
        public GeoJSONObjectType Type { get; internal set; }

        /// <summary>
        /// Gets or sets the (optional) <see>Coordinate Reference System Object
        ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
        ///     </see>.
        /// </summary>
        /// <value>
        /// The Coordinate Reference System Objects.
        /// </value>
        [JsonProperty(PropertyName = "crs", Required = Required.Default)]
        [JsonConverter(typeof(CoordinateReferenceSystemConverter))]
        [DataMember]
        public CoordinateReferenceSystem.ICRSObject CRS { get; set; }

        /// <summary>
        /// Gets or sets the (optional) <see>Bounding Boxes
        ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
        ///     </see>.
        /// </summary>
        /// <value>
        /// The value of the bbox member must be a 2*n array where n is the number of dimensions represented in the
        /// contained geometries, with the lowest values for all axes followed by the highest values.
        /// The axes order of a bbox follows the axes order of geometries.
        /// In addition, the coordinate reference system for the bbox is assumed to match the coordinate reference
        /// system of the GeoJSON object of which it is a member.
        /// </value>
        [JsonProperty(PropertyName = "bbox", Required = Required.Default)]
        [DataMember]
        public double[] BoundingBoxes { get; set; }

        /// <summary>
        /// Determines whether the specified GeoJSONObject is equal to this instance.
        /// </summary>
        /// <param name="other">The GeoJSONObject to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified GeoJSONObject is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(GeoJSONObject other)
        {
            return Type == other.Type && BoundingBoxes.SequenceEqualEx(other.BoundingBoxes);
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
            return Equals((GeoJSONObject) obj);
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
                return ((int) Type*397) ^ (BoundingBoxes != null ? BoundingBoxes.GetHashCode() : 0);
            }
        }
        
    }
}
