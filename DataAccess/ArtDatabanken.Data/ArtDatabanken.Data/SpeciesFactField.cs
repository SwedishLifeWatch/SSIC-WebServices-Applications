using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a species fact field.
    /// </summary>
    [Serializable]
    public class SpeciesFactField : ISpeciesFactField
    {
        /// <summary>
        /// The numeric value for  this species fact field.
        /// </summary>
        private Double _numericValue;

        /// <summary>
        /// The string value for this species fact field.
        /// </summary>
        private String _stringValue;

        /// <summary>
        /// Create a SpeciesFactField instance.
        /// </summary>
        /// <param name="speciesFact">The SpeciesFact that this SpeciesFactField belongs to.</param>
        /// <param name="factorField">Factor field for the species fact field.</param>
        public SpeciesFactField(ISpeciesFact speciesFact,
                                IFactorField factorField)
            : this(speciesFact, factorField, false, null)
        {
        }

        /// <summary>
        /// Create a SpeciesFactField instance.
        /// </summary>
        /// <param name="speciesFact">The SpeciesFact that this SpeciesFactField belongs to.</param>
        /// <param name="factorField">Factor field for the species fact field.</param>
        /// <param name="hasValue">Indication whether or not a value is initially specified.</param>
        /// <param name="value">The value of the species fact field.</param>
        public SpeciesFactField(ISpeciesFact speciesFact,
                                IFactorField factorField,
                                Boolean hasValue,
                                Object value)
        {
            FactorField = factorField;
            Id = factorField.Id;
            SpeciesFact = speciesFact;
            this.Update(hasValue, value);
        }

        /// <summary>
        /// Value as Boolean type for this species fact field.
        /// </summary>
        public Boolean BooleanValue
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.Boolean);
                if (HasValue)
                {
                    if (IsTextField)
                    {
                        return Boolean.Parse(StringValue);
                    }

                    return ((Int32)_numericValue) == 1;
                }

                return false;
            }

            set
            {
                SpeciesFact.CheckManualUpdate();
                SetBooleanValue(value);
                SpeciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Value as Double type for this species fact field.
        /// </summary>
        public Double DoubleValue
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.Double);
                if (HasValue)
                {
                    return IsTextField ? Double.Parse(_stringValue) : _numericValue;
                }

                return Double.MinValue;
            }

            set
            {
                SpeciesFact.CheckManualUpdate();
                SetDoubleValue(value);
                SpeciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Value as enumeration type for this species fact field.
        /// </summary>
        public IFactorFieldEnumValue EnumValue
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.Enum);
                if (HasValue)
                {
                    if (IsTextField)
                    {
                        return FactorFieldEnum.Values.GetByKey(_stringValue);
                    }
                    else
                    {
                        return FactorFieldEnum.Values.GetByKey((Int32)_numericValue);
                    }
                }

                return null;
            }

            set
            {
                SpeciesFact.CheckManualUpdate();
                SetEnumValue(value);
                SpeciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Factor field of this species fact field.
        /// </summary>
        public IFactorField FactorField { get; private set; }

        /// <summary>
        /// Factor field enumeration associated with this species
        /// fact field. This property is null if this species fact
        /// field is not of data type factor field enumeration.
        /// </summary>
        public IFactorFieldEnum FactorFieldEnum
        {
            get
            {
                return FactorField.Enum;
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact field has changed.
        /// </summary>
        public Boolean HasChanged
        {
            get
            {
                if (HasValue != OldHasValue)
                {
                    return true;
                }

                if (!HasValue && !OldHasValue)
                {
                    return false;
                }

                return OldValue.ToString().Trim() != Value.ToString().Trim();
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact has a value.
        /// </summary>
        public Boolean HasValue { get; set; }

        /// <summary>
        /// Id of this species fact field.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Index of the factor field in the species fact.
        /// </summary>
        public Int32 Index
        {
            get
            {
                return FactorField.Index;
            }
        }

        /// <summary>
        /// Information for this species fact field.
        /// </summary>
        public String Information
        {
            get
            {
                return FactorField.Information;
            }
        }

        /// <summary>
        /// Value as Int32 type for this species fact.
        /// </summary>
        public Int32 Int32Value
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.Int32);
                if (HasValue)
                {
                    if (IsTextField)
                    {
                        return Int32.Parse(_stringValue);
                    }
    
                    return (Int32)_numericValue;
                }

                return Int32.MinValue;
            }

            set
            {
                SpeciesFact.CheckManualUpdate();
                SetInt32Value(value);
                SpeciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Test if this species fact field is stored as Double.
        /// </summary>
        public Boolean IsDoubleField
        {
            get
            {
                return (!IsTextField);
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact
        /// field is a main field.
        /// </summary>
        public Boolean IsMain
        {
            get
            {
                return FactorField.IsMain;
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact field is a substantial field.
        /// </summary>
        public Boolean IsSubstantial
        {
            get
            {
                return FactorField.IsSubstantial;
            }
        }

        /// <summary>
        /// Test if this species fact field is stored as String.
        /// </summary>
        public Boolean IsTextField
        {
            get
            {
                return ((Index == 3) || (Index == 4));
            }
        }

        /// <summary>
        /// Label for this species fact field.
        /// </summary>
        public String Label
        {
            get
            {
                return FactorField.Label;
            }
        }

        /// <summary>
        /// Factor field max size for this species fact field.
        /// </summary>
        public Int32 MaxSize
        {
            get
            {
                return FactorField.Size;
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact had a value.
        /// </summary>
        private Boolean OldHasValue { get; set; }

        /// <summary>
        /// Old value of this species fact field.
        /// </summary>
        private Object OldValue { get; set; }

        /// <summary>
        /// Species fact that this species fact field belongs to.
        /// </summary>
        public ISpeciesFact SpeciesFact { get; set; }

        /// <summary>
        /// Value as String type for this species fact field.
        /// </summary>
        public String StringValue
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.String);
                if (HasValue)
                {
                    if (IsTextField)
                    {
                        return _stringValue;
                    }

                    return _numericValue.WebToString();
                }

                return null;
            }

            set
            {
                SpeciesFact.CheckManualUpdate();
                SetStringValue(value);
                SpeciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Factor field type for this species fact field.
        /// </summary>
        public IFactorFieldType Type
        {
            get
            {
                return FactorField.Type;
            }
        }

        /// <summary>
        /// Unit label for this species fact field.
        /// </summary>
        public String Unit
        {
            get
            {
                return FactorField.Unit;
            }
        }

        /// <summary>
        /// Value of this species fact field.
        /// </summary>
        public Object Value
        {
            get
            {
                switch (Type.DataType)
                {
                    case FactorFieldDataTypeId.Boolean:
                        return BooleanValue;
                    case FactorFieldDataTypeId.Double:
                        return DoubleValue;
                    case FactorFieldDataTypeId.Enum:
                        return EnumValue;
                    case FactorFieldDataTypeId.Int32:
                        return Int32Value;
                    case FactorFieldDataTypeId.String:
                        return StringValue;
                    default:
                        throw new Exception("Unknown factor field data type!");
                }
            }

            set
            {
                if (SpeciesFact.AllowManualUpdate)
                {
                    if (value.IsNull())
                    {
                        HasValue = false;
                        _stringValue = null;
                    }
                    else
                    {
                        switch (Type.DataType)
                        {
                            case FactorFieldDataTypeId.Boolean:
                                // Check if the boolean value is represented by a string
                                string boolString = value as string;

                                if (boolString.IsNotNull())
                                {
                                    SetBooleanValue(bool.Parse(value.ToString()));
                                }
                                else
                                {
                                    SetBooleanValue((bool)value);
                                }

                                break;
                            case FactorFieldDataTypeId.Double:
                                SetDoubleValue(double.Parse(value.ToString()));
                                break;
                            case FactorFieldDataTypeId.Enum:
                                SetEnumValue((FactorFieldEnumValue)value);
                                break;
                            case FactorFieldDataTypeId.Int32:
                                SetInt32Value(int.Parse(value.ToString()));
                                break;
                            case FactorFieldDataTypeId.String:
                                SetStringValue((string)value);
                                break;
                            default:
                                throw new Exception("Unknown factor field data type!");
                        }
                    }
                }

                SpeciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Check that species fact field is of the same data type
        /// as the one that is used to access it's data.
        /// </summary>
        /// <param name="dataType">Data type that is used to access the species fact fields data.</param>
        private void CheckDataType(FactorFieldDataTypeId dataType)
        {
            if (dataType != Type.DataType)
            {
                throw new Exception("This species fact field is not of type " + dataType + "!");
            }
        }

        /// <summary>
        /// Get Boolean value.
        /// Difference between property BooleanValue and method GetBoolean
        /// is that GetBoolean may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        public Boolean GetBoolean()
        {
            return BooleanValue;
        }

        /// <summary>
        /// Get Double value.
        /// Difference between property DoubleValue and method GetDouble
        /// is that GetDouble may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        public Double GetDouble()
        {
            return Double.Parse(Value.ToString());
        }

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
        public Double GetDoubleValue()
        {
            if (IsTextField)
            {
                throw new Exception("Not a numeric field!");
            }
            else
            {
                return _numericValue;
            }
        }

        /// <summary>
        /// Get Int32 value.
        /// Difference between property Int32Value and method GetInt32
        /// is that GetInt32 may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        public Int32 GetInt32()
        {
            if ((Type.DataType == FactorFieldDataTypeId.Enum) &&
                EnumValue.KeyInt.HasValue)
            {
                return EnumValue.KeyInt.Value;
            }
            else
            {
                return (Int32)GetDouble();
            }
        }

        /// <summary>
        /// Get Int64 value.
        /// </summary>
        /// <returns>The value.</returns>
        public Int64 GetInt64()
        {
            if ((Type.DataType == FactorFieldDataTypeId.Enum) &&
                EnumValue.KeyInt.HasValue)
            {
                return EnumValue.KeyInt.Value;
            }
            else
            {
                return (Int64)GetDouble();
            }
        }

        /// <summary>
        /// Get String value.
        /// Difference between property StringValue and method GetString
        /// is that GetString may change the data type of the field.
        /// </summary>
        /// <returns>The value.</returns>
        public String GetString()
        {
            if (HasValue)
            {
                return Value.ToString();
            }
            else
            {
                return null;
            }
        }

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
        public String GetStringValue()
        {
            if (IsTextField)
            {
                return _stringValue;
            }
            else
            {
                throw new Exception("Not a string field!");
            }
        }

        /// <summary>
        /// Reset species fact field that has been deleted from database.
        /// Set all values to default or null.
        /// </summary>
        public void Reset()
        {
            this.Update(false, null);
        }

        /// <summary>
        /// Sets the right value for a boolean data type.
        /// </summary>
        /// <param name="value">Value to set.</param>
        private void SetBooleanValue(Boolean value)
        {
            CheckDataType(FactorFieldDataTypeId.Boolean);
            if (IsTextField)
            {
                _stringValue = value.ToString();
            }
            else
            {
                _numericValue = value ? 1 : 0;
            }

            HasValue = true;
        }

        /// <summary>
        /// Sets the right value for a Double data type.
        /// </summary>
        /// <param name="value">Value to set.</param>
        private void SetDoubleValue(Double value)
        {
            CheckDataType(FactorFieldDataTypeId.Double);
            if (IsTextField)
            {
                _stringValue = value.WebToString();
            }
            else
            {
                _numericValue = value;
            }

            HasValue = true;
        }

        /// <summary>
        /// Sets the value for an enumeration data type.
        /// </summary>
        /// <param name="value">Value to set.</param>
        private void SetEnumValue(IFactorFieldEnumValue value)
        {
            CheckDataType(FactorFieldDataTypeId.Enum);
            if (value.IsNull())
            {
                HasValue = false;
                _stringValue = null;
            }
            else
            {
                if (IsTextField)
                {
                    _stringValue = value.KeyText;
                }
                else
                {
                    _numericValue = value.KeyInt.HasValue ? value.KeyInt.Value : Double.MinValue;
                }

                HasValue = true;
            }
        }

        /// <summary>
        /// Sets the value for an Int32 data type.
        /// </summary>
        /// <param name="value">Value to set.</param>
        private void SetInt32Value(Int32 value)
        {
            CheckDataType(FactorFieldDataTypeId.Int32);
            if (IsTextField)
            {
                _stringValue = value.WebToString();
            }
            else
            {
                _numericValue = value;
            }

            HasValue = true;
        }

        /// <summary>
        /// Sets the value for a String data type.
        /// </summary>
        /// <param name="value">Value to set.</param>
        private void SetStringValue(string value)
        {
            CheckDataType(FactorFieldDataTypeId.String);
            if (IsTextField)
            {
                _stringValue = value;
            }
            else
            {
                _numericValue = double.Parse(value);
            }

            HasValue = value.IsNotEmpty();
        }

        /// <summary>
        /// Set species fact field value as a result of
        /// an automatic calculation.
        /// </summary>
        /// <param name='value'>New value for species fact field.</param>
        public void SetValueAutomatic(Object value)
        {
            SpeciesFact.CheckAutomaticUpdate();

            if (value.IsNull())
            {
                HasValue = false;
                _stringValue = null;
            }
            else
            {
                switch (Type.DataType)
                {
                    case FactorFieldDataTypeId.Boolean:
                        SetBooleanValue((Boolean)value);
                        break;
                    case FactorFieldDataTypeId.Double:
                        SetDoubleValue((Double)value);
                        break;
                    case FactorFieldDataTypeId.Enum:
                        SetEnumValue((FactorFieldEnumValue)value);
                        break;
                    case FactorFieldDataTypeId.Int32:
                        SetInt32Value((Int32)value);
                        break;
                    case FactorFieldDataTypeId.String:
                        SetStringValue((String)value);
                        break;
                    default:
                        throw new Exception("Unknown factor field data type!");
                }
            }

            SpeciesFact.FireUpdateEvent();
        }

        /// <summary>
        /// Update species fact field.
        /// </summary>
        /// <param name="hasValue">Indication if value has been set.</param>
        /// <param name="value">New value.</param>
        public void Update(Boolean hasValue, Object value)
        {
            HasValue = hasValue;
            if (HasValue)
            {
                if (IsTextField)
                {
                    if (value.IsNull())
                    {
                        _stringValue = null;
                    }
                    else
                    {
                        _stringValue = value.ToString();
                    }
                }
                else
                {
                    _numericValue = (Double)value;
                    _stringValue = null;
                }
            }

            OldHasValue = hasValue;
            if (OldHasValue)
            {
                OldValue = Value;
                if (OldValue.IsNull())
                {
                    HasValue = false;
                    OldHasValue = false;
                }
            }
            else
            {
                OldValue = null;
            }
        }
    }
}