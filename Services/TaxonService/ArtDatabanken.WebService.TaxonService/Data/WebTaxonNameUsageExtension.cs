using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for WebTaxonNameUsage
    /// </summary>
    public static class WebTaxonNameUsageExtension
    {
        /// <summary>
        /// Load data into the WebTaxonNameUsage instance.
        /// </summary>
        /// <param name='taxonNameUsage'>WebTaxonNameUsage object.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonNameUsage taxonNameUsage,
                                    DataReader dataReader)
        {
            taxonNameUsage.Id = dataReader.GetInt32(TaxonCommon.ID);
            taxonNameUsage.Name = dataReader.GetString(TaxonNameUsageData.NAME);
            taxonNameUsage.Description = dataReader.GetString(TaxonNameUsageData.DESCRIPTION);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonNameUsage'>The TaxonNameUsage object.</param>
        public static void CheckData(this WebTaxonNameUsage taxonNameUsage)
        {
            if (!taxonNameUsage.IsDataChecked)
            {
                taxonNameUsage.CheckStrings();
                taxonNameUsage.IsDataChecked = true;
            }
        }
    }
}
