using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum for read list categories.
    /// </summary>
    [DataContract]
    public enum RedListCategory
    {
        /// <summary>
        /// Extinct.
        /// </summary>
        [EnumMember]
        EX = -1,
        /// <summary>
        /// Data Deficient.
        /// </summary>
        [EnumMember]
        DD = 0,
        /// <summary>
        /// Regionally Extinct.
        /// </summary>
        [EnumMember]
        RE = 1,
        /// <summary>
        /// Critically Endangered.
        /// </summary>
        [EnumMember]
        CR = 2,
        /// <summary>
        /// Endangered.
        /// </summary>
        [EnumMember]
        EN = 3,
        /// <summary>
        /// Vulnerable.
        /// </summary>
        [EnumMember]
        VU = 4,
        /// <summary>
        /// Near Threatened.
        /// </summary>
        [EnumMember]
        NT = 5,
        /// <summary>
        /// Least Concern.
        /// </summary>
        [EnumMember]
        LC = 6,
        /// <summary>
        /// Not Applicable.
        /// </summary>
        [EnumMember]
        NA = 7,
        /// <summary>
        /// Not Evaluated.
        /// </summary>
        [EnumMember]
        NE = 8
    }
}
