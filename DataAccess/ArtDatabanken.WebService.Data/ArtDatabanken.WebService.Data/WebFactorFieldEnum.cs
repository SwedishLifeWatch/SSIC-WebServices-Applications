using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a factor field enumeration.
    /// </summary>
    [DataContract]
    public class WebFactorFieldEnum : WebData
    {
        /// <summary>
        /// Id for this factor field enumeration.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Factor field enumeration values for this enumeration
        /// </summary>
        [DataMember]
        public List<WebFactorFieldEnumValue> Values { get; set; }
    }
}