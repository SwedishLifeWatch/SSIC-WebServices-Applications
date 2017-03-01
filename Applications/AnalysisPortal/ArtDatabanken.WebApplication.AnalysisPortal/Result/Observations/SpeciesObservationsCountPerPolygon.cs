using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations
{
    /// <summary>
    /// Used to save the number of observations and species per polygon in a selected environmental data layer.
    /// </summary>
    public class SpeciesObservationsCountPerPolygon
    {
        /// <summary>
        /// A flattened list of properties as a single string.
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// The number of species observations.
        /// </summary>
        public string SpeciesObservationsCount { get; set; }

        /// <summary>
        /// The number of species.
        /// </summary>
        public string SpeciesCount { get; set; }
    }
}