using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test
{
    [TestClass]
    public class WebServiceContextExtensionTest : TestBase
    {
        private WebServiceContext _context;

        public WebServiceContextExtensionTest()
        {
            _context = null;
        }

        private WebServiceContext GetContext(Boolean refresh = false)
        {
            if (_context.IsNull() || refresh)
            {
                if (_context.IsNotNull())
                {
                    TestCleanup();
                    TestInitialize();
                }

                _context = Context;
            }

            return _context;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationDatabase()
        {
            SpeciesObservationServer dataServer;

            dataServer = GetContext().GetSpeciesObservationDatabase();
            Assert.IsNotNull(dataServer);
        }
    }
}
