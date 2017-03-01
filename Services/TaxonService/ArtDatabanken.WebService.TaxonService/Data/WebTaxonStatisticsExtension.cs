using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for WebTaxonStatistics
    /// </summary>
    public static class WebTaxonStatisticsExtension
    {

        /// <summary>
        /// Get SwedishReproCount.
        /// </summary>
        /// <param name='taxonStatistics'>TaxonStatistics.</param>
        /// <returns>Value for SwedishReproCount.</returns>
        public static Int32 GetSwedishReproCount(this WebTaxonChildStatistics taxonStatistics)
        {
            return taxonStatistics.DataFields.GetInt32("SwedishReproCount");
        }
        
        /// <summary>
        /// Load data into the WebTaxonStatistics instance.
        /// </summary>
        /// <param name='taxonStatistics'>TaxonStatistics.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonChildStatistics taxonStatistics, DataReader dataReader)
        {
            taxonStatistics.RootTaxonId = dataReader.GetInt32(TaxonStatistics.ROOT_TAXON_ID);
            taxonStatistics.CategoryId = dataReader.GetInt32(TaxonCategoryData.CATEGORY_ID);
            taxonStatistics.ChildTaxaCount = dataReader.GetInt32(TaxonStatistics.NUMBER_IN_DYNTAXA);
            taxonStatistics.SwedishChildTaxaCount = dataReader.GetInt32(TaxonStatistics.NUMBER_OF_SWEDISH_OCCURRENCE);
            taxonStatistics.SetSwedishReproCount(dataReader.GetInt32(TaxonStatistics.NUMBER_OF_SWEDISH_REPRO));
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonStatistics'>The taxon statistics object.</param>
        public static void CheckData(this WebTaxonChildStatistics taxonStatistics)
        {
            if (!taxonStatistics.IsDataChecked)
            {
                taxonStatistics.CheckStrings();
                taxonStatistics.IsDataChecked = true;
            }
        }

        /// <summary>
        /// Set dynamic data property SwedishReproCount in TaxonChildStatistics.
        /// </summary>
        /// <param name='taxonChildStatistics'>TaxonChildStatistics.</param>
        /// <param name='swedishReproCount'>SwedishReproCount.</param>
        public static void SetSwedishReproCount(this WebTaxonChildStatistics taxonChildStatistics, Int32 swedishReproCount)
        {
            // Add version as dynamic property.
            WebDataField dataField = new WebDataField();
            dataField.Name = "SwedishReproCount";
            dataField.Type = WebDataType.Int32;
            dataField.Value = swedishReproCount.WebToString();
            if (taxonChildStatistics.DataFields.IsNull())
            {
                taxonChildStatistics.DataFields = new List<WebDataField>();
            }
            taxonChildStatistics.DataFields.Add(dataField);
        }
    }
}
