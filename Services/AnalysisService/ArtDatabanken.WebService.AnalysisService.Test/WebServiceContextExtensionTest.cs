using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.AnalysisService.Database;

namespace ArtDatabanken.WebService.AnalysisService.Test
{
    [TestClass]
    public class WebServiceContextExtensionTest : TestBase
    {
        private WebServiceContext _context;

        public WebServiceContextExtensionTest()
        {
            _context = null;
        }

        private WebServiceContext GetContext()
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
                _context = Context;
            }
            return _context;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetAnalysisDatabase()
        {
            AnalysisServer dataServer;

            dataServer = GetContext().GetAnalysisDatabase();
            Assert.IsNotNull(dataServer);
        }
    }
}
