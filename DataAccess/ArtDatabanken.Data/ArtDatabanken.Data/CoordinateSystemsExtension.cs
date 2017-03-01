using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains extension methods for coordinate system classes/enums.
    /// </summary>
    public static class CoordinateSystemsExtension
    {
        /// <summary>
        /// Gets the coordinate system id.
        /// </summary>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>The coordinate system id.</returns>        
        public static CoordinateSystemId GetCoordinateSystemId(this GridCoordinateSystem gridCoordinateSystem)
        {
            switch (gridCoordinateSystem)
            {
                case GridCoordinateSystem.GoogleMercator:
                    return CoordinateSystemId.GoogleMercator;
                case GridCoordinateSystem.Rt90_25_gon_v:
                    return CoordinateSystemId.Rt90_25_gon_v;
                case GridCoordinateSystem.SWEREF99_TM:
                    return CoordinateSystemId.SWEREF99_TM;
                case GridCoordinateSystem.UnKnown:
                    return CoordinateSystemId.None;
                default:
                    throw new ArgumentException("Unhandled value: " + gridCoordinateSystem);
            }
        }


        /// <summary>
        /// Converts a GridCoordinateSystem to a CoordinateSystem.
        /// </summary>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>The CoordinateSystem that is equal to GridCoordinateSystem.</returns>
        public static CoordinateSystem ToCoordinateSystem(this GridCoordinateSystem gridCoordinateSystem)
        {
            return new CoordinateSystem(GetCoordinateSystemId(gridCoordinateSystem));
        }

        /// <summary>
        /// Converts a GridCoordinateSystem to a WebCoordinateSystem.
        /// </summary>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>The WebCoordinateSystem that is equal to GridCoordinateSystem.</returns>
        public static WebCoordinateSystem ToWebCoordinateSystem(this GridCoordinateSystem gridCoordinateSystem)
        {
            return new WebCoordinateSystem {Id = GetCoordinateSystemId(gridCoordinateSystem)};
        }


        /// <summary>
        /// Gets the srid for the specified grid coordinate system.
        /// </summary>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>The coordinate system srid.</returns>
        public static int Srid(this GridCoordinateSystem gridCoordinateSystem)
        {
            switch (gridCoordinateSystem)
            {
                case GridCoordinateSystem.GoogleMercator:
                    return 900913;
                case GridCoordinateSystem.Rt90_25_gon_v:
                    return 3021;
                case GridCoordinateSystem.SWEREF99_TM:
                    return 3006;
                case GridCoordinateSystem.UnKnown:
                    return 0;
                default:
                    throw new ArgumentException("Unhandled value: " + gridCoordinateSystem);
            }
        }

        /// <summary>
        /// Gets the GridCoordinateSystem for the specified srid.
        /// </summary>
        /// <param name="srId">The coordinate system srid</param>
        /// <returns>The grid coordinate system.</returns>
        public static GridCoordinateSystem ToGridCoordinateSystem(this int srId)
        {
            switch (srId)
            {
                case 900913:
                    return GridCoordinateSystem.GoogleMercator;
                case 3021:
                    return GridCoordinateSystem.Rt90_25_gon_v;
                case 3006:
                    return GridCoordinateSystem.SWEREF99_TM;
                case 0:
                    return GridCoordinateSystem.UnKnown;
                default:
                    throw new ArgumentException("Unhandled value: " + srId);
            }
        }          
    }
}
