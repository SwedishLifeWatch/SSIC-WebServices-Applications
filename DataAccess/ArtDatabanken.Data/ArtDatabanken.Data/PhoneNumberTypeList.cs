using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPhoneNumberType interface.
    /// </summary>
    [Serializable]
    public class PhoneNumberTypeList : DataId32List<IPhoneNumberType>
    {
        /// <summary>
        /// Get phone number type with specified id.
        /// </summary>
        /// <param name='phoneNumberTypeId'>Id of phone number type.</param>
        /// <returns>Requested phone number type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public IPhoneNumberType Get(PhoneNumberTypeId phoneNumberTypeId)
        {
            return Get((Int32)phoneNumberTypeId);
        }
    }
}

