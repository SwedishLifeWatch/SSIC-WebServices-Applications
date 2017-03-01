using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
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
        /// None.
        /// </summary>
        [EnumMember]
        None,

        /// <summary>
        /// WKT used for RT90.
        /// GEOGCS["RT90",
        ///     DATUM["Rikets_koordinatsystem_1990",
        ///         SPHEROID["Bessel 1841",6377397.155,299.1528128,
        ///             AUTHORITY["EPSG","7004"]],
        ///         AUTHORITY["EPSG","6124"]],
        ///     PRIMEM["Greenwich",0,
        ///         AUTHORITY["EPSG","8901"]],
        ///     UNIT["degree",0.01745329251994328,
        ///         AUTHORITY["EPSG","9122"]],
        ///     AUTHORITY["EPSG","4124"]]
        /// </summary>
        [EnumMember]
        RT90,

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
        [EnumMember]
        Rt90_25_gon_v,

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
        [EnumMember]
        SWEREF99,

        /// <summary>
        /// WKT used for WGS84.
        /// GEOGCS["GCS_WGS_1984",
        ///     DATUM["WGS_1984",
        ///         SPHEROID["WGS_1984",6378137,298.257223563]],
        ///     PRIMEM["Greenwich",0],
        ///     UNIT["Degree",0.017453292519943295]]
        /// </summary>
        [EnumMember]
        WGS84
    }
}
