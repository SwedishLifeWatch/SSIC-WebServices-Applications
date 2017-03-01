using System;
using System.Threading;
using AnalysisPortal.CoypuTest;
using Coypu;
using Coypu.Drivers;
using Dyntaxa.CoypuTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dyntaxa.CoypuTest
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

        [TestMethod]
        public void SearchTaxonFromInputFile()
        {
            using (var browserSession = StartBrowser(Browser.Chrome))
            {
                foreach (var line in System.IO.File.ReadAllLines("TestInputFiles\\SearchTaxon_Input.txt"))
                {
                    browserSession.ExecuteSearchTaxon(line);
                }
            }
        }

        private void Browser_StartAndWaitTenSeconds(Browser browser)
        {
            using (var browserSession = StartBrowser(browser))
            {
                Thread.Sleep(10000);
            }
        }



    }
}
