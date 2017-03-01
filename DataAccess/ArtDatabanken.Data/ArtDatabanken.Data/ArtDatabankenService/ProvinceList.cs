using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Province class.
    /// </summary>
    [Serializable()]
    public class ProvinceList : DataIdList
    {
        /// <summary>
        /// Get Province with specified id.
        /// </summary>
        /// <param name='provinceId'>Id of requested province.</param>
        /// <returns>Requested province.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public Province Get(Int32 provinceId)
        {
            return (Province)(GetById(provinceId));
        }

        /// <summary>
        /// Get/set Province by list index.
        /// </summary>
        public new Province this[Int32 index]
        {
            get
            {
                return (Province)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
