using ArtDatabanken.Database;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAuthorityAttributeType class.
    /// </summary>
    public static class WebAuthorityAttributeTypeExtension
    {

        /// <summary>
        /// Load data into the WebAuthorityAttributeType instance.
        /// </summary>
        /// <param name="authorityAttributeType">This authority attribute type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebAuthorityAttributeType authorityAttributeType,
                                    DataReader dataReader)
        {
            authorityAttributeType.Id = dataReader.GetInt32(AuthorityData.ID);
            authorityAttributeType.Identifier = dataReader.GetString(AuthorityData.AUTHORITY_ATTRIBUTE_TYPE);
        }
    }
}
