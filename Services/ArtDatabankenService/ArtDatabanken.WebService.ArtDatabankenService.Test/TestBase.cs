using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Test.Data;
using ArtDatabanken.WebService.Proxy;
using ApplicationManager = ArtDatabanken.WebService.ArtDatabankenService.Data.ApplicationManager;
using UserManager = ArtDatabanken.WebService.ArtDatabankenService.Data.UserManager;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test
{
    [TestClass]
    public class TestBase
    {
        // These test assumes that the following information
        // has been stored in the tabel ObsUsers:ClientApplicationInformation:
        // WebServiceTest	1.2.3.4	1
        protected const Int32 AGRICULTURAL_LANDSCAPE_FACTOR_ID = 663;
        protected const Int32 BIUS_FOREST_FACTOR_ID = 2;
        protected const Int32 FOREST_LANSCAPE_FACTOR_ID = 662;
        protected const Int32 LANDSCAPE_FACTOR_ID = 750;
        protected const Int32 LANDSCAPES_FACTOR_ID = 661;
        protected const Int32 LANDSCAPE_FOREST_FACTOR_ID = 662;
        protected const Int32 LANDSCAPE_AGRICULTURE_FACTOR_ID = 663;
        protected const Int32 LANDSCAPE_MOUNTAIN_FACTOR_ID = 665;
        protected const Int32 LANDSCAPE_FRESH_WATER_FACTOR_ID = 667;
        protected const Int32 ORGANISM_GROUP_FACTOR_ID = 656;
        protected const Int32 REDLIST_FACTOR_ID = 542;
        protected const Int32 REDLIST_TAXON_TYPE_FACTOR_ID = 655;
        protected const Int32 SPECIES_FACT_DATABASE_FACTOR_ID = 985;

        protected const Int32 JAN_EDELSJO_REFERENCE_ID = 384;

        protected const Int32 BEAR_TAXON_ID = 100145;
        protected const Int32 BEWICKS_SWAN_ID = 102610; // Mindre sångsvan
        protected const Int32 BIRD_TAXON_ID = 4000104;
        protected const Int32 COMMON_BUZZARD_TAXON_ID = 102942; // Ormvråk.
        protected const Int32 ROOT_TAXON_ID = 0;
        protected const Int32 EURASIAN_EAGLE_OWL_TAXON_ID = 100020;
        protected const Int32 EURASIAN_PYGMY_OWL_TAXON_ID = 102621; // Sparvuggla
        protected const Int32 GOLDEN_EAGLE_TAXON_ID = 100011;
        protected const Int32 INSECTS_TAXON_ID = 4000072;
        protected const Int32 MUTE_SWAN_TAXON_ID = 102927; // Knölsvan.
        protected const Int32 NORTHERN_HAWK_OWL_TAXON_ID = 102620;
        protected const Int32 PINTAIL_ID = 100006; // Stjärtand
        protected const Int32 RED_FOX_TAXON_ID = 206026;
        protected const Int32 HAWK_BIRDS_TAXON_ID = 2002066;
        protected const Int32 BADGER_TAXON_ID = 206036; // Grävling.
        protected const Int32 BEAVER_TAXON_ID = 102607; // Bäver.
        protected const Int32 HEDGEHOG_TAXON_ID = 100053; // Igelkott.
        protected const Int32 FALLOW_DEER_TAXON_ID = 206044; // Dovhjort.
        protected const Int32 MAMMAL_TAXON_ID = 4000107; // Däggdjur.

        protected const Int32 FAMILY_TAXON_TYPE_ID = 11;
        protected const Int32 GENUS_TAXON_TYPE_ID = 14;
        protected const Int32 SPECIES_TAXON_TYPE_ID = 17;

        protected const Int32 TEST_USER_ROLE_ID = 5;
        protected const String TEST_APPLICATION_IDENTIFIER = "EVA";
        protected const String TEST_CLIENT_APPLICATION_NAME = "WebServiceTest";
        protected const String TEST_CLIENT_APPLICATION_VERSION = "1.2.3.4";
        protected const String TEST_PASSWORD = "Qwertyasdfg123";
        protected const String TEST_PASSWORD_HASH = "420B9B97AC94BD17353E71D4B6227563AF829A41";
        protected const String TEST_USER_NAME = "testuser";

        private static WebServiceContext _context;

        private Boolean _useTransaction;
        private Int32 _transactionTimeout;
        private String _testApplicationIdentifier = null;

        public TestBase()
            : this (true, 10)
        {
        }

        public TestBase(Boolean useTransaction,
                        Int32 transactionTimeout)
        {
            _context = null;
            _useTransaction = useTransaction;
            _transactionTimeout = transactionTimeout;
        }

        protected String ApplicationIdentifier
        {
            get
            {
                if (_testApplicationIdentifier.IsEmpty())
                {
                    return TEST_APPLICATION_IDENTIFIER;
                }
                else
                {
                    return _testApplicationIdentifier;
                }
            }
            set
            {
                TestCleanup();
                _testApplicationIdentifier = value;
                TestInitialize();
            }
        }

        protected WebServiceContext GetContext()
        {
            return _context;
        }

        protected String GetString(Int32 stringLength)
        {
            Int32 stringIndex;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder(stringLength);
            for (stringIndex = 0; stringIndex < stringLength; stringIndex++)
            {
                stringBuilder.Append("a");
            }
            return stringBuilder.ToString();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            if (_context.IsNotNull())
            {
                if (_useTransaction)
                {
                    _context.RollbackTransaction();
                }
                _context.Dispose();
                _context = null;
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.RegionManager = new ArtDatabankenService.Data.RegionManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.TaxonManager = new ArtDatabankenService.Data.TaxonManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            CoreData.TaxonManager = new TaxonManagerSingleThreadCache();
            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
//            _context = new WebServiceContextCached(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier).Token);
            //_context = new WebServiceContext(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier).Token);
            _context = new WebServiceContext(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token);
            if (_useTransaction)
            {
                _context.StartTransaction(_transactionTimeout);
            }
        }
    }
}
