namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains other classes that manages
    /// different types of data.
    /// This data is used by all web services.
    /// </summary>
    public static class WebServiceData
    {
        /// <summary>
        /// Get/set analysis manager instance.
        /// </summary>
        public static IAnalysisManager AnalysisManager { get; set; }
        
        /// <summary>
        /// Get/set application manager instance.
        /// </summary>
        public static IApplicationManager ApplicationManager { get; set; }

        /// <summary>
        /// Get/set authorization manager instance.
        /// </summary>
        public static IAuthorizationManager AuthorizationManager { get; set; }

        /// <summary>
        /// Get/set coordinate conversion manager instance.
        /// </summary>
        public static ICoordinateConversionManager CoordinateConversionManager { get; set; }

        /// <summary>
        /// Get/set database manager instance.
        /// </summary>
        public static IDatabaseManager DatabaseManager { get; set; }

        /// <summary>
        /// Get/set factor manager instance.
        /// </summary>
        public static IFactorManager FactorManager { get; set; }

        /// <summary>
        /// Get/set geometry manager instance.
        /// </summary>
        public static IGeometryManager GeometryManager { get; set; }

        /// <summary>
        /// Get/set log manager instance.
        /// </summary>
        public static ILogManager LogManager { get; set; }

        /// <summary>
        /// Get/set metadata manager instance.
        /// </summary>
        public static IMetadataManager MetadataManager { get; set; }

        /// <summary>
        /// Get/set reference manager instance.
        /// </summary>
        public static IReferenceManager ReferenceManager { get; set; }

        /// <summary>
        /// Get/set region manager instance.
        /// </summary>
        public static IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Get/set species activity manager instance.
        /// </summary>
        public static ISpeciesActivityManager SpeciesActivityManager { get; set; }

        /// <summary>
        /// Get/set species fact manager instance.
        /// </summary>
        public static ISpeciesFactManager SpeciesFactManager { get; set; }

        /// <summary>
        /// Get/set species observation manager instance.
        /// </summary>
        public static ISpeciesObservationManager SpeciesObservationManager { get; set; }

        /// <summary>
        /// Get/set taxon manager instance.
        /// </summary>
        public static ITaxonManager TaxonManager { get; set; }

        /// <summary>
        /// Get/set user manager instance.
        /// </summary>
        public static IUserManager UserManager { get; set; }

        /// <summary>
        /// Get/set web service manager instance.
        /// </summary>
        public static IWebServiceManager WebServiceManager { get; set; }
    }
}
