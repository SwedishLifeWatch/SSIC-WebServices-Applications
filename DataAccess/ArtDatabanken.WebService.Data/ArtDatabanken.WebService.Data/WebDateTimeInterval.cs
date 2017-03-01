using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a date and time interval.
    /// </summary>
    [DataContract]
    public class WebDateTimeInterval : WebData
    {
        /// <summary>
        /// Start of date and time interval.
        /// </summary>
        [DataMember]
        public DateTime Begin { get; set; }

        /// <summary>
        /// End of date and time interval.
        /// </summary>
        [DataMember]
        public DateTime End { get; set; }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// If property IsDayOfYearSpecified is true then property
        /// Begin and End should be interpreted as day of year 
        /// instead of the specified date.
        /// For example 2012-03-01 is not the same day of year
        /// as 2013-03-01 since 2012 is leap year.
        /// </summary>
        [DataMember]
        public Boolean IsDayOfYearSpecified { get; set; }
#endif
    }
}
