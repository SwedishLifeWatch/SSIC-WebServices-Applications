using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor origin.
    /// </summary>
    public interface IFactorOrigin : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this origin.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Name for this origin.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Sort order.
        /// </summary>
        Int32 SortOrder { get; set; }
    }
}
