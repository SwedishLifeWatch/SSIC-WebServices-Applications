using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a Message Type.
    ///  Message types determine type of message that should be sent automatically by the web service once an user is added to a specific role.
    /// </summary>
    [DataContract]
    public class WebMessageType : WebData
    {
        /// <summary>
        /// Id for this Message Type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// StringId for the Name property.
        /// </summary>
        [DataMember]
        public Int32 NameStringId
        { get; set; }

        /// <summary>
        /// Name of this Message Type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
