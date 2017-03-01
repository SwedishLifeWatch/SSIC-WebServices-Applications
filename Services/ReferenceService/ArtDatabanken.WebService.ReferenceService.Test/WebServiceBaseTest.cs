using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ReferenceService.Test
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
        public void Login()
        {
            DateTime start, end;
            TimeSpan duration1, duration2;

            // This is a copy of the actual code in WebServiceBase.Login.
            // Can not use an instance of WebServiceBase if we would like
            // to test caching since WebServiceBase.Login uses class
            // WebServiceContext which needs ASP.NET to cache information.
            using (WebServiceContext context = new WebServiceContextCached(Settings.Default.TestUserName,
                                                                           ApplicationIdentifier.UserAdmin.ToString()))
            {
                try
                {
                    start = DateTime.Now;
                    WebServiceData.UserManager.Login(context,
                                                     Settings.Default.TestUserName,
                                                     Settings.Default.TestPassword,
                                                     ApplicationIdentifier.UserAdmin.ToString(),
                                                     false);
                    end = DateTime.Now;
                    duration1 = end - start;
                    Debug.WriteLine("Duration during first loggin = " + duration1.TotalMilliseconds);

                    // Now everything should be cached.
                    start = DateTime.Now;
                    WebServiceData.UserManager.Login(context,
                                                     Settings.Default.TestUserName,
                                                     Settings.Default.TestPassword,
                                                     ApplicationIdentifier.UserAdmin.ToString(),
                                                     false);
                    end = DateTime.Now;
                    duration2 = end - start;
                    Debug.WriteLine("Duration during second loggin = " + duration2.TotalMilliseconds);
                    Assert.IsTrue(duration2.TotalMilliseconds <= duration1.TotalMilliseconds);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
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
