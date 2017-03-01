// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#polygon">Polygon</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text;
using ArtDatabanken.GIS.Extensions;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see>Polygon
    ///         <cref>http://geojson.org/geojson-spec.html#polygon</cref>
    ///     </see> type.
    /// Coordinates of a Polygon are a list of <see>linear rings
    ///         <cref>http://geojson.org/geojson-spec.html#linestring</cref>
    ///     </see>
    /// coordinate arrays. The first element in the array represents the exterior ring. Any subsequent elements
    /// represent interior rings (or holes).
    /// </summary>
    /// <seealso>
    ///     <cref>http://geojson.org/geojson-spec.html#polygon</cref>
    /// </seealso>
    [DataContract]
    public class Polygon : GeoJSONObject, IGeometryObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        /// <param name="linearRings">
        /// The <see>linear rings
        ///         <cref>http://geojson.org/geojson-spec.html#linestring</cref>
        ///     </see> with the first element
        /// in the array representing the exterior ring. Any subsequent elements represent interior rings (or holes).
        /// </param>
        public Polygon(List<LineString> linearRings)
        {
            if (linearRings == null)
            {
                throw new ArgumentNullException("linearRings");
            }
            

            if (linearRings.Any(linearRing => !linearRing.IsLinearRing()))
            {
                throw new ArgumentOutOfRangeException("linearRings", "All elements must be closed LineStrings with 4 or more positions (see GeoJSON spec at 'http://geojson.org/geojson-spec.html#linestring').");
            }

            this.Coordinates = linearRings;
            this.Type = GeoJSONObjectType.Polygon;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        public Polygon()
        {
            this.Coordinates = new List<LineString>();
            this.Type = GeoJSONObjectType.Polygon;
        }

        /// <summary>
        /// Gets the list of points outlining this Polygon.
        /// </summary>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(CoordinatesConverter))] // todo - use this one
        [DataMember]
        public List<LineString> Coordinates { get; private set; }

        /// <summary>
        /// Determines whether the specified Polygon is equal to this instance.
        /// </summary>
        /// <param name="other">The Polygon to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified Polygon is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(Polygon other)
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
            return Equals((Polygon) obj);
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

        /// <summary>
        /// Gets the Polygon as WKT.
        /// </summary>        
        public String GetWkt()
        {
            Boolean isFirstLinearRing, isFirstPoint;
            StringBuilder wkt;

            wkt = new StringBuilder("POLYGON");
            wkt.Append("(");
            if (Coordinates.IsNotEmpty())
            {
                isFirstLinearRing = true;
                foreach (LineString linearRing in Coordinates)
                {
                    if (isFirstLinearRing)
                    {
                        isFirstLinearRing = false;
                    }
                    else
                    {
                        wkt.Append(", ");
                    }
                    wkt.Append("(");
                    if (linearRing.Coordinates.IsNotEmpty())
                    {
                        isFirstPoint = true;
                        foreach (GeographicPosition point in linearRing.Coordinates)
                        {
                            if (isFirstPoint)
                            {
                                isFirstPoint = false;
                            }
                            else
                            {
                                wkt.Append(", ");
                            }
                            wkt.Append(point.X.WebToStringR().Replace(",", "."));
                            wkt.Append(" " + point.Y.WebToStringR().Replace(",", "."));
                        }
                    }
                    wkt.Append(")");
                }
            }
            wkt.Append(")");
            return wkt.ToString();
        }

    }
}
