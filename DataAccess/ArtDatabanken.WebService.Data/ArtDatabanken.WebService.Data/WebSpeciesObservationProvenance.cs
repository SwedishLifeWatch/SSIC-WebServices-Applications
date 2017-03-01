using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a provenance property.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationProvenance
    {
        /// <summary>
        /// Contains name of the property.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Contains a list of values related to the property.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationProvenanceValue> Values { get; set; }
    }
}
