using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorUpdateMode class.
    /// </summary>
    [Serializable]
    public class FactorUpdateModeList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorUpdateModeList class.
        /// </summary>
        public FactorUpdateModeList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorUpdateModeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorUpdateModeList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get FactorUpdateMode with specified id.
        /// </summary>
        /// <param name='factorUpdateModeId'>Id of requested factor update mode.</param>
        /// <returns>Requested factor update mode.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorUpdateMode Get(Int32 factorUpdateModeId)
        {
            return (FactorUpdateMode)(GetById(factorUpdateModeId));
        }

        /// <summary>
        /// Get/set FactorUpdateMode by list index.
        /// </summary>
        public new FactorUpdateMode this[Int32 index]
        {
            get
            {
                return (FactorUpdateMode)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

