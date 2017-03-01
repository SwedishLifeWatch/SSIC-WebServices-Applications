using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Each enumeration value specifies a predefined
    /// set of species observation fields.
    /// </summary>
    [DataContract]
    public enum SpeciesObservationSpecificationId
    {
        /// <summary>
        /// No species observation fields.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// All data for each species observation.
        /// </summary>
        [EnumMember]
        All,

        /// <summary>
        /// The same set of species observation fields as defined by
        /// Species Information Centers extended definition of Darwin Core.
        /// </summary>
        [EnumMember]
        DarwinCore,

        /// <summary>
        /// The minimum set of species observation fields that can 
        /// be used to plot species observations on a map.
        /// Included species observation fields:
        /// Class: DarwinCore           Property: Id.
        /// Class: Location             Property: CoordinateX.
        /// Class: Location             Property: CoordinateY.
        /// </summary>
        [EnumMember]
        MapMinimum
    }
}
