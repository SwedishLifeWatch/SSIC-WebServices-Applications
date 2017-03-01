using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a period.
    /// Period is one of the dimensions used to handle species facts.
    /// </summary>
    public interface IPeriod : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for this period.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Test if update of species facts belonging to
        /// this period is allowed.
        /// </summary>
        Boolean AllowUpdate { get; }

        /// <summary>
        /// Name for this period.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Stop updates for species facts, that
        /// are related to this period, at this date.
        /// </summary>
        DateTime StopUpdates { get; set; }

        /// <summary>
        /// Period type id for this period.
        /// </summary>
        IPeriodType Type { get; set; }

        /// <summary>
        /// Year for this period.
        /// </summary>
        Int32 Year { get; set; }
    }
}