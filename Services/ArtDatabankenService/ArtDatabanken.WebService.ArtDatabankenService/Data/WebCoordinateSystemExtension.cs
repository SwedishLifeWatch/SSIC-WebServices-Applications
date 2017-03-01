using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ProjNet.Converters.WellKnownText;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
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

        /// <summary>
        /// Get coordinate system wkt.
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system.</param>
        /// <returns>Coordinate system wkt.</returns>
        public static String GetWkt(this WebCoordinateSystem coordinateSystem)
        {
            switch (coordinateSystem.Id)
            {
                case CoordinateSystemId.None:
                    return coordinateSystem.WKT;
                case CoordinateSystemId.GoogleMercator:
                    return ArtDatabanken.Settings.Default.GoogleMercator_WKT;
                case CoordinateSystemId.Rt90_25_gon_v:
                    return ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
                case CoordinateSystemId.SWEREF99:
                // SWEREF99 is mapped to SWEREF99_TM in order to
                // support old definition of the enum.
                // This should be changed in the future when
                // SWEREF99 no longer is used in its old meaning.
                case CoordinateSystemId.SWEREF99_TM:
                    return ArtDatabanken.Settings.Default.SWEREF99_TM_WKT;
                case CoordinateSystemId.WGS84:
                    return ArtDatabanken.Settings.Default.WGS84_WKT;
                //case CoordinateSystemId.ETRS89_LAEA:
                //    return ArtDatabanken.Settings.Default.ETRS89_LAEA;
                default:
                    throw new ArgumentException("Not handled coordinate system id " + coordinateSystem.Id.ToString());
            }
        }
    }
}
