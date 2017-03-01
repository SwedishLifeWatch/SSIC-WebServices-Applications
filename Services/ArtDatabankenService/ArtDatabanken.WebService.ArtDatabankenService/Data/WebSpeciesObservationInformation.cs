using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains information related to one request
    /// of species observations.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationInformation : WebData
    {
        /// <summary>
        /// Create a WebSpeciesObservationInformation instance.
        /// </summary>
        public WebSpeciesObservationInformation()
        {
            MaxSpeciesObservationCount = SpeciesObservationManager.MAX_SPECIES_OBSERVATIONS_WITH_INFORMATION;
            SpeciesObservationCount = 0;
            SpeciesObservationIds = null;
            SpeciesObservations = new List<WebSpeciesObservation>();
        }

        /// <summary>
        /// Get max number of species observations (with information)
        /// that are returned in one call to the client.
        /// </summary>
        [DataMember]
        public Int64 MaxSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// Species observation count.
        /// </summary>
        [DataMember]
        public Int64 SpeciesObservationCount
        { get; set; }

        /// <summary>
        /// Species observations.
        /// </summary>
        [DataMember]
        public List<Int64> SpeciesObservationIds
        { get; set; }

        /// <summary>
        /// Species observations.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservation> SpeciesObservations
        { get; set; }
    }
}
