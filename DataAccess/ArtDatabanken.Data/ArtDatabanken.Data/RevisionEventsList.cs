using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IRevisionEvent interface.
    /// </summary>
    [Serializable()]
    public class RevisionEventList : DataIdList
    {
        /// <summary>
        /// Get revision event with specified id.
        /// </summary>
        /// <param name='revisionEventId'>Id of revision event.</param>
        /// <returns>Requested revision event.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public IRevisionEvent Get(Int32 revisionEventId)
        {
            return (IRevisionEvent)(GetById(revisionEventId));
        }

        /// <summary>
        /// Get/set IRegion by list index.
        /// </summary>
        public new IRevisionEvent this[Int32 index]
        {
            get
            {
                return (IRevisionEvent)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
