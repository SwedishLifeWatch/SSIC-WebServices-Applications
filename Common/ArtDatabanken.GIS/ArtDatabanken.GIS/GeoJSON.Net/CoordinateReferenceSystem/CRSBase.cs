// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CRSBase.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the CRSBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;

namespace ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Base class for all IGeometryObject implementing types.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    public abstract class CRSBase : ICRSObject
    {
        /// <summary>
        /// Gets the type of the GeometryObject object.
        /// </summary>
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        [JsonConverter(typeof(CRSTypeEnumConverter))]
        [DataMember]
        public CRSType Type { get; internal set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        [JsonProperty(PropertyName = "properties", Required = Required.Always)]
        [DataMember]
        public Dictionary<string, object> Properties { get; internal set; }
    }
}
