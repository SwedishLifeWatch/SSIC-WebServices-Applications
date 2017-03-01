using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish life watch.
    /// </summary>
    public interface ISpeciesObservationHarvestStatistic
    {
        /// <summary>
        /// Data provider id.
        /// </summary>
        Int32 DataProviderId { get; set; }

        /// <summary>
        /// Harvest job status.
        /// </summary>
        String JobStatus { get; set; }

        /// <summary>
        /// Harvest date for the data provider.
        /// </summary>
        DateTime HarvestDate { get; set; } 
    }
}
