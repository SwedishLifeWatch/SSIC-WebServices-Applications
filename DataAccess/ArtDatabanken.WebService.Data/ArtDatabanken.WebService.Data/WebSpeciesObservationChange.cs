using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains information about changes in species observations.
    /// This class has three different types of changes.
    /// Created species observations.
    /// Deleted species observations.
    /// Updated species observations.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationChange : WebData
    {
        /// <summary>
        /// Information about created species observations.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservation> CreatedSpeciesObservations { get; set; }

        /// <summary>
        /// GUIDs (Globally Unique Identifier) for deleted species observations.
        /// It is a LSID, which is unique for each species observation. 
        /// </summary>
        [DataMember]
        public List<String> DeletedSpeciesObservationGuids { get; set; }

        /// <summary>
        /// Indicates if more species observations are available in database.
        /// </summary>
        [DataMember]
        public Boolean IsMoreSpeciesObservationsAvailable { get; set; }

        /// <summary>
        /// Max number of species observations changes that are returned
        /// as created, deleted or updated in a single web service call.
        /// </summary>
        [DataMember]
        public Int64 MaxChangeCount { get; set; }

        /// <summary>
        /// Highest change id for the species observation changes
        /// that are returned in current web service call.
        /// </summary>
        [DataMember]
        public Int64 MaxChangeId { get; set; }

        /// <summary>
        /// Information about updated species observations.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservation> UpdatedSpeciesObservations { get; set; }
    }
}
