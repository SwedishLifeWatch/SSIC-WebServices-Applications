using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains information about changes in species observations.
    /// This class has three different types of changes.
    /// Created species observations.
    /// Deleted species observations.
    /// Updated species observations.
    /// </summary>
    public interface ISpeciesObservationChange
    {
        /// <summary>
        /// Information about created species observations.
        /// </summary>
        SpeciesObservationList CreatedSpeciesObservations { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// GUIDs (Globally Unique Identifier) for deleted species observations.
        /// It is a LSID, which is unique for each species observation. 
        /// </summary>
        List<String> DeletedSpeciesObservationGuids { get; set; }

        /// <summary>
        /// Indicates if more species observations are available in database.
        /// </summary>
        Boolean IsMoreSpeciesObservationsAvailable { get; set; }

        /// <summary>
        /// Max number of species observations changes that are returned
        /// as created, deleted or updated in a single web service call.
        /// </summary>
        Int64 MaxChangeCount { get; set; }

        /// <summary>
        /// Highest change id for the species observation changes
        /// that are returned in current web service call.
        /// </summary>
        Int64 MaxChangeId { get; set; }

        /// <summary>
        /// Information about updated species observations.
        /// </summary>
        SpeciesObservationList UpdatedSpeciesObservations { get; set; }
    }
}
