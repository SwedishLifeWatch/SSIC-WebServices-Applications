using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Contains information about changes in species observations.
    /// This class has three different types of changes.
    /// Deleted species observations.
    /// New species observations.
    /// Updated species observations.
    /// </summary>
    public class SpeciesObservationChange
    {
        /// <summary>
        /// GUIDs for deleted species observations.
        /// This property is currently not used.
        /// </summary>
        public List<String> DeletedSpeciesObservationGuids
        { get; set; }

        /// <summary>
        /// New species observations.
        /// </summary>
        public SpeciesObservationList NewSpeciesObservations
        { get; set; }

        /// <summary>
        /// Updated species observations.
        /// </summary>
        public SpeciesObservationList UpdatedSpeciesObservations
        { get; set; }
    }
}
