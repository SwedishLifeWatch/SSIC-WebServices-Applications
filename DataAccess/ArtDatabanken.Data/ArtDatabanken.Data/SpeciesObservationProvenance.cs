using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a provenance property.
    /// </summary>
    [DataContract]
    public class SpeciesObservationProvenance : ISpeciesObservationProvenance
    {
        /// <summary>
        /// Default constructor. Instanciates Values as a list.
        /// </summary>
        public SpeciesObservationProvenance()
        {
            Values = new List<ISpeciesObservationProvenanceValue>();
        }

        /// <summary>
        /// Contains name of the property.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Contains a list of values related to the property.
        /// </summary>
        [DataMember]
        public List<ISpeciesObservationProvenanceValue> Values { get; set; }
    }
}
