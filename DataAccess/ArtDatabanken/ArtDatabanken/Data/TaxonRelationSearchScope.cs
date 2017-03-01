using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Possible scopes when retrieving taxon relations.
    /// </summary>
    [DataContract]
    public enum TaxonRelationSearchScope
    {
        /// <summary>
        /// All child relations.
        /// </summary>
        [EnumMember]
        AllChildRelations,

        /// <summary>
        /// All parent relations.
        /// </summary>
        [EnumMember]
        AllParentRelations,

        /// <summary>
        /// Nearest child relations.
        /// </summary>
        [EnumMember]
        NearestChildRelations,

        /// <summary>
        /// Nearest parent relations.
        /// </summary>
        [EnumMember]
        NearestParentRelations
    }
}
