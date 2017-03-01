using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Alternative restrictions of returned data.
    /// </summary>
    [DataContract]
    public enum FactorSearchScope
    {
        /// <summary>
        /// No special scope.
        /// </summary>
        [EnumMember]
        NoScope,

        /// <summary>
        /// Include child factors.
        /// </summary>
        [EnumMember]
        AllChildFactors,

        /// <summary>
        /// Include parent factors.
        /// </summary>
        [EnumMember]
        AllParentFactors,

        /// <summary>
        /// Include nearest child factors.
        /// </summary>
        [EnumMember]
        NearestChildFactors,

        /// <summary>
        /// Include nearest parent factors.
        /// </summary>
        [EnumMember]
        NearestParentFactors,

        /// <summary>
        /// Include child leaf factors.
        /// </summary>
        [EnumMember]
        LeafFactors
    }
}