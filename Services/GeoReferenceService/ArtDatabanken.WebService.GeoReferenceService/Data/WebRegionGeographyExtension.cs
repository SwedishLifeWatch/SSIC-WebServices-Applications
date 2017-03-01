using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionGeography class.
    /// </summary>
    public static class WebRegionGeographyExtension
    {
        /// <summary>
        /// Load data into the WebRegionGeography instance.
        /// </summary>
        /// <param name="regionGeography">This region geography.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebRegionGeography regionGeography,
                                    DataReader dataReader)
        {
            Int32 categoryId;
            String nativeId;

            regionGeography.Id = dataReader.GetInt32(RegionData.ID);
            regionGeography.BoundingBox = new WebBoundingBox();
            regionGeography.BoundingBox.LoadData(dataReader, RegionData.BOUNDING_BOX);
            categoryId = dataReader.GetInt32(RegionData.CATEGORY_ID);
            nativeId = dataReader.GetString(RegionData.NATIVE_ID);
            regionGeography.GUID = new RegionGUID(categoryId, nativeId).GUID;
            regionGeography.MultiPolygon = new WebMultiPolygon();
            regionGeography.MultiPolygon.LoadData(dataReader, RegionData.POLYGON);
        }
    }
}
