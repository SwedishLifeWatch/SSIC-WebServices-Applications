using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about an area.
    /// An area can for example represent a county or a municipality.
    /// </summary>
    [DataContract]
    public class WebArea : WebData
    {
        /// <summary>
        /// The surrounding bounding box of the geographical area.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        [DataMember]
        public WebBoundingBox BoundingBox
        { get; set; }

        /// <summary>
        /// Area category id.
        /// </summary>
        [DataMember]
        public Int32 CategoryId
        { get; set; }

        /// <summary>
        /// Area id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Indicates if this area is a subpart of another area
        /// of the same area category.
        /// </summary>
        [DataMember]
        public Boolean IsAreaPart
        { get; set; }

        /// <summary>
        /// Definition of the geographical area.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        [DataMember]
        public WebMultiPolygon MultiPolygon
        { get; set; }

        /// <summary>
        /// Name of the area.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Native id according to source specified in area category.
        /// </summary>
        [DataMember]
        public String NativeId
        { get; set; }

        /// <summary>
        /// This area is a subpart of another area of the same
        /// area category and with the specified area id.
        /// This property should only be used if property IsAreaPart
        /// has the value True.
        /// </summary>
        public Int32 PartOfAreaId
        { get; set; }

        /// <summary>
        /// A short version of the area name.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// Used to sort areas within specified category.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }
    }
}
