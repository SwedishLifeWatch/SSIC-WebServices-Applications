using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains information about changes in species observations.
    /// This class has three different types of changes.
    /// Deleted species observations.
    /// New species observations.
    /// Updated species observations.
    /// </summary>
    [DataContract]
    public class WebDarwinCoreChange : WebData
    {
#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Number of deleted species observations that are
        /// returned in this response.
        /// </summary>
        [DataMember]
        public Int64 DeletedSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// GUIDs for deleted species observations.
        /// </summary>
        [DataMember]
        public List<String> DeletedSpeciesObservationGuids
        { get; set; }

        /// <summary>
        /// Max number of species observations (with information)
        /// that are returned as new or updated in a single web service
        /// call. It may be up to max number of species observations
        /// of each change type (new or updated).
        /// </summary>
        [DataMember]
        public Int64 MaxSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// Number of new species observations that are
        /// returned in this response.
        /// </summary>
        [DataMember]
        public Int64 NewSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// If NewSpeciesObservationCount is greater than
        /// MaxSpeciesObservationCount then only ids for
        /// species observations are returned in this property.
        /// </summary>
        [DataMember]
        public List<Int64> NewSpeciesObservationIds
        { get; set; }

        /// <summary>
        /// If NewSpeciesObservationCount is less or equal to
        /// MaxSpeciesObservationCount then species observations
        /// are returned in this property.
        /// </summary>
        [DataMember]
        public List<WebDarwinCore> NewSpeciesObservations
        { get; set; }

        /// <summary>
        /// Number of updated species observations that are
        /// returned in this response.
        /// </summary>
        [DataMember]
        public Int64 UpdatedSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// If UpdatedSpeciesObservationCount is greater than
        /// MaxSpeciesObservationCount then only ids for
        /// species observations are returned in this property.
        /// </summary>
        [DataMember]
        public List<Int64> UpdatedSpeciesObservationIds
        { get; set; }

        /// <summary>
        /// If UpdatedSpeciesObservationCount is less or equal to
        /// MaxSpeciesObservationCount then species observations
        /// are returned in this property.
        /// </summary>
        [DataMember]
        public List<WebDarwinCore> UpdatedSpeciesObservations
        { get; set; }
#endif

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Information about created species observations.
        /// </summary>
        [DataMember]
        public List<WebDarwinCore> CreatedSpeciesObservations { get; set; }

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
        /// If the result set is empty. -1 is returned since there are no MaxChangeId for current search criteria.
        /// </summary>
        [DataMember]
        public Int64 MaxChangeId { get; set; }

        /// <summary>
        /// Information about updated species observations.
        /// </summary>
        [DataMember]
        public List<WebDarwinCore> UpdatedSpeciesObservations { get; set; }
#endif
    }
}
