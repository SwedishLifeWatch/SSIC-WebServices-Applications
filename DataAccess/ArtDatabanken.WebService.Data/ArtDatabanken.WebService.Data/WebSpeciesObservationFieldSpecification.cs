using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class is used to specify a subset of the data for a
    /// species observation. Usually exactly one species observation
    /// field for each species observation.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationFieldSpecification : WebData
    {
        /// <summary>
        /// SpecificationId of the class.
        /// </summary>
        [DataMember]
        public WebSpeciesObservationClass Class { get; set; }

        /// <summary>
        /// SpecificationId of max class index.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int64 ClassIndexEnd { get; set; }

        /// <summary>
        /// SpecificationId of min class index.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int64 ClassIndexStart { get; set; }

        /// <summary>
        /// Specifies if property ClassIndexEnd and
        /// ClassIndexStart has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsClassIndexSpecified { get; set; }

        /// <summary>
        /// Specifies if property PropertyIndexEnd and
        /// PropertyIndexStart has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPropertyIndexSpecified { get; set; }

        /// <summary>
        /// SpecificationId of the property.
        /// </summary>
        [DataMember]
        public WebSpeciesObservationProperty Property { get; set; }

        /// <summary>
        /// SpecificationId of max property index.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int64 PropertyIndexEnd { get; set; }

        /// <summary>
        /// SpecificationId of min property index.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int64 PropertyIndexStart { get; set; }
    }
}
