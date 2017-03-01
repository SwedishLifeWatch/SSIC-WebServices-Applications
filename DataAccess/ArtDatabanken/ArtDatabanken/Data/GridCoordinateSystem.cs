using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of predefined grid cell coordinate system.
    /// This enumeration makes it easier to handle supported
    /// (by ArtDatabanken) grid cell coordinate systems.
    /// </summary>
    [DataContract]
    public enum GridCoordinateSystem
    {
        /// <summary>
        /// Name used for finding grid cell data for planar GoogleMercator reference frame.
        /// </summary>
        [EnumMember]
        UnKnown,

        /// <summary>
        /// Name used for finding grid cell data for planar GoogleMercator reference frame.
        /// </summary>
        [EnumMember]
        GoogleMercator,
       
        ///// <summary>
        ///// Name used for finding grid cell data for RT90 in SpeciesObservationDatabase.
        ///// </summary>
        //[EnumMember]
        //RT90

        /// <summary>
        /// Name used for finding grid cell data for the old swedish national reference frame 
        /// RT 90 2.5 gon V 0:-15 in SpeciesObservationDatabase.
        /// </summary>
        [EnumMember]
        Rt90_25_gon_v,

        /// <summary>
        /// Name used for finding grid cell data for the current swedish national referenace frame
        /// SWEREF 99 TM in SpeciesObservationDatabase.
        /// </summary>
        [EnumMember]
        SWEREF99_TM

        ///// <summary>
        ///// Name used for finding grid cell data for ETRS89_LAEA in SpeciesObservationDatabase.
        ///// </summary>
        //[EnumMember]
        //ETRS89_LAEA
    }
}
