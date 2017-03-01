using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorFieldEnumValue class.
    /// </summary>
    [Serializable]
    public class FactorFieldEnumValueList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorFieldEnumValueList class.
        /// </summary>
        public FactorFieldEnumValueList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the FactorFieldEnumValueList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorFieldEnumValueList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get FactorFieldEnumValue with specified factor field enum value id.
        /// </summary>
        /// <param name='factorFieldEnumValueId'>Id of requested factor field enum value.</param>
        /// <returns>Requested factor field enum value.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorFieldEnumValue Get(Int32 factorFieldEnumValueId)
        {
            return (FactorFieldEnumValue)(GetById(factorFieldEnumValueId));
        }

        /// <summary>
        /// Get/set FactorFieldEnumValue by list index.
        /// </summary>
        public new FactorFieldEnumValue this[Int32 index]
        {
            get
            {
                return (FactorFieldEnumValue)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Get a subset of the Factor Field Enum Value List object with specified factor field enum id
        /// </summary>
        /// <param name="factorFieldEnumId">Factor Field Enum Id</param>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        /// <returns>A list of Factor Field Enum Values</returns>
        public FactorFieldEnumValueList GetSubSetByEnumId(Int32 factorFieldEnumId)
        {
            FactorFieldEnumValueList factorFieldEnumValues = new FactorFieldEnumValueList();
            foreach (FactorFieldEnumValue factorFieldEnumValue in this)
            {
                if (factorFieldEnumValue.FactorFieldEnumId == factorFieldEnumId)
                {
                    factorFieldEnumValues.Add(factorFieldEnumValue);
                }
            }

            if (factorFieldEnumValues.Count > 0)
            {
                return factorFieldEnumValues;
            }
            
            // No data found with requested id.
            throw new ArgumentException("No data with id " + factorFieldEnumId + "!");
        }
        


    }
}
