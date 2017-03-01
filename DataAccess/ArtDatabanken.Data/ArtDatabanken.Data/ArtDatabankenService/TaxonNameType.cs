using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains taxon name type ids.
    /// </summary>
    public enum TaxonNameTypeId
    {
        /// <summary>Scientific = 0</summary>
        Scientific = 0,
        /// <summary>Swedish = 1</summary>
        Swedish = 1
    }

    /// <summary>
    ///  This class represents a taxon name type.
    /// </summary>
    [Serializable]
    public class TaxonNameType : DataSortOrder
    {
        private String _name;

        /// <summary>
        /// Create a TaxonNameType instance.
        /// </summary>
        /// <param name='id'>Id for taxon name type.</param>
        /// <param name='name'>Name for taxon name type.</param>
        /// <param name='sortOrder'>Sort order among taxon name types.</param>
        public TaxonNameType(Int32 id, String name, Int32 sortOrder)
            : base(id, sortOrder)
        {
            _name = name;
        }

        /// <summary>
        /// Get name for this taxon name type.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }
    }
}
