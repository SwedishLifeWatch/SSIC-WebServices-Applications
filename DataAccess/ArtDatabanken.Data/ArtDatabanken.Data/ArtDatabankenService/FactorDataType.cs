using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a factor data type.
    /// </summary>
    [Serializable]
    public class FactorDataType : DataSortOrder, IListableItem
    {
        private String _name;
        private String _definition;
        private FactorField _mainField;
        private FactorField[] _fieldArray;
        private FactorFieldList _fields;
        private FactorFieldList _substantialFields;

        /// <summary>
        /// Create a FactorDataType instance.
        /// </summary>
        /// <param name="id">Id of the factor data type</param>
        /// <param name="sortOrder">Sorting order of the factor data type</param>
        /// <param name="name">Name of the factor data type</param>
        /// <param name="definition">Definition of the factor data type</param>
        /// <param name="fields">Fields for this factor data type</param>
        public FactorDataType(Int32 id,
                              Int32 sortOrder,
                              String name,
                              String definition,
                              FactorFieldList fields)
            : base(id, sortOrder)
        {
            _name = name;
            _definition = definition;
            _fields = fields;
            InitFields();
        }

        /// <summary>
        /// Get name of this factor data type object.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get definition of this factor data type object.
        /// </summary>
        public String Definition
        {
            get { return _definition; }
        }

        /// <summary>
        /// Get field 1 of this factor data type.
        /// </summary>
        public FactorField Field1
        {
            get { return _fieldArray[0]; }
        }

        /// <summary>
        /// Get field 2 of this factor data type.
        /// </summary>
        public FactorField Field2
        {
            get { return _fieldArray[1]; }
        }

        /// <summary>
        /// Get field 3 of this factor data type.
        /// </summary>
        public FactorField Field3
        {
            get { return _fieldArray[2]; }
        }

        /// <summary>
        /// Get field 4 of this factor data type.
        /// </summary>
        public FactorField Field4
        {
            get { return _fieldArray[3]; }
        }

        /// <summary>
        /// Get field 5 of this factor data type.
        /// </summary>
        public FactorField Field5
        {
            get { return _fieldArray[4]; }
        }

        /// <summary>
        /// Get all fields of this factor data type.
        /// </summary>
        public FactorFieldList Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// Get the main field of this factor data type.
        /// </summary>
        public FactorField MainField
        {
            get { return _mainField; }
        }

        /// <summary>
        /// Get all substantial fields of this factor data type.
        /// </summary>
        public FactorFieldList SubstantialFields
        {
            get { return _substantialFields; }
        }


        #region IListableItem Members

        /// <summary>
        /// Get label of this factor data type object.
        /// </summary>
        public string Label
        {
            get { return _name; }
        }

        #endregion

        /// <summary>
        /// Split existing fields into different field types.
        /// </summary>
        private void InitFields()
        {
            Int32 fieldIndex;

            // Init field holders.
            _fieldArray = new FactorField[FactorManager.GetFactorFieldMaxCount()];
            for (fieldIndex = 0; fieldIndex < FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
            {
                _fieldArray[fieldIndex] = null;
            }
            _mainField = null;
            _substantialFields = new FactorFieldList();

            // Add information to field holders.
            foreach (FactorField field in Fields)
            {
                _fieldArray[field.Index] = field;
                if (field.IsSubstantial)
                {
                    _substantialFields.Add(field);
                }
                if (field.IsMain)
                {
                    _mainField = field;
                }
            }
        }
    }
}
