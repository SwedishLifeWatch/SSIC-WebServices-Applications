using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebPhoneNumberType class.
    /// </summary>
    public static class WebPhoneNumberTypeExtension
    {
        private static String DEFAULT_NAME = "DEFAULT_NAME";

        /// <summary>
        /// Load data into the WebPhoneNumberType instance.
        /// </summary>
        /// <param name="phoneNumberType">This phone number type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebPhoneNumberType phoneNumberType,
                                    DataReader dataReader)
        {
            phoneNumberType.Id = dataReader.GetInt32(PhoneNumberTypeData.ID);
            phoneNumberType.NameStringId = dataReader.GetInt32(PhoneNumberTypeData.NAME_STRING_ID);
            phoneNumberType.Name = dataReader.GetString(PhoneNumberTypeData.NAME, DEFAULT_NAME);
        }
    }
}