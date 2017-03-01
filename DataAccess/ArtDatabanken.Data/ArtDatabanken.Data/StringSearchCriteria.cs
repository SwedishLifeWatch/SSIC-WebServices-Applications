using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a string search criteria.
    /// </summary>
    public class StringSearchCriteria : IStringSearchCriteria
    {
        /// <summary>
        /// Specifies how search string should be compared to
        /// actual data.
        /// Normaly exactly one string compare operator is specified.
        /// If more than one operator is specified, each operator
        /// (starting from index 0 and upwards) is tested until at
        /// least one match is found.
        /// </summary>
        public List<StringCompareOperator> CompareOperators
        { get; set; }

        /// <summary>
        /// String that data should be compared to.
        /// </summary>
        public String SearchString
        { get; set; }
    }
}
