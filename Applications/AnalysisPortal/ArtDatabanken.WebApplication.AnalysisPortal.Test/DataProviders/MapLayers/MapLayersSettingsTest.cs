using System.Linq;
using System.Web;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test
{
    using System;
    using System.Web.Hosting.Fakes;

    using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

    /// <summary>
    ///This is a test class for FilterSettingsTest and is intended
    ///to contain all FilterSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MapLayersSettingsTest
    {


        


        /// <summary>
        ///A test for FilterSettings Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void Create_WFS_LayerSettings()
        {
            var mapLayersSettings = new MapLayersSetting();
            var wfsLayers = mapLayersSettings.WfsLayers;
            int previousIndex = 0;
            if (wfsLayers.Count > 0)
            {
                previousIndex = wfsLayers.Last().Id;
            }
            var layer = mapLayersSettings.AddWfsLayer(new WfsLayerSetting()
                                              {
                                                  Name = "Test",
                                                  GeometryType = GeometryType.Polygon,
                                                  Color = "#336699",
                                                  Filter = "",
                                                  ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                                                  TypeName = "SLW:MapOfSwedishCounties"
                                              });

            Assert.AreEqual(previousIndex + 1, layer.Id);
        }

        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void SerializeAndDeserializeProtectedSettings()
        {
            // When this test is run a file called test.dat will be saved to 
            // c:\InetPub\wwwRoot\AnalysisPortalTestRun\temp\settings
           using (ShimsContext.Create())
           {

                string fullPath = @"C:\InetPub\wwwRoot\AnalysisPortalTestRun\temp\settings\myTempFile.txt";
                ShimHostingEnvironment.MapPathString = (path) => {return fullPath; };
                   

                var mySettings = new ArtDatabanken.WebApplication.AnalysisPortal.MySettings.MySettings();
                mySettings.Filter.Taxa.TaxonIds.Add(1);
                mySettings.Filter.Taxa.TaxonIds.Add(2);
                mySettings.Filter.Taxa.TaxonIds.Add(5);

                int wfsLayersCount = mySettings.DataProvider.MapLayers.WfsLayers.Count;
                var layer = mySettings.DataProvider.MapLayers.AddWfsLayer(new WfsLayerSetting()
                {
                    Name = "Test",
                    GeometryType = GeometryType.Polygon,
                    Color = "#336699",
                    Filter = "",
                    ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                    TypeName = "SLW:MapOfSwedishCounties"
                });


                MySettingsManager.SaveToDisk("test", mySettings);
                var mySettings2 = MySettingsManager.LoadFromDisk("test");

                Assert.IsNotNull(mySettings2);
                Assert.IsTrue(mySettings2.Filter.Taxa.TaxonIds.Contains(1));
                Assert.IsTrue(mySettings2.Filter.Taxa.TaxonIds.Contains(2));
                Assert.IsTrue(mySettings2.Filter.Taxa.TaxonIds.Contains(5));
                Assert.AreEqual(wfsLayersCount + 1, mySettings.DataProvider.MapLayers.WfsLayers.Count);
            }
       }

    }

}
