using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Is used to decide which information that should be
    /// returned together with the taxon information.
    /// </summary>
    [DataContract]
    public enum FactorUpdateModeType
    {
        /// <summary>
        /// Can not be changed.
        /// </summary>
        [EnumMember]
        Archive,

        /// <summary>
        /// Is changed by automatic calculations.
        /// </summary>
        [EnumMember]
        AutomaticUpdate,

        /// <summary>
        /// Factor without data. Used as header.
        /// </summary>
        [EnumMember]
        Header,

        /// <summary>
        /// Can be changed by user anytime.
        /// </summary>
        [EnumMember]
        ManualUpdate
    }
}
