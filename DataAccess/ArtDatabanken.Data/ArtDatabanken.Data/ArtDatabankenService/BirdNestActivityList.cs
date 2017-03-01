using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the BirdNestActivity class.
    /// </summary>
    [Serializable()]
    public class BirdNestActivityList : DataIdList
    {
        /// <summary>
        /// Get BirdNestActivity with specified id.
        /// </summary>
        /// <param name='birdNestActivityId'>Id of requested bird nest activity.</param>
        /// <returns>Requested bird nest activity.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public BirdNestActivity Get(Int32 birdNestActivityId)
        {
            return (BirdNestActivity)(GetById(birdNestActivityId));
        }

        /// <summary>
        /// Get/set BirdNestActivity by list index.
        /// </summary>
        public new BirdNestActivity this[Int32 index]
        {
            get
            {
                return (BirdNestActivity)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
