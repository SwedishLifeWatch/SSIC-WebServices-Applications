using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about an region category.
    /// Examples of region categories are county and municipality.
    /// Most region categories are related to a specific country.
    /// </summary>
    [DataContract]
    public class WebRegionCategory : WebData
    {
        /// <summary>
        /// Country iso code as specified in standard ISO-3166.
        /// Not all region categories has a country iso code.
        /// This property should only be used if property
        /// IsCountryIsoCodeSpecified has the value True.
        /// </summary>
        [DataMember]
        public Int32 CountryIsoCode
        { get; set; }

        /// <summary>
        /// Globally unique identifier (GUID) implemented according to 
        /// the Life Science Identifier (LSID) resolution protocol.
        /// Handling of this property is not implemented in current
        /// web services.
        /// This property is currently not implemented.
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// Region category id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Indicates if property CountryIsoCode has a value.
        /// </summary>
        [DataMember]
        public Boolean IsCountryIsoCodeSpecified
        { get; set; }

        /// <summary>
        /// Indicates if property Level has a value.
        /// </summary>
        [DataMember]
        public Boolean IsLevelSpecified
        { get; set; }

        /// <summary>
        /// Specifies level of the region category according to
        /// some hierarchical order.
        /// Not all region categories has a level value.
        /// This property should only be used if property
        /// IsLevelSpecified has the value True.
        /// </summary>
        [DataMember]
        public Int32 Level
        { get; set; }

        /// <summary>
        /// Region category name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Used source when native ids are assigned to regions.
        /// An example of source is Statistics Sweden (SCB) that
        /// defines ids for counties, municipalities, etc.
        /// This property is currently not implemented.
        /// </summary>
        [DataMember]
        public String NativeIdSource
        { get; set; }

        /// <summary>
        /// Used to sort region categories.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }

        /// <summary>
        /// Region type id.
        /// </summary>
        [DataMember]
        public Int32 TypeId
        { get; set; }
    }
}
