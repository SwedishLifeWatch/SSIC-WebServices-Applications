using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a Person Gender
    /// </summary>
    [DataContract]
    public class WebPersonGender : WebData
    {
        /// <summary>
        /// Id for this person gender
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
