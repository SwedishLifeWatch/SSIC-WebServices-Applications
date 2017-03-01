using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a region.
    /// A region can for example represent a county or a municipality.
    /// </summary>
    [DataContract]
    public class WebRegion : WebData
    {
        /// <summary>
        /// Region category id.
        /// </summary>
        [DataMember]
        public Int32 CategoryId
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
        /// Name of the region.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Native id according to source specified in region category.
        /// </summary>
        [DataMember]
        public String NativeId
        { get; set; }

        /// <summary>
        /// A short version of the region name.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// Used to sort regions within specified category.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }

        /// <summary>
        /// Date region is valid from.
        /// This property is curretly not used.
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date region is valid to.
        /// This property is curretly not used.
        /// </summary>
        [DataMember]
        public DateTime ValidToDate
        { get; set; }
    }
}
