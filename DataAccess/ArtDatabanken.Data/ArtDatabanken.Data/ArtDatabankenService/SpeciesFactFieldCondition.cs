using System;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Contains a condition on a species fact field.
    /// This class is used in data query handling.
    /// </summary>
    public class SpeciesFactFieldCondition
    {
        private DataConditionOperatorId _operator;
        private String _value;
        private WebDataType _type;

        /// <summary>
        /// Create a SpeciesFactFieldCondition instance.
        /// </summary>
        public SpeciesFactFieldCondition()
        {
            _operator = DataConditionOperatorId.Equal;
            FactorField = null;
            IsEnumAsString = false;
            _type = WebDataType.String;
            _value = null;
        }

        /// <summary>
        /// Get factor field for which the condition applies.
        /// </summary>
        public FactorField FactorField
        { get; set; }

        /// <summary>
        /// This property is only used in combination with
        /// enum factor fields.
        /// IsEnumAsString must be set to true if a string
        /// value is used to compare enum values.
        /// </summary>
        public Boolean IsEnumAsString
        { get; set; }


        /// <summary>
        /// Get operator to apply on the value.
        /// </summary>
        public DataConditionOperatorId Operator
        {
            get { return _operator; }
        }

        /// <summary>
        /// Get type of the data.
        /// </summary>
        public WebDataType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Get string representation of the value.
        /// </summary>
        public String Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Set Boolean value.
        /// </summary>
        /// <param name="value">A Boolean value.</param>
        public void SetValue(Boolean value)
        {
            _type = WebDataType.Boolean;
            _value = value.WebToString();
        }

        /// <summary>
        /// Set DateTime value.
        /// </summary>
        /// <param name="value">A DateTime value.</param>
        public void SetValue(DateTime value)
        {
            _type = WebDataType.DateTime;
            _value = value.WebToString();
        }

        /// <summary>
        /// Set Double value.
        /// </summary>
        /// <param name="value">A Double value.</param>
        public void SetValue(Double value)
        {
            _type = WebDataType.Float;
            _value = value.WebToString();
        }

        /// <summary>
        /// Set Int32 value.
        /// </summary>
        /// <param name="value">A Int32 value.</param>
        public void SetValue(Int32 value)
        {
            _type = WebDataType.Int32;
            _value = value.WebToString();
        }

        /// <summary>
        /// Set Int64 value.
        /// </summary>
        /// <param name="value">A Int32 value.</param>
        public void SetValue(Int64 value)
        {
            _type = WebDataType.Int64;
            _value = value.WebToString();
        }

        /// <summary>
        /// Set String value.
        /// </summary>
        /// <param name="value">A String value.</param>
        public void SetValue(String value)
        {
            _type = WebDataType.String;
            _value = value;
        }
    }
}
