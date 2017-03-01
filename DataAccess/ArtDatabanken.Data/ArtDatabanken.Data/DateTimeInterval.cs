using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a date and time interval.
    /// </summary>
    public class DateTimeInterval : IDateTimeInterval
    {
        /// <summary>
        /// Start of date and time interval.
        /// </summary>
        public DateTime Begin
        { get; set; }

        /// <summary>
        /// End of date and time interval.
        /// </summary>
        public DateTime End
        { get; set; }

        /// <summary>
        /// If property IsDayOfYearSpecified is true then property
        /// Begin and End should be interpreted as day of year 
        /// instead of the specified date.
        /// For example 2012-03-01 is not the same day of year
        /// as 2013-03-01 since 2012 is leap year.
        /// </summary>
        public Boolean IsDayOfYearSpecified { get; set; }
    }
}
