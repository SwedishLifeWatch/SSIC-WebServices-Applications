using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface defines a time step specific count of species observations.
    /// </summary>
    public interface ITimeStepSpeciesObservationCount : ITimeStepBase
    {
        /// <summary>
        /// Number of species observations is based on selected species
        /// observation search criteria and time step properties.
        /// </summary>
        Int64 ObservationCount { get; set; }
    }
}
