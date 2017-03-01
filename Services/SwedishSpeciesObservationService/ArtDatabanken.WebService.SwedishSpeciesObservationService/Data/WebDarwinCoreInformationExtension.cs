using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to the WebDarwinCoreInformation class.
    /// </summary>
    public static class WebDarwinCoreInformationExtension
    {
        /// <summary>
        /// Set species observation count information.
        /// </summary>
        /// <param name="darwinCoreInformation">Darwin core information.</param>
        public static void SetCount(this WebDarwinCoreInformation darwinCoreInformation)
        {
            darwinCoreInformation.MaxSpeciesObservationCount = Settings.Default.MaxSpeciesObservationWithInformation;
            if (darwinCoreInformation.SpeciesObservationIds.IsNotNull())
            {
                darwinCoreInformation.SpeciesObservationCount = darwinCoreInformation.SpeciesObservationIds.Count;
            }
            else if (darwinCoreInformation.SpeciesObservations.IsNotNull())
            {
                // ReSharper disable once PossibleNullReferenceException
                darwinCoreInformation.SpeciesObservationCount = darwinCoreInformation.SpeciesObservations.Count;
            }
            else
            {
                darwinCoreInformation.SpeciesObservationCount = 0;
            }
        }
    }
}
