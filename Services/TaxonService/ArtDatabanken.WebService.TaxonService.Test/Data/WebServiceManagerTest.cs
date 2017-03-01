using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    [TestClass]
    public class WebServiceManagerTest : TestBase
    {
        [TestMethod]
        public void GetStatus()
        {
            Dictionary<Int32, List<WebResourceStatus>> status;

            status = WebServiceData.WebServiceManager.GetStatus();
            Assert.IsTrue(status.IsNotEmpty());
            Assert.IsTrue(status[(Int32)(LocaleId.en_GB)].IsNotEmpty());
            Assert.IsTrue(status[(Int32)(LocaleId.sv_SE)].IsNotEmpty());
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
    }
}
