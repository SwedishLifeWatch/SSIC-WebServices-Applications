using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a string search criteria.
    /// </summary>
    [DataContract]
    public class WebStringSearchCriteria : WebData
    {
        /// <summary>
        /// Specifies how search string should be compared to
        /// actual data. At least one string compare operator must
        /// be specified. Normally exactly one operator is specified.
        /// If more than one operator is specified, each operator
        /// (starting from index 0 and upwards) is tested until at
        /// least one match is found.
        /// </summary>
        [DataMember]
        public List<StringCompareOperator> CompareOperators { get; set; }

        /// <summary>
        /// String that data should be compared to.
        /// </summary>
        [DataMember]
        public String SearchString { get; set; }
    }
}
