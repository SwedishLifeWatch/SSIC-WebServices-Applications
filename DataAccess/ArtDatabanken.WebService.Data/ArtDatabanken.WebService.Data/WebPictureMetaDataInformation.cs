using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about metadata set to a picture for a list of pictures.
    /// </summary>
    [DataContract]
    public class WebPictureMetaDataInformation : WebData
    {
        /// <summary>
        /// Picture id.
        /// </summary>
        [DataMember]
        public Int64 PictureId { get; set; }

        /// <summary>
        /// Meta data about the picture.
        /// </summary>
        [DataMember]
        public List<WebPictureMetaData> PictureMetaDataList { get; set; }


    }
}
