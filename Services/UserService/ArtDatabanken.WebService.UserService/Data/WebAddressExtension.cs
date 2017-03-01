using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAddress class.
    /// </summary>
    public static class WebAddressExtension
    {

        /// <summary>
        /// Load data into the WebAddress instance.
        /// </summary>
        /// <param name="address">this address.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebAddress address,
                                    DataReader dataReader)
        {
            address.Id = dataReader.GetInt32(AddressData.ID);
            address.PersonId = dataReader.GetInt32(AddressData.PERSON_ID, 0);
            address.OrganizationId = dataReader.GetInt32(AddressData.ORGANIZATION_ID, 0);
            address.PostalAddress1 = dataReader.GetString(AddressData.POSTALADDRESS1);
            address.PostalAddress2 = dataReader.GetString(AddressData.POSTALADDRESS2);
            address.ZipCode = dataReader.GetString(AddressData.ZIPCODE);
            address.City = dataReader.GetString(AddressData.CITY);
            address.CountryId = dataReader.GetInt32(AddressData.COUNTRY_ID);
            address.TypeId = dataReader.GetInt32(AddressData.ADDRESS_TYPE_ID);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='address'>The address.</param>
        public static void CheckData(this WebAddress address)
        {
            if (!address.IsDataChecked)
            {
                address.CheckStrings();
                address.IsDataChecked = true;
            }
        }
    }
}
