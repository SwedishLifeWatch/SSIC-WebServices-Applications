using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class handles search criterias that
    /// are used to find authorities.
    /// The operator 'AND' is used between the
    /// different search conditions.
    /// </summary>
    [DataContract]
    public class WebAuthoritySearchCriteria : WebData
    {
        /// <summary>
        /// Finds Authority Identifier.
        /// </summary>
        [DataMember]
        public String AuthorityIdentifier
        { get; set; }

        /// <summary>
        /// Finds application identifier.
        /// </summary>
        [DataMember]
        public String ApplicationIdentifier
        { get; set; }

        /// <summary>
        /// Finds authority data type identifier.
        /// </summary>
        [DataMember]
        public String AuthorityDataTypeIdentifier
        { get; set; }

        /// <summary>
        /// Finds authority name ie name of the authority.
        /// </summary>
        [DataMember]
        public String AuthorityName
        { get; set; }
    }
}
