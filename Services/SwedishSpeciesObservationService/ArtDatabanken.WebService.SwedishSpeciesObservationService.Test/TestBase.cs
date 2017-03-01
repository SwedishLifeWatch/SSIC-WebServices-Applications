using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using ApplicationManager = ArtDatabanken.WebService.Data.ApplicationManager;
using DatabaseManager = ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.DatabaseManager;
using MetadataManager = ArtDatabanken.WebService.Data.MetadataManager;
using RegionManager = ArtDatabanken.WebService.Data.RegionManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test
{
    [TestClass]
    public class TestBase
    {
        public TestBase()
        {
            Context = null;
        }

        protected WebServiceContext Context { get; private set; }

        protected WebClientInformation GetClientInformation()
        {
            WebClientInformation clientInformation;
            WebServiceContext context;

            context = Context;
            clientInformation = new WebClientInformation();
            clientInformation.Token = context.ClientToken.Token;
            return clientInformation;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            Context.Dispose();
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            Configuration.SetInstallationType();
            // Configuration.InstallationType = InstallationType.Production;

            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.MetadataManager = new MetadataManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManagerLocalWebService();
            WebServiceData.SpeciesFactManager = new SpeciesFactManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManagerAdapter();
            WebServiceData.TaxonManager = new WebService.Data.TaxonManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();

            Context = new WebServiceContextCached(Settings.Default.TestUserName,
                                                  ApplicationIdentifier.PrintObs.ToString());
        }
    }
}
