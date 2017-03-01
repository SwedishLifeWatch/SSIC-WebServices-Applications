using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a factor field enumeration.
    /// </summary>
    public class FactorFieldEnum : IFactorFieldEnum
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for this factor field enumeration.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Get factor field enumeration values for
        /// this factor field enumeration.
        /// </summary>
        public FactorFieldEnumValueList Values { get; set; }

        /// <summary>
        /// Get factor field enumeration value with
        /// specified key integer value.
        /// </summary>
        /// <param name="keyInt">Key integer value.</param>
        /// <returns>Factor field enumeration value with specified key integer value.</returns>
        public FactorFieldEnumValue GetByKeyInt(Int32 keyInt)
        {
            foreach (FactorFieldEnumValue enumValue in Values)
            {
                if (enumValue.KeyInt == keyInt)
                {
                    return enumValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Get max key int value for this enumeration.
        /// </summary>
        /// <returns>Max key int value for this enumeration.</returns>
        public Int32 GetMaxKeyInt()
        {
            Int32 maxKeyInt = Int32.MinValue;

            foreach (FactorFieldEnumValue enumValue in Values)
            {
                if (enumValue.KeyInt.HasValue &&
                    (maxKeyInt < enumValue.KeyInt.Value))
                {
                    maxKeyInt = enumValue.KeyInt.Value;
                }
            }

            return maxKeyInt;
        }

        /// <summary>
        /// Get min key int value for this enumeration.
        /// </summary>
        /// <returns>Min key int value for this enumeration.</returns>
        public Int32 GetMinKeyInt()
        {
            Int32 minKeyInt = Int32.MaxValue;

            foreach (FactorFieldEnumValue enumValue in Values)
            {
                if (enumValue.KeyInt.HasValue &&
                    (enumValue.KeyInt.Value < minKeyInt))
                {
                    minKeyInt = enumValue.KeyInt.Value;
                }
            }

            return minKeyInt;
        }
    }
}