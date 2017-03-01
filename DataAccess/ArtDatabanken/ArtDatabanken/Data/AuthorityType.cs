using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumerates the different types of authorities that web services handles.
    /// </summary>
    [DataContract]
    public enum AuthorityType
    {
        /// <summary>
        /// DataType is a type of Authority that is set if authority
        /// is address to an AuthorityDataType i.e. authority that is not
        /// addressed to an application i.e. could be used in several applications.
        /// </summary>
        [EnumMember]
        DataType,

        /// <summary>
        /// Application is a type of Authority that is set if authority
        /// is address to an application.
        /// </summary>
        [EnumMember]
        Application
     }
}
