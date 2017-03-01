using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains geography information about a region.
    /// </summary>
    [DataContract]
    public class WebRegionGeography : WebData
    {
        /// <summary>
        /// The surrounding bounding box of the geographical region.
        /// </summary>
        [DataMember]
        public WebBoundingBox BoundingBox
        { get; set; }

        /// <summary>
        /// Globally unique identifier (GUID) implemented according to 
        /// the Life Science Identifier (LSID) resolution protocol.
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// Region id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Definition of the geographical region.
        /// </summary>
        [DataMember]
        public WebMultiPolygon MultiPolygon
        { get; set; }
    }
}
