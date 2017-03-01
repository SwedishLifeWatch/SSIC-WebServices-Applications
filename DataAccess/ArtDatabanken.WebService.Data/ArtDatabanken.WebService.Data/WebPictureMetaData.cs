using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains one piece of meta data about a picture.
    /// </summary>
    [DataContract]
    public class WebPictureMetaData : WebData
    {
        /// <summary>
        /// Metadata id for the metadata.
        /// </summary>
        [DataMember]
        public Int32 PictureMetaDataId { get; set; }

        /// <summary>
        /// If metadata has a specified id.
        /// </summary>
        [DataMember]
        public Boolean HasPictureMetaDataId { get; set; }

        /// <summary>
        /// Name for the metadata.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Value of the metadata.
        /// </summary>
        [DataMember]
        public String Value { get; set; }
    }
}
