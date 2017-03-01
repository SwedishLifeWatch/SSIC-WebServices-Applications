using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionCategory class.
    /// </summary>
    public static class WebRegionCategoryExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="regionCategory">This region category.</param>
        public static void CheckData(this WebRegionCategory regionCategory)
        {
            regionCategory.Name.CheckNotEmpty("regionCategory.Name");
            regionCategory.Name = regionCategory.Name.CheckInjection();
        }

        /// <summary>
        /// Load data into the WebRegionCategory instance.
        /// </summary>
        /// <param name="regionCategory">This region category.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebRegionCategory regionCategory,
                                    DataReader dataReader)
        {
            regionCategory.Id = dataReader.GetInt32(RegionCategoryData.ID);
            regionCategory.IsCountryIsoCodeSpecified = dataReader.IsNotDbNull(RegionCategoryData.COUNTRY_ISO_CODE);
            if (regionCategory.IsCountryIsoCodeSpecified)
            {
                regionCategory.CountryIsoCode = dataReader.GetInt32(RegionCategoryData.COUNTRY_ISO_CODE);
            }
            regionCategory.IsLevelSpecified = dataReader.IsNotDbNull(RegionCategoryData.LEVEL);
            if (regionCategory.IsLevelSpecified)
            {
                regionCategory.Level = dataReader.GetInt32(RegionCategoryData.LEVEL);    
            }
            regionCategory.Name = dataReader.GetString(RegionCategoryData.NAME);
            regionCategory.SortOrder = dataReader.GetInt32(RegionCategoryData.SORT_ORDER);
            regionCategory.TypeId = dataReader.GetInt32(RegionCategoryData.TYPE_ID);
        }
    }
}
