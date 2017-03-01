using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// This class contains geography information about a region.
    /// </summary>
    [DataContract]
    public class WebRegionGeometry : WebData
    {
        /// <summary>
        /// Region id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

               /// <summary>
        /// Region AreaDatasetId.
        /// </summary>
        [DataMember]
        public Int32 AreaDatasetId
        { get; set; }

        /// <summary>
        /// FeatureId
        /// </summary>
        [DataMember]
        public String FeatureId
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// ShortName
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// Definition of the geometry region.
        /// </summary>
        [DataMember]
        public WebMultiPolygon Polygon
        { get; set; }

        /// <summary>
        /// The surrounding bounding box of the geographical region.
        /// </summary>
        [DataMember]
        public WebBoundingBox BoundingBox
        { get; set; }

        /// <summary>
        /// Region AreaDatasetId.
        /// </summary>
        [DataMember]
        public Int32 AreaDatasetSubTypeId
        { get; set; }

        /// <summary>
        /// AttributesXml
        /// </summary>
        [DataMember]
        public String AttributesXml
        { get; set; } 

        /// <summary>
        /// Definition of the geographical region.
        /// </summary>
        [DataMember]
        public WebMultiPolygon PolygonGeography
        { get; set; }
    }
}