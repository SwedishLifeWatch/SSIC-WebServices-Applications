using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorFieldType class.
    /// </summary>
    public static class WebFactorFieldTypeExtension
    {
        /// <summary>
        /// Load data into the WebFactorFieldType instance.
        /// </summary>
        /// <param name="factorFieldType">The factor field type instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorFieldType factorFieldType,
                                    DataReader dataReader)
        {
            factorFieldType.Definition = dataReader.GetString(FactorFieldTypeData.DEFINITION_SWEDISH);
            factorFieldType.Id = dataReader.GetInt32(FactorFieldTypeData.ID);
            factorFieldType.Name = dataReader.GetString(FactorFieldTypeData.NAME);
        }
    }
}