using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a species fact field.
    /// </summary>
    [Serializable]
    public class SpeciesFactField : DataSortOrder
    {
        private Boolean _hasValue;
        private Boolean _oldHasValue;
        private Double _numericValue;
        private FactorField _factorField;
        private Object _oldValue;
        private SpeciesFact _speciesFact;
        private String _stringValue;
        
        /// <summary>
        /// Create a SpeciesFactField instance with data from database.
        /// </summary>
        /// <param name="speciesFact">The SpeciesFact that this SpeciesFactField belongs to.</param>
        /// <param name="factorField">Factor field for the species fact field</param>
        /// <param name="hasValue">Indication whether or not a value is initially specified</param>
        /// <param name="value">The value of the species fact field</param>
        public SpeciesFactField(
            SpeciesFact speciesFact,
            FactorField factorField,
            Boolean hasValue,
            Object value)
            : base(factorField.Id, factorField.Id)
        {
            _speciesFact = speciesFact;
            _factorField = factorField;

            UpdateData(hasValue, value);
        }

        /// <summary>
        /// Create a SpeciesFactField instance with default data.
        /// </summary>
        /// <param name="speciesFact">The SpeciesFact that this SpeciesFactField belongs to.</param>
        /// <param name="factorField">Factor field for the species fact field</param>
        public SpeciesFactField(SpeciesFact speciesFact,
                                FactorField factorField)
            : base(factorField.Id, factorField.Id)
        {
            _speciesFact = speciesFact;
            _factorField = factorField;

            UpdateData(false, null);
        }

        /// <summary>
        /// Value as Boolean type for this species fact.
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
                        return Boolean.Parse(_stringValue);
                    }
                    else
                    {
                        return _numericValue == 1;
                    }
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _speciesFact.CheckManualUpdate();
                SetBooleanValue(value);
                _speciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Value as Double type for this species fact.
        /// </summary>
        public Double DoubleValue
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.Double);
                if (HasValue)
                {
                    if (IsTextField)
                    {
                        return Double.Parse(_stringValue);
                    }
                    else
                    {
                        return _numericValue;
                    }
                }
                return Double.MinValue;
            }
            set
            {
                _speciesFact.CheckManualUpdate();
                SetDoubleValue(value);
                _speciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Value for this species fact field.
        /// </summary>
        public FactorFieldEnumValue EnumValue
        {
            get
            {
                CheckDataType(FactorFieldDataTypeId.Enum);
                if (HasValue)
                {
                    if (IsTextField)
                    {
                        foreach (FactorFieldEnumValue enumVal in this.FactorFieldEnum.Values)
                        {
                            if (enumVal.KeyText == _stringValue)
                            {
                                return enumVal;
                            }
                        }
                    }
                    else
                    {
                        foreach (FactorFieldEnumValue enumVal in this.FactorFieldEnum.Values)
                        {
                            if (enumVal.KeyInt == _numericValue)
                            {
                                return enumVal;
                            }
                        }
                    }
                }
                return null;
            }
            set
            {
                _speciesFact.CheckManualUpdate();
                SetEnumValue(value);
                _speciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Get the factor field of this species fact field object.
        /// </summary>
        public FactorField FactorField
        {
            get { return _factorField; }
        }

        /// <summary>
        /// Get the factor field enum object assosiated with this species fact field. 
        /// </summary>
        public FactorFieldEnum FactorFieldEnum
        {
            get { return _factorField.FactorFieldEnum; }
        }

        /// <summary>
        /// Indicates whether or not this species fact field has changed.
        /// </summary>
        public Boolean HasChanged
        {
            get
            {
                if (_hasValue != _oldHasValue)
                {
                    return true;
                }
                if (!_hasValue && !_oldHasValue)
                {
                    return false;
                }
                return _oldValue.ToString().Trim() != Value.ToString().Trim();
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact field
        /// has min and max values.
        /// </summary>
        public Boolean HasMinMax
        {
            get
            {
                switch (_speciesFact.Factor.Id)
                {
                    case ((Int32)FactorId.AreaOfOccupancy_B2Estimated):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.ExtentOfOccurrence_B1Estimated):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.MaxProportionLocalPopulation):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.MaxSizeLocalPopulation):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.NumberOfLocations):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.Reduction_A1):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.Reduction_A2):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.Reduction_A3):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.Reduction_A4):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    case ((Int32)FactorId.PopulationSize_Total):
                        return (_factorField.Type.DataType == FactorFieldDataTypeId.Double);
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact has a value
        /// </summary>
        public Boolean HasValue
        {
            get { return _hasValue; }
        }

        /// <summary>
        /// Get index of this species fact field related to
        /// all possible species fact fields.
        /// </summary>
        public Int32 Index
        {
            get { return _factorField.Index; }
        }

        /// <summary>
        /// Get species fact field information.
        /// </summary>
        public String Information
        {
            get { return _factorField.Information; }
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
                    else
                    {
                        return (Int32)_numericValue;
                    }
                }
                return Int32.MinValue;
            }
            set
            {
                _speciesFact.CheckManualUpdate();
                SetInt32Value(value);
                _speciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Value as Int64 type for this species fact.
        /// </summary>
        public Int64 Int64Value
        {
            get
            {
                return Int32Value;
            }
            set
            {
                Int32Value = (Int32)value;
            }
        }

        /// <summary>
        /// Indicates whether or not this species fact field is a main field
        /// </summary>
        public Boolean IsMain
        {
            get { return _factorField.IsMain; }
        }

        /// <summary>
        /// Indicates whether or not this species fact field is a substantial field
        /// </summary>
        public Boolean IsSubstantial
        {
            get { return _factorField.IsSubstantial; }
        }

        /// <summary>
        /// Get species fact field label.
        /// </summary>
        public String Label
        {
            get { return _factorField.Label; }
        }

        /// <summary>
        /// Get factor field max size
        /// </summary>
        public Int32 MaxSize
        {
            get { return _factorField.Size; }
        }

        /// <summary>
        /// Value as String type for this species fact.
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
                    else
                    {
                        return _numericValue.ToString();
                    }
                }
                return null;
            }
            set
            {
                _speciesFact.CheckManualUpdate();
                SetStringValue(value);
                _speciesFact.FireUpdateEvent();
            }
        }

        /// <summary>
        /// Get factor field type
        /// </summary>
        public FactorFieldType Type
        {
            get { return _factorField.Type; }
        }

        /// <summary>
        /// Get species fact field unit label.
        /// </summary>
        public String UnitLabel
        {
            get { return _factorField.UnitLabel; }
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
        /// Test if this species fact field is stored as String.
        /// </summary>
        public Boolean IsTextField
        {
            get { return ((Index == 3) || (Index == 4)); }
        }

        /// <summary>
        /// Value for this species fact.
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
                _speciesFact.CheckManualUpdate();

                if (value.IsNull())
                {
                    _hasValue = false;
                    _stringValue = null;
                }
                else
                {

                    switch (Type.DataType)
                    {
                        case FactorFieldDataTypeId.Boolean:
                            
                            //Check if the boolean value is represented by a string
                            string boolString = value as string;
                            if (boolString != null)
                            {
                                SetBooleanValue(Boolean.Parse(value.ToString()));
                            }
                            else
                            {
                                SetBooleanValue((Boolean)value);
                            }
                            break;
                        case FactorFieldDataTypeId.Double:
                            SetDoubleValue(Double.Parse(value.ToString()));
                            break;
                        case FactorFieldDataTypeId.Enum:
                            SetEnumValue((FactorFieldEnumValue)value);
                            break;
                        case FactorFieldDataTypeId.Int32:
                            SetInt32Value(Int32.Parse(value.ToString()));
                            break;
                        case FactorFieldDataTypeId.String:
                            SetStringValue((String)value);
                            break;
                        default:
                            throw new Exception("Unknown factor field data type!");
                    }
                }
                _speciesFact.FireUpdateEvent();
            }
        }


        /// <summary>
        /// Value for this species fact.
        /// </summary>
        public Object OldValue
        {
            get
            {
                return _oldValue;
            }
        }

        /// <summary>
        /// check that speices fact field is of the same data type
        /// as the one that is used to access it's data.
        /// </summary>
        /// <param name='dataType'>Data type that is used to access the species fact fields data.</param>
        private void CheckDataType(FactorFieldDataTypeId dataType)
        {
            if (dataType != Type.DataType)
            {
                throw new Exception("This species fact field is not of type " + dataType.ToString() + "!");
            }
        }

        /// <summary>
        /// Get Boolean value.
        /// </summary>
        /// <returns>The value.</returns>
        public Boolean GetBoolean()
        {
//            return (GetInt32() == 1);
            return BooleanValue;
        }

        /// <summary>
        /// Get Double value.
        /// </summary>
        /// <returns>The value.</returns>
        public Double GetDouble()
        {
            return Double.Parse(Value.ToString());
        }

        /// <summary>
        /// Get Double value.
        /// </summary>
        /// <returns>The value.</returns>
        internal Double GetDoubleValue()
        {
            return _numericValue;
        }

        /// <summary>
        /// Get Int32 value.
        /// </summary>
        /// <returns>The value.</returns>
        public Int32 GetInt32()
        {
            if (Type.DataType == FactorFieldDataTypeId.Enum)
            {
                return EnumValue.KeyInt;
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
            if (Type.DataType == FactorFieldDataTypeId.Enum)
            {
                return EnumValue.KeyInt;
            }
            else
            {
                return (Int64)GetDouble();
            }
        }

        /// <summary>
        /// Get min and max values for this species fact field.
        /// Values are returned in parameter minValue and maxValue.
        /// </summary>
        /// <param name='minValue'>Is set to min value.</param>
        /// <param name='maxValue'>Is set to max value.</param>
        public void GetMinMax(ref Double minValue, ref Double maxValue)
        {
            switch (_speciesFact.Factor.Id)
            {
                case ((Int32)FactorId.AreaOfOccupancy_B2Estimated):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = RedListCalculation.AREA_OF_OCCUPANCY_MIN;
                        maxValue = RedListCalculation.AREA_OF_OCCUPANCY_MAX;
                        return;
                    }
                    break;
                case ((Int32)FactorId.ExtentOfOccurrence_B1Estimated):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN;
                        maxValue = RedListCalculation.EXTENT_OF_OCCURRENCE_MAX;
                        return;
                    }
                    break;
                case ((Int32)FactorId.MaxProportionLocalPopulation):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN;
                        maxValue = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX;
                        return;
                    }
                    break;
                case ((Int32)FactorId.MaxSizeLocalPopulation):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN);
                        maxValue = (Double)(RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
                        return;
                    }
                    break;
                case ((Int32)FactorId.NumberOfLocations):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.NUMBER_OF_LOCATIONS_MIN);
                        maxValue = (Double)(RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
                        return;
                    }
                    break;
                case ((Int32)FactorId.Reduction_A1):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A1_MIN);
                        maxValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A1_MAX);
                        return;
                    }
                    break;
                case ((Int32)FactorId.Reduction_A2):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A2_MIN);
                        maxValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A2_MAX);
                        return;
                    }
                    break;
                case ((Int32)FactorId.Reduction_A3):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A3_MIN);
                        maxValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A3_MAX);
                        return;
                    }
                    break;
                case ((Int32)FactorId.Reduction_A4):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A4_MIN);
                        maxValue = (Double)(RedListCalculation.POPULATION_REDUCTION_A4_MAX);
                        return;
                    }
                    break;
                case ((Int32)FactorId.PopulationSize_Total):
                    if (_factorField.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        minValue = (Double)(RedListCalculation.POPULATION_SIZE_MIN);
                        maxValue = (Double)(RedListCalculation.POPULATION_SIZE_MAX);
                        return;
                    }
                    break;
            }
            throw new Exception("This species fact field does not have min and max values.");
        }

        /// <summary>
        /// Get min and max values for this species fact field.
        /// Values are returned in parameter minValue and maxValue.
        /// </summary>
        /// <param name='minValue'>Is set to min value.</param>
        /// <param name='maxValue'>Is set to max value.</param>
        public void GetMinMax(ref Int32 minValue, ref Int32 maxValue)
        {
            throw new Exception("This species fact field does not have min and max values.");
        }

        /// <summary>
        /// Get String value.
        /// </summary>
        /// <returns>The value.</returns>
        internal String GetStringValue()
        {
            return _stringValue;
        }

        /// <summary>
        /// Reset species fact field that has been deleted from database.
        /// Set all values to default or null.
        /// </summary>
        internal void Reset()
        {
            UpdateData(false, null);
        }

        private void SetBooleanValue(Boolean value)
        {
            CheckDataType(FactorFieldDataTypeId.Boolean);
            _hasValue = true;
            if (IsTextField)
            {
                _stringValue = value.ToString();
            }
            else
            {
                if (value)
                {
                    _numericValue = 1;
                }
                else
                {
                    _numericValue = 0;
                }
            }
        }

        private void SetDoubleValue(Double value)
        {
            CheckDataType(FactorFieldDataTypeId.Double);
            if (IsTextField)
            {
                _stringValue = value.ToString();
            }
            else
            {
                _numericValue = value;
            }
            _hasValue = true;
        }

        private void SetEnumValue(FactorFieldEnumValue value)
        {
            CheckDataType(FactorFieldDataTypeId.Enum);
            if (value.IsNull())
            {
                _hasValue = false;
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
                    _numericValue = value.KeyInt;
                }
                _hasValue = true;
            }
        }

        private void SetInt32Value(Int32 value)
        {
            CheckDataType(FactorFieldDataTypeId.Int32);
            if (IsTextField)
            {
                _stringValue = value.ToString();
            }
            else
            {
                _numericValue = (Double)value;
            }
            _hasValue = true;
        }

        private void SetStringValue(String value)
        {
            CheckDataType(FactorFieldDataTypeId.String);
            if (IsTextField)
            {
                _stringValue = value;
            }
            else
            {
                _numericValue = Double.Parse(value);
            }
            _hasValue = value.IsNotEmpty();
        }

        /// <summary>
        /// Set species fact field value as a result of
        /// an automatic calculation.
        /// </summary>
        /// <param name='value'>New value for species fact field.</param>
        public void SetValueAutomatic(Object value)
        {
            _speciesFact.CheckAutomaticUpdate();

            if (value.IsNull())
            {
                _hasValue = false;
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
           _speciesFact.FireUpdateEvent();
        }

        /// <summary>
        /// Update species fact field with new data from database.
        /// </summary>
        /// <param name="hasValue">Indication if value has been set.</param>
        /// <param name="value">New value.</param>
        internal void Update(Boolean hasValue, Object value)
        {
            UpdateData(hasValue, value);
        }

        /// <summary>
        /// Update species fact field.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <param name="hasValue">Indication if value has been set.</param>
        private void UpdateData(Boolean hasValue, Object value)
        {
            _hasValue = hasValue;
            if (_hasValue)
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
            _oldHasValue = _hasValue;
            if (_oldHasValue)
            {
                _oldValue = Value;

                if (_oldValue.IsNull())
                {
                    // This has happened with enum values 
                    // that has been set to no value.
                    // Probably a bug that should be fixed 
                    // when saving enum values that has
                    // been set to nothing.
                    _hasValue = false;
                    _oldHasValue = false;
                }
            }
            else
            {
                _oldValue = null;
            }
        }
    }
}
