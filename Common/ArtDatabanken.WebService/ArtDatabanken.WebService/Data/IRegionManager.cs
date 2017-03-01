using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// interface that handles region related information.
    /// </summary>
    public interface IRegionManager
    {
        /// <summary>
        /// Get ids for specified regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <returns>Ids for specified regions.</returns>       
        List<Int32> GetRegionIdsByGuids(WebServiceContext context,
                                        List<String> regionGuids);

        /// <summary>
        /// Get specified regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <returns>Geography for regions.</returns>       
        List<WebRegion> GetRegionsByGuids(WebServiceContext context,
                                          List<String> regionGuids);

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        List<WebRegionGeography> GetRegionsGeographyByGuids(WebServiceContext context,
                                                            List<String> regionGuids,
                                                            WebCoordinateSystem coordinateSystem);
 
        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        List<WebRegionGeography> GetRegionsGeographyByIds(WebServiceContext context,
                                                          List<Int32> regionIds,
                                                          WebCoordinateSystem coordinateSystem);
    }
}
