using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains change response and pictureId for picture that has been created, updated 
    /// or deleted.
    /// </summary>
    [DataContract]
    public class WebPictureResponse : WebData
    {
        /// <summary>
        /// Picture id of picture that has been created, updated 
        /// or deleted.
        /// </summary>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// Number of rows in DB that has beenChanged, used for checking that 
        /// change in de was correct.
        /// </summary>
        [DataMember]
        public Int64 AffectedRows { get; set; }
    }
}
