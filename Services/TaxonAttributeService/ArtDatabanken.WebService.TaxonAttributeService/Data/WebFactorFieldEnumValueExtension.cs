using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the factor field enumeration value class.
    /// </summary>
    public static class WebFactorFieldEnumValueExtension
    {
        /// <summary>
        /// Load data into the WebFactorFieldType instance.
        /// </summary>
        /// <param name="factorFieldEnumValue">The factor field enumeration value instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorFieldEnumValue factorFieldEnumValue, DataReader dataReader)
        {
            factorFieldEnumValue.EnumId = dataReader.GetInt32(FactorFieldEnumValueData.FACTOR_FIELD_ENUM_ID);
            factorFieldEnumValue.Id = dataReader.GetInt32(FactorFieldEnumValueData.ID);
            factorFieldEnumValue.IsKeyIntegerSpecified = dataReader.IsNotDbNull(FactorFieldEnumValueData.KEY_INT);
            factorFieldEnumValue.KeyText = dataReader.GetString(FactorFieldEnumValueData.KEY_TEXT);
            factorFieldEnumValue.KeyInteger = factorFieldEnumValue.IsKeyIntegerSpecified ? dataReader.GetInt32(FactorFieldEnumValueData.KEY_INT) : -999;
            factorFieldEnumValue.Information = dataReader.GetString(FactorFieldEnumValueData.INFORMATION);
            factorFieldEnumValue.Label = dataReader.GetString(FactorFieldEnumValueData.LABEL);
            factorFieldEnumValue.ShouldBeSaved = dataReader.GetBoolean(FactorFieldEnumValueData.SHOULD_BE_SAVED);
            factorFieldEnumValue.SortOrder = dataReader.GetInt32(FactorFieldEnumValueData.SORT_ORDER);
        }
    }
}