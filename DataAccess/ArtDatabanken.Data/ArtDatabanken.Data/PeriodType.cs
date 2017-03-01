using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a period type.
    /// </summary>
    public class PeriodType : IPeriodType
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for this period type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this period type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this period type.
        /// </summary>
        public String Name { get; set; }
    }
}