using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.AnalysisService.Data;

namespace ArtDatabanken.WebService.AnalysisService.Test.Data
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
        [TestCategory("NightlyTest")]
        public void Constructor()
        {
            WebServiceManager webServiceManager;

            webServiceManager = new WebServiceManager();
            Assert.IsNotNull(webServiceManager);
        }

        [TestMethod]
        public void GetStatus()
        {
            Dictionary<Int32, List<WebResourceStatus>> status;

            status = GetWebServiceManager(true).GetStatus();
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
        [TestCategory("NightlyTest")]
        public void Name()
        {
            String name;

            name = GetWebServiceManager(true).Name;
            Assert.IsTrue(name.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Password()
        {
            String password;

            password = GetWebServiceManager(true).Password;
            Assert.IsTrue(password.IsNotEmpty());
        }
    }
}
