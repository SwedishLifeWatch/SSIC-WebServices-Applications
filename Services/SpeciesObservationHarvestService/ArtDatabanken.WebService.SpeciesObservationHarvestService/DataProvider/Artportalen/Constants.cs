using System;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen
{
    /// <summary>
    /// Constants used when accessing data from Artportalen.
    /// </summary>
    public struct Artportalen
    {
        /// <summary>
        /// Start Date.
        /// </summary>
        public const String CHANGED_FROM = "changedFrom";

        /// <summary>
        /// End Date.
        /// </summary>
        public const String CHANGED_TO = "changedTo";

        /// <summary>
        /// Is production.
        /// </summary>
        public const String IS_PRODUCTION = "isProduction";

        /// <summary>
        /// Sighting Ids.
        /// </summary>
        public const String SIGHTING_IDS = "sightingIds";
    }
}
