namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Enumeration that contains values for HarvestStatus.
    /// Used as current status by harvest service and requested status by user.
    /// </summary>
    public enum HarvestStatusEnum
    {
        /// <summary>
        /// Status when work is completed.
        /// </summary>
        Done = 0,

        /// <summary>
        /// Status when work is ongoing.
        /// </summary>
        Working = 1,

        /// <summary>
        /// Status when work is paused.
        /// </summary>
        Paused = 2,

        /// <summary>
        /// Status when work is stopped.
        /// </summary>
        Stopped = 3
    }
}