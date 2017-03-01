using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a period type.
    /// </summary>
    [DataContract]
    public class WebPeriodType : WebData
    {
        /// <summary>
        /// Description for this period type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for this period type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this period type.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}