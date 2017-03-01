using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a relation between a picture
    /// and an object of arbitrary type.
    /// </summary>
    [DataContract]
    public class WebPictureRelation : WebData
    {
        /// <summary>
        /// Id for this picture relation.
        /// </summary>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// Indicates if the picture that is referenced in
        /// this relation is the recommended picture for the
        /// related object.
        /// </summary>
        [DataMember]
        public Boolean IsRecommended { get; set; }

        /// <summary>
        /// GUID for the object that is related to a picture.
        /// </summary>
        [DataMember]
        public String ObjectGuid { get; set; }

        /// <summary>
        /// Id for the picture that the object is related to.
        /// </summary>
        [DataMember]
        public Int64 PictureId { get; set; }

        /// <summary>
        /// Sort order among pictures that are
        /// related to the same object.
        /// </summary>
        [DataMember]
        public Int64 SortOrder { get; set; }

        /// <summary>
        /// Id for the picture relation type.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }
    }
}
