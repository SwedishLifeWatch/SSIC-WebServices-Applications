using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Interface to manager of coordinate conversions.
    /// </summary>
    public interface ICoordinateConversionManager
    {
        /// <summary>
        /// Convert bounding box from one coordinate system to
        /// another coordinate system.
        /// Converted bounding box is returned as a polygon
        /// since it probably is not a rectangle any more.
        /// </summary>
        /// <param name="boundingBox">Bounding box that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Polygon with coordinates according to toCoordinateSystem</returns>
        WebPolygon GetConvertedBoundingBox(WebBoundingBox boundingBox,
                                           WebCoordinateSystem fromCoordinateSystem,
                                           WebCoordinateSystem toCoordinateSystem);

            /// <summary>
        /// Convert linear rings from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="linearRings">Linear rings that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Linear rings with coordinates according to toCoordinateSystem</returns>
        List<WebLinearRing> GetConvertedLinearRings(List<WebLinearRing> linearRings,
                                                    WebCoordinateSystem fromCoordinateSystem,
                                                    WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert a multi polygon from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="multiPolygon">Multi polygon that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Multi polygon with coordinates according to toCoordinateSystem</returns>
        WebMultiPolygon GetConvertedMultiPolygon(WebMultiPolygon multiPolygon,
                                                 WebCoordinateSystem fromCoordinateSystem,
                                                 WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert multi polygons from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="multiPolygons">Multi polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Multi polygons with coordinates according to toCoordinateSystem</returns>
        List<WebMultiPolygon> GetConvertedMultiPolygons(List<WebMultiPolygon> multiPolygons,
                                                        WebCoordinateSystem fromCoordinateSystem,
                                                        WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert a point from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="fromPoint">Point that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>WebPoint with coordinates according to toCoordinateSystem</returns>
        WebPoint GetConvertedPoint(WebPoint fromPoint, 
                                   WebCoordinateSystem fromCoordinateSystem,
                                   WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert a point from one coordinate system to
        /// another coordinate system.
        /// This version of GetConvertedPoint uses caching.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="fromPoint">Point that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>WebPoint with coordinates according to toCoordinateSystem</returns>
        WebPoint GetConvertedPoint(WebServiceContext context,
                                   WebPoint fromPoint,
                                   WebCoordinateSystem fromCoordinateSystem,
                                   WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert points from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="fromPoints">Points that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Points with coordinates according to toCoordinateSystem</returns>
        List<WebPoint> GetConvertedPoints(List<WebPoint> fromPoints,
                                          WebCoordinateSystem fromCoordinateSystem,
                                          WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert polygon from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="polygon">Polygon that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Polygon with coordinates according to toCoordinateSystem</returns>
        WebPolygon GetConvertedPolygon(WebPolygon polygon,
                                       WebCoordinateSystem fromCoordinateSystem,
                                       WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert polygons from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Polygons with coordinates according to toCoordinateSystem</returns>
        List<WebPolygon> GetConvertedPolygons(List<WebPolygon> polygons,
                                              WebCoordinateSystem fromCoordinateSystem,
                                              WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Convert region geography from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="regionGeography">Region geography that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Region geography with coordinates according to toCoordinateSystem</returns>
        WebRegionGeography GetConvertedRegionGeography(WebRegionGeography regionGeography,
                                                       WebCoordinateSystem fromCoordinateSystem,
                                                       WebCoordinateSystem toCoordinateSystem);
    }
}
