using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class WebServiceManagerTest : TestBase
    {
        private WebServiceManager _webServiceManager;

        public WebServiceManagerTest()
        {
            _webServiceManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebServiceManager webServiceManager = new WebServiceManager();
            Assert.IsNotNull(webServiceManager);
        }

        [TestMethod]
        public void GetStatus()
        {
            Dictionary<Int32, List<WebResourceStatus>> status;

            status = WebServiceData.WebServiceManager.GetStatus();
            Assert.IsTrue(status.IsNotEmpty());
            Assert.IsTrue(status[(Int32)(LocaleId.en_GB)].IsNotEmpty());
            Assert.IsTrue(status[(Int32)(LocaleId.sv_SE)].IsNotEmpty());
        }

        private WebServiceManager GetWebServiceManager(Boolean refresh = false)
        {
            if (_webServiceManager.IsNull() || refresh)
            {
                _webServiceManager = new WebServiceManager();
            }

            return _webServiceManager;
        }

        [TestMethod]
        public void Name()
        {
            string name = GetWebServiceManager(true).Name;
            Assert.IsTrue(name.IsNotEmpty());
        }

        [TestMethod]
        public void Password()
        {
            string password = GetWebServiceManager(true).Password;
            Assert.IsTrue(password.IsNotEmpty());
        }
    }
}
