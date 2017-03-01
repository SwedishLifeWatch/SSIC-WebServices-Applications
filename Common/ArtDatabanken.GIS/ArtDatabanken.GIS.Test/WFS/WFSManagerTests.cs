using System;
using System.Diagnostics;
using System.IO;
using System.Net.Fakes;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.GIS.WFS;
using ArtDatabanken.GIS.WFS.Capabilities;
using ArtDatabanken.GIS.WFS.Capabilities.Parsers;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using System.Net;
using System.Reflection;
using WfsBoundingBox = ArtDatabanken.GIS.WFS.WfsBoundingBox;

namespace ArtDatabanken.GIS.Test.WFS
{
    [TestClass]
    public class WFSManagerTests
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
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
        public void AddOrReplaceSrsInUrl_UrlWithoutSrs_SrsIsAdded()
        {
            string newSrsName = "EPSG:1001";
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            string newUrl = WFSManager.AddOrReplaceSrsInUrl(featuresUrl, newSrsName);
            Assert.IsTrue(WFSManager.GetCoordinateSystemSrsFromUrl(newUrl) == newSrsName);
        }

        [TestMethod]
        public void AddOrReplaceSrsInUrl_UrlContainingSrs_SrsIsReplaced()
        {
            string newSrsName = "EPSG:1001";
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:3857";
            string newUrl = WFSManager.AddOrReplaceSrsInUrl(featuresUrl, newSrsName);
            Assert.IsTrue(WFSManager.GetCoordinateSystemSrsFromUrl(newUrl) == newSrsName);

            // Google mercator
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913";
            newUrl = WFSManager.AddOrReplaceSrsInUrl(featuresUrl, newSrsName);
            Assert.IsTrue(WFSManager.GetCoordinateSystemSrsFromUrl(newUrl) == newSrsName);

            // extra space in the end
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913    ";            
            newUrl = WFSManager.AddOrReplaceSrsInUrl(featuresUrl, newSrsName);
            Assert.IsTrue(WFSManager.GetCoordinateSystemSrsFromUrl(newUrl) == newSrsName);

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:900913&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            newUrl = WFSManager.AddOrReplaceSrsInUrl(featuresUrl, newSrsName);
            Assert.IsTrue(WFSManager.GetCoordinateSystemSrsFromUrl(newUrl) == newSrsName);

            // inside the url same srs name
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:1001&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            newUrl = WFSManager.AddOrReplaceSrsInUrl(featuresUrl, newSrsName);
            Assert.IsTrue(WFSManager.GetCoordinateSystemSrsFromUrl(newUrl) == newSrsName);            
        }

        [TestMethod]
        public void GetCoordinateSystemSrsFromUrl_SrsIsMissingInUrl_NullIsReturned()
        {
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            string srsName = WFSManager.GetCoordinateSystemSrsFromUrl(featuresUrl);
            Assert.IsNull(srsName);
        }

        [TestMethod]
        public void GetCoordinateSystemSrsFromUrl_ValidSrsInUrl_SridIsReturned()
        {
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:3857";
            string srsName = WFSManager.GetCoordinateSystemSrsFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("EPSG:3857"));

            // Google mercator
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913";
            srsName = WFSManager.GetCoordinateSystemSrsFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("EPSG:900913"));

            // extra space in the end
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913    ";
            srsName = WFSManager.GetCoordinateSystemSrsFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("EPSG:900913"));

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:900913&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            srsName = WFSManager.GetCoordinateSystemSrsFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("EPSG:900913"));

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:3857&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            srsName = WFSManager.GetCoordinateSystemSrsFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("EPSG:3857"));
        }


        [TestMethod]
        public void GetCoordinateSystemSridFromUrl_SrsIsMissingInUrl_NullIsReturned()
        {
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            string srsName = WFSManager.GetCoordinateSystemSridFromUrl(featuresUrl);
            Assert.IsNull(srsName);
        }

        [TestMethod]
        public void GetCoordinateSystemSridFromUrl_ValidSridInUrl_SridIsReturned()
        {
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:3857";
            string srsName = WFSManager.GetCoordinateSystemSridFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("3857"));

            // Google mercator
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913";
            srsName = WFSManager.GetCoordinateSystemSridFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("900913"));

            // extra space in the end
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913    ";
            srsName = WFSManager.GetCoordinateSystemSridFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("900913"));

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:900913&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            srsName = WFSManager.GetCoordinateSystemSridFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("900913"));

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:3857&version=1.1.0&typeName=SLW:Sverigekarta_med_lan";
            srsName = WFSManager.GetCoordinateSystemSridFromUrl(featuresUrl);
            Assert.IsTrue(srsName.Equals("3857"));
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Get_WFSVersion_From_GetCapabilitiesRequest()
        {
            WFSVersion version;

            version = WFSManager.GetWFSVersionFromRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&version=1.1.0&request=GetCapabilities");
            Assert.AreEqual(WFSVersion.Ver110, version);

            version = WFSManager.GetWFSVersionFromRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&version=1.0.0&request=GetCapabilities");
            Assert.AreEqual(WFSVersion.Ver100, version);

            version = WFSManager.GetWFSVersionFromRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&request=GetCapabilities&version=1.0.0");
            Assert.AreEqual(WFSVersion.Ver100, version);

            version = WFSManager.GetWFSVersionFromRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&request=GetCapabilities&version=1.5.0");
            Assert.AreEqual(WFSVersion.Unknown, version);

            version = WFSManager.GetWFSVersionFromRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&request=GetCapabilities&version=1.1.");
            Assert.AreEqual(WFSVersion.Unknown, version);

            version = WFSManager.GetWFSVersionFromRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&request=GetCapabilities");
            Assert.AreEqual(WFSVersion.Unknown, version);

            version = WFSManager.GetWFSVersionFromRequestUrl("abc");
            Assert.AreEqual(WFSVersion.Unknown, version);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Create_WFS_GetCapabilitiesRequestString()
        {
            string url;
            
            url = WFSManager.CreateGetCapabiltiesRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&version=1.1.0&request=GetCapabilities", WFSVersion.Ver110);
            Assert.AreEqual("http://slwgeo.artdata.slu.se:8080/geoserver/slw/wfs?service=wfs&request=getcapabilities&version=1.1.0", url.ToLower());

            url = WFSManager.CreateGetCapabiltiesRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&version=1.1.0&request=GetCapabilities", WFSVersion.Ver100);
            Assert.AreEqual("http://slwgeo.artdata.slu.se:8080/geoserver/slw/wfs?service=wfs&request=getcapabilities&version=1.0.0", url.ToLower());

            url = WFSManager.CreateGetCapabiltiesRequestUrl("https://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=WFS&version=1.1.0&request=GetCapabilities", WFSVersion.Ver110);
            Assert.AreEqual("https://slwgeo.artdata.slu.se:8080/geoserver/slw/wfs?service=wfs&request=getcapabilities&version=1.1.0", url.ToLower());

            url = WFSManager.CreateGetCapabiltiesRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs", WFSVersion.Ver110);
            Assert.AreEqual("http://slwgeo.artdata.slu.se:8080/geoserver/slw/wfs?service=wfs&request=getcapabilities&version=1.1.0", url.ToLower());

            url = WFSManager.CreateGetCapabiltiesRequestUrl("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs", WFSVersion.Ver100);
            Assert.AreEqual("http://slwgeo.artdata.slu.se:8080/geoserver/slw/wfs?service=wfs&request=getcapabilities&version=1.0.0", url.ToLower());
        }


        [TestMethod]                        
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("Sample files", "Sample files")]
        public void ParseCapabilities_LocalSmhiFile_SuccessfulParse()
        {            
            string text = System.IO.File.ReadAllText(@"Sample files\smhi getcapabilities.xml");            
            var caps = WFSManager.ParseCapabilities(text, WFSVersion.Ver100);
            Assert.AreEqual(132, caps.FeatureTypes.Count);
        }


        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void Get_WFSCapabilities_From_Server()
        {
            var smhi100 = WFSManager.GetWFSCapabilities("http://map.smhi.se/geoserver/ows?", WFSVersion.Ver100);
            Assert.IsTrue(smhi100.Version == "1.0.0");

            var smhi110 = WFSManager.GetWFSCapabilities("http://map.smhi.se/geoserver/ows?", WFSVersion.Ver110);
            Assert.IsTrue(smhi110.Version == "1.1.0");
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void Get_WFSDescribeFeatures_From_Server()
        {
            Dictionary<string, WFSDescribeFeatureType> dicDescribeFeatures;
            WFSCapabilities capabilities;
            String url;

            url = "http://map.smhi.se/geoserver/ows";
            capabilities = WFSManager.GetWFSCapabilities(url);
            dicDescribeFeatures = WFSManager.GetWFSDescribeFeatureTypes(url, capabilities.FeatureTypes);
            Assert.IsTrue(dicDescribeFeatures.Count > 0);
            Assert.IsTrue(dicDescribeFeatures.Count == capabilities.FeatureTypes.Count);
        
            url = "http://map.smhi.se/geoserver/ows";
            capabilities = WFSManager.GetWFSCapabilities(url, WFSVersion.Ver110);
            dicDescribeFeatures = WFSManager.GetWFSDescribeFeatureTypes(url, capabilities.FeatureTypes, WFSVersion.Ver110);
            Assert.IsTrue(dicDescribeFeatures.Count > 0);
            Assert.IsTrue(dicDescribeFeatures.Count == capabilities.FeatureTypes.Count);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        [DeploymentItem("Sample files", "Sample files")]
        public void GetWFSCapabilities_LocalCapabilitiesFile_SuccessfulParse()
        {
            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, url2) =>
                    File.ReadAllText(@"Sample files\smhi getcapabilities.xml");
                
                WFSCapabilities capabilities;
                String url;

                url = "http://map.smhi.se/geoserver/ows";
                capabilities = WFSManager.GetWFSCapabilities(url, WFSVersion.Ver100);                
                Assert.AreEqual(132, capabilities.FeatureTypes.Count);
                Assert.AreEqual("1.0.0", capabilities.Version);
                Assert.AreEqual("http://map.smhi.se/geoserver/wfs?request=DescribeFeatureType", capabilities.Capability.Requests.DescribeFeatureTypeRequest.GetUrl);                
            }
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void Get_WFSDescribeFeature_From_Server()
        {
            WFSDescribeFeatureType wfsDescribeFeatureType;
            
            wfsDescribeFeatureType = WFSManager.GetWFSDescribeFeatureType(
                "http://map.smhi.se/geoserver/ows", "smhi:alla_oar");
            Assert.IsTrue(wfsDescribeFeatureType.Fields.Count > 0);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetWfsFeatures()
        {        
            WfsTypeName typeName = new WfsTypeName();
            WFSVersion version = WFSVersion.Ver110;
            string serverUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs";
            string parameter = string.Empty;
            string parameterValue = string.Empty;
            string bbox = string.Empty;//&BBOX=133499, 628499, 154501, 635501;//&BBOX=628499,133499,635501,154501";            
            typeName.Namespace = "SLW:Sverigekarta_med_lan";            
            string srsName = string.Empty;
            FeatureCollection featureCollection;
            featureCollection = WFSManager.GetWfsFeatures(serverUrl, version, bbox, typeName, srsName, parameter, parameterValue);
            Assert.IsNotNull(featureCollection);
            Assert.IsTrue(featureCollection.Features.Count == 22);
         }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        [DeploymentItem("Sample files", "Sample files")]        
        public void GetWfsFeaturesUsingMsFakes()
        {
            using (ShimsContext.Create())
            {                                
                ShimWebClient.AllInstances.DownloadStringString = (client, url) =>
                    File.ReadAllText(@"Sample files\SLW_Sverigekarta_med_lan - All Features.json");

                WfsTypeName typeName = new WfsTypeName();
                WFSVersion version = WFSVersion.Ver110;
                string serverUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs";
                string parameter = string.Empty;
                string parameterValue = string.Empty;
                string bbox = string.Empty; // &BBOX=133499, 628499, 154501, 635501;//&BBOX=628499,133499,635501,154501";                
                typeName.Namespace = "SLW:Sverigekarta_med_lan";
                string srsName = string.Empty;
                FeatureCollection featureCollection;
                featureCollection = WFSManager.GetWfsFeatures(serverUrl, version, bbox, typeName, srsName, parameter, parameterValue);
                Assert.IsNotNull(featureCollection);
                Assert.IsTrue(featureCollection.Features.Count == 22);
            }
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetWfsFeaturesWithBoundingBox()
        {
            WfsTypeName typeName = new WfsTypeName();
            WFSVersion version = WFSVersion.Ver110;
            string serverUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs";
            string parameter = string.Empty;
            string parameterValue = string.Empty;
            string bbox = "6400000,1400000, 6500000,1500000";            
            typeName.Namespace = "SLW:Sverigekarta_med_lan";            
            string srsName = string.Empty;
            FeatureCollection featureCollection;
            featureCollection = WFSManager.GetWfsFeatures(serverUrl, version, bbox, typeName, srsName, parameter, parameterValue);
            Assert.IsNotNull(featureCollection);
            Assert.IsTrue(featureCollection.Features.Count == 5);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        [DeploymentItem("Sample files", "Sample files")]        
        public void GetWfsFeaturesWithBoundingBoxUsingMsFakes()
        {
            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, url) =>
                    File.ReadAllText(@"Sample files\SLW_Sverigekarta_med_lan - BBox Filter.json");
                WfsTypeName typeName = new WfsTypeName();
                WFSVersion version = WFSVersion.Ver110;
                string serverUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs";
                string parameter = string.Empty;
                string parameterValue = string.Empty;
                string bbox = "6400000,1400000, 6500000,1500000";
                typeName.Namespace = "SLW:Sverigekarta_med_lan";
                string srsName = string.Empty;
                FeatureCollection featureCollection;
                featureCollection = WFSManager.GetWfsFeatures(serverUrl, version, bbox, typeName, srsName, parameter, parameterValue);
                Assert.IsNotNull(featureCollection);
                Assert.AreEqual(5, featureCollection.Features.Count);
            }
        }        


        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetWfsFeaturesWithFilter()
        {
            WfsTypeName typeName = new WfsTypeName();
            WFSVersion version = WFSVersion.Ver110;
            string serverUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs";
            string bbox = string.Empty;
            string parameter = "SLW:LÃ¤nSKOD";
            string parameterValue = "17";
            //Todo: vilken är det:?
            typeName.Namespace = "SLW:Sverigekarta_med_lan";
            //typeName.Name = "SLW:Sverigekarta_med_lan";
            string srsName = string.Empty;
            FeatureCollection featureCollection;
            featureCollection = WFSManager.GetWfsFeatures(serverUrl, version, bbox, typeName, srsName, parameter, parameterValue);
            Assert.IsNotNull(featureCollection);
            Assert.IsTrue(featureCollection.Features.Count == 1);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetFeatureRequestUrl()
        {
            string url = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs";
            string bbox = "6400000,1400000, 6500000,1500000"; 
            string parameter = string.Empty;
            string parametervalue = string.Empty;
            string resultUrl = string.Empty;
            WfsTypeName typeName = new WfsTypeName();
            typeName.Namespace = "SLW:Sverigekarta_med_lan";
            string srsName = string.Empty;
            resultUrl = WFSManager.CreateGetFeatureRequestUrl(url, WFSVersion.Ver110, bbox, typeName, srsName, parameter, parametervalue);
            Assert.AreEqual("http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&bbox=6400000,1400000, 6500000,1500000&typeName=SLW:Sverigekarta_med_lan&outputFormat=json", resultUrl);            
        }

        ////[TestMethod]
        ////public void FakedGetWfsFeaturesUsingHttpPost_UrlWithAndWithoutBoundingBox_ResultWithBoundingBoxIsLessThanResultWithoutBoundingBox()
        ////{
        ////    using (ShimsContext.Create())
        ////    {
        ////        ArtDatabanken.GIS.WFS.Fakes.ShimWFSManager.MakeHttpPostRequestStringString = (url, postData) => 
        ////            File.ReadAllText(@"Sample files\SLW_Sverigekarta_med_lan - BBox Filter.json");
        ////        ////ShimWebClient.AllInstances.DownloadStringString = (client, url) =>
        ////        ////    File.ReadAllText(@"Sample files\SLW_Sverigekarta_med_lan - BBox Filter.json");                
        ////        FeatureCollection result1 = WFSManager.GetWfsFeaturesUsingHttpPost("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913&bbox=1356438.6966421,8158952.8379623,2616120.9226065,8931884.0678745");
        ////        FeatureCollection result2 = WFSManager.GetWfsFeaturesUsingHttpPost("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913", new WfsBoundingBox(1356438.6966421, 8158952.8379623, 2616120.9226065, 8931884.0678745, "EPSG:900913"));
        ////        FeatureCollection resultWithoutBbox = WFSManager.GetWfsFeaturesUsingHttpPost("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913");

        ////        Assert.IsTrue(result1.Features.Count == result2.Features.Count);
        ////        Assert.IsTrue(result1.Features.Count < resultWithoutBbox.Features.Count);            
        ////    }
        ////}       

        [TestMethod]
        public void GetWfsFeaturesUsingHttpPost_UrlWithAndWithoutBoundingBox_ResultWithBoundingBoxIsLessThanResultWithoutBoundingBox()
        {                        
            FeatureCollection result1 = WFSManager.GetWfsFeaturesUsingHttpPost("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913&bbox=1356438.6966421,8158952.8379623,2616120.9226065,8931884.0678745");
            FeatureCollection result2 = WFSManager.GetWfsFeaturesUsingHttpPost("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913", new WfsBoundingBox(1356438.6966421, 8158952.8379623, 2616120.9226065, 8931884.0678745, "EPSG:900913"));
            FeatureCollection resultWithoutBbox = WFSManager.GetWfsFeaturesUsingHttpPost("http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&srsName=EPSG:900913");

            Assert.IsTrue(result1.Features.Count == result2.Features.Count);
            Assert.IsTrue(result1.Features.Count < resultWithoutBbox.Features.Count);                        
        }   

    }
}
