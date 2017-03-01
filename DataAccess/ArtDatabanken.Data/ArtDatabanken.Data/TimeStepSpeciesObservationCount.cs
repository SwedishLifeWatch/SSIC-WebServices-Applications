using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains a time step specific count of species observations.
    /// </summary>
    public class TimeStepSpeciesObservationCount : TimeStepBase, ITimeStepSpeciesObservationCount
    {
        /// <summary>
        /// Number of species observations is based on selected species
        /// observation search criteria and time step properties.
        /// </summary>
        public Int64 ObservationCount { get; set; }
    }
}
