using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a picture.
    /// </summary>
    [DataContract]
    public class WebPictureInformation : WebData
    {
        /// <summary>
        /// Picture id.
        /// </summary>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// Meta data about the picture.
        /// </summary>
        [DataMember]
        public List<WebPictureMetaData> MetaData { get; set; }

        /// <summary>
        /// The picture.
        /// </summary>
        [DataMember]
        public WebPicture Picture { get; set; }

        /// <summary>
        /// Relations that this picture has to other objects.
        /// </summary>
        [DataMember]
        public List<WebPictureRelation> Relations { get; set; }
    }
}
