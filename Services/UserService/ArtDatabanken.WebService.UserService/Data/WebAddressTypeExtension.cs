using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAddressType class.
    /// </summary>
    public static class WebAddressTypeExtension
    {
        private static String DEFAULT_NAME = "DEFAULT_NAME";

        /// <summary>
        /// Load data into the WebAddressType instance.
        /// </summary>
        /// <param name="addressType">This address type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebAddressType addressType,
                                    DataReader dataReader)
        {
            addressType.Id = dataReader.GetInt32(AddressTypeData.ID);
            addressType.NameStringId = dataReader.GetInt32(AddressTypeData.NAME_STRING_ID);
            addressType.Name = dataReader.GetString(AddressTypeData.NAME, DEFAULT_NAME);
        }
    }
}
