using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a picture metadata description.
    /// </summary>
    [DataContract]
    public class WebPictureMetaDataDescription : WebData
    {
        /// <summary>
        /// Description for picture meta data description.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Exif code (hexadecimal format) for picture meta data description.
        /// </summary>
        [DataMember]
        public String Exif { get; set; }

        /// <summary>
        /// Id for picture meta data description.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for picture meta data description.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}