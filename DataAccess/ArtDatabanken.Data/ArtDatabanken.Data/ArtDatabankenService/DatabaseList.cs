using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Database class.
    /// </summary>
    [Serializable()]
    public class DatabaseList : DataIdList
    {
        /// <summary>
        /// Get Database with specified id.
        /// </summary>
        /// <param name='databaseId'>Id of requested database.</param>
        /// <returns>Requested database.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public Database Get(Int32 databaseId)
        {
            return (Database)(GetById(databaseId));
        }

        /// <summary>
        /// Get/set Database by list index.
        /// </summary>
        public new Database this[Int32 index]
        {
            get
            {
                return (Database)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
