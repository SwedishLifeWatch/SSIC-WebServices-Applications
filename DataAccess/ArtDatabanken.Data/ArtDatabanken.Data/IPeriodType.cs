using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a period type.
    /// </summary>
    public interface IPeriodType : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for this period type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Name for this period type.
        /// </summary>
        String Name { get; set; }
    }
}