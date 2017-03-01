using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class is used to specify a subset of the data for a
    /// species observation update status.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationHarvestStatus
    {
        /// <summary>
        /// Default constructor. Creates an instance of DataProviders.
        /// </summary>
        public WebSpeciesObservationHarvestStatus()
        {
            ActiveDataProviders = new List<WebSpeciesObservationDataProvider>();
            DataProviders = new List<WebSpeciesObservationDataProvider>();
            Statistics = new List<WebSpeciesObservationStatistic>();
        }

        /// <summary>
        /// List of harvest job statistics.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationStatistic> Statistics { get; set; }

        /// <summary>
        /// List of all active data providers.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationDataProvider> ActiveDataProviders { get; set; }

        /// <summary>
        /// List of current selected data providers.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationDataProvider> DataProviders { get; set; }

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
        /// Harvest job current status.
        /// </summary>
        [DataMember]
        public String JobStatus { get; set; }

        /// <summary>
        /// If set then harvest job status will become this status as soon as possible.
        /// </summary>
        [DataMember]
        public String RequestedJobStatus { get; set; }

        /// <summary>
        /// Current harvest date.
        /// </summary>
        [DataMember]
        public DateTime CurrentHarvestDate { get; set; }

        /// <summary>
        /// Current harvest from date.
        /// </summary>
        [DataMember]
        public DateTime HarvestFromDate { get; set; }

        /// <summary>
        /// Current harvest to date.
        /// </summary>
        [DataMember]
        public DateTime HarvestToDate { get; set; }
    }
}
