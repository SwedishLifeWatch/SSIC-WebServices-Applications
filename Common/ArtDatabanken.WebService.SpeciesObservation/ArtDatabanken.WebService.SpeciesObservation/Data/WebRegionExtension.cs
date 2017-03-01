using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Contains extension to the WebRegion class.
    /// </summary>
    public static class WebRegionExtension
    {
        /// <summary>
        /// Load data into the WebRegion instance.
        /// </summary>
        /// <param name="region">This region.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebRegion region,
                                    DataReader dataReader)
        {
            region.CategoryId = dataReader.GetInt32(RegionData.CATEGORY_ID);
            region.Id = dataReader.GetInt32(RegionData.ID);
            region.Name = dataReader.GetString(RegionData.NAME);
            region.NativeId = dataReader.GetString(RegionData.NATIVE_ID);
            region.ShortName = dataReader.GetString(RegionData.SHORT_NAME);
            region.SortOrder = Int32.MinValue;
            region.GUID = new RegionGUID(region.CategoryId, region.NativeId).GUID;
        }
    }
}
