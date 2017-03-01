using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about an area category.
    /// Examples of area categories are county and municipality.
    /// Most area categories are related to a specific country.
    /// </summary>
    [DataContract]
    public class WebAreaCategory : WebData
    {
        /// <summary>
        /// Country iso code as specified in standard ISO-3166.
        /// Not all area categories has a country iso code.
        /// This property should only be used if property
        /// IsCountryIsoCodeSpecified has the value True.
        /// </summary>
        [DataMember]
        public Int32 CountryIsoCode
        { get; set; }

        /// <summary>
        /// Area category id.
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
        /// Specifies level of the area category according to some hierarchical order.
        /// Not all area categories has a level value.
        /// This property should only be used if property
        /// IsLevelSpecified has the value True.
        /// </summary>
        [DataMember]
        public Int32 Level
        { get; set; }

        /// <summary>
        /// Area category name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Used source when native ids are assigned to areas.
        /// An example of source is Statistics Sweden (SCB) that
        /// defines ids for counties, municipalities, etc.
        /// </summary>
        [DataMember]
        public String NativeIdSource
        { get; set; }

        /// <summary>
        /// Used to sort area categories.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }

        /// <summary>
        /// Area type id.
        /// </summary>
        [DataMember]
        public Int32 TypeId
        { get; set; }
    }
}
