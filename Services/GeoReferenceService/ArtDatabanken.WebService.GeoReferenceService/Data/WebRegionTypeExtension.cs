using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionType class.
    /// </summary>
    public static class WebRegionTypeExtension
    {
        /// <summary>
        /// Load data into the WebRegionType instance.
        /// </summary>
        /// <param name="regionType">This region type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebRegionType regionType,
                                    DataReader dataReader)
        {
            regionType.Id = dataReader.GetInt32(RegionTypeData.ID);
            regionType.Name = dataReader.GetString(RegionTypeData.NAME);
        }
    }
}
