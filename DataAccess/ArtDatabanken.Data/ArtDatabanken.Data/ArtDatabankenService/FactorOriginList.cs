using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Factor Origin class.
    /// </summary>
    [Serializable]
    public class FactorOriginList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorOriginList class.
        /// </summary>
        public FactorOriginList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorOriginList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorOriginList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get Factor Origin with specified id.
        /// </summary>
        /// <returns>Requested factor origins.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorOrigin Get(Int32 referenceId)
        {
            return (FactorOrigin)(GetById(referenceId));
        }

        /// <summary>
        /// Get/set Factor Origin by list index.
        /// </summary>
        public new FactorOrigin this[Int32 index]
        {
            get
            {
                return (FactorOrigin)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
