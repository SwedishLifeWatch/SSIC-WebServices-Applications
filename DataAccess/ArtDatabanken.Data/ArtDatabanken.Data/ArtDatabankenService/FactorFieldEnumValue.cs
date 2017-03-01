using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a factor field enum value.
    /// </summary>
    [Serializable]
    public class FactorFieldEnumValue : DataSortOrder
    {
        private Int32 _factorFieldEnumId;
        private String _keyText;
        private Int32 _keyInt;
        private String _label;
        private String _information;
        private String _originalLabel;
        private Boolean _shouldBeSaved;
        private Boolean _keyHasIntegerValue;

        /// <summary>
        /// Create a FactorFieldEnumValue instance.
        /// </summary>
        /// <param name="id">Id of the factor field enum value</param>
        /// <param name="factorFieldEnumId">Id of the factor field enum</param>
        /// <param name="keyText">Text value to be used for storage of text enum fields in SpeciesFacts</param>
        /// <param name="keyInt">Integer value to be used for storage of text enum fields in SpeciesFacts</param>
        /// <param name="keyHasIntegerValue">Indication of whether or not keyInt can be used for this factor field enum value </param>
        /// <param name="label">Label of the factor field enum value</param>
        /// <param name="information">Information about the factor field enum value</param>
        /// <param name="shouldBeSaved">Indication of whether or not the factor field enum value shoul be stored in database</param>
        /// <param name="sortOrder">Sorting order of the factor field enum value</param>
        public FactorFieldEnumValue(
            Int32 id, 
            Int32 factorFieldEnumId,
            String keyText,
            Int32 keyInt,
            Boolean keyHasIntegerValue,
            String label,
            String information,
            Boolean shouldBeSaved,
            Int32 sortOrder)
            : base(id, sortOrder)
        {
            _factorFieldEnumId = factorFieldEnumId;
            _keyText = keyText;
            _keyInt = keyInt;
            _keyHasIntegerValue = keyHasIntegerValue;
            _originalLabel = label;
            if (_keyHasIntegerValue)
            {
                _label = "[" + _keyInt.ToString() + "] ";
            }
            else
            {
                _label = "[" + _keyText + "] ";
            }
            _label = _label + label;
            _information = information;
            _shouldBeSaved = shouldBeSaved;
        }

        /// <summary>
        /// Get factor field enum id for this factor field enum value.
        /// </summary>
        public Int32 FactorFieldEnumId
        {
            get { return _factorFieldEnumId; }
        }

        /// <summary>
        /// Get key text value for this factor field enum value.
        /// </summary>
        public String KeyText
        {
            get { return _keyText; }
        }

        /// <summary>
        /// Get key integer value for this factor field enum value.
        /// </summary>
        public Int32 KeyInt
        {
            get { return _keyInt; }
        }

        /// <summary>
        /// Indicates whether or not KeyInt has a value.
        /// </summary>
        public Boolean KeyHasIntegerValue
        {
            get { return _keyHasIntegerValue; }
        }

        /// <summary>
        /// Get label for this factor field enum value.
        /// </summary>
        public String Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Get information text for this factor field enum value.
        /// </summary>
        public String Information
        {
            get { return _information; }
        }

        /// <summary>
        /// Get  original label (without modifications)
        /// for this factor field enum value.
        /// </summary>
        public String OriginalLabel
        {
            get { return _originalLabel; }
        }

        /// <summary>
        /// Get indication about whether or not this factor field enum value will be saved to database.
        /// </summary>
        public Boolean ShouldBeSaved
        {
            get { return _shouldBeSaved; }
        }

        /// <summury>
        /// Get string representation of this class.
        /// Overriden from base class.
        /// </summury>
        public override string ToString()
        {
            return Label;
        }
    }
}
