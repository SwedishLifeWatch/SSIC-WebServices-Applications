using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumerates the different types of taxon name categories.
    /// </summary>
    [DataContract]
    public enum TaxonNameCategoryTypeId
    {
        /// <summary>
        /// Scientific name.
        /// </summary>
        [EnumMember]
        ScientificName = 1,

        /// <summary>
        /// Common name.
        /// </summary>
        [EnumMember]
        CommonName = 2,

        /// <summary>
        /// Identifier.
        /// </summary>
        [EnumMember]
        Identifier = 3
    }
}
