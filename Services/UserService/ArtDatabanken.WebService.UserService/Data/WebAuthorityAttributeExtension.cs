using ArtDatabanken.Database;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAuthorityAttribute class.
    /// </summary>
    public static class WebAuthorityAttributeExtension
    {
        /// <summary>
        /// Load data into the WebAuthorityAttribute instance.
        /// </summary>
        /// <param name="authorityAttribute">This authority attribute type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebAuthorityAttribute authorityAttribute,
                                    DataReader dataReader)
        {
            authorityAttribute.AuthorityId = dataReader.GetInt32(AuthorityData.AUTHORITY_ID);
            authorityAttribute.TypeId = dataReader.GetInt32(AuthorityData.AUTHORITY_ATTRIBUTE_TYPE_ID);
            authorityAttribute.Guid = dataReader.GetString(AuthorityData.ATTRIBUTE_VALUE);
        }
    }
}
