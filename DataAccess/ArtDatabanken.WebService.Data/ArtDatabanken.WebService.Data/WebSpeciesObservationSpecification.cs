using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class is used to specify a subset of the data for a
    /// species observation.
    /// It is the union of species observation fields defined by
    /// property Fields and Specification that defines the subset.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationSpecification
    {
        /// <summary>
        /// Species observation fields that are
        /// included in the specification.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationFieldSpecification> Fields { get; set; }

        /// <summary>
        /// Enumeration value that specifies a predefined
        /// set of species observation fields.
        /// </summary>
        [DataMember]
        public SpeciesObservationSpecificationId Specification { get; set; }
    }
}
