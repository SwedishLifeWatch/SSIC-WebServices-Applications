namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// This class contains other classes that manages
    /// different types of data. This data is used by all
    /// web services related to species observations.
    /// </summary>
    public static class WebSpeciesObservationServiceData
    {
        static WebSpeciesObservationServiceData()
        {
            TaxonManager = new TaxonManager();
            SpeciesObservationManager = new SpeciesObservationManager();
        }

        /// <summary>
        /// Get database manager instance.
        /// </summary>
        public static ISpeciesObservationManager SpeciesObservationManager { get; set; }

        /// <summary>
        /// Get taxon manager instance.
        /// </summary>
        public static ITaxonManager TaxonManager { get; set; }
    }
}
