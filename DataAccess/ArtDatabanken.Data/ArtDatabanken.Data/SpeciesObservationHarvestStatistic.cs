using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish life watch.
    /// </summary>
    public class SpeciesObservationHarvestStatistic : ISpeciesObservationHarvestStatistic
    {
        /// <summary>
        /// Data provider id.
        /// </summary>
        public Int32 DataProviderId { get; set; }

        /// <summary>
        /// Harvest job status.
        /// </summary>
        public String JobStatus { get; set; }

        /// <summary>
        /// Harvest date for the data provider.
        /// </summary>
        public DateTime HarvestDate { get; set; } 
    }
}
