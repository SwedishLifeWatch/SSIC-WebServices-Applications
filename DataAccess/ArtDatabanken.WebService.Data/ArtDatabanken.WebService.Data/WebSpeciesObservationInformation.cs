using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains information related to one request
    /// of species observations.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationInformation : WebData
    {
        /// <summary>
        /// Max number of species observations (with information)
        /// that are returned in one call to the client.
        /// </summary>
        [DataMember]
        public Int64 MaxSpeciesObservationCount { get; set; }

        /// <summary>
        /// Number of species observations that are
        /// returned in this response.
        /// </summary>
        [DataMember]
        public Int64 SpeciesObservationCount { get; set; }

        /// <summary>
        /// If SpeciesObservationCount is greater than
        /// MaxSpeciesObservationCount then only ids for
        /// species observations are returned in this property.
        /// </summary>
        [DataMember]
        public List<Int64> SpeciesObservationIds { get; set; }

        /// <summary>
        /// If SpeciesObservationCount is less or equal to
        /// MaxSpeciesObservationCount then species observations
        /// are returned in this property.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservation> SpeciesObservations { get; set; }
    }
}
