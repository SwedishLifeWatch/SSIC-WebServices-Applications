using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of taxon lump split event types.
    /// </summary>
    [DataContract]
    public enum LumpSplitEventTypeId
    {
        /// <summary>
        /// Lump of taxa.
        /// </summary>
        [EnumMember]
        Lump = 2,
        /// <summary>
        /// Split of taxa.
        /// </summary>
        [EnumMember]
        Split = 4
    }
}
