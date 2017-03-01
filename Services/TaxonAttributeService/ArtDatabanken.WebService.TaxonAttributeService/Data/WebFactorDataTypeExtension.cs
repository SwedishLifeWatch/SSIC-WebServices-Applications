using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorDataType class.
    /// </summary>
    public static class WebFactorDataTypeExtension
    {
        /// <summary>
        /// Load data into the WebFactorDataType instance.
        /// </summary>
        /// <param name="factorDataType">The factor data type instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorDataType factorDataType,
                                    DataReader dataReader)
        {
            factorDataType.Definition = dataReader.GetString(FactorDataTypeData.DEFINITION);
            factorDataType.Fields = new List<WebFactorField>();
            factorDataType.Id = dataReader.GetInt32(FactorDataTypeData.ID);
            factorDataType.Name = dataReader.GetString(FactorDataTypeData.NAME);
        }
    }
}