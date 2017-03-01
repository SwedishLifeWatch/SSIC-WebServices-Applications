using System;
using System.Threading;
using AnalysisPortal.CoypuTest.Properties;
using Coypu.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalysisPortal.CoypuTest
{
    [TestClass]
    public class UITests : UITestBase
    {
        public UITests() : base(Settings.Default.AppHost)
        {
        }

        [TestMethod]
        public void AllSupportedBrowsers_StartAndWaitTenSeconds()
        {
            foreach (var browser in SupportedBrowsers)
            {
                Browser_StartAndWaitTenSeconds(browser);
            }
        }

        [TestMethod]
        public void BrowserChrome_StartAndWaitTenSeconds()
        {
            Browser_StartAndWaitTenSeconds(Browser.Chrome);
        }

        [TestMethod]
        public void BrowserFireFox_StartAndWaitTenSeconds()
        {
            Browser_StartAndWaitTenSeconds(Browser.Firefox);
        }

        [TestMethod]
        public void BrowserExplorer_StartAndWaitTenSeconds()
        {
            Browser_StartAndWaitTenSeconds(Browser.InternetExplorer);
        }

        private void Browser_StartAndWaitTenSeconds(Browser browser)
        {
            using (var browserSession = StartBrowser(browser))
            {
                browserSession.ClickButton("Data");
                Thread.Sleep(10000);
//                CloseBrowser(browserSession);
            }
        }
    }
}
