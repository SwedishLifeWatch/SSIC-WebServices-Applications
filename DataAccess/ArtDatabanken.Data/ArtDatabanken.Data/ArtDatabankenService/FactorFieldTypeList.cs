using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorFieldType class.
    /// </summary>
    [Serializable]
    public class FactorFieldTypeList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorFieldTypeList class.
        /// </summary>
        public FactorFieldTypeList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorFieldTypeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorFieldTypeList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get FactorFieldType with specified factor field type id.
        /// </summary>
        /// <param name='factorFieldTypeId'>Id of requested factor field type.</param>
        /// <returns>Requested factor field type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorFieldType Get(Int32 factorFieldTypeId)
        {
            return (FactorFieldType)(GetById(factorFieldTypeId));
        }

        /// <summary>
        /// Get/set FactorFieldType by list index.
        /// </summary>
        public new FactorFieldType this[Int32 index]
        {
            get
            {
                return (FactorFieldType)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
