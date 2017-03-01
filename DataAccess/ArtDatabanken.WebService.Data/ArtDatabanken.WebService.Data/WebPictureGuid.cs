using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds information on what GUID is connected to corresponding picture.
    /// </summary>
    [DataContract]
    public class WebPictureGuid : WebData
    {
        /// <summary>
        /// GUID for the picture.
        /// </summary>
        [DataMember]
        public String ObjectGuid { get; set; }

        /// <summary>
        /// Id for the picture.
        /// </summary>
        [DataMember]
        public Int64 PictureId { get; set; }
    }
}
