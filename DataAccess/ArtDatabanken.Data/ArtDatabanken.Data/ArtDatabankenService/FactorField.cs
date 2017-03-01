using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a factor field.
    /// </summary>
    [Serializable]
    public class FactorField : DataSortOrder
    {
        private Int32 _factorDataTypeId;
        private Int32 _fieldIndex;
        private String _label;
        private String _information;
        private Boolean _isMain;
        private Boolean _isSubstantial;
        private FactorFieldType _type;
        private Int32 _size;
        private FactorFieldEnum _factorFieldEnum = null;
        private String _unitLabel;

        /// <summary>
        /// Create a FactorField instance.
        /// </summary>
        /// <param name="id">Id of the factor field</param>
        /// <param name="sortOrder">Sort order of the factor field</param>
        /// <param name="factorDataTypeId">Id of the datatype of the factor field</param>
        /// <param name="fieldIndex">Index of this factor field related to all possible factor fields.</param>
        /// <param name="label">Label of the factor field</param>
        /// <param name="information">Information about the factor field</param>
        /// <param name="isMain">Indicates whether or not the factor field is the main field</param>
        /// <param name="isSubstantial">Indicates whether or not the factor field is a substantial field</param>
        /// <param name="factorFieldTypeId">Id of the field type</param>
        /// <param name="size">Size of the field (Max length of text fields)</param>
        /// <param name="factorFieldEnumId">Id of the factor field enum object. Id less than 0 indicates that the factor has no factor field enum</param>
        /// <param name="unitLabel">Unit label of the factor field</param>
        public FactorField(
            Int32 id,
            Int32 sortOrder,
            Int32 factorDataTypeId,
            Int32 fieldIndex,
            String label,
            String information,
            Boolean isMain,
            Boolean isSubstantial,
            Int32 factorFieldTypeId,
            Int32 size,
            Int32 factorFieldEnumId,
            String unitLabel)
            : base(id, sortOrder)
        {
            _factorDataTypeId = factorDataTypeId;
            _fieldIndex = fieldIndex;
            _information = information;
            _isMain = isMain;
            _isSubstantial = isSubstantial;
            _label = label;
            if (factorFieldEnumId > -1)
            {
                _factorFieldEnum = FactorManager.GetFactorFieldEnum(factorFieldEnumId);
            }
            _type = FactorManager.GetFactorFieldType(factorFieldTypeId);
            _size = size;
            _unitLabel = unitLabel;
        }

        /// <summary>
        /// Get the factor data type id of this factor field object.
        /// </summary>
        public Int32 FactorDataTypeId
        {
            get { return _factorDataTypeId; }
        }

        /// <summary>
        /// Get the factor field enum object assosiated with this field. 
        /// </summary>
        public FactorFieldEnum FactorFieldEnum
        {
            get { return _factorFieldEnum; }
        }

        /// <summary>
        /// Get index of this factor field related to all possible factor fields.
        /// </summary>
        public Int32 Index
        {
            get { return _fieldIndex; }
        }

        /// <summary>
        /// Get factor field information.
        /// </summary>
        public String Information
        {
            get { return _information; }
        }

        /// <summary>
        /// Indicates whether or not this factor field is a main field
        /// </summary>
        public Boolean IsMain
        {
            get { return _isMain; }
        }

        /// <summary>
        /// Indicates whether or not this factor field is a substantial field
        /// </summary>
        public Boolean IsSubstantial
        {
            get { return _isSubstantial; }
        }

        /// <summary>
        /// Get factor field label.
        /// </summary>
        public String Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Get max length of the factor field
        /// </summary>
        public Int32 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Get factor field type
        /// </summary>
        public FactorFieldType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Get factor field unit label.
        /// </summary>
        public String UnitLabel
        {
            get { return _unitLabel; }
        }
    }
}
