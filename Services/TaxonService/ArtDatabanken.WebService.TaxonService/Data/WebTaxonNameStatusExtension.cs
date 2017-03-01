using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for WebTaxonNameStatus
    /// </summary>
    public static class WebTaxonNameStatusExtension
    {
        /// <summary>
        /// Load data into the WebTaxonNameStatus instance.
        /// </summary>
        /// <param name='taxonNameStatus'>WebTaxonNameStatus object.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonNameStatus taxonNameStatus,
                                    DataReader dataReader)
        {
            taxonNameStatus.Id = dataReader.GetInt32(TaxonCommon.ID);
            taxonNameStatus.Name = dataReader.GetString(TaxonNameUsageData.NAME);
            taxonNameStatus.Description = dataReader.GetString(TaxonNameUsageData.DESCRIPTION);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonNameStatus'>The TaxonNameStatus object.</param>
        public static void CheckData(this WebTaxonNameStatus taxonNameStatus)
        {
            if (!taxonNameStatus.IsDataChecked)
            {
                taxonNameStatus.CheckStrings();
                taxonNameStatus.IsDataChecked = true;
            }
        }
    }
}
