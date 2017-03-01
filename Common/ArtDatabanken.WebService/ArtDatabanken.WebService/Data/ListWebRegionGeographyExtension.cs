using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list of
    /// type WebRegionGeography.
    /// </summary>
    public static class ListWebRegionGeographyExtension
    {
        /// <summary>
        /// Get region geography with specified id.
        /// </summary>
        /// <param name="regionsGeography">Regions geography.</param>
        /// <param name="regionId">Region id.</param>
        /// <returns>Region geography with specified id.</returns>
        public static WebRegionGeography Get(this List<WebRegionGeography> regionsGeography,
                                             Int32 regionId)
        {
            if (regionsGeography.IsNotEmpty())
            {
                foreach (WebRegionGeography regionGeography in regionsGeography)
                {
                    if (regionId == regionGeography.Id)
                    {
                        return regionGeography;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get region geography with specified GUID.
        /// </summary>
        /// <param name="regionsGeography">Regions geography.</param>
        /// <param name="regionGuid">Region GUID.</param>
        /// <returns>Region geography with specified GUID.</returns>
        public static WebRegionGeography Get(this List<WebRegionGeography> regionsGeography,
                                             String regionGuid)
        {
            if (regionsGeography.IsNotEmpty())
            {
                foreach (WebRegionGeography regionGeography in regionsGeography)
                {
                    if (regionGuid.ToUpper() == regionGeography.GUID.ToUpper())
                    {
                        return regionGeography;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Test if point is located inside any of the regions.
        /// Currently only two dimensions are handled.
        /// </summary>
        /// <param name="regionsGeography">Regions geography.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in region.</param>
        /// <param name='point'>Point.</param>
        /// <returns>True if point is located inside at least one of the regions.</returns>
        public static Boolean IsPointInsideGeography(this List<WebRegionGeography> regionsGeography,
                                                     WebServiceContext context,
                                                     WebCoordinateSystem coordinateSystem,
                                                     WebPoint point)
        {
            if (regionsGeography.IsNotEmpty())
            {
                foreach (WebRegionGeography regionGeography in regionsGeography)
                {
                    if (regionGeography.IsPointInsideGeography(context, coordinateSystem, point))
                    {
                        // Species observation is inside one of the regions.
                        return true;
                    }
                }
            }

            // Species observation is not inside any of the regions.
            return false;
        }

        /// <summary>
        /// Test if point is located inside any of the regions.
        /// Currently only two dimensions are handled.
        /// </summary>
        /// <param name="regionsGeography">Regions geography.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in region.</param>
        /// <param name='point'>Point.</param>
        /// <returns>True if point is located inside at least one of the regions.</returns>
        public static Boolean IsPointInsideGeometry(this List<WebRegionGeography> regionsGeography,
                                                    WebServiceContext context,
                                                    WebCoordinateSystem coordinateSystem,
                                                    WebPoint point)
        {
            if (regionsGeography.IsNotEmpty())
            {
                foreach (WebRegionGeography regionGeography in regionsGeography)
                {
                    if (regionGeography.IsPointInsideGeometry(context, coordinateSystem, point))
                    {
                        // Species observation is inside one of the regions.
                        return true;
                    }
                }
            }

            // Species observation is not inside any of the regions.
            return false;
        }
    }
}
