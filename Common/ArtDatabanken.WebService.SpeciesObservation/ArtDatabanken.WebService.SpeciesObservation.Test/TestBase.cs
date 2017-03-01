using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationManager = ArtDatabanken.WebService.Data.ApplicationManager;
using MetadataManager = ArtDatabanken.WebService.Data.MetadataManager;
using RegionManager = ArtDatabanken.WebService.Data.RegionManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;

namespace ArtDatabanken.WebService.SpeciesObservation.Test
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
            //Context.Dispose();
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            //Context = new WebServiceContextCached(Settings.Default.TestUserName,
            //                                      ApplicationIdentifier.PrintObs.ToString());
        }
    }
}
