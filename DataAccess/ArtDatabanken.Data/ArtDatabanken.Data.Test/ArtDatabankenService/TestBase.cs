using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TestBase
    {
        protected const Int32 BADGER_TAXON_ID = 206036; // Grävling.
        protected const Int32 BEAR_TAXON_ID = 100145;
        protected const Int32 BEAVER_TAXON_ID = 102607; // Bäver.
        protected const Int32 BLACK_THROATED_LOON_TAXON_ID = 100065; // Storlom.
        protected const Int32 COMMON_BUZZARD_ID = 102942; // Ormvråk.
        protected const Int32 FALLOW_DEER_TAXON_ID = 206044; // Dovhjort.
        protected const Int32 GOLDEN_EAGLE_TAXON_ID = 100011;
        protected const Int32 HAWK_BIRDS_TAXON_ID = 2002066;
        protected const Int32 HEDGEHOG_TAXON_ID = 100053; // Igelkott.
        protected const Int32 LEPTOCHITON_ALVEOLUS_TAXON_ID = 102915; // A mollusc.
        protected const Int32 NORTHERN_HAWK_OWL_TAXON_ID = 102620;
        protected const Int32 RED_FOX_TAXON_ID = 206026;

        protected const Int32 FAMILY_TAXON_TYPE_ID = 11;
        protected const Int32 GENUS_TAXON_TYPE_ID = 14;
        protected const Int32 SPECIES_TAXON_TYPE_ID = 17;

        protected const String TEST_PASSWORD = "Qwertyasdfg123";
        protected const String TEST_USER_NAME = "testuser";

        protected const Int32 LANDSCAPE_FACTOR_ID = 750;
        protected const Int32 LANDSCAPES_FACTOR_ID = 661;
        protected const Int32 LANDSCAPE_FOREST_FACTOR_ID = 662;
        protected const Int32 LANDSCAPE_AGRICULTURE_FACTOR_ID = 663;
        protected const Int32 LANDSCAPE_MOUNTAIN_FACTOR_ID = 665;
        protected const Int32 LANDSCAPE_FRESH_WATER_FACTOR_ID = 667;
        protected const Int32 HISTORIC_DECREASE_FACTOR_ID = 691;
        protected const Int32 BIUS_FOREST_FACTOR_ID = 2;

        protected const String TEST_CLIENT_APPLICATION_NAME = "WebServiceTest";
        protected const String TEST_CLIENT_APPLICATION_VERSION = "1.2.3.4";

        public TestBase()
        {
        }

        protected Boolean AreEqual(DataId data1, DataId data2)
        {
            if ((data1.IsNull() && data2.IsNotNull()) ||
                (data1.IsNotNull() && data2.IsNull()))
            {
                return false;
            }
            if (data1.IsNull() && data2.IsNull())
            {
                return true;
            }
            return (data1.Id == data2.Id) &&
                   (data1.GetType().Name == data2.GetType().Name);
        }

        protected Boolean RunAllTests
        {
            get { return false; }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            Data.ArtDatabankenService.UserManager.Logout();
            /*            try
                        {
                            UserManager.Logout();
                        }
                        catch
                        {
                            // Test is done.
                            // We are not interested in problems that
                            // occures due to test of error handling.
                        }*/
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            TestInitialize("EVA");
        }

        public void TestInitialize(String applicationIdentifier)
        {
            TestCleanup();
            // WebServiceClient.WebServiceAddress = @"https://artdatabanken.artdatabankensoa.se/ArtDatabankenService.svc/Fast";
            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME,
                                                        TEST_PASSWORD,
                                                        applicationIdentifier,
                                                        false);
        }
    }
}
