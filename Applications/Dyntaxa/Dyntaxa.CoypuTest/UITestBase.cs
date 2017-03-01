using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AnalysisPortal.CoypuTest
{
    public class UITestBase
    {
        public UITestBase(string appHost)
        {
            SessionConfiguration = new SessionConfiguration
            {
                AppHost = appHost,
                Driver = typeof(SeleniumWebDriver)
            };
        }

        protected readonly List<Browser> SupportedBrowsers = new List<Browser> { Browser.Chrome, Browser.Firefox, Browser.InternetExplorer };

        protected SessionConfiguration SessionConfiguration { get; private set; }

        protected void CloseBrowser(BrowserSession browser)
        {
            ((RemoteWebDriver)browser.Native).Quit();
        }

        protected void SaveScreenshot(BrowserSession browser)
        {
            browser.SaveScreenshot(string.Format("{0}_{1}.jpg", @".\Screenshot", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")), ImageFormat.Jpeg);
        }

        protected BrowserSession StartBrowser(Browser browser = null)
        {
            SessionConfiguration.Browser = browser ?? Browser.Chrome;
            try
            {
                var initiatedBrowser = new BrowserSession(SessionConfiguration);
                initiatedBrowser.MaximiseWindow();
                initiatedBrowser.Visit(@"\");

                return initiatedBrowser;
            }
            catch (DriverServiceNotFoundException ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, string.Format("Driver saknas för [{0}], kopiera filerna från katalogen Drivers och lägg dem i bin-katalogen!\r\n\r\n{1}", SessionConfiguration.Browser, ex.Message));
                throw;
            }
        }

    }

}
