using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Definition of how search string is compared to
    /// stored string data in Dyntaxa.
    /// </summary>
    [DataContract]
    public enum DyntaxaStringCompareOperator
    {
        /// <summary>
        /// Search for strings that begins with the specified
        /// search string. Wild chard is added to the search
        /// string (at the end) before the search begins.
        /// </summary>
        [EnumMember]
        BeginsWith,

        /// <summary>
        /// Search for strings that contains the specified
        /// search string. Wild chards are added to the search string
        /// (at the beginning and end) before the search begins.
        /// </summary>
        [EnumMember]
        Contains,

        /// <summary>
        /// Search for strings that ends with the specified
        /// search string. Wild chard is added to the search
        /// string (at the beginning) before the search begins.
        /// </summary>
        [EnumMember]
        EndsWith,

        /// <summary>
        /// Search for strings that are equal to the specified
        /// search string.
        /// </summary>
        [EnumMember]
        Equal,

        /// <summary>
        /// Search for strings that similar the specified
        /// search string.
        /// No wild chards are added to the search string.
        /// </summary>
        [EnumMember]
        Like,

        /// <summary>
        /// Search in sequence Equal, BeginsWith, Contains
        /// search string.
        /// Checks result after each step in the sequence and returns if one (1) or more record(s) are found.
        /// </summary>
        [EnumMember]
        Iterative
    }
}
