using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider
{
    /// <summary>
    /// Contains information about changes in species observations.
    /// This class has three different types of changes.
    /// Created species observations.
    /// Deleted species observations.
    /// Updated species observations.
    /// </summary>
    public class SpeciesObservationChange
    {
        /// <summary>
        /// Information about new species observations.
        /// </summary>
        public List<HarvestSpeciesObservation> CreatedSpeciesObservations { get; set; }

        /// <summary>
        /// CatalogNumber for deleted species observations.
        /// </summary>
        public List<String> DeletedSpeciesObservationGuids { get; set; }

        /// <summary>
        /// Information about updated species observations.
        /// </summary>
        public List<HarvestSpeciesObservation> UpdatedSpeciesObservations { get; set; }
    }
}
