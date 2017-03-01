using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class WebServiceProxyBaseTest
    {
        public WebServiceProxyBaseTest()
        {
        }

        [TestMethod]
        public void GetWebServiceName()
        {
            WebServiceProxyBase webServiceProxyBase;

            webServiceProxyBase = WebServiceProxy.UserService;
            Assert.AreEqual("UserService", webServiceProxyBase.GetWebServiceName());
        }
    }
}
