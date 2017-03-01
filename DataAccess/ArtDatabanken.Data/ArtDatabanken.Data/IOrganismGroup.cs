using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a organism group.
    /// </summary>
    public interface IOrganismGroup : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this organism group.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Name for this organism group.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Sort order for this organism group.
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Type of organism group.
        /// </summary>
        OrganismGroupType Type { get; set; }
    }
}
