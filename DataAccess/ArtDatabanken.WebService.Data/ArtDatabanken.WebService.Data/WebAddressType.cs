using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an Address Type
    /// </summary>
    [DataContract]
    public class WebAddressType : WebData
    {
        /// <summary>
        /// Id for this address type.
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
        /// Name of this address type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
