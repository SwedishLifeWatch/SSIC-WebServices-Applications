using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class is used to specify a subset of the data for a
    /// species observation update status.
    /// </summary>
    [DataContract]
    public class SpeciesObservationHarvestStatus
    {
        /// <summary>
        /// Data providers.
        /// </summary>
        [DataMember]
        public List<SpeciesObservationDataProvider> DataProviders { get; set; }

        /// <summary>
        /// Data provider's log.
        /// </summary>
        [DataMember]
        public List<SpeciesObservationHarvestStatistic> Statistics { get; set; }

        /// <summary>
        /// Harvest job start date.
        /// </summary>
        [DataMember]
        public DateTime JobStartDate { get; set; }

        /// <summary>
        /// Harvest job end date.
        /// </summary>
        [DataMember]
        public DateTime JobEndDate { get; set; }

        /// <summary>
        /// Harvest job status.
        /// </summary>
        [DataMember]
        public String JobStatus { get; set; }

        /// <summary>
        /// If set then harvest job status will become this status as soon as possible.
        /// </summary>
        [DataMember]
        public String RequestedJobStatus { get; set; }

        /// <summary>
        /// Harvest current date.
        /// </summary>
        [DataMember]
        public DateTime CurrentHarvestDate { get; set; }

        /// <summary>
        /// Harvest planned from date.
        /// </summary>
        [DataMember]
        public DateTime HarvestFromDate { get; set; }

        /// <summary>
        /// Harvest planned to date.
        /// </summary>
        [DataMember]
        public DateTime HarvestToDate { get; set; }
    }
}
