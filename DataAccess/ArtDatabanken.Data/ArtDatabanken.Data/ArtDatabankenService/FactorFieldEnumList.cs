using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorFieldEnum class.
    /// </summary>
    [Serializable]
    public class FactorFieldEnumList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorFieldEnumList class.
        /// </summary>
        public FactorFieldEnumList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorFieldEnumList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorFieldEnumList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get FactorFieldEnum with specified factor field enum id.
        /// </summary>
        /// <param name='factorFieldEnumId'>Id of requested factor field enum.</param>
        /// <returns>Requested factor field enum.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorFieldEnum Get(Int32 factorFieldEnumId)
        {
            return (FactorFieldEnum)(GetById(factorFieldEnumId));
        }

        /// <summary>
        /// Get/set FactorFieldEnum by list index.
        /// </summary>
        public new FactorFieldEnum this[Int32 index]
        {
            get
            {
                return (FactorFieldEnum)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
