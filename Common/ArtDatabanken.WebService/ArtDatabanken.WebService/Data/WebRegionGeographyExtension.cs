using System;
using System.Web.Caching;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionGeography class.
    /// </summary>
    public static class WebRegionGeographyExtension
    {
        /// <summary>
        /// Get a SqlGeography instance with same information as 
        /// property MultiPolygon in provided WebRegionGeography.
        /// </summary>
        /// <param name="regionGeography">This region geography.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in region.</param>
        /// <returns>
        /// A SqlGeography instance with same information as 
        /// property MultiPolygon in provided WebRegionGeography.
        /// </returns>
        private static SqlGeography GetMultiPolygonGeography(this WebRegionGeography regionGeography,
                                                             WebServiceContext context,
                                                             WebCoordinateSystem coordinateSystem)
        {
            SqlGeography geographyMultiPolygon;
            String cacheKey;

            // Get cached information.
            cacheKey = "RegionSqlGeography:" +
                       regionGeography.Id +
                       ":CoordinateSystem:" +
                       coordinateSystem.GetWkt();
            geographyMultiPolygon = (SqlGeography)context.GetCachedObject(cacheKey);

            if (geographyMultiPolygon.IsNull())
            {
                geographyMultiPolygon = regionGeography.MultiPolygon.GetGeography();

                // Add information to cache.
                context.AddCachedObject(cacheKey, geographyMultiPolygon, new TimeSpan(1, 0, 0), CacheItemPriority.BelowNormal);
            }

            return geographyMultiPolygon;
        }

        /// <summary>
        /// Get a SqlGeometry instance with same information as 
        /// property MultiPolygon in provided WebRegionGeography.
        /// </summary>
        /// <param name="regionGeography">This region geography.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in region.</param>
        /// <returns>
        /// A SqlGeometry instance with same information as 
        /// property MultiPolygon in provided WebRegionGeography.
        /// </returns>
        private static SqlGeometry GetMultiPolygonGeometry(this WebRegionGeography regionGeography,
                                                           WebServiceContext context,
                                                           WebCoordinateSystem coordinateSystem)
        {
            SqlGeometry geometryMultiPolygon;
            String cacheKey;

            // Get cached information.
            cacheKey = "RegionSqlGeometry:" +
                       regionGeography.Id +
                       ":CoordinateSystem:" +
                       coordinateSystem.GetWkt();
            geometryMultiPolygon = (SqlGeometry)context.GetCachedObject(cacheKey);

            if (geometryMultiPolygon.IsNull())
            {
                geometryMultiPolygon = regionGeography.MultiPolygon.GetGeometry();

                // Add information to cache.
                context.AddCachedObject(cacheKey, geometryMultiPolygon, new TimeSpan(1, 0, 0), CacheItemPriority.BelowNormal);
            }

            return geometryMultiPolygon;
        }

        /// <summary>
        /// Test if point is located inside region.
        /// Currently only two dimensions are handled.
        /// </summary>
        /// <param name="regionGeography">This region geography.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in region.</param>
        /// <param name='point'>Point.</param>
        /// <returns>True if point is located inside region.</returns>
        public static Boolean IsPointInsideGeography(this WebRegionGeography regionGeography,
                                                     WebServiceContext context,
                                                     WebCoordinateSystem coordinateSystem,
                                                     WebPoint point)
        {
            SqlGeography geographyMultiPolygon, geographyPoint;

            if (regionGeography.BoundingBox.IsPointInside(point))
            {
                geographyPoint = point.GetGeography();
                geographyMultiPolygon = regionGeography.GetMultiPolygonGeography(context, coordinateSystem);
                return geographyMultiPolygon.STIntersects(geographyPoint).Value;
            }
            else
            {
                // Species observation can not be inside region
                // since it is not inside the regions bounding box.
                return false;
            }
        }

        /// <summary>
        /// Test if point is located inside region.
        /// Currently only two dimensions are handled.
        /// </summary>
        /// <param name="regionGeography">This region geography.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in region.</param>
        /// <param name='point'>Point.</param>
        /// <returns>True if point is located inside region.</returns>
        public static Boolean IsPointInsideGeometry(this WebRegionGeography regionGeography,
                                                    WebServiceContext context,
                                                    WebCoordinateSystem coordinateSystem,
                                                    WebPoint point)
        {
            SqlGeometry geometryMultiPolygon, geometryPoint;

            if (regionGeography.BoundingBox.IsPointInside(point))
            {
                geometryPoint = point.GetGeometry();
                geometryMultiPolygon = regionGeography.GetMultiPolygonGeometry(context, coordinateSystem);
                return geometryMultiPolygon.STIntersects(geometryPoint).Value;
            }
            else
            {
                // Species observation can not be inside region
                // since it is not inside the regions bounding box.
                return false;
            }
        }
    }
}
