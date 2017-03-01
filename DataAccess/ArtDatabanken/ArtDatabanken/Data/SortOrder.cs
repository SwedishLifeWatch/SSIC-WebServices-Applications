using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of possible sort orders.
    /// </summary>
    [DataContract]
    public enum SortOrder
    {
        /// <summary>Ascending.</summary>
        [EnumMember]
        Ascending,

        /// <summary>Descending.</summary>
        [EnumMember]
        Descending
    }
}
