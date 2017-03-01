using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Search criteria used when references are retrieved.
    /// </summary>
    [DataContract]
    public class WebReferenceSearchCriteria : WebData
    {
        /// <summary>
        /// Specify how search criteria should be logically combined
        /// when more than one search criteria is specified.
        /// Only logical operator And and Or are supported.
        /// </summary>
        [DataMember]
        public LogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// Search references based on name of person
        /// that modified the references.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria ModifiedBySearchString { get; set; }

        /// <summary>
        /// Search references based on modified date and time.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public WebDateTimeSearchCriteria ModifiedDateTime { get; set; }

        /// <summary>
        /// Search references based on names of references.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Search references based on titles on references.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria TitleSearchString { get; set; }

        /// <summary>
        /// Search references based on years.
        /// </summary>
        [DataMember]
        public List<Int32> Years { get; set; }
    }
}
