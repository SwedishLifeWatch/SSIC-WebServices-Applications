namespace ArtDatabanken.Data
{
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
}