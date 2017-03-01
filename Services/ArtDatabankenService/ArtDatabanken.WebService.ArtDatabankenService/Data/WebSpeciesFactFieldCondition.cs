using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains a condition on a species fact field.
    /// This class is used in WebDataQuery handling.
    /// </summary>
    [DataContract]
    public class WebSpeciesFactFieldCondition : WebData
    {
        /// <summary>
        /// Create a WebSpeciesFactFieldCondition instance.
        /// </summary>
        public WebSpeciesFactFieldCondition()
        {
            FactorField = null;
            IsEnumAsString = false;
            Operator = DataConditionOperatorId.NoOperator;
            Value = null;
        }

        /// <summary>
        /// Get name for this factor field in database.
        /// </summary>
        public String DatabaseFieldName
        {
            get
            {
                return FactorField.DatabaseFieldName;
            }
        }

        /// <summary>
        /// Get data type for this species factor field condition.
        /// </summary>
        public WebDataType DataType
        {
            get
            {
                if (FactorField.IsEnumField && IsEnumAsString)
                {
                    return WebDataType.String;
                }
                else
                {
                    return FactorField.DataType;
                }
            }
        }

        /// <summary>
        /// Limit condition to specified factor field.
        /// </summary>
        [DataMember]
        public WebFactorField FactorField
        { get; set; }

        /// <summary>
        /// This property is only used in combination with
        /// enum factor fields.
        /// IsEnumAsString must be set to true if a string
        /// value is used to compare enum values.
        /// </summary>
        [DataMember]
        public Boolean IsEnumAsString
        { get; set; }

        /// <summary>
        /// Operator for this condition.
        /// </summary>
        [DataMember]
        public DataConditionOperatorId Operator
        { get; set; }

        /// <summary>
        /// Value to compare operator with.
        /// DataType holds information about which 
        /// datatype value contains. FactorField.DatabaseFieldName
        /// holds information about which field in the database
        /// that the condition is related to.
        /// </summary>
        [DataMember]
        public String Value
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            FactorField.CheckNotNull("FactorField");
            FactorField.CheckData();
            if (Operator != DataConditionOperatorId.Equal)
            {
                throw new ApplicationException("Species fact field condition with operator " + Operator + " is not implemted!");
            }
            Value.CheckNotEmpty("Value");
            Value = Value.CheckSqlInjection();
        }

        /// <summary>
        /// Get Boolean value.
        /// </summary>
        /// <returns>This conditions value as a Boolean value.</returns>
        public Boolean GetBoolean()
        {
            return Value.WebParseBoolean();
        }

        /// <summary>
        /// Get DateTime value.
        /// </summary>
        /// <returns>This conditions value as a DateTime value.</returns>
        public DateTime GetDateTime()
        {
            return Value.WebParseDateTime();
        }

        /// <summary>
        /// Get Double value.
        /// </summary>
        /// <returns>This conditions value as a Double value.</returns>
        public Double GetDouble()
        {
            return Value.WebParseDouble();
        }

        /// <summary>
        /// Get Int32 value.
        /// </summary>
        /// <returns>This conditions value as a Int32 value.</returns>
        public Int32 GetInt32()
        {
            return Value.WebParseInt32();
        }

        /// <summary>
        /// Get Int64 value.
        /// </summary>
        /// <returns>This conditions value as a Int64 value.</returns>
        public Int64 GetInt64()
        {
            return Value.WebParseInt64();
        }

        /// <summary>
        /// Get String value.
        /// </summary>
        /// <returns>This conditions value as a String value.</returns>
        public String GetString()
        {
            return Value;
        }
    }
}
