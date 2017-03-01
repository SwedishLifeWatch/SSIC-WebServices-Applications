using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a date and time search criteria.
    /// </summary>
    public interface IDateTimeSearchCriteria
    {
        /// <summary>
        /// Requested minimum accuracy of compared data.
        /// This search criteria is only relevant if 
        /// compare data has a range.
        /// </summary>
        TimeSpan? Accuracy { get; set; }

        /// <summary>
        /// Start of date and time of the search criteria.
        /// </summary>
        DateTime Begin { get; set; }

        /// <summary>
        /// End of date and time of the search criteria.
        /// </summary>
        DateTime End { get; set; }

        /// <summary>
        /// Operator that should be used between this date time
        /// search criteria and the data that it is compared to.
        /// This operator is only relevant if both the date time
        /// search criteria and compare data has a range.
        /// Supported operators are CompareOperator:Excluding and
        /// CompareOperator:Including.
        /// This operator is used for properties Begin, End and
        /// PartOfYear if it has been specified.
        /// </summary>
        CompareOperator Operator { get; set; }

        /// <summary>
        /// Add intervals to property PartOfYear if data
        /// should be search based on specified parts of years.
        /// Year part of information in these 
        /// WebDateTimeInterval objects are not significant.
        /// </summary>
        List<IDateTimeInterval> PartOfYear { get; set; }
    }
}
