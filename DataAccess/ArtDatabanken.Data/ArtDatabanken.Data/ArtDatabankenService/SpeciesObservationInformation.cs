using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds information returned in response
    /// to a call to any of the
    /// SpeciesObservationManager.GetSpeciesObservation...
    /// methods.
    /// </summary>
    public class SpeciesObservationInformation
    {
        /// <summary>
        /// Create a SpeciesObservationInformation instance.
        /// </summary>
        public SpeciesObservationInformation()
        {
            Cancelled = false;
            SpeciesObservations = new SpeciesObservationList();
        }

        /// <summary>
        /// Test if operation was cancelled before the
        /// result was returned.
        /// </summary>
        public Boolean Cancelled
        { get; set; }

        /// <summary>
        /// Handle species observations.
        /// </summary>
        public SpeciesObservationList SpeciesObservations
        { get; set; }
    }
}
