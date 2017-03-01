using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Possible scopes when retrieving taxon trees.
    /// </summary>
    [DataContract]
    public enum TaxonTreeSearchScope
    {
        /// <summary>
        /// All child taxa.
        /// </summary>
        [EnumMember]
        AllChildTaxa,

        /// <summary>
        /// All parent taxa.
        /// </summary>
        [EnumMember]
        AllParentTaxa,

        /// <summary>
        /// Nearest child taxa.
        /// </summary>
        [EnumMember]
        NearestChildTaxa,

        /// <summary>
        /// Nearest parent taxa.
        /// </summary>
        [EnumMember]
        NearestParentTaxa
    }
}
