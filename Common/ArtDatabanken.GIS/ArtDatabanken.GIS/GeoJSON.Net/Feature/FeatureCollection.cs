// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeatureCollection.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the FeatureCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArtDatabanken.GIS.Helpers;

namespace ArtDatabanken.GIS.GeoJSON.Net.Feature
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the FeatureCollection type.
    /// </summary>
    [DataContract]
    public class FeatureCollection : GeoJSONObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureCollection"/> class.
        /// </summary>
        /// <param name="features">The features.</param>        
        public FeatureCollection(List<Feature> features) //, Dictionary<string, object> properties = null)
        {
            this.Features = features;
            //this.Properties = properties;

            this.Type = GeoJSONObjectType.FeatureCollection;
        }

        /// <summary>
        /// Gets the features.
        /// </summary>
        /// <value>The features.</value>
        [JsonProperty(PropertyName = "features", Required = Required.Always, Order = 1)]
        [DataMember]
        public List<Feature> Features { get; private set; }

        ///// <summary>
        ///// Gets the properties.
        ///// </summary>
        ///// <value>The properties.</value>
        //[JsonProperty(PropertyName = "properties", Required = Required.AllowNull)]
        //public Dictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// Determines whether the specified FeatureCollection is equal to this instance.
        /// </summary>
        /// <param name="other">The FeatureCollection to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified FeatureCollection is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(FeatureCollection other)
        {
            return base.Equals(other) && Features.SequenceEqualEx(other.Features);
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
            return Equals((FeatureCollection) obj);
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
                return (base.GetHashCode()*397) ^ (Features != null ? Features.GetHashCode() : 0);
            }
        }
       
    }
}
