using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A classification of the need for communication of
    /// problems related to the taxon status and recognition.
    /// </summary>
    [DataContract]
    public enum TaxonAlertStatusId
    {
        /// <summary>
        /// The taxon is valid and no particular problems
        /// exists concerning its classification and recognition.
        /// </summary>
        [EnumMember]
        Green = 0,

        /// <summary>
        /// The taxon is valid but it is usually mixed up
        /// with other taxa.
        /// </summary>
        [EnumMember]
        Yellow = 1,

        /// <summary>
        /// The taxon is not valid.
        /// </summary>
        [EnumMember]
        Red = 2
    }
}
