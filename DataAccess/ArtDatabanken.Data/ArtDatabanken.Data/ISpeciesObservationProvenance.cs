using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information about a SpeciesObservationProvenance class.
    /// </summary>
    public interface ISpeciesObservationProvenance
    {
        /// <summary>
        /// Contains name of the property.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Contains a list of values related to the property.
        /// </summary>
        List<ISpeciesObservationProvenanceValue> Values { get; set; }
    }
}
