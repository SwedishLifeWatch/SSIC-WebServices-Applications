using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a period.
    /// </summary>
    [DataContract]
    public class WebPeriod : WebData
    {
        /// <summary>
        /// Description for this period.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for this period.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this period.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Stop updates for species facts, that
        /// are related to this period, at this date.
        /// </summary>
        [DataMember]
        public DateTime StopUpdates { get; set; }

        /// <summary>
        /// Period type id for this period.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }

        /// <summary>
        /// Year for this period.
        /// </summary>
        [DataMember]
        public Int32 Year { get; set; }
    }
}