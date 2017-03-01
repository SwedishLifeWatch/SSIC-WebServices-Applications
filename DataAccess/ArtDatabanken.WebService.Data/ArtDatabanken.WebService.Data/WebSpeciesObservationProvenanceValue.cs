using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains values for a property
    /// that is included in a provenance class.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationProvenanceValue
    {
        /// <summary>
        /// Id for the property.
        /// Can be empty or null.
        /// </summary>
        [DataMember]
        public String Id { get; set; }

        /// <summary>
        /// Number of species observations related to the property.
        /// </summary>
        [DataMember]
        public Int64 SpeciesObservationCount { get; set; }

        /// <summary>
        /// Contains value for the property.
        /// </summary>
        [DataMember]
        public String Value
        { get; set; }
    }
}
