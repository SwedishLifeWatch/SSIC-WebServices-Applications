using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a region search criteria.
    /// </summary>
    [DataContract]
    public class WebRegionSearchCriteria : WebData
    {
        /// <summary>
        /// Search regions that belongs to specified categories.
        /// </summary>
        [DataMember]
        public List<WebRegionCategory> Categories
        { get; set; }

        /// <summary>
        /// Search regions that belongs to the specified countries.
        /// </summary>
        [DataMember]
        public List<Int32> CountryIsoCodes
        { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for not valid regions.
        /// This property is curretly not used.
        /// </summary>
        [DataMember]
        public Boolean IncludeNotValidRegions
        { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for valid regions.
        /// This property is curretly not used.
        /// </summary>
        [DataMember]
        public Boolean IncludeValidRegions
        { get; set; }

        /// <summary>
        /// Search for regions with matching names.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria NameSearchString
        { get; set; }

        /// <summary>
        /// Search regions of specified type.
        /// </summary>
        [DataMember]
        public WebRegionType Type
        { get; set; }

        /// <summary>
        /// Search regions based on valid date.
        /// This property is curretly not used.
        /// </summary>
        [DataMember]
        public DateTime ValidDate
        { get; set; }
    }
}
