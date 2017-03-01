using ArtDatabanken.WebService.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for WebTaxonCategory
    /// </summary>
    public static class WebTaxonCategoryExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonCategory'>The taxon Category.</param>
        public static void CheckData(this WebTaxonCategory taxonCategory)
        {
            if (!taxonCategory.IsDataChecked)
            {
                taxonCategory.CheckStrings();
                taxonCategory.IsDataChecked = true;
            }
        }

        /// <summary>
        /// Load data into the WebTaxon instance.
        /// </summary>
        /// <param name='taxonCategory'>Taxon category.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonCategory taxonCategory, DataReader dataReader)
        {
            taxonCategory.Id = dataReader.GetInt32(TaxonCommon.ID);
            taxonCategory.IsMainCategory = dataReader.GetBoolean(TaxonCategoryData.MAIN_CATEGORY);
            taxonCategory.IsTaxonomic = dataReader.GetBoolean(TaxonCategoryData.TAXONOMIC);
            taxonCategory.Name = dataReader.GetString(TaxonCategoryData.CATEGORY_NAME);
            taxonCategory.ParentId = dataReader.GetInt32(TaxonCategoryData.PARENT_CATEGORY);
            taxonCategory.SortOrder = dataReader.GetInt32(TaxonCommon.SORT_ORDER);
        }
    }
}
