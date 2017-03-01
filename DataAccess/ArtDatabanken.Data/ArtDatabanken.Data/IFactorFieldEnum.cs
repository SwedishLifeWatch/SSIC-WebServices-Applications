using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor field enumeration.
    /// </summary>
    public interface IFactorFieldEnum : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Get factor field enumeration values for this
        /// factor field enumeration.
        /// </summary>
        FactorFieldEnumValueList Values { get; set; }

        /// <summary>
        /// Get factor field enumeration value with
        /// specified key integer value.
        /// </summary>
        /// <param name="keyInt">Key integer value.</param>
        /// <returns>Factor field enumeration value with specified key integer value.</returns>
        FactorFieldEnumValue GetByKeyInt(Int32 keyInt);

        /// <summary>
        /// Get max key int value for this enumeration.
        /// </summary>
        /// <returns>Max key int value for this enumeration.</returns>
        Int32 GetMaxKeyInt();

        /// <summary>
        /// Get min key int value for this enumeration.
        /// </summary>
        /// <returns>Min key int value for this enumeration.</returns>
        Int32 GetMinKeyInt();
    }
}