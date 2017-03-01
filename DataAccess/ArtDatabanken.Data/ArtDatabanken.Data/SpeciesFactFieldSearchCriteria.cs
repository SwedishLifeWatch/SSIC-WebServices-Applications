using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Search criteria used when species facts are retrieved.
    /// Only parts of the possible functionality is implemented.
    /// </summary>
    public class SpeciesFactFieldSearchCriteria : ISpeciesFactFieldSearchCriteria
    {
        /// <summary>
        /// Search criteria is related to this factor field.
        /// </summary>
        public IFactorField FactorField { get; set; }

        /// <summary>
        /// This property is only used in combination with
        /// factor fields where data is of type enumeration.
        /// Property is enumeration as string must be set to true if
        /// a string value is used to compare enumeration values.
        /// </summary>
        public Boolean IsEnumAsString { get; set; }

        /// <summary>
        /// Operator for this condition.
        /// </summary>
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
        public List<String> Values { get; set; }

        /// <summary>
        /// Add value to property Values.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddValue(Int32 value)
        {
            if (Values.IsNull())
            {
                Values = new List<String>();
            }

            Values.Add(value.WebToString());
        }

        /// <summary>
        /// Add value to property Values.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddValue(String value)
        {
            if (Values.IsNull())
            {
                Values = new List<String>();
            }

            Values.Add(value);
        }
    }
}
