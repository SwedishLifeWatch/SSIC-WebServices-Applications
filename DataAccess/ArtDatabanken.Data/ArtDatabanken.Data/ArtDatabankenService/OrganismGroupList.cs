using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the OrganismGroup class.
    /// </summary>
    [Serializable()]
    public class OrganismGroupList : DataIdList
    {
        /// <summary>
        /// Get OrganismGroup with specified id.
        /// </summary>
        /// <param name='organismGroupId'>Id of requested organism group.</param>
        /// <returns>Requested organism group.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public OrganismGroup Get(Int32 organismGroupId)
        {
            return (OrganismGroup)(GetById(organismGroupId));
        }

        /// <summary>
        /// Get/set OrganismGroup by list index.
        /// </summary>
        public new OrganismGroup this[Int32 index]
        {
            get
            {
                return (OrganismGroup)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
