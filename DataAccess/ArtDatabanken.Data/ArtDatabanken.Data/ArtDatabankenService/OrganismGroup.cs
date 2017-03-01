using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Is used to distinguish between different types of
    /// organism groups.
    /// </summary>
    public enum OrganismGroupType
    {
        /// <summary>
        /// Most used organism type.
        /// </summary>
        Standard
    }

    /// <summary>
    /// Id's for organism groups in standard organism group type.
    /// </summary>
    public enum OrganismGroupStandardId
    {
        /// <summary>
        /// OtherOrganisms.
        /// </summary>
        OtherOrganisms = 25
    }

    /// <summary>
    ///  This class represents a OrganismGroup.
    /// </summary>
    [Serializable]
    public class OrganismGroup : DataSortOrder
    {
        private String _definition;
        private String _name;
        private OrganismGroupType _type;

        /// <summary>
        /// Create a OrganismGroup instance.
        /// </summary>
        /// <param name='id'>Id for organism group.</param>
        /// <param name='sortOrder'>Sort order among organism groups.</param>
        /// <param name="name">Name for this organism group.</param>
        /// <param name="type">Type of organism group.</param>
        /// <param name="definition">Definition of this organism group.</param>
        public OrganismGroup(Int32 id,
                             Int32 sortOrder,
                             String name,
                             OrganismGroupType type,
                             String definition)
            : base(id, sortOrder)
        {
            _name = name;
            _type = type;
            _definition = definition;
        }

        /// <summary>
        /// Get definition for this organism group.
        /// </summary>
        public String Definition
        {
            get { return _definition; }
        }

        /// <summary>
        /// Get name for this organism group.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get type of organism group.
        /// </summary>
        public OrganismGroupType Type
        {
            get { return _type; }
        }
    }
}
