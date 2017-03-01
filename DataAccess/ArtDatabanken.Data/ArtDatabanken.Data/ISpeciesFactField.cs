using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a species fact field.
    /// </summary>
    public interface ISpeciesFactField : IDataId32
    {
        /// <summary>
        /// Value as Boolean type for this species fact.
        /// </summary>
        Boolean BooleanValue { get; set; }

        /// <summary>
        /// Value as Double type for this species fact.
        /// </summary>
        Double DoubleValue { get; set; }

        /// <summary>
        /// Value for this species fact field.
        /// </summary>
        IFactorFieldEnumValue EnumValue { get; set; }

        /// <summary>
        /// Factor field of this species fact field.
        /// </summary>
        IFactorField FactorField { get; }

        /// <summary>
        /// Factor field enumeration associated with this species
        /// fact field. This property is null if this species fact
        /// field is not of data type factor field enumeration.
        /// </summary>
        IFactorFieldEnum FactorFieldEnum { get; }

        /// <summary>
        /// Indicates whether or not this species fact field has changed.
        /// </summary>
        Boolean HasChanged { get; }

        /// <summary>
        /// Indicates whether or not this species fact has a value.
        /// </summary>
        Boolean HasValue { get; set; }

        /// <summary>
        /// Index of the factor field in the species fact.
        /// </summary>
        Int32 Index { get; }

        /// <summary>
        /// Information for this species fact field.
        /// </summary>
        String Information { get; }

        /// <summary>
        /// Value as Int32 type for this species fact.
        /// </summary>
        Int32 Int32Value { get; set; }

        /// <summary>
        /// Test if this species fact field is stored as Double.
        /// </summary>
        Boolean IsDoubleField { get; }

        /// <summary>
        /// Indicates whether or not this species fact
        /// field is a main field.
        /// </summary>
        Boolean IsMain { get; }

        /// <summary>
        /// Indicates whether or not this species fact field is a substantial field.
        /// </summary>
        Boolean IsSubstantial { get; }

        /// <summary>
        /// Test if this species fact field is stored as String.
        /// </summary>
        Boolean IsTextField { get; }

        /// <summary>
        /// Label for this species fact field.
        /// </summary>
        String Label { get; }

        /// <summary>
        /// Factor field max size for this species fact field.
        /// </summary>
        Int32 MaxSize { get; }

        /// <summary>
        /// Species fact that this species fact field belongs to.
        /// </summary>
        ISpeciesFact SpeciesFact { get; set; }

        /// <summary>
        /// Value as String type for this species fact.
        /// </summary>
        String StringValue { get; set; }

        /// <summary>
        /// Factor field type for this species fact field.
        /// </summary>
        IFactorFieldType Type { get; }

        /// <summary>
        /// Unit label for this species fact field.
        /// </summary>
        String Unit { get; }

        /// <summary>
        /// Value of this species fact field.
        /// </summary>
        Object Value { get; set; }

        /// <summary>
        /// Get Boolean value.
        /// Difference between property BooleanValue and method GetBoolean
        /// is that GetBoolean may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        Boolean GetBoolean();

        /// <summary>
        /// Get Double value.
        /// Difference between property DoubleValue and method GetDouble
        /// is that GetDouble may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        Double GetDouble();

        /// <summary>
        /// Get Double value.
        /// Difference between property DoubleValue and method
        /// GetDoubleValue:
        /// Property DoubleValue only works if species fact
        /// field is of data type Double.
        /// Method GetDoubleValue returns the numeric value without
        /// checking data type for the species fact field.
        /// </summary>
        /// <returns>The value.</returns>
        Double GetDoubleValue();

        /// <summary>
        /// Get Int32 value.
        /// Difference between property Int32Value and method GetInt32
        /// is that GetInt32 may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        Int32 GetInt32();

        /// <summary>
        /// Get Int64 value.
        /// Difference between property Int64Value and method GetInt64
        /// is that GetInt64 may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        Int64 GetInt64();

        /// <summary>
        /// Get String value.
        /// Difference between property StringValue and method GetString
        /// is that GetString may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        String GetString();

        /// <summary>
        /// Get String value.
        /// Difference between property StringValue and method
        /// GetStringValue:
        /// Property StringValue only works if species fact
        /// field is of data type String.
        /// Method GetStringValue returns the string value without
        /// checking data type for the species fact field.
        /// </summary>
        /// <returns>The value.</returns>
        String GetStringValue();

        /// <summary>
        /// Reset species fact field that has been deleted from database.
        /// Set all values to default or null.
        /// </summary>
        void Reset();

        /// <summary>
        /// Set species fact field value as a result of
        /// an automatic calculation.
        /// </summary>
        /// <param name='value'>New value for species fact field.</param>
        void SetValueAutomatic(Object value);

        /// <summary>
        /// Update species fact field.
        /// </summary>
        /// <param name="hasValue">Indication if value has been set.</param>
        /// <param name="value">New value.</param>
        void Update(Boolean hasValue, Object value);
    }
}