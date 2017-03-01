using ArtDatabanken.Data;
using ArtDatabanken.GIS.CoordinateConversion;

namespace ArtDatabanken.GIS.SwedenExtent
{
    /// <summary>
    /// This class contains methods and properties for getting the Sweden extent.
    /// </summary>
    public static class SwedenExtentManager
    {
        /// <summary>
        /// Gets the sweden extent bounding box.
        /// </summary>
        /// <param name="coordinateSystem">
        /// The coordinate system.
        /// </param>
        /// <returns>
        /// The sweden extent bounding box.
        /// </returns>
        public static BoundingBox GetSwedenExtentBoundingBox(CoordinateSystem coordinateSystem)
        {
            Polygon boundingBoxPolygon = GetSwedenExtentBoundingBoxPolygon(coordinateSystem);
            return boundingBoxPolygon.GetBoundingBox();
        }

        /// <summary>
        /// Gets the sweden extent bounding box polygon.
        /// </summary>
        /// <param name="coordinateSystem">
        /// The coordinate system.
        /// </param>
        /// <returns>
        /// The sweden extent as polygon.
        /// </returns>
        public static Polygon GetSwedenExtentBoundingBoxPolygon(CoordinateSystem coordinateSystem)
        {            
            var swedenBoundingBox = SwedenExtentCoordinates.GetSwedenExtentBoundingBoxSweref99();

            if (coordinateSystem.IsNotNull() && coordinateSystem.Id.IsNotNull() && (coordinateSystem.Id != CoordinateSystemId.SWEREF99_TM))
            {
                CoordinateSystem sweref99CoordinateSystem = new CoordinateSystem();
                sweref99CoordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
                Polygon boundingBox = GisTools.CoordinateConversionManager.GetConvertedBoundingBox(
                    swedenBoundingBox, sweref99CoordinateSystem, coordinateSystem);                
                return boundingBox;                
            }

            return swedenBoundingBox.GetPolygon();
        }
    }
}
