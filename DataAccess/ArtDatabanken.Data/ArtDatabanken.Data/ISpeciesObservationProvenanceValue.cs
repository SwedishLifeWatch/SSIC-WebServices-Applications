using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information about a SpeciesObservationProvenanceValue class.
    /// </summary>
    public interface ISpeciesObservationProvenanceValue
    {
        /// <summary>
        /// Id for the property.
        /// Can be empty or null.
        /// </summary>
        String Id { get; set; }

        /// <summary>
        /// Number of species observations related to the property.
        /// </summary>
        Int64 SpeciesObservationCount { get; set; }

        /// <summary>
        /// Contains value for the property.
        /// </summary>
        String Value { get; set; }
    }
}
