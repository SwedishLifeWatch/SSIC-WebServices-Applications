using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.GIS.CoordinateConversion
{
    /// <summary>
    /// Contains extension to classes implementing the ICoordinateSystem interface.
    /// </summary>
    public static class ICoordinateSystemExtension
    {
        /// <summary>
        /// Get coordinate system wkt.
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system.</param>
        /// <returns>Coordinate system wkt.</returns>
        public static String GetWkt(this ICoordinateSystem coordinateSystem)
        {
            return coordinateSystem.Id.GetWkt();
        }
    }
}