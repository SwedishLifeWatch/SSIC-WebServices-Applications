using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Species fact quality class.
    /// </summary>
    [Serializable]
    public class SpeciesFactQualityList : DataIdList
    {
        /// <summary>
        /// Get Species Fact Quality with specified id.
        /// </summary>
        /// <param name='speciesFactQualityId'>Id of requested factor update mode.</param>
        /// <returns>Requested factor update mode.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public SpeciesFactQuality Get(Int32 speciesFactQualityId)
        {
            return (SpeciesFactQuality)(GetById(speciesFactQualityId));
        }

        /// <summary>
        /// Get/set Species fact quality by list index.
        /// </summary>
        public new SpeciesFactQuality this[Int32 index]
        {
            get
            {
                return (SpeciesFactQuality)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
