namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains other classes that manages
    /// different types of data.
    /// All data is accessed with the onion design pattern.
    /// </summary>
    public class CoreData
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static CoreData()
        {
            AnalysisManager = new AnalysisManager();
            ApplicationManager = new ApplicationManager();
            CountryManager = new CountryManagerMultiThreadCache();
            FactorManager = new FactorManagerMultiThreadCache();
            GeoReferenceManager = new GeoReferenceManager();
            LocaleManager = new LocaleManagerMultiThreadCache();
            MetadataManager = new MetadataManagerMultiThreadCache();
            OrganizationManager = new OrganizationManager();
            PictureManager = new PictureManagerMultiThreadCache();
            ReferenceManager = new ReferenceManagerMultiThreadCache();
            RegionManager = new RegionManagerMultiThreadCache(new CoordinateSystem());
            SpeciesFactManager = new SpeciesFactManagerMultiThreadCache();
            SpeciesObservationManager = new SpeciesObservationManagerMultiThreadCache();
            TaxonManager = new TaxonManagerMultiThreadCache();
            UserManager = new UserManagerMultiThreadCache();
        }

        /// <summary>
        /// Get/set analysis manager instance.
        /// </summary>
        public static IAnalysisManager AnalysisManager { get; set; }

        /// <summary>
        /// Get/set application manager instance.
        /// </summary>
        public static IApplicationManager ApplicationManager { get; set; }

        /// <summary>
        /// Get/set country manager instance.
        /// </summary>
        public static ICountryManager CountryManager { get; set; }

        /// <summary>
        /// Get/set factor manager instance.
        /// </summary>
        public static IGeoReferenceManager GeoReferenceManager { get; set; }

        /// <summary>
        /// Get/set factor manager instance.
        /// </summary>
        public static IFactorManager FactorManager { get; set; }

        /// <summary>
        /// Get/set locale manager instance.
        /// </summary>
        public static ILocaleManager LocaleManager { get; set; }

        /// <summary>
        /// Get/set organization manager instance.
        /// </summary>
        public static IOrganizationManager OrganizationManager { get; set; }

        /// <summary>
        /// Get/set picture manager instance.
        /// </summary>
        public static IPictureManager PictureManager { get; set; }

        /// <summary>
        /// Get/set reference manager instance.
        /// </summary>
        public static IReferenceManager ReferenceManager { get; set; }

        /// <summary>
        /// Get/set region manager instance.
        /// </summary>
        public static IRegionManager RegionManager { get; set; }

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
        /// Get/set metadata manager instance.
        /// </summary>
        public static IMetadataManager MetadataManager { get; set; }
    }
}
