using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionGeography class.
    /// </summary>
    public static class WebRegionGeometryExtension
    {
        /// <summary>
        /// Load data into the WebRegionGeometry instance.
        /// </summary>
        /// <param name="webRegionGeometry">This region geometry</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebRegionGeometry webRegionGeometry,
                                    DataReader dataReader)
        {
            webRegionGeometry.AreaDatasetId = dataReader.GetInt32("AreaDatasetId");
            webRegionGeometry.AreaDatasetSubTypeId = dataReader.GetInt32("AreaDatasetSubTypeId", 0);
            webRegionGeometry.AttributesXml = dataReader.GetString("AttributesXml");
            webRegionGeometry.BoundingBox = new WebBoundingBox();
            webRegionGeometry.BoundingBox.LoadData(dataReader, "GoogleBbox");
            webRegionGeometry.FeatureId = dataReader.GetString("FeatureId");
            webRegionGeometry.Id = dataReader.GetInt32("Id");
            webRegionGeometry.Name = dataReader.GetString("Name");
            webRegionGeometry.Polygon = new WebMultiPolygon();
            webRegionGeometry.Polygon.LoadData(dataReader, "GooglePolygon");
            webRegionGeometry.ShortName = dataReader.GetString("ShortName");
        }
    }
}
