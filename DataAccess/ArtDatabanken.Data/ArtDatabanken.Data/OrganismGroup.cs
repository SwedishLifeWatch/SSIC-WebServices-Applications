using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents an organism group.
    /// </summary>
    [Serializable]
    public class OrganismGroup : IOrganismGroup
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this organism group.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Id for this organism group.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this organism group.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Sort order for this organism group.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Type of organism group.
        /// </summary>
        public OrganismGroupType Type { get; set; }
    }
}
