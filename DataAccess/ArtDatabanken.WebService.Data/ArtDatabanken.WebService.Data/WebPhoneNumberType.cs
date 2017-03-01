using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a Phone Number Type
    /// </summary>
    [DataContract]
    public class WebPhoneNumberType : WebData
    {
        /// <summary>
        /// Id for this Phone Number Type
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// StringId for the Name property
        /// </summary>
        [DataMember]
        public Int32 NameStringId
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

    }
}