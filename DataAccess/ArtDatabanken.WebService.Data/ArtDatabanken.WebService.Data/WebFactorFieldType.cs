using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a factor field type.
    /// </summary>
    [DataContract]
    public class WebFactorFieldType : WebData
    {
        /// <summary>
        /// Definition for this factor field type.
        /// </summary>
        [DataMember]
        public String Definition { get; set; }

        /// <summary>
        /// Id for this factor field type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this factor field type.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
