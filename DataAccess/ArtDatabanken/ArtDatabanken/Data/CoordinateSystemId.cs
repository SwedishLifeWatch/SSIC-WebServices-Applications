using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumeration of predefined coordinate system.
    /// This enumeration makes it easier to handle commonly
    /// used coordinate systems.
    /// Definitions of WKT's was retrieved from
    /// http://spatialreference.org/
    /// </summary>
    [DataContract]
    public enum CoordinateSystemId
    {
        /// <summary>
        /// No coordinate system has been specified.
        /// </summary>
        [EnumMember]
        None,

        /// <summary>
        /// WKT used for GoogleMercator.
        /// PROJCS["Google Mercator",
        ///     GEOGCS["WGS 84",
        ///         DATUM["World Geodetic System 1984",
        ///             SPHEROID["WGS 84", 6378137.0, 298.257223563,
        ///                 AUTHORITY["EPSG","7030"]],
        ///             AUTHORITY["EPSG","6326"]],
        ///         PRIMEM["Greenwich", 0.0,
        ///             AUTHORITY["EPSG","8901"]],
        ///         UNIT["degree", 0.017453292519943295],
        ///         AXIS["Geodetic latitude", NORTH],
        ///         AXIS["Geodetic longitude", EAST],
        ///         AUTHORITY["EPSG","4326"]],
        ///     PROJECTION["Mercator_1SP"],
        ///     PARAMETER["semi_minor", 6378137.0],
        ///     PARAMETER["latitude_of_origin", 0.0],
        ///     PARAMETER["central_meridian", 0.0],
        ///     PARAMETER["scale_factor", 1.0],
        ///     PARAMETER["false_easting", 0.0],
        ///     PARAMETER["false_northing", 0.0],
        ///     UNIT["m", 1.0],
        ///     AXIS["Easting", EAST],
        ///     AXIS["Northing", NORTH],
        ///     AUTHORITY["EPSG","900913"]]
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), EnumMember]
        GoogleMercator,

        /// <summary>
        /// WKT from Stein Hoem RT90 + SWEREF99 (?)
        ///  "PROJCS[\"SWEREF99 / RT90 2.5 gon V emulation\",
        ///     GEOGCS[\"SWEREF99\",
        ///         DATUM[\"SWEREF99\",
        ///         SPHEROID[\"GRS 1980\",6378137.0,298.257222101,
        ///             AUTHORITY[\"EPSG\",\"7019\"]],
        ///         TOWGS84[0.0,0.0,0.0,0.0,0.0,0.0,0.0],
        ///             AUTHORITY[\"EPSG\",\"6619\"]],
        ///         PRIMEM[\"Greenwich\",0.0,
        ///             AUTHORITY[\"EPSG\",\"8901\"]],
        ///             UNIT[\"degree\",0.017453292519943295],
        ///             AXIS[\"Geodetic latitude\",NORTH],
        ///             AXIS[\"Geodetic longitude\",EAST],
        ///             AUTHORITY[\"EPSG\",\"4619\"]],
        ///         PROJECTION[\"Transverse Mercator\"],
        ///             PARAMETER[\"central_meridian\",15.806284529444449],
        ///             PARAMETER[\"latitude_of_origin\",0.0],
        ///             PARAMETER[\"scale_factor\",1.00000561024],
        ///             PARAMETER[\"false_easting\",1500064.274],
        ///             PARAMETER[\"false_northing\",-667.711],
        ///             UNIT[\"m\",1.0],
        ///             AXIS[\"Northing\",NORTH],
        ///             AXIS[\"Easting\",EAST],
        ///             AUTHORITY[\"EPSG\",\"3847\"]]";
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rt")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "gon")]
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rt")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "gon")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "v")]
        [EnumMember]
        // ReSharper disable once InconsistentNaming
        Rt90_25_gon_v,

        /// <summary>
        /// This coordinate system id is currently not used.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SWEREF")]
        [EnumMember]
        // ReSharper disable once InconsistentNaming
        SWEREF99,

        /// <summary>
        /// WKT used for SWEREF99.
        /// PROJCS["SWEREF99 TM",
        ///     GEOGCS["SWEREF99",
        ///         DATUM["SWEREF99",
        ///             SPHEROID["GRS 1980",6378137,298.257222101,
        ///                 AUTHORITY["EPSG","7019"]],
        ///             TOWGS84[0,0,0,0,0,0,0],
        ///             AUTHORITY["EPSG","6619"]],
        ///         PRIMEM["Greenwich",0,
        ///             AUTHORITY["EPSG","8901"]],
        ///         UNIT["degree",0.01745329251994328,
        ///             AUTHORITY["EPSG","9122"]],
        ///         AUTHORITY["EPSG","4619"]],
        ///     UNIT["metre",1,
        ///         AUTHORITY["EPSG","9001"]],
        ///     PROJECTION["Transverse_Mercator"],
        ///     PARAMETER["latitude_of_origin",0],
        ///     PARAMETER["central_meridian",15],
        ///     PARAMETER["scale_factor",0.9996],
        ///     PARAMETER["false_easting",500000],
        ///     PARAMETER["false_northing",0],
        ///     AUTHORITY["EPSG","3006"],
        ///     AXIS["y",EAST],
        ///     AXIS["x",NORTH]]
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SWEREF")]
        [EnumMember]
        // ReSharper disable once InconsistentNaming
        SWEREF99_TM,

        /// <summary>
        /// WKT used for WGS84.
        /// GEOGCS["GCS_WGS_1984",
        ///     DATUM["WGS_1984",
        ///         SPHEROID["WGS_1984",6378137,298.257223563]],
        ///     PRIMEM["Greenwich",0],
        ///     UNIT["Degree",0.017453292519943295]]
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "WGS")]
        [EnumMember]
        // ReSharper disable once InconsistentNaming
        WGS84

        ///// <summary>
        ///// WKT used for WGS84.
        ///// GEOGCS["GCS_WGS_1984",
        /////     DATUM["WGS_1984",
        /////         SPHEROID["WGS_1984",6378137,298.257223563]],
        /////     PRIMEM["Greenwich",0],
        /////     UNIT["Degree",0.017453292519943295]]
        ///// </summary>
        // [EnumMember]
        // ETRS89_LAEA
    }

    public static class CoordinateSystemIdExtensions
    {
        /// <summary>
        /// Get WKT for specified Coordinate system id
        /// </summary>
        /// <param name="coordinateSystemId"></param>
        /// <param name="ownWkt">WKT to return if coordinate system id equals None</param>
        /// <returns></returns>
        public static string GetWkt(this CoordinateSystemId coordinateSystemId, string ownWkt = null)
        {
            switch (coordinateSystemId)
            {
                case CoordinateSystemId.None:
                    return ownWkt;
                case CoordinateSystemId.GoogleMercator:
                    return Settings.Default.GoogleMercator_WKT;
                case CoordinateSystemId.Rt90_25_gon_v:
                    return Settings.Default.Rt90_25_gon_v_WKT;
                case CoordinateSystemId.SWEREF99:
                // SWEREF99 is mapped to SWEREF99_TM in order to
                // support old definition of the enum.
                // This should be changed in the future when
                // SWEREF99 no longer is used in its old meaning.
                case CoordinateSystemId.SWEREF99_TM:
                    return Settings.Default.SWEREF99_TM_WKT;
                case CoordinateSystemId.WGS84:
                    return Settings.Default.WGS84_WKT;
                default:
                    throw new ArgumentException("Not handled coordinate system id " + coordinateSystemId);
            }
        }

        /// <summary>
        /// Gets the Srid for the specified coordinate system.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system id.</param>
        /// <returns>The Srid.</returns>
        public static int Srid(this CoordinateSystemId coordinateSystemId)
        {
            switch (coordinateSystemId)
            {
                case CoordinateSystemId.GoogleMercator:
                    return 900913;
                case CoordinateSystemId.Rt90_25_gon_v:
                    return 3021;
                case CoordinateSystemId.SWEREF99:
                // SWEREF99 is mapped to SWEREF99_TM in order to
                // support old definition of the enum.
                // This should be changed in the future when
                // SWEREF99 no longer is used in its old meaning.                
                case CoordinateSystemId.SWEREF99_TM:
                    return 3006;
                case CoordinateSystemId.WGS84:
                    return 4326;
                case CoordinateSystemId.None:
                    return 0;
                default:
                    throw new ArgumentException(coordinateSystemId + " is not handled");
            }
        }

        /// <summary>
        /// Gets the EPSG code for the coordinate system.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system id.</param>
        /// <returns>The EPSG code.</returns>
        public static string EpsgCode(this CoordinateSystemId coordinateSystemId)
        {
            return string.Format("EPSG:{0}", coordinateSystemId.Srid());
        }  
    }
}
