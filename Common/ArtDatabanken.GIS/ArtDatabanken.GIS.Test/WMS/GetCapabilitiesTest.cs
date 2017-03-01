using System.Text;
using ArtDatabanken.GIS.WFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;


namespace ArtDatabanken.GIS.Test.WMS
{
    [TestClass]
    public class GetCapabilitiesTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod]
        public void Get_WFSVersion_From_GetCapabilitiesRequest()
        {
           

            string strXmlCapabilities = GetWFSCapabilitiesVer1_1_1XMLString();

            WmsCapabilities_1_1_1.WMT_MS_Capabilities obj = WmsCapabilities_1_1_1.WMT_MS_Capabilities.Deserialize(strXmlCapabilities);

            strXmlCapabilities = GetWFSCapabilitiesVer1_3_0XMLString();
            WmsCapabilites_1_3_0.WMS_Capabilities obj2 = WmsCapabilites_1_3_0.WMS_Capabilities.Deserialize(strXmlCapabilities);

           

        }

       


        private string GetWFSCapabilitiesVer1_1_1XMLString()
        {
            string url = "http://193.183.24.3/ArcGIS/services/sksNyckelbiotoper/MapServer/WMSServer?service=WMS&request=getCapabilities&version=1.1.1";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string xml = wc.DownloadString(url);
                return xml;
            }
        }

        private string GetWFSCapabilitiesVer1_3_0XMLString()
        {
            string url = "http://193.183.24.3/ArcGIS/services/sksNyckelbiotoper/MapServer/WMSServer?service=WMS&request=getCapabilities&version=1.3.0";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string xml = wc.DownloadString(url);
                return xml;
            }
        }
        

    }
}
