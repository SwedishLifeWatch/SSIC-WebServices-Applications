using System;

using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test
{
    [TestClass]
    public class WebServiceContextExtensionTest : TestBase
    {
        private WebServiceContext _context;

        public WebServiceContextExtensionTest()
        {
            _context = null;
        }

        protected override WebServiceContext GetContext()
        {
            return GetContext(false);
        }

        private WebServiceContext GetContext(Boolean refresh)
        {
            if (_context.IsNull() || refresh)
            {
                if (_context.IsNotNull())
                {
                    TestCleanup();
                    TestInitialize();
                }
            }

            return _context;
        }

        [TestMethod]
        public void GetSpeciesObservationDatabase()
        {
            SpeciesObservationHarvestServer dataServer = GetContext().GetSpeciesObservationDatabase();
            Assert.IsNotNull(dataServer);
        }
    }
}
