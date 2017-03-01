using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorDataType class.
    /// </summary>
    [Serializable]
    public class FactorDataTypeList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorDataTypeList class.
        /// </summary>
        public FactorDataTypeList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorDataTypeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorDataTypeList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get FactorDataType with specified factor DataType id.
        /// </summary>
        /// <param name='factorDataTypeId'>Id of requested factor DataType.</param>
        /// <returns>Requested factor DataType.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorDataType Get(Int32 factorDataTypeId)
        {
            return (FactorDataType)(GetById(factorDataTypeId));
        }

        /// <summary>
        /// Get/set FactorDataType by list index.
        /// </summary>
        public new FactorDataType this[Int32 index]
        {
            get
            {
                return (FactorDataType)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
