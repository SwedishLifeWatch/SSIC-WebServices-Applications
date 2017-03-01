using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains SpeciesFactQuality ids.
    /// </summary>
    public enum SpeciesFactQualityId
    {
        /// <summary>VerryGood = 1</summary>
        VerryGood = 1,
        /// <summary>Acceptable = 3</summary>
        Acceptable = 3
    }

    /// <summary>
    /// This class represents a category of species fact quality.
    /// </summary>
    [Serializable]
    public class SpeciesFactQuality : DataSortOrder
    {
        private String _name;
        private String _definition;

        /// <summary>
        /// Create a Species Fact Quality instance.
        /// </summary>
        /// <param name="id">Id for the species fact quality.</param>
        /// <param name="name">Name of the species fact quality.</param>
        /// <param name="definition">Definition of the species fact quality.</param>
        /// <param name="sortOrder"></param>
        public SpeciesFactQuality(
            Int32 id,
            String name,
            String definition,
            Int32 sortOrder)
            : base(id, sortOrder)
        {
            _name = name;
            _definition = definition;

        }

        /// <summary>
        /// Get name for this species fact quality.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get definition of this species fact quality.
        /// </summary>
        public String Definition
        {
            get { return _definition; }
        }

    }
}
