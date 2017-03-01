using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Search criteria used when species facts are retrieved.
    /// Only parts of the possible functionality is implemented.
    /// </summary>
    [DataContract]
    public class WebSpeciesFactFieldSearchCriteria : WebData
    {
        /// <summary>
        /// Search criteria is related to this factor field.
        /// </summary>
        [DataMember]
        public WebFactorField FactorField { get; set; }

        /// <summary>
        /// Include species facts with empty values (null in database)
        /// in returned species facts in addition to the species facts
        /// that matches this factor field search criteria.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IncludeEmptyValues { get; set; }

        /// <summary>
        /// This property is only used in combination with
        /// factor fields where data is of type enumeration.
        /// Property is enumeration as string must be set to true if
        /// a string value is used to compare enumeration values.
        /// </summary>
        [DataMember]
        public Boolean IsEnumAsString { get; set; }

        /// <summary>
        /// Operator for this condition.
        /// </summary>
        [DataMember]
        public CompareOperator Operator { get; set; }

        /// <summary>
        /// Values to compare operator with.
        /// Normally exactly one value is specified but property Values
        /// may contain more than one value if property Operator has
        /// the value Equal or NotEqual.
        /// Operator Equal together with more than one value returns true
        /// if the comparison value is equal to one of the listed values. 
        /// Operator NotEqual compared to more than one
        /// value returns true if the comparison value
        /// is not equal to any of the listed values. 
        /// Property FactorField holds information about which 
        /// data type values contains.
        /// </summary>
        [DataMember]
        public List<String> Values { get; set; }
    }
}
