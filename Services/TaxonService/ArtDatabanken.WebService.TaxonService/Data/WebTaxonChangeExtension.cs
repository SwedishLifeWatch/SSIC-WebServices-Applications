using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for WebTaxonChange
    /// </summary>
    public static class WebTaxonChangeExtension
    {
        
        /// <summary>
        /// Load data into the WebTaxonChange instance.
        /// </summary>
        /// <param name='taxonChange'>TaxonChange.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonChange taxonChange, DataReader dataReader)
        {
            taxonChange.TaxonId = dataReader.GetInt32(TaxonChange.TAXON_ID);
            taxonChange.ScientificName = dataReader.GetString(TaxonChange.SCIENTIFIC_NAME);
            if (dataReader.IsNotDbNull(TaxonChange.TAXON_ID_AFTER))
            {
                taxonChange.TaxonIdAfter = dataReader.GetInt32(TaxonChange.TAXON_ID_AFTER);
            }
            if (dataReader.IsNotDbNull(TaxonChange.CATEGORY))
            {
                taxonChange.TaxonCategoryId = dataReader.GetInt32(TaxonChange.CATEGORY);
            }
            taxonChange.TaxonRevisionEventTypeId = dataReader.GetInt32(TaxonChange.EVENT_TYPE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonChange'>The taxon quality summary object.</param>
        public static void CheckData(this WebTaxonChange taxonChange)
        {
            if (!taxonChange.IsDataChecked)
            {
                taxonChange.CheckStrings();
                taxonChange.IsDataChecked = true;
            }
        }
    }
}
