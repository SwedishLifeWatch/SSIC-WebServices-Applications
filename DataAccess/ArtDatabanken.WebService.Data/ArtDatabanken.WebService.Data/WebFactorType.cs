using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a factor type.
    /// This class is currently not used.
    /// </summary>
    [DataContract]
    public class WebFactorType : WebData
    {
        /// <summary>
        /// Description for this factor type.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for this factor type.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name of this factor type.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Sort order for this factor type.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }
    }
}
