using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
{
    /// <summary>
    /// Summary description for LinkManagerTest
    /// </summary>
    [TestClass]
    public class LinkManagerTest : TestBase
    {
        private LinkManager _manager;

        public LinkManagerTest()
        {
            this._manager = null;
        }

        private LinkManager GetManager(Boolean refresh)
        {
            if (_manager.IsNull() || refresh)
            {
                _manager = new LinkManager();
            }
            return _manager;
        }

        [TestMethod]
        public void GetUrlToGBIFTest()
        {
            String url = GetManager(true).GetUrlToGBIF("Psophus stridulus");
            Assert.IsTrue(url.IsNotEmpty());
        }
    }
}
