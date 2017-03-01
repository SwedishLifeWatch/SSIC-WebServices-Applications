namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Enumeration of supported Internet protocols.
    /// </summary>
    public enum InternetProtocol
    {
        /// <summary>
        /// HyperText Transfer Protocol.
        /// </summary>
        Http,

        /// <summary>
        /// Hypertext Transfer Protocol Secure.
        /// </summary>
        Https
    }

    /// <summary>
    /// Enumeration of computers where web services are hosted.
    /// </summary>
    public enum WebServiceComputer
    {
        /// <summary>
        /// Logical address to production server.
        /// </summary>
        ArtDatabankenSoa,

        /// <summary>
        /// Development test server for team Artportalen.
        /// </summary>
        ArtportalenTest,

        /// <summary>
        /// Production server.
        /// </summary>
        Lampetra2,

        /// <summary>
        /// Development with local hosting of web applications or web services.
        /// </summary>
        LocalTest,

        /// <summary>
        /// Old test server.
        /// </summary>
        Moneses,

        /// <summary>
        /// Production server.
        /// </summary>
        Silurus2,

        /// <summary>
        /// Development test server for team Species Fact.
        /// </summary>
        SpeciesFactTest,

        /// <summary>
        /// Test server used for system testing.
        /// </summary>
        SystemTest,

        /// <summary>
        /// Development test server for team Two Blueberries.
        /// </summary>
        TwoBlueberriesTest
    }

    /// <summary>
    /// Enumeration of supported web service protocols.
    /// </summary>
    public enum WebServiceProtocol
    {
        /// <summary>
        /// The Binary.
        /// </summary>
        Binary,

        /// <summary>
        /// SOAP version 1.1.
        /// </summary>
        SOAP11,

        /// <summary>
        /// SOAP version 1.2.
        /// </summary>
        SOAP12
    }

    /// <summary>
    /// This class handles information related to
    /// web service proxies.
    /// </summary>
    public class WebServiceProxy
    {
        /// <summary>
        /// The _internet protocol.
        /// </summary>
        private static InternetProtocol _internetProtocol;

        /// <summary>
        /// The _web service protocol.
        /// </summary>
        private static WebServiceProtocol _webServiceProtocol;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static WebServiceProxy()
        {
            // Set default proxy configuration values.
            _internetProtocol = InternetProtocol.Https;
            _webServiceProtocol = WebServiceProtocol.Binary;
            Configuration = new WebServiceConfiguration();

            // Create web service proxies.
            AnalysisService = new AnalysisServiceProxy();
            NorsService = new NorsServiceProxy();
            GeoReferenceService = new GeoReferenceServiceProxy();
            KulService = new KulServiceProxy();
            MvmService = new MvmServiceProxy();
            NorwayTaxonService = new NorwayTaxonServiceProxy();
            PESINameService = new PESINameServiceProxy();
            PictureService = new PictureServiceProxy();
            ReferenceService = new ReferenceServiceProxy();
            SersService = new SersServiceProxy();
            SpeciesObservationHarvestService = new SpeciesObservationHarvestServiceProxy();
            SwedishSpeciesObservationService = new SwedishSpeciesObservationServiceProxy();
            SwedishSpeciesObservationSOAPService = new SwedishSpeciesObservationSOAPServiceProxy();
            TaxonAttributeService = new TaxonAttributeServiceProxy();
            TaxonService = new TaxonServiceProxy();
            DyntaxaInternalService = new DyntaxaInternalServiceProxy();
            UserService = new UserServiceProxy();
            WramService = new WramServiceProxy();
        }

        /// <summary>
        /// Get/set Analysis service instance.
        /// </summary>
        public static AnalysisServiceProxy AnalysisService { get; set; }

        /// <summary>
        /// Get/set KUL service instance.
        /// </summary>
        public static KulServiceProxy KulService { get; set; }

        /// <summary>
        /// Get/set Nors service instance.
        /// </summary>
        public static NorsServiceProxy NorsService { get; set; }

        /// <summary>
        /// Configuration information related to web services.
        /// This information may be used by all web services.
        /// </summary>
        public static WebServiceConfiguration Configuration { get; set; }

        /// <summary>
        /// Get/set GeoReference service instance.
        /// </summary>
        public static GeoReferenceServiceProxy GeoReferenceService { get; set; }

        /// <summary>
        /// Information about which Internet protocol that are used.
        /// </summary>
        public static InternetProtocol InternetProtocol
        {
            get
            {
                return _internetProtocol;
            }

            set
            {
                _internetProtocol = value;
                if (GeoReferenceService.IsNotNull())
                {
                    GeoReferenceService.InternetProtocol = InternetProtocol;
                }

                if (SpeciesObservationHarvestService.IsNotNull())
                {
                    SpeciesObservationHarvestService.InternetProtocol = InternetProtocol;
                }

                if (SwedishSpeciesObservationService.IsNotNull())
                {
                    SwedishSpeciesObservationService.InternetProtocol = InternetProtocol;
                }

                if (SwedishSpeciesObservationSOAPService.IsNotNull())
                {
                    SwedishSpeciesObservationSOAPService.InternetProtocol = InternetProtocol;
                }

                if (UserService.IsNotNull())
                {
                    UserService.InternetProtocol = InternetProtocol;
                }

                if (AnalysisService.IsNotNull())
                {
                    AnalysisService.InternetProtocol = InternetProtocol;
                }

                if (TaxonService.IsNotNull())
                {
                    TaxonService.InternetProtocol = InternetProtocol;
                }

                if (DyntaxaInternalService.IsNotNull())
                {
                    DyntaxaInternalService.InternetProtocol = InternetProtocol;
                }
            }
        }

        /// <summary>
        /// Get/set taxon service instance.
        /// </summary>
        public static MvmServiceProxy MvmService { get; set; }

        /// <summary>
        /// Get/set taxon service instance.
        /// </summary>
        public static NorwayTaxonServiceProxy NorwayTaxonService { get; set; }

        /// <summary>
        /// Get/set PESI service instance.
        /// </summary>
        public static PESINameServiceProxy PESINameService { get; set; }

        /// <summary>
        /// Picture service instance.
        /// </summary>
        public static PictureServiceProxy PictureService { get; set; }

        /// <summary>
        /// Get/set Reference Service instance.
        /// </summary>
        public static ReferenceServiceProxy ReferenceService { get; set; }

        /// <summary>
        /// Get/set Sers service instance.
        /// </summary>
        public static SersServiceProxy SersService { get; set; }

        /// <summary>
        /// Get/set species observation harvest service instance.
        /// </summary>
        public static SpeciesObservationHarvestServiceProxy SpeciesObservationHarvestService { get; set; }

        /// <summary>
        /// Get/set SwedishSpeciesObservation service instance.
        /// </summary>
        public static SwedishSpeciesObservationServiceProxy SwedishSpeciesObservationService { get; set; }

        /// <summary>
        /// Get/set SwedishSpeciesObservationSOAP service instance.
        /// </summary>
        public static SwedishSpeciesObservationSOAPServiceProxy SwedishSpeciesObservationSOAPService { get; set; }

        /// <summary>
        /// Get/set taxon attribute service instance.
        /// </summary>
        public static TaxonAttributeServiceProxy TaxonAttributeService { get; set; }

        /// <summary>
        /// Get/set taxon service instance.
        /// </summary>
        public static TaxonServiceProxy TaxonService { get; set; }

        /// <summary>
        /// Get/set Dyntaxa internal service instance.
        /// </summary>
        public static DyntaxaInternalServiceProxy DyntaxaInternalService { get; set; }

        /// <summary>
        /// Get/set user service instance.
        /// </summary>
        public static UserServiceProxy UserService { get; set; }
        
        /// <summary>
        /// Get/set Wram service instance.
        /// </summary>
        public static WramServiceProxy WramService { get; set; }

        /// <summary>
        /// Information about which web service protocol that are used.
        /// </summary>
        public static WebServiceProtocol WebServiceProtocol 
        {
            get
            {
                return _webServiceProtocol;
            }

            set
            {
                _webServiceProtocol = value;
                if (GeoReferenceService.IsNotNull())
                {
                    GeoReferenceService.WebServiceProtocol = _webServiceProtocol;
                }

                if (SpeciesObservationHarvestService.IsNotNull())
                {
                    SpeciesObservationHarvestService.WebServiceProtocol = _webServiceProtocol;
                }

                if (SwedishSpeciesObservationService.IsNotNull())
                {
                    SwedishSpeciesObservationService.WebServiceProtocol = _webServiceProtocol;
                }

                if (SwedishSpeciesObservationSOAPService.IsNotNull())
                {
                    SwedishSpeciesObservationSOAPService.WebServiceProtocol = _webServiceProtocol;
                }

                if (UserService.IsNotNull())
                {
                    UserService.WebServiceProtocol = _webServiceProtocol;
                }

                if (AnalysisService.IsNotNull())
                {
                    AnalysisService.WebServiceProtocol = _webServiceProtocol;
                }

                if (TaxonService.IsNotNull())
                {
                    TaxonService.WebServiceProtocol = _webServiceProtocol;                
                }

                if (DyntaxaInternalService.IsNotNull())
                {
                    DyntaxaInternalService.WebServiceProtocol = _webServiceProtocol;
                }
            }
        }

        /// <summary>
        /// Close all clients (connections) to web services.
        /// </summary>
        public static void CloseClients()
        {
            if (AnalysisService.IsNotNull())
            {
                AnalysisService.CloseClients();
            }

            if (GeoReferenceService.IsNotNull())
            {
                GeoReferenceService.CloseClients();
            }

            if (KulService.IsNotNull())
            {
                KulService.CloseClients();
            }

            if (MvmService.IsNotNull())
            {
                MvmService.CloseClients();
            }

            if (NorsService.IsNotNull())
            {
                NorsService.CloseClients();
            }

            if (NorwayTaxonService.IsNotNull())
            {
                NorwayTaxonService.CloseClients();
            }

            if (PESINameService.IsNotNull())
            {
                PESINameService.CloseClients();
            }

            if (PictureService.IsNotNull())
            {
                PictureService.CloseClients();
            }

            if (ReferenceService.IsNotNull())
            {
                ReferenceService.CloseClients();
            }

            if (SersService.IsNotNull())
            {
                SersService.CloseClients();
            }

            if (SpeciesObservationHarvestService.IsNotNull())
            {
                SpeciesObservationHarvestService.CloseClients();
            }

            if (SwedishSpeciesObservationService.IsNotNull())
            {
                SwedishSpeciesObservationService.CloseClients();
            }

            if (SwedishSpeciesObservationSOAPService.IsNotNull())
            {
                SwedishSpeciesObservationSOAPService.CloseClients();
            }

            if (TaxonAttributeService.IsNotNull())
            {
                TaxonAttributeService.CloseClients();
            }

            if (TaxonService.IsNotNull())
            {
                TaxonService.CloseClients();
            }

            if (DyntaxaInternalService.IsNotNull())
            {
                DyntaxaInternalService.CloseClients();
            }

            if (UserService.IsNotNull())
            {
                UserService.CloseClients();
            }

            if (WramService.IsNotNull())
            {
                WramService.CloseClients();
            }
        }
    }
}
