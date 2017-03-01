using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorOrigin class.
    /// </summary>
    public static class WebFactorOriginExtension
    {
        /// <summary>
        /// Load data into the WebFactorOrigin instance.
        /// </summary>
        /// <param name="factorOrigin">The factor origin instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorOrigin factorOrigin,
                                    DataReader dataReader)
        {
            factorOrigin.Definition = dataReader.GetString(FactorOriginData.DEFINITION);
            factorOrigin.Id = dataReader.GetInt32(FactorOriginData.ID);
            factorOrigin.Name = dataReader.GetString(FactorOriginData.NAME);
            factorOrigin.SortOrder = dataReader.GetInt32(FactorOriginData.SORT_ORDER);
        }
    }
}
