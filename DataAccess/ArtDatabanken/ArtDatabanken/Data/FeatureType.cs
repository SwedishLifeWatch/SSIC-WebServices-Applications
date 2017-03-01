using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of spatial feature types.
    /// </summary>
    [DataContract]
    public enum FeatureType
    {
        /// <summary> Point feature. </summary>
        [EnumMember]
        Point,

        /// <summary> Line feature. </summary>
        [EnumMember]
        Line,

        /// <summary> Multi line feature. </summary>
        [EnumMember]
        Multiline,

        /// <summary> Polygon feature. </summary>
        [EnumMember]
        Polygon,

        /// <summary> MultiPolygon feature. </summary>
        [EnumMember]
        Multipolygon
    }
}
