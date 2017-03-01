using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a date and time search criteria.
    /// </summary>
    [DataContract]
    public class WebDateTimeSearchCriteria : WebData
    {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Requested minimum accuracy of compared data.
        /// This search criteria is only relevant if 
        /// compare data has a range.
        /// </summary>
        [DataMember]
        public WebTimeSpan Accuracy
        { get; set; }
#endif

        /// <summary>
        /// Start of date and time of the search criteria.
        /// </summary>
        [DataMember]
        public DateTime Begin
        { get; set; }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Operator that should be used between this date time
        /// search criteria and the data that it is compared to.
        /// This operator is only relevant if both the date time
        /// search criteria and compare data has a range.
        /// Operator must be set to CompareOperator:Excluding or
        /// CompareOperator:Including if both the date time
        /// search criteria and compare data has a range.
        /// This operator is used for properties Begin, End and
        /// PartOfYear if it has been specified.
        /// </summary>
        [DataMember]
        public CompareOperator Operator { get; set; }
#endif

        /// <summary>
        /// End of date and time of the search criteria.
        /// </summary>
        [DataMember]
        public DateTime End
        { get; set; }

        /// <summary>
        /// Add intervals to property PartOfYear if data
        /// should be search based on specified parts of years.
        /// Year part of information in these 
        /// WebDateTimeInterval objects are not significant.
        /// </summary>
        [DataMember]
        public List<WebDateTimeInterval> PartOfYear
        { get; set; }
    }
}
