using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish life watch.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationStatistic : WebData
    {
        /// <summary>
        /// Data provider id.
        /// </summary>
        [DataMember]
        public Int32 DataProviderId { get; set; }

        /// <summary>
        /// Harvest job status.
        /// </summary>
        [DataMember]
        public String JobStatus { get; set; }

        /// <summary>
        /// Harvest date.
        /// </summary>
        [DataMember]
        public DateTime HarvestDate { get; set; }
    }
}
