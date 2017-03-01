using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorField class.
    /// </summary>
    [Serializable]
    public class FactorFieldList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorFieldList class.
        /// </summary>
        public FactorFieldList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorFieldList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorFieldList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get FactorField with specified factor field id.
        /// </summary>
        /// <param name='factorFieldId'>Id of requested factor field.</param>
        /// <returns>Requested factor field.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorField Get(Int32 factorFieldId)
        {
            return (FactorField)(GetById(factorFieldId));
        }

        /// <summary>
        /// Get/set FactorField by list index.
        /// </summary>
        public new FactorField this[Int32 index]
        {
            get
            {
                return (FactorField)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
