using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a period.
    /// </summary>
    public class Period : IPeriod
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for this period.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this period.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Test if update of species facts belonging to
        /// this period is allowed.
        /// </summary>
        public Boolean AllowUpdate
        {
            get { return DateTime.Today <= StopUpdates; }
        }

        /// <summary>
        /// Name for this period.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Stop updates for species facts, that
        /// are related to this period, at this date.
        /// </summary>
        public DateTime StopUpdates { get; set; }

        /// <summary>
        /// Period type id for this period.
        /// </summary>
        public IPeriodType Type { get; set; }

        /// <summary>
        /// Year for this period.
        /// </summary>
        public Int32 Year { get; set; }
    }
}