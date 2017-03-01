using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// A LinearRing is a LineString that is both closed and simple.
    /// A LineString is a curve with linear interpolation
    /// between points. Each consecutive pair of points
    /// defines a line segment.
    /// </summary>
    [DataContract]
    public class WebLinearRing : WebData
    {
        /// <summary>
        /// Points that defines the LinearRing.
        /// </summary>
        [DataMember]
        public List<WebPoint> Points
        { get; set; }
    }
}
