using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IAddressType interface.
    /// </summary>
    [Serializable]
    public class AddressTypeList : DataId32List<IAddressType>
    {
        /// <summary>
        /// Get address type with specified id.
        /// </summary>
        /// <param name='addressTypeId'>Id of address type.</param>
        /// <returns>Requested address type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public IAddressType Get(AddressTypeId addressTypeId)
        {
            return Get((Int32)addressTypeId);
        }
    }
}
