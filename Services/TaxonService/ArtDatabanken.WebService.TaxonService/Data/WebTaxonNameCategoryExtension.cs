using ArtDatabanken.WebService.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for WebTaxonNameCategory
    /// </summary>
    public static class WebTaxonNameCategoryExtension
    {
        /// <summary>
        /// Load data into the WebTaxonNameCategory instance.
        /// </summary>
        /// <param name="taxonNameCategory"> The taxon name category</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonNameCategory taxonNameCategory, DataReader dataReader)
        {
            taxonNameCategory.Id = dataReader.GetInt32(TaxonCommon.ID);
            taxonNameCategory.IsLocaleIdSpecified = dataReader.IsNotDbNull(LocaleData.LOCALE_ID);
            if (taxonNameCategory.IsLocaleIdSpecified)
            {
                taxonNameCategory.LocaleId = dataReader.GetInt32(LocaleData.LOCALE_ID);
            }
            taxonNameCategory.Name = dataReader.GetString(TaxonCategoryData.CATEGORY_NAME);
            taxonNameCategory.ShortName = dataReader.GetString(TaxonCommon.SHORT_NAME);
            taxonNameCategory.SortOrder = dataReader.GetInt32(TaxonCommon.SORT_ORDER);
            taxonNameCategory.TypeId = dataReader.GetInt32(TaxonCategoryData.TYPE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonNameCategory'>The taxon name category.</param>
        public static void CheckData(this WebTaxonNameCategory taxonNameCategory)
        {
            if (!taxonNameCategory.IsDataChecked)
            {
                taxonNameCategory.CheckStrings();
                taxonNameCategory.IsDataChecked = true;
            }
        }
    }
}
