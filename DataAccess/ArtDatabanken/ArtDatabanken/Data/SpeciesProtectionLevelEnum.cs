namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum that contains values for species protection levels.
    /// These protection levels are used to handle access rights
    /// to species observations.
    /// </summary>
    public enum SpeciesProtectionLevelEnum
    {
        /// <summary>
        /// Public = 1.
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
}