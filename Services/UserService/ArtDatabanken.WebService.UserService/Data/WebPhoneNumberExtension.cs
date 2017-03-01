using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAddress class.
    /// </summary>
    public static class WebPhoneNumberExtension
    {

        /// <summary>
        /// Load data into the WebPhoneNumber instance.
        /// </summary>
        /// <param name="phoneNumber">this phoneNumber.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebPhoneNumber phoneNumber,
                                    DataReader dataReader)
        {
            phoneNumber.Id = dataReader.GetInt32(PhoneNumberData.ID);
            phoneNumber.PersonId = dataReader.GetInt32(PhoneNumberData.PERSON_ID, 0);
            phoneNumber.OrganizationId = dataReader.GetInt32(PhoneNumberData.ORGANIZATION_ID, 0);
            phoneNumber.Number = dataReader.GetString(PhoneNumberData.PHONENUMBER);
            phoneNumber.CountryId = dataReader.GetInt32(PhoneNumberData.COUNTRY_ID);
            phoneNumber.TypeId = dataReader.GetInt32(PhoneNumberData.PHONENUMBER_TYPE_ID);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='phoneNumber'>The phone number.</param>
        public static void CheckData(this WebPhoneNumber phoneNumber)
        {
            if (!phoneNumber.IsDataChecked)
            {
                phoneNumber.CheckStrings();
                phoneNumber.IsDataChecked = true;
            }
        }
    }
}
