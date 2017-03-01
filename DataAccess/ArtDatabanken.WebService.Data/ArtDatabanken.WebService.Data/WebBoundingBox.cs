using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// A set of 2, 4, 6 or 8 numbers indicating the upper and
    /// lower bounds of an interval (1D), rectangle (2D),
    /// parallelpiped (3D), or hypercube along each axis of a
    /// given coordinate reference system.
    /// </summary>
    [DataContract]
    public class WebBoundingBox : WebData
    {
        /// <summary>
        /// Max values of the bounding box.
        /// </summary>
        [DataMember]
        public WebPoint Max
        { get; set; }

        /// <summary>
        /// Min values of the bounding box.
        /// </summary>
        [DataMember]
        public WebPoint Min
        { get; set; }
    }
}
