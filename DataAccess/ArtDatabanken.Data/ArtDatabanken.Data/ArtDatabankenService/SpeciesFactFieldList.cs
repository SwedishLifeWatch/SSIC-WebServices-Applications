using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class of the species fact field classes
    /// </summary>
    [Serializable]
    public class SpeciesFactFieldList : DataIdList
    {
        /// <summary>
        /// Get SpeciesFactField with specified id.
        /// </summary>
        /// <param name='speciesFactFieldId'>Id of requested species fact field.</param>
        /// <returns>Requested species fact field.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public SpeciesFactField Get(Int32 speciesFactFieldId)
        {
            return (SpeciesFactField)(GetById(speciesFactFieldId));
        }

        /// <summary>
        /// Get/set species fact field by list index.
        /// </summary>
        public new SpeciesFactField this[Int32 index]
        {
            get
            {
                return (SpeciesFactField)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }


    }
}
