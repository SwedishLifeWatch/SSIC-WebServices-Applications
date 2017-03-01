using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a string search criteria.
    /// </summary>
    public interface IStringSearchCriteria
    {
        /// <summary>
        /// Specifies how search string should be compared to
        /// actual data.
        /// Normaly exactly one string compare operator is specified.
        /// If more than one operator is specified, each operator
        /// (starting from index 0 and upwards) is tested until at
        /// least one match is found.
        /// </summary>
        List<StringCompareOperator> CompareOperators
        { get; set; }

        /// <summary>
        /// String that data should be compared to.
        /// </summary>
        String SearchString
        { get; set; }
    }
}
