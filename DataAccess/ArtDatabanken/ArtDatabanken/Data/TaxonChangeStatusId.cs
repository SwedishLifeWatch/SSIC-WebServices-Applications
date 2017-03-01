using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum that contains all possible taxon change statuses.
    /// </summary>
    [DataContract]
    public enum TaxonChangeStatusId
    {
        /// <summary>
        /// The taxon concept has split into several taxa.
        /// The taxon concept is no longer valid.
        /// </summary>
        [EnumMember]
        InvalidDueToSplit = -4,

        /// <summary>
        /// The taxon concept has been lumped (together with other
        /// taxon concepts) into one new taxon concept.
        /// The taxon concept is no longer valid.
        /// </summary>
        [EnumMember]
        InvalidDueToLump = -2,

        /// <summary>
        /// The taxon concept has been deleted.
        /// The taxon concept is no longer valid.
        /// </summary>
        [EnumMember]
        InvalidDueToDelete = -1,

        /// <summary>
        /// Unchanged taxon concept.
        /// </summary>
        [EnumMember]
        Unchanged = 0,

        /// <summary>
        /// Taxon concept which resulted from a lumping
        /// of at least two taxa.
        /// </summary>
        [EnumMember]
        ValidAfterLump = 2,

        /// <summary>
        /// One of the new taxon concepts as a result
        /// from a splitting of another taxon.
        /// </summary>
        [EnumMember]
        ValidAfterSplit = 4
    }
}
