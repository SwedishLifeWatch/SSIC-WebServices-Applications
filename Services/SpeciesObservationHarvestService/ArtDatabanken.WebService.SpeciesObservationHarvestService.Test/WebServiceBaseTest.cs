using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test
{
    [TestClass]
    public class WebServiceBaseTest : TestBase
    {
        private WebServiceBase _webServiceBase;

        public WebServiceBaseTest()
        {
            _webServiceBase = null;
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = GetWebServiceBase(true).GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = GetWebServiceBase().GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        private WebServiceBase GetWebServiceBase(Boolean refresh = false)
        {
            if (_webServiceBase.IsNull() || refresh)
            {
                _webServiceBase = new WebServiceBase();
            }

            return _webServiceBase;
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = GetWebServiceBase(true).Ping();
            Assert.IsTrue(ping);
        }
    }
}
