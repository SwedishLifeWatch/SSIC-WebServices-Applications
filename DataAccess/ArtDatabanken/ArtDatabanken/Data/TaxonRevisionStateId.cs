using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of possible states for taxon revisions.
    /// </summary>
    [DataContract]
    public enum TaxonRevisionStateId
    {
        /// <summary>
        /// Created.
        /// </summary>
        [EnumMember]
        Created = 1,

        /// <summary>
        /// Ongoing.
        /// </summary>
        [EnumMember]
        Ongoing = 2,

        /// <summary>
        /// Closed.
        /// </summary>
        [EnumMember]
        Closed = 3
    }
}
