using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorField class.
    /// </summary>
    public static class WebFactorFieldExtension
    {
        /// <summary>
        /// Load data into the WebFactorField instance.
        /// </summary>
        /// <param name="factorField">The factor field instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorField factorField, DataReader dataReader)
        {
            factorField.DatabaseFieldName = dataReader.GetString(FactorFieldData.DATABASE_FIELD_NAME);
            factorField.FactorDataTypeId = dataReader.GetInt32(FactorFieldData.FACTOR_DATA_TYPE_ID);
            factorField.EnumId = dataReader.GetInt32(FactorFieldData.FACTOR_FIELD_ENUM_ID, -1);
            factorField.Id = dataReader.GetInt32(FactorFieldData.ID);
            factorField.Information = dataReader.GetString(FactorFieldData.INFORMATION);
            factorField.IsEnumField = dataReader.IsNotDbNull(FactorFieldData.FACTOR_FIELD_ENUM_ID);
            factorField.IsMain = dataReader.GetBoolean(FactorFieldData.IS_MAIN);
            factorField.IsSubstantial = dataReader.GetBoolean(FactorFieldData.IS_SUBSTANTIAL);
            factorField.Label = dataReader.GetString(FactorFieldData.LABEL);
            factorField.Size = dataReader.GetInt32(FactorFieldData.SIZE, -1);
            factorField.TypeId = dataReader.GetInt32(FactorFieldData.FACTOR_FIELD_TYPE_ID);
            factorField.Unit = dataReader.GetString(FactorFieldData.UNIT);
        }
    }
}