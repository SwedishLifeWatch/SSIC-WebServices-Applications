using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Possible scope types when searching for taxa.
    /// </summary>
    [DataContract]
    public enum TaxonSearchScope
    {
        /// <summary>
        /// No special handling.
        /// </summary>
        [EnumMember]
        NoScope,

        /// <summary>
        /// Include all child taxa.
        /// </summary>
        [EnumMember]
        AllChildTaxa,

        /// <summary>
        /// Include all parent taxa.
        /// </summary>
        [EnumMember]
        AllParentTaxa
    }
}
