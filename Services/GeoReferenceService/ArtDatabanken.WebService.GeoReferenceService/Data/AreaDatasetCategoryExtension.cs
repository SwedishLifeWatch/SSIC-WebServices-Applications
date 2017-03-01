using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionGeography class.
    /// </summary>
    public static class AreaDatasetCategoryExtension
    {
        /// <summary>
        /// Load data into the WebRegionGeometry instance.
        /// </summary>
        /// <param name="areaDataset">This region geometry</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this AreaDatasetCategory areaDataset,
                                    DataReader dataReader)
        {
            areaDataset.Id = dataReader.GetInt32("Id");
            areaDataset.Name = dataReader.GetString("Name");

        }
    }
}
