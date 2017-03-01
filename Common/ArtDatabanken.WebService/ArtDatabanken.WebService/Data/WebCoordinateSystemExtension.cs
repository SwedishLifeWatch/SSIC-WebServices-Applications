using System;
using ArtDatabanken.Data;
using ProjNet.Converters.WellKnownText;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebCoordinateSystem class.
    /// </summary>
    public static class WebCoordinateSystemExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="coordinateSystem">This coordinate system.</param>
        public static void CheckData(this WebCoordinateSystem coordinateSystem)
        {
            String wkt;

            coordinateSystem.CheckNotNull("coordinateSystem");
            if (coordinateSystem.Id == CoordinateSystemId.None)
            {
                // Verify externally defined WKT.
                // Internally defined WKT's are assumed to be correct.
                wkt = coordinateSystem.GetWkt();
                wkt.CheckNotEmpty("coordinateSystem.wkt");
                try
                {
                    CoordinateSystemWktReader.Parse(wkt);
                }
                catch
                {
                    throw new ArgumentException("Wrong format in coordinate system: " + wkt);
                }
            }
        }

        /// <summary>
        /// Get coordinate system wkt.
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system.</param>
        /// <returns>Coordinate system wkt.</returns>
        public static String GetWkt(this WebCoordinateSystem coordinateSystem)
        {
            return coordinateSystem.Id.GetWkt(coordinateSystem.WKT);
        }

        /// <summary>
        /// Get coordinate system as string.
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system.</param>
        /// <returns>Coordinate system as string.</returns>
        public static String WebToString(this WebCoordinateSystem coordinateSystem)
        {
            if (coordinateSystem.IsNull())
            {
                return String.Empty;
            }
            else
            {
                return "Coordinate system: Id = " + coordinateSystem.Id + 
                       ", Wkt = [" + coordinateSystem.GetWkt() + "]" +
                       coordinateSystem.DataFields.WebToString();
            }
        }
    }
}
