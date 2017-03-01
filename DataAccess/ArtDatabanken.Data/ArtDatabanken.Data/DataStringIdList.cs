using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the DataStringId class.
    /// </summary>
    [Serializable()]
    public class DataStringIdList : ArrayList
    {
        /// <summary>
        /// Get data with specified string id.
        /// </summary>
        /// <param name='id'>Id of requested data.</param>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        /// <returns>Requested item.</returns>
        protected DataStringId GetById(String id)
        {
            foreach (DataStringId dataId in this)
            {
                if (dataId.Id == id)
                {
                    // Data found. Return it.
                    return dataId;
                }
            }

            // No data found with requested id.
            throw new ArgumentException("No data with id " + id + "!");
        }
    }
}

