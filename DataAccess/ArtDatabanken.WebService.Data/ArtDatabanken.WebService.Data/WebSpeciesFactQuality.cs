using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a species fact quality.
    /// </summary>
    [DataContract]
    public class WebSpeciesFactQuality : WebData
    {
        /// <summary>
        /// Definition for this species fact quality.
        /// </summary>
        [DataMember]
        public String Definition { get; set; }

        /// <summary>
        /// Id for this species fact quality.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this species fact quality.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}