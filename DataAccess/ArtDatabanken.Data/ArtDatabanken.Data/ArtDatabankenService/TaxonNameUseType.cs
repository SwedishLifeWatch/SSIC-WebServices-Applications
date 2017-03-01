using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a taxon name use type.
    /// </summary>
    [Serializable]
    public class TaxonNameUseType : DataId
    {
        private String _name;

        /// <summary>
        /// Create a TaxonNameUseType instance.
        /// </summary>
        /// <param name='id'>Id for taxon name use type.</param>
        /// <param name='name'>Name for taxon name use type.</param>
        public TaxonNameUseType(Int32 id, String name)
            : base(id)
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
