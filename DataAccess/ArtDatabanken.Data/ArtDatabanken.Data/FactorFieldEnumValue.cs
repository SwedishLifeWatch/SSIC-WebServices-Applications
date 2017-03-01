using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a factor field enumeration value.
    /// </summary>
    public class FactorFieldEnumValue : IFactorFieldEnumValue
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Get factor field enumeration for this
        /// factor field enumeration value.
        /// </summary>
        public IFactorFieldEnum Enum { get; set; }

        /// <summary>
        /// Id for this factor field enumeration value.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Get key integer value for this
        /// factor field enumeration value.
        /// </summary>
        public Int32? KeyInt { get; set; }

        /// <summary>
        /// Get key text value for this factor field enumeration value.
        /// </summary>
        public String KeyText { get; set; }

        /// <summary>
        /// Get label for this factor field enumeration value.
        /// </summary>
        public String Label { get; set; }

        /// <summary>
        /// Get information text for this factor field enumeration value.
        /// </summary>
        public String Information { get; set; }

        /// <summary>
        /// Get original label (without modifications)
        /// for this factor field enumeration value.
        /// </summary>
        public String OriginalLabel { get; set; }

        /// <summary>
        /// Get indication about whether or not this
        /// factor field enumeration value will be saved to database.
        /// </summary>
        public Boolean ShouldBeSaved { get; set; }

        /// <summary>
        /// Sort order for this factor field enumeration vale.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Get string representation of this class.
        /// Overridden from base class.
        /// </summary>
        /// <returns>String representation of this class.</returns>
        public override string ToString()
        {
            return Label;
        }
    }
}