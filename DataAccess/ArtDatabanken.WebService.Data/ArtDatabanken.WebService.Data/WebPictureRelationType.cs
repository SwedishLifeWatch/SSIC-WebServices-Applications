using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a picture relation type.
    /// </summary>
    [DataContract]
    public class WebPictureRelationType : WebData
    {
        /// <summary>
        /// Picture relations of this type is related to
        /// data type (WebPictureRelationDataType) with this id.
        /// </summary>
        [DataMember]
        public Int32 DataTypeId { get; set; }

        /// <summary>
        /// Description for this picture relation type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for this picture relation type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this picture relation type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }

        /// <summary>
        /// Name for this picture relation type.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
