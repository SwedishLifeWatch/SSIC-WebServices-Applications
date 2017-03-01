using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for WebTaxonQualitySummary
    /// </summary>
    public static class WebTaxonQualitySummaryExtension
    {
        
        /// <summary>
        /// Load data into the WebTaxonQualitySummary instance.
        /// </summary>
        /// <param name='taxonQualitySummary'>TaxonQualitySummary.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonChildQualityStatistics taxonQualitySummary, DataReader dataReader)
        {
            taxonQualitySummary.RootTaxonId = dataReader.GetInt32(TaxonStatistics.ROOT_TAXON_ID);
            taxonQualitySummary.QualityId = dataReader.GetInt32(TaxonStatistics.QUALITY_CATEGORY);
            taxonQualitySummary.ChildTaxaCount = dataReader.GetInt32(TaxonStatistics.NUMBER_OF_TAXA);

        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonQualitySummary'>The taxon quality summary object.</param>
        public static void CheckData(this WebTaxonChildQualityStatistics taxonQualitySummary)
        {
            if (!taxonQualitySummary.IsDataChecked)
            {
                taxonQualitySummary.CheckStrings();
                taxonQualitySummary.IsDataChecked = true;
            }
        }
    }
}
