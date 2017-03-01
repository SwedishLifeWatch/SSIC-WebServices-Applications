using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the factor field enumeration class.
    /// </summary>
    public static class WebFactorFieldEnumExtension
    {
        /// <summary>
        /// Load data into the factor field enumeration instance.
        /// </summary>
        /// <param name="factorFieldEnum">The factor field enumeration instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorFieldEnum factorFieldEnum, DataReader dataReader)
        {
            factorFieldEnum.Id = dataReader.GetInt32(FactorFieldEnumData.ID);
            factorFieldEnum.Values = new List<WebFactorFieldEnumValue>();
        }
    }
}