using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the County class.
    /// </summary>
    [Serializable()]
    public class CountyList : DataIdList
    {
        /// <summary>
        /// Get County with specified id.
        /// </summary>
        /// <param name='countyId'>Id of requested county.</param>
        /// <returns>Requested county.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public County Get(Int32 countyId)
        {
            return (County)(GetById(countyId));
        }

        /// <summary>
        /// Get/set County by list index.
        /// </summary>
        public new County this[Int32 index]
        {
            get
            {
                return (County)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
