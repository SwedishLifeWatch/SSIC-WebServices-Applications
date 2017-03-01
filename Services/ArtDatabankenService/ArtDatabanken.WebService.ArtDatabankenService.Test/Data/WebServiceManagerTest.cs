using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebServiceManagerTest : TestBase
    {
        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceData.WebServiceManager.GetStatus(GetContext());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(WebServiceData.WebServiceManager.Name.IsNotEmpty());
        }

        [TestMethod]
        public void Password()
        {
            Assert.IsTrue(WebServiceData.WebServiceManager.Password.IsNotEmpty());
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceData.WebServiceManager.Ping(GetContext());
            Assert.IsTrue(ping);
        }
    }
}
