using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Enum that contains factor field enum ids.
    /// </summary>
    public enum FactorFieldEnumId
    {
        /// <summary>RedListTaxonType = 9</summary>
        RedListTaxonType = 9,
        /// <summary>OrganismGroup = 10</summary>
        OrganismGroup = 10,
        /// <summary>CountyOccurrence = 24</summary>
        CountyOccurrence = 24,
        /// <summary>RedlistCategory = 29</summary>
        RedlistCategory = 29
    }

    /// <summary>
    /// Enum that contains values for CountyOccurrence enum.
    /// This enum should only be used if a program handles
    /// the values differently.
    /// </summary>
    public enum CountyOccurrenceEnum
    {
        /// <summary>Missing = 0</summary>
        Missing = 0,
        /// <summary>Uncertain = 1</summary>
        Uncertain = 1,
        /// <summary>Temporary = 2</summary>
        Temporary = 2,
        /// <summary>Disappeared = 3</summary>
        Disappeared = 3,
        /// <summary>Resident = 4</summary>
        Resident = 4
    }

    /// <summary>
    /// Enum that contains values for NatureTypeImportance enum.
    /// This enum should only be used if a program handles
    /// the values differently.
    /// </summary>
    public enum LandscapeTypeImportanceEnum
    {
        /// <summary>NoImportance = 0</summary>
        NoImportance = 0,
        /// <summary>HasImportance = 1</summary>
        HasImportance = 1,
        /// <summary>VeryImportant = 2</summary>
        VeryImportant = 2
    }

    /// <summary>
    /// Enum that contains values for RedListCategory enum.
    /// This enum should only be used if a program handles
    /// the values differently.
    /// </summary>
    public enum RedListCategoryEnum
    {
        /// <summary>EX = -1</summary>
        EX = -1,
        /// <summary>DD = 0</summary>
        DD = 0,
        /// <summary>RE = 1</summary>
        RE = 1,
        /// <summary>CR = 2</summary>
        CR = 2,
        /// <summary>EN = 3</summary>
        EN = 3,
        /// <summary>VU = 4</summary>
        VU = 4,
        /// <summary>NT = 5</summary>
        NT = 5,
        /// <summary>LC = 6</summary>
        LC = 6,
        /// <summary>NA = 7</summary>
        NA = 7,
        /// <summary>NE = 8</summary>
        NE = 8
    }

    /// <summary>
    /// Enum that contains values for RedListTaxonType enum.
    /// This integer value for this enum corresonds to
    /// the index in FactorFieldEnumList where the real
    /// enum value can be found as text.
    /// This enum should only be used if a program handles
    /// the values differently.
    /// </summary>
    public enum RedListTaxonTypeEnum
    {
        /// <summary>Species = 0</summary>
        Species = 0,
        /// <summary>SmallSpecies = 1</summary>
        SmallSpecies = 1,
        /// <summary>SubSpecies = 2</summary>
        SubSpecies = 2
    }

    /// <summary>
    /// Enum that contains values for species protection levels.
    /// These protection levels are used to handle access rights
    /// to species observations.
    /// </summary>
    public enum SpeciesProtectionLevelEnum
    {
        /// <summary>
        /// Public = 0.
        /// Anyone may view observations with this protection level.
        /// </summary>
        Public = 1,
        /// <summary>Protected1 = 2</summary>
        Protected1 = 2,
        /// <summary>Protected5 = 3</summary>
        Protected5 = 3,
        /// <summary>Protected25 = 4</summary>
        Protected25 = 4,
        /// <summary>Protected50 = 5</summary>
        Protected50 = 5,
        /// <summary>MaxProtected = 6</summary>
        MaxProtected = 6
    }

    /// <summary>
    /// This class represents a factor field enum.
    /// </summary>
    [Serializable]
    public class FactorFieldEnum : DataSortOrder
    {
        private FactorFieldEnumValueList _values;

        /// <summary>
        /// Create a FactorFieldEnum instance.
        /// </summary>
        /// <param name="id">If of the factor field enum</param>
        /// <param name="sortOrder">Sorting order of the factor field enum</param>
        /// <param name="values">Factor field enum values.</param>
        public FactorFieldEnum(Int32 id,
                               Int32 sortOrder,
                               FactorFieldEnumValueList values)
            : base(id, sortOrder)
        {
            _values = values;
        }

        /// <summary>
        /// Get factor field enum values for this factor field enum.
        /// </summary>
        public FactorFieldEnumValueList Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Get factor field enum value with specified key integer value.
        /// </summary>
        /// <param name="keyInt">Key integer value.</param>
        /// <returns>Factor field enum value with specified key integer value.</returns>
        public FactorFieldEnumValue GetByKeyInt(Int32 keyInt)
        {
            foreach (FactorFieldEnumValue enumValue in _values)
            {
                if (enumValue.KeyInt == keyInt)
                {
                    return enumValue;
                }
            }
            return null;
        }

        /// <summary>
        /// Get max key int value for this enum.
        /// </summary>
        /// <returns>Max key int value for this enum.</returns>
        public Int32 GetMaxKeyInt()
        {
            Int32 maxKeyInt = Int32.MinValue;

            foreach (FactorFieldEnumValue enumValue in _values)
            {
                if (maxKeyInt < enumValue.KeyInt)
                {
                    maxKeyInt = enumValue.KeyInt;
                }
            }
            return maxKeyInt;
        }

        /// <summary>
        /// Get min key int value for this enum.
        /// </summary>
        /// <returns>Min key int value for this enum.</returns>
        public Int32 GetMinKeyInt()
        {
            Int32 minKeyInt = Int32.MaxValue;

            foreach (FactorFieldEnumValue enumValue in _values)
            {
                if (enumValue.KeyInt < minKeyInt)
                {
                    minKeyInt = enumValue.KeyInt;
                }
            }
            return minKeyInt;
        }
    }
}
