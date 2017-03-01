using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents the number of observed species related to a taxon.
    /// </summary>
    [DataContract]
    public class WebTaxonSpeciesObservationCount
    {
        /// <summary>
        /// Number of observed species.
        /// </summary>
        [DataMember]
        public Int32 SpeciesObservationCount { get; set; }

        /// <summary>
        /// Related taxon.
        /// </summary>
        [DataMember]
        public WebTaxon Taxon { get; set; }
    }
}