using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactor class.
    /// </summary>
    public static class WebFactorExtension
    {
        /// <summary>
        /// Load data into the WebFactor instance.
        /// </summary>
        /// <param name="factor">The factor instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactor factor,
                                    DataReader dataReader)
        {
            factor.DataTypeId = dataReader.GetInt32(FactorData.FACTOR_DATA_TYPE_ID, -1);
            factor.DefaultHostParentTaxonId = dataReader.GetInt32(FactorData.DEFAULT_HOST_PARENT_TAXON_ID, 0);
            factor.HostLabel = dataReader.GetString(FactorData.HOST_LABEL);
            factor.Id = dataReader.GetInt32(FactorData.ID);
            factor.Information = dataReader.GetString(FactorData.INFORMATION);
            factor.IsDataTypeIdSpecified = dataReader.IsNotDbNull(FactorData.FACTOR_DATA_TYPE_ID);
            factor.IsLeaf = dataReader.GetBoolean(FactorData.IS_LEAF, false);
            factor.IsPeriodic = dataReader.GetBoolean(FactorData.IS_PERIODIC, false);
            factor.IsPublic = dataReader.GetBoolean(FactorData.IS_PUBLIC, false);
            factor.IsTaxonomic = factor.HostLabel.IsNotNull();
            factor.Label = dataReader.GetString(FactorData.LABEL);
            factor.Name = dataReader.GetString(FactorData.NAME);
            factor.OriginId = dataReader.GetInt32(FactorData.FACTOR_ORIGIN_ID);
            factor.SortOrder = dataReader.GetInt32(FactorData.SORT_ORDER);
            factor.UpdateModeId = dataReader.GetInt32(FactorData.FACTOR_UPDATE_MODE_ID);
        }
    }
}