namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum that contains values for ImpactImportance enum.
    /// This enum should only be used if a program handles
    /// the values differently.
    /// </summary>
    public enum ImpactImportanceEnum
    {
        /// <summary>
        /// Large negative impact = -2.
        /// </summary>
        LargeNeagtiveImpact = -2,

        /// <summary>
        /// Negative impact = -1.
        /// </summary>
        NeagtiveImpact = -1,

        /// <summary>
        /// No impact = 0.
        /// </summary>
        NoImpact = 0,

        /// <summary>
        /// Positive impact = 1.
        /// </summary>
        PositiveImpact = 1,

        /// <summary>
        /// Large positive impact = 2.
        /// </summary>
        LargePositiveImpact = 2
    }
}