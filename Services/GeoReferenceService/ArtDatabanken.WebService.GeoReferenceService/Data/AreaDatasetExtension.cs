using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionGeography class.
    /// </summary>
    public static class AreaDatasetExtension
    {
        /// <summary>
        /// Load data into the WebRegionGeometry instance.
        /// </summary>
        /// <param name="areaDataset">This region geometry</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this AreaDataset areaDataset,
                                    DataReader dataReader)
        {
            areaDataset.Id = dataReader.GetInt32("Id");
            areaDataset.CountryIsoCode = dataReader.GetInt32("CountryIsoCode");
            areaDataset.Name = dataReader.GetString("Name");
            areaDataset.IsRequired = dataReader.GetBoolean("IsRequired");
            areaDataset.IsIndirect = dataReader.GetBoolean("IsIndirect");
            areaDataset.AllowOverrideIndirectType = dataReader.GetBoolean("AllowOverrideIndirectType");
            areaDataset.AreaDatasetCategoryId = dataReader.GetInt32("AreaDatasetCategoryId");
            areaDataset.SortOrder = dataReader.GetInt16("SortOrder");
            areaDataset.AttributesToHtmlXslt = dataReader.GetString("AttributesToHtmlXslt");
            areaDataset.AreaLevel = dataReader.GetInt32("AreaLevel");
            areaDataset.HasStatistics = dataReader.GetBoolean("HasStatistics");
            areaDataset.IsValidationArea = dataReader.GetBoolean("IsValidationArea");

        }
    }
}
