using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents an individual category.
    /// </summary>
    [DataContract]
    public class WebIndividualCategory : WebData
    {
        /// <summary>
        /// Definition for this individual category.
        /// </summary>
        [DataMember]
        public String Definition { get; set; }

        /// <summary>
        /// Id for this individual category.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this individual category.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}