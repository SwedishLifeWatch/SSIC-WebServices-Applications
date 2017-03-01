using System;
using System.Collections.Generic;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Holds factor field data
    /// </summary>
    public class DyntaxaFactorField
    {
        string label = string.Empty;
        string unitLabel = string.Empty;
        DyntaxaFactorFieldDataTypeId dataType = DyntaxaFactorFieldDataTypeId.String;
        object dataValue = null;
        string factorFieldId = string.Empty;
        bool hasValue = false;
        bool isMain = false;
        DyntaxaFactorFieldValues factorFieldValues = null;

        public DyntaxaFactorField(string factorFieldId, string label, string unitLabel, DyntaxaFactorFieldDataTypeId dataType, object dataValue, bool hasValue, DyntaxaFactorFieldValues factorFieldValues, bool isMain)
        {
            this.factorFieldId = factorFieldId;
            this.label = label;
            this.unitLabel = unitLabel;
            this.dataType = dataType;
            this.dataValue = dataValue;
            this.hasValue = hasValue;
            this.factorFieldValues = factorFieldValues;
            this.isMain = isMain;
        }

        public bool IsMain
        {
            get
            {
                return isMain;
            }
            set
            {
                isMain = value;
            }
        }

        public string FactorFieldId
        {
            get
            {
                return factorFieldId;
            }
            set
            {
                factorFieldId = value;
            }
        }

        public string UnitLabel
        {
            get
            {
                return unitLabel;
            }
            set
            {
                unitLabel = value;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        public DyntaxaFactorFieldDataTypeId DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }

        public object DataValue
        {
            get
            {
                return dataValue;
            }
            set
            {
                dataValue = value;
            }
        }

        public bool HasValue
        {
            get { return hasValue; }
        }

        public DyntaxaFactorFieldValues FactorFieldValues
        {
            get
            {
                return factorFieldValues;
            }
            set
            {
                factorFieldValues = value;
            }
        }
    }

    /// <summary>
    ///  Enum that contains factor field type ids.
    /// </summary>
    public enum DyntaxaFactorFieldDataTypeId
    {
        /// <summary>
        /// Boolean data type.
        /// </summary>
        Boolean = 0,

        /// <summary>
        /// Enum data type.
        /// </summary>
        Enum = 1,

        /// <summary>
        /// String data type.
        /// </summary>
        String = 2,

        /// <summary>
        /// Int32 data type.
        /// </summary>
        Int32 = 3,

        /// <summary>
        /// Double data type.
        /// </summary>
        Double = 4
    }

    public class DyntaxaFactorFieldValues
    {
        private string fieldName = String.Empty;
        private int fieldValue = SpeciesFactModelManager.SpeciesFactNoValueSet;
        IList<KeyValuePair<string, int>> factorFields = new List<KeyValuePair<string, int>>();

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        public int FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        public IList<KeyValuePair<string, int>> FactorFields
        {
            get { return factorFields; }
            set { factorFields = value; }
        }
    }
}