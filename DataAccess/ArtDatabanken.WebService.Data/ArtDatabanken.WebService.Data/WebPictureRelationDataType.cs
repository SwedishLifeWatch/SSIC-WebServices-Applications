using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a picture relation data type.
    /// It is used to specify which type of data
    /// that a picture is related to.
    /// </summary>
    [DataContract]
    public class WebPictureRelationDataType : WebData
    {
        /// <summary>
        /// Description for this picture relation data type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for this picture relation data type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this picture relation data type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }

        /// <summary>
        /// Name for this picture relation data type.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
