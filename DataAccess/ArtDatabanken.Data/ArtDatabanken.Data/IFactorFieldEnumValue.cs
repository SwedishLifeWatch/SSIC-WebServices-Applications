using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor field enumeration value.
    /// </summary>
    public interface IFactorFieldEnumValue : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Get factor field enumeration for this
        /// factor field enumeration value.
        /// </summary>
        IFactorFieldEnum Enum { get; set; }

        /// <summary>
        /// Get key integer value for this
        /// factor field enumeration value.
        /// </summary>
        Int32? KeyInt { get; set; }

        /// <summary>
        /// Get key text value for this factor field enumeration value.
        /// </summary>
        String KeyText { get; set; }

        /// <summary>
        /// Get label for this factor field enumeration value.
        /// </summary>
        String Label { get; set; }

        /// <summary>
        /// Get information text for this factor field enumeration value.
        /// </summary>
        String Information { get; set; }

        /// <summary>
        /// Get original label (without modifications)
        /// for this factor field enumeration value.
        /// </summary>
        String OriginalLabel { get; set; }

        /// <summary>
        /// Get indication about whether or not this
        /// factor field enumeration value will be saved to database.
        /// </summary>
        Boolean ShouldBeSaved { get; set; }

        /// <summary>
        /// Sort order for this factor field enumeration vale.
        /// </summary>
        Int32 SortOrder { get; set; }
    }
}