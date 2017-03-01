using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorUpdateMode class.
    /// </summary>
    public static class WebFactorUpdateModeExtension
    {
        /// <summary>
        /// Load data into the WebFactorUpdateMode instance.
        /// </summary>
        /// <param name="factorUpdateMode">The factor update mode instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorUpdateMode factorUpdateMode,
                                    DataReader dataReader)
        {
            factorUpdateMode.Definition = dataReader.GetString(FactorUpdateModeData.DEFINITION);
            factorUpdateMode.Id = dataReader.GetInt32(FactorUpdateModeData.ID);
            factorUpdateMode.IsUpdateAllowed = dataReader.GetBoolean(FactorUpdateModeData.IS_UPDATE_ALLOWED);
            factorUpdateMode.Name = dataReader.GetString(FactorUpdateModeData.NAME);
            factorUpdateMode.Type = (FactorUpdateModeType)Enum.Parse(typeof(FactorUpdateModeType), dataReader.GetString(FactorUpdateModeData.TYPE), true);
        }
    }
}