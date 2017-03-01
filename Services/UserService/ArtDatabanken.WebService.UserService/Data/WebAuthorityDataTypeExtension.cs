using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAuthorityDataType class.
    /// </summary>
    public static class WebAuthorityDataTypeExtension
    {
        /// <summary>
        /// Load data from data reader into the WebAuthorityDataType instance.
        /// </summary>
        /// <param name="authorityDataType">This authority data type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebAuthorityDataType authorityDataType,
                                    DataReader dataReader)
        {
            authorityDataType.Id = dataReader.GetInt32(AuthorityDataType.ID);
            authorityDataType.Identifier = dataReader.GetString(AuthorityDataType.AUTHORITY_DATA_TYPE_IDENTITY);
        }
    }
}
