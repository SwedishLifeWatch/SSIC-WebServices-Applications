using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to the WebSpeciesObservationInformation class.
    /// </summary>
    public static class WebSpeciesObservationInformationExtension
    {
        /// <summary>
        /// Set species observation count information.
        /// </summary>
        /// <param name="speciesObservationInformation">Species observation information.</param>
        public static void SetCount(this WebSpeciesObservationInformation speciesObservationInformation)
        {
            speciesObservationInformation.MaxSpeciesObservationCount = Settings.Default.MaxSpeciesObservationWithInformation;
            if (speciesObservationInformation.SpeciesObservationIds.IsNotNull())
            {
                speciesObservationInformation.SpeciesObservationCount = speciesObservationInformation.SpeciesObservationIds.Count;
            }
            else if (speciesObservationInformation.SpeciesObservations.IsNotNull())
            {
                // ReSharper disable once PossibleNullReferenceException
                speciesObservationInformation.SpeciesObservationCount = speciesObservationInformation.SpeciesObservations.Count;
            }
            else
            {
                speciesObservationInformation.SpeciesObservationCount = 0;
            }
        }
    }
}
