using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an WebAuthorityDataType
    /// </summary>
    [DataContract]
    public class WebAuthorityDataType : WebData
    {
        /// <summary>
        /// Id for this AuthorityDataType
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }
    }
}
