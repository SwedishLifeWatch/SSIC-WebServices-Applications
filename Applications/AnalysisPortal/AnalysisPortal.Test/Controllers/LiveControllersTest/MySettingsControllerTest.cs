using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalysisPortal;
using AnalysisPortal.Controllers;
using Microsoft.QualityTools.Testing.Fakes;

namespace AnalysisPortal.Tests
{
    using System.Runtime.Remoting.Messaging;
    using System.Web;
    using System.Web.Routing;

    using ArtDatabanken.Data.WebService;
    using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;
    using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization.Fakes;
    using ArtDatabanken.WebService.Client.SpeciesObservationService;

    /// <summary>
    /// This class 
    /// </summary>
    [TestClass]
    public class MySettingsControllerTest : DBTestControllerBaseTest
    {
      
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Filter_Data()
        {
            var mySettings = new MySettings();

            // Add taxa
            mySettings.Filter.Taxa.AddTaxonIds(new List<int>() { 1, 8, 12 });
            CollectionAssert.AreEqual(new List<int>() { 1, 8, 12 }, mySettings.Filter.Taxa.TaxonIds);
            CollectionAssert.AreNotEqual(new List<int>() { 1, 7, 12 }, mySettings.Filter.Taxa.TaxonIds);

            // Add polygon
            var polygon = GetPolygon1();
            mySettings.Filter.Spatial.Polygons.Add(polygon);
            Assert.IsNotNull(mySettings.Filter.Spatial.Polygons);
        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Changes_that_not_trigger_cache_refresh()
        {
            MySettings mySettings = new MySettings();
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);            
            
            // Changes to presentation map doesn't trigger cache refresh
            Assert.IsTrue(mySettings.Presentation.Map.IsActive);
            Assert.IsTrue(oldMySettings.Presentation.Map.IsActive);
            mySettings.Presentation.Map.IsActive = false;            
            Assert.IsFalse(mySettings.Presentation.Map.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.Presentation.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);


            // Changes to presentation table doesn't trigger cache refresh
            Assert.IsTrue(mySettings.Presentation.Table.IsActive);
            Assert.IsTrue(oldMySettings.Presentation.Table.IsActive);
            mySettings.Presentation.Table.IsActive = false;            
            Assert.IsFalse(mySettings.Presentation.Table.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.Presentation.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);


            // Changes to map layers doesn't trigger cache refresh
            Assert.IsFalse(mySettings.DataProvider.MapLayers.IsActive);
            Assert.IsFalse(oldMySettings.DataProvider.MapLayers.IsActive);
            mySettings.DataProvider.MapLayers.IsActive = true;
            WfsLayerSetting wfsLayer = new WfsLayerSetting();
            wfsLayer.Name = "Test";
            wfsLayer.Id = 0;
            mySettings.DataProvider.MapLayers.WfsLayers.Add(wfsLayer);            
            Assert.IsFalse(mySettings.DataProvider.MapLayers.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.DataProvider.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);

        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Taxa()
        {
            MySettings mySettings = new MySettings();            
            mySettings.Filter.Taxa.AddTaxonIds(new List<int>() {23, 45, 67});
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);            
            
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);
            mySettings.Filter.Taxa.RemoveTaxonId(67);            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            mySettings.Filter.Taxa.AddTaxonId(67);            
            mySettings.Filter.Taxa.AddTaxonId(67); // duplicates is removed
            

            mySettings.ResultCacheNeedsRefresh = false;
            Assert.IsTrue(mySettings.Filter.Taxa.IsActive); // is active as default
            Assert.IsTrue(oldMySettings.Filter.Taxa.IsActive);
            mySettings.Filter.Taxa.IsActive = false;            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
        }


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void GridStatistics()
        {
            MySettings mySettings = new MySettings();
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);
            // Is true by default
            Assert.IsTrue(mySettings.Calculation.GridStatistics.IsActive);
            mySettings.Calculation.GridStatistics.IsActive = true;            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            mySettings.Calculation.GridStatistics.IsActive = false;            

            mySettings.ResultCacheNeedsRefresh = false;
            mySettings.Calculation.GridStatistics.CoordinateSystemId = 2;            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            mySettings.Calculation.GridStatistics.CoordinateSystemId = null;            

            mySettings.ResultCacheNeedsRefresh = false;
            mySettings.Calculation.GridStatistics.GridSize = 200;            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            mySettings.Calculation.GridStatistics.GridSize = null;            

            mySettings.ResultCacheNeedsRefresh = false;
            Assert.IsTrue(mySettings.Calculation.GridStatistics.CalculateNumberOfObservations);
            mySettings.Calculation.GridStatistics.CalculateNumberOfObservations = false;            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            mySettings.Calculation.GridStatistics.CalculateNumberOfObservations = true;                 
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GridStatisticsSettingsSummaryPropertyTest()
        {

            using (ShimsContext.Create())
            {
                // First we must login
                LoginTestUser();
                IUserContext user = SessionHandler.UserContext;
                SessionHandler.MySettings.Calculation.GridStatistics = new GridStatisticsSetting();
                // Created from SessionHandler.MySettings.Calculation.SummaryStatistics
                GridStatisticsSettingSummary settingSummary = new GridStatisticsSettingSummary();

                //************ Check all settings at startup as designed *************************
                //These setting is active at startup
                Assert.IsTrue(settingSummary.HasActiveSettings);
                Assert.IsTrue(settingSummary.HasSettings);
                Assert.IsTrue(settingSummary.IsActive);
                Assert.IsTrue(settingSummary.HasSettingsSummary);
                Assert.IsTrue(settingSummary.Identifier == MySettingsSummaryItemIdentifier.CalculationGridStatistics);
                // Verify HardCodedData //  TODO why hardcoded???
                Assert.IsTrue(settingSummary.PageInfo.Controller.Equals("Calculation"));
                Assert.IsTrue(settingSummary.PageInfo.Action.Equals("GridStatistics"));
                Assert.IsTrue(settingSummary.SettingsSummaryWidth == 350);
                Assert.IsTrue(settingSummary.Title.Equals(Resources.Resource.StateButtonCalculationGridStatistics));

                GridStatisticsViewModel model = settingSummary.GetSettingsSummaryModel(user);
                //Check defaults
                Assert.IsTrue(model.CalculateNumberOfObservations);
                Assert.IsTrue(model.CalculateNumberOfTaxa);
                CoordinateSystemId enumDisplayStatus = ((CoordinateSystemId)model.CoordinateSystemId);
                string stringValue = enumDisplayStatus.ToString();
                Assert.IsTrue(stringValue.Equals(CoordinateSystemId.SWEREF99.ToString()));
                Assert.IsTrue(model.GridSize == 10000);
                Assert.IsTrue(model.CoordinateSystems.Count > 0);
                Assert.IsTrue(model.IsSettingsDefault);


                //************ Check all settings at startup as designed *************************
                //These setting is active at startup
                Assert.IsTrue(settingSummary.HasActiveSettings);
                Assert.IsTrue(settingSummary.HasSettings);
                Assert.IsTrue(settingSummary.IsActive);
                Assert.IsTrue(settingSummary.HasSettingsSummary);
                Assert.IsTrue(settingSummary.Identifier == MySettingsSummaryItemIdentifier.CalculationGridStatistics);
            }


        }

        // Test Moved to ArtDatabanken.WebApplication.AnalysisPortal.Test.MySettings.Calculation.SummaryStatisticsSettingTest
        //[TestMethod()]
        //[TestCategory("NightlyTestApp")]
        //public void SummaryStatisticsTest()
        //{
        //    MySettings mySettings = new MySettings();
        //    mySettings.ResultCacheNeedsRefresh = false;
        //    MySettings oldMySettings = ObjectCopier.Clone(mySettings);
        //    //Check that is active and has settings by default.
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.IsActive);            
        //    Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasSettings);
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasActiveSettings);
            
        //    // Deactivate summary statistics settings
        //    mySettings.Calculation.SummaryStatistics.IsActive = false;
        //    Assert.IsFalse(mySettings.Calculation.SummaryStatistics.IsActive);            
        //    Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasSettings);
        //    Assert.IsFalse(mySettings.Calculation.SummaryStatistics.HasActiveSettings);
        //   // Set summary statistics active again
        //    mySettings.Calculation.SummaryStatistics.IsActive = true;
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.IsActive);
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasSettings);
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasActiveSettings);

        //    // Check that CalculateNumberOfObservationsfromObsData is updated correct
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData);
        //    mySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData= false;
        //    Assert.IsFalse(mySettings.Calculation.SummaryStatistics.HasSettings);
        //    Assert.IsFalse(mySettings.Calculation.SummaryStatistics.HasActiveSettings);            
        //    Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
        //    mySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = true;
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasSettings);
        //    Assert.IsTrue(mySettings.Calculation.SummaryStatistics.HasActiveSettings);
            
        //}
        //[TestMethod()]
        //[TestCategory("NightlyTestApp")]
        //public void PresentationReportTest()
        //{
        //    MySettings mySettings = new MySettings();
        //    mySettings.ResultCacheNeedsRefresh = false;
        //   MySettings oldMySettings = ObjectCopier.Clone(mySettings);
        //    //Check that is active and has settings by default.
        //    Assert.IsTrue(mySettings.Presentation.Report.IsActive);            
        //    Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);

        //    // Deactivate report settings
        //    mySettings.Presentation.Report.IsActive = false;
        //    Assert.IsFalse(mySettings.Presentation.Report.IsActive);
        //    // Since Presentation report not have any settings to set. It is
        //    // for the moment alvays returning true; Button never goes green...
        //    //Assert.IsTrue(mySettings.Presentation.Report.HasSettings);
            
           
        //    // Set report active again
        //    mySettings.Presentation.Report.IsActive = true;
        //    Assert.IsTrue(mySettings.Presentation.Report.IsActive);
          
        //}


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void PresentationSummaryStatisticsTest()
        {
            MySettings mySettings = new MySettings();
            mySettings.ResultCacheNeedsRefresh = false;
            
            //Check that is active and has settings by default.
            Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.IsActive);            
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);
            // IS set to false by default since ther is no settings for the moment to set
            //Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.HasSettings);
            Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.SelectedReportId == 0);

            // Deactivate summary statistics settings
            mySettings.Presentation.Report.SummaryStatisticsReportSetting.IsActive = false;
            Assert.IsFalse(mySettings.Presentation.Report.SummaryStatisticsReportSetting.IsActive);
            //Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.HasSettings);
            // Set summary statistics active again
            mySettings.Presentation.Report.SummaryStatisticsReportSetting.IsActive = true;
            Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.IsActive);
            //Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.HasSettings);
           
            // Check that CalculateNumberOfObservationsfromObsData is updated correct
            Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.SelectedReportId == 0);
           // Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.HasSettings);

            mySettings.Presentation.Report.SummaryStatisticsReportSetting.SelectedReportId = 1;
            //Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.HasSettings);
            Assert.IsTrue(mySettings.Presentation.Report.SummaryStatisticsReportSetting.SelectedReportId == 1);

        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Test_PresentationTableSettings()
        {
            MySettings mySettings = new MySettings();
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);

            // presentation settings doesn't trigger cache refresh
            mySettings.Presentation.Table.IsActive = true;                        
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);
            mySettings.Presentation.Table.SpeciesObservationTable.SelectedTableId = 1;                        
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);

            mySettings.Presentation.Table.IsActive = false;
            mySettings.Presentation.Table.SpeciesObservationTable.SelectedTableId = 0;



            using (ShimsContext.Create())
            {
  
                base.LoginTestUserAnalyser();
                IUserContext userContext = SessionHandler.UserContext;

                List<ISpeciesObservationFieldDescription> tableFields = mySettings.Presentation.Table.SpeciesObservationTable.GetTableFields(userContext);
                Assert.IsTrue(tableFields.Count > 0);

                tableFields = mySettings.Presentation.Table.SpeciesObservationTable.GetTableFields(userContext, 1, false);
                Assert.IsTrue(tableFields.Count > 0);

                tableFields = mySettings.Presentation.Table.SpeciesObservationTable.GetTableFields(userContext, 0, true);
                Assert.IsTrue(tableFields.Count > 0);
            }

        }


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void DataPoint_Equality()
        {
            DataPoint p1 = new DataPoint(1.0, 2.0, null);
            DataPoint p2 = new DataPoint(1.0, 2.0, null);
            Assert.AreEqual(p1, p2);
            DataPoint p3 = new DataPoint(1.0, 2.0, 3.0);
            DataPoint p4 = new DataPoint(1.0, 2.0, 3.0);
            Assert.AreEqual(p3, p4);
            DataPoint p5 = new DataPoint(1.0, 2.0, null);
            DataPoint p6 = new DataPoint(1.0, 2.0, 3.0);
            Assert.AreNotEqual(p5, p6);
            DataPoint p7 = new DataPoint(1.0, 2.0, 3.0);
            DataPoint p8 = new DataPoint(1.0, 2.0, null);
            Assert.AreNotEqual(p7, p8);
        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void DataLinearRing_Equality()
        {
            DataLinearRing dataLinearRing1 = new DataLinearRing();
            dataLinearRing1.Points = new List<DataPoint>();
            dataLinearRing1.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing1.Points.Add(new DataPoint(2.0, 3.0, null));
            dataLinearRing1.Points.Add(new DataPoint(3.0, 4.0, null));

            DataLinearRing dataLinearRing2 = new DataLinearRing();
            dataLinearRing2.Points = new List<DataPoint>();
            dataLinearRing2.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing2.Points.Add(new DataPoint(2.0, 3.0, null));
            dataLinearRing2.Points.Add(new DataPoint(3.0, 4.0, null));

            Assert.AreEqual(dataLinearRing1, dataLinearRing2);            
            Assert.AreEqual(dataLinearRing1.GetHashCode(), dataLinearRing2.GetHashCode());

            dataLinearRing2.Points.RemoveAt(2);
            Assert.AreNotEqual(dataLinearRing1, dataLinearRing2);
            Assert.AreNotEqual(dataLinearRing1.GetHashCode(), dataLinearRing2.GetHashCode());

            dataLinearRing2.Points.Add(new DataPoint(3.0, 4.0, null));
            Assert.AreEqual(dataLinearRing1, dataLinearRing2);
            Assert.AreEqual(dataLinearRing1.GetHashCode(), dataLinearRing2.GetHashCode());

            DataLinearRing dataLinearRing3 = new DataLinearRing();
            dataLinearRing3.Points = new List<DataPoint>();
            dataLinearRing3.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing3.Points.Add(new DataPoint(2.0, 5.0, null));
            dataLinearRing3.Points.Add(new DataPoint(3.0, 4.0, null));

            Assert.AreNotEqual(dataLinearRing1, dataLinearRing3);
            Assert.AreNotEqual(dataLinearRing1.GetHashCode(), dataLinearRing3.GetHashCode());
        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void DataPolygon_Equality()
        {
            DataLinearRing dataLinearRing1 = new DataLinearRing();
            dataLinearRing1.Points = new List<DataPoint>();
            dataLinearRing1.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing1.Points.Add(new DataPoint(2.0, 3.0, null));
            dataLinearRing1.Points.Add(new DataPoint(3.0, 4.0, null));

            DataLinearRing dataLinearRing2 = new DataLinearRing();
            dataLinearRing2.Points = new List<DataPoint>();
            dataLinearRing2.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing2.Points.Add(new DataPoint(2.0, 3.0, null));
            dataLinearRing2.Points.Add(new DataPoint(3.0, 4.0, null));

            DataPolygon dataPolygon1 = new DataPolygon();
            dataPolygon1.LinearRings = new List<DataLinearRing>();
            dataPolygon1.LinearRings.Add(dataLinearRing1);

            DataPolygon dataPolygon2 = new DataPolygon();
            dataPolygon2.LinearRings = new List<DataLinearRing>();
            dataPolygon2.LinearRings.Add(dataLinearRing2);

            Assert.AreEqual(dataPolygon1, dataPolygon2);
            Assert.AreEqual(dataPolygon1.GetHashCode(), dataPolygon2.GetHashCode());


            dataLinearRing2.Points.RemoveAt(2);
            Assert.AreNotEqual(dataPolygon1, dataPolygon2);
            Assert.AreNotEqual(dataPolygon1.GetHashCode(), dataPolygon2.GetHashCode());


            dataLinearRing2.Points.Add(new DataPoint(3.0, 4.0, null));
            Assert.AreEqual(dataPolygon1, dataPolygon2);
            Assert.AreEqual(dataPolygon1.GetHashCode(), dataPolygon2.GetHashCode());

            DataLinearRing dataLinearRing3 = new DataLinearRing();
            dataLinearRing3.Points = new List<DataPoint>();
            dataLinearRing3.Points.Add(new DataPoint(1.0, 2.0, null));
            dataLinearRing3.Points.Add(new DataPoint(2.0, 5.0, null));
            dataLinearRing3.Points.Add(new DataPoint(3.0, 4.0, null));
            DataPolygon dataPolygon3 = new DataPolygon();
            dataPolygon3.LinearRings = new List<DataLinearRing>();
            dataPolygon3.LinearRings.Add(dataLinearRing3);


            Assert.AreNotEqual(dataPolygon1, dataPolygon3);
            Assert.AreNotEqual(dataPolygon1.GetHashCode(), dataPolygon3.GetHashCode());
        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Polygons()
        {
            MySettings mySettings = new MySettings();
            mySettings.Filter.Spatial.Polygons.Add(GetPolygon1());
            mySettings.Filter.Spatial.Polygons.Add(GetPolygon2());
            mySettings.ResultCacheNeedsRefresh = false;
            
            // No changes is made => cache is up to date            
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);

            mySettings.Filter.Spatial.Polygons.RemoveAt(1);
            // One polygon is removed => cache needs refresh            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            

            mySettings.ResultCacheNeedsRefresh = false;
            Assert.IsFalse(mySettings.Filter.Spatial.IsActive); // is not active as default            
            mySettings.Filter.Spatial.IsActive = true;
            // changing IsActive => cache needs refresh.            
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Regions()
        {
            MySettings mySettings = new MySettings();
            mySettings.Filter.Spatial.RegionIds.Add(1);
            mySettings.Filter.Spatial.RegionIds.Add(25);
            mySettings.Filter.Spatial.RegionIds.Add(32);
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);

            // No changes is made => cache is up to date
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);

            mySettings.Filter.Spatial.RegionIds.RemoveAt(1);
            // One polygon is removed => cache needs refresh
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);

            mySettings.Filter.Spatial.RegionIds.Add(25);
            // The removed region is added again => no changes is made => cache is up to date
            
            mySettings.ResultCacheNeedsRefresh = false;
            Assert.IsFalse(mySettings.Filter.Spatial.IsActive); // is active as default
            Assert.IsFalse(oldMySettings.Filter.Spatial.IsActive);
            mySettings.Filter.Spatial.IsActive = true;
            // changing IsActive => cache needs refresh.
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
        }

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Change_DataProviders()
        {
            MySettings mySettings = new MySettings();
            mySettings.DataProvider.DataProviders.DataProvidersGuids.Add("a");
            mySettings.DataProvider.DataProviders.DataProvidersGuids.Add("b");
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);
            
            // No changes is made => cache is up to date
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);

            mySettings.DataProvider.DataProviders.DataProvidersGuids.Remove("a");
            // One data provider is removed => cache needs refresh
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);

            mySettings.DataProvider.DataProviders.DataProvidersGuids.Add("a");
            // The removed data provider is added again => no changes is made => cache is up to date
            }


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Is_Cache_Refresh_Needed()
        {
            MySettings mySettings = new MySettings();
            mySettings.ResultCacheNeedsRefresh = false;
            MySettings oldMySettings = ObjectCopier.Clone(mySettings);            

            // No changes => cache is up to date
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);            

            mySettings.Filter.Taxa.AddTaxonIds(new List<int>() {1,2});
            // No changes to spatial = > spatial settings doesn't trigger cache refresh
            Assert.IsFalse(mySettings.Filter.Spatial.ResultCacheNeedsRefresh);
            // Taxa is changed => cache is out of date
            Assert.IsTrue(mySettings.Filter.Taxa.ResultCacheNeedsRefresh);
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
        }


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Test_EnsureInitialized()
        {
            MySettings mySettings = new MySettings();
            mySettings.Filter = null;
            mySettings.Presentation = null;
            mySettings.Calculation = null;
            mySettings.DataProvider = null;

            mySettings.EnsureInitialized();
            Assert.IsNotNull(mySettings.Filter);
            Assert.IsNotNull(mySettings.Presentation);
            Assert.IsNotNull(mySettings.Calculation);
            Assert.IsNotNull(mySettings.DataProvider);

            Assert.IsNotNull(mySettings.Filter.Taxa.TaxonIds);
            Assert.IsNotNull(mySettings.Filter.Spatial.Polygons);

            Assert.IsNotNull(mySettings.DataProvider.DataProviders.DataProvidersGuids);

        }

        private DataPolygon GetPolygon1()
        {
            DataPolygon polygon = new DataPolygon();
            DataLinearRing linearRing = new DataLinearRing();
            List<DataPoint> points = new List<DataPoint>();
            points.Add(new DataPoint(0, 0, null));
            points.Add(new DataPoint(0, 20, null));
            points.Add(new DataPoint(20, 20, null));
            points.Add(new DataPoint(20, 0, null));
            points.Add(new DataPoint(0, 0, null));
            linearRing.Points = points;
            polygon.LinearRings = new List<DataLinearRing>();
            polygon.LinearRings.Add(linearRing);
            return polygon;
        }

        private DataPolygon GetPolygon2()
        {
            DataPolygon polygon = new DataPolygon();
            DataLinearRing linearRing = new DataLinearRing();
            List<DataPoint> points = new List<DataPoint>();
            points.Add(new DataPoint(10, 10, null));
            points.Add(new DataPoint(10, 20, null));
            points.Add(new DataPoint(20, 20, null));
            points.Add(new DataPoint(20, 10, null));
            points.Add(new DataPoint(10, 10, null));
            linearRing.Points = points;
            polygon.LinearRings = new List<DataLinearRing>();
            polygon.LinearRings.Add(linearRing);
            return polygon;
        }
       
        /// <summary>
        /// Property test for SummaryStatisticsSettings summary.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SummaryStatisticsSettingsSummaryPropertyTest()
        {
            using (ShimsContext.Create())
            {
                // First we must login
                LoginTestUser();
                IUserContext user = SessionHandler.UserContext;
                SessionHandler.MySettings.Calculation.SummaryStatistics = new SummaryStatisticsSetting();

                // Created from SessionHandler.MySettings.Calculation.SummaryStatistics
                SummaryStatisticsSettingSummary settingSummary = new SummaryStatisticsSettingSummary();

                // ************ Check all settings at startup as designed *************************
                // These setting is active at startup
                Assert.IsTrue(settingSummary.HasActiveSettings);
                Assert.IsTrue(settingSummary.HasSettings);
                Assert.IsTrue(settingSummary.IsActive);
                Assert.IsTrue(settingSummary.HasSettingsSummary);
                Assert.IsTrue(settingSummary.Identifier == MySettingsSummaryItemIdentifier.CalculationSummaryStatistics);
                
                // Verify HardCodedData //  TODO why hardcoded???
                Assert.IsTrue(settingSummary.PageInfo.Controller.Equals("Calculation"));
                Assert.IsTrue(settingSummary.PageInfo.Action.Equals("SummaryStatistics"));
                Assert.IsTrue(settingSummary.SettingsSummaryWidth == 350);
                Assert.IsTrue(settingSummary.Title.Equals(Resources.Resource.StateButtonCalculationSummaryStatistics));

                SummaryStatisticsViewModel model = settingSummary.GetSettingsSummaryModel(user);

                // Check defaults
                Assert.IsTrue(model.CalculateNumberOfObservationsfromObsData);
                Assert.IsTrue(model.CalculateNumberOfSpeciesfromObsData);
                Assert.IsNull(model.WfsGridStatisticsLayerId);
                Assert.IsTrue(model.IsSettingsDefault);

                // ************ Check all settings at startup as designed *************************
                // These setting is active at startup
                Assert.IsTrue(settingSummary.HasActiveSettings);
                Assert.IsTrue(settingSummary.HasSettings);
                Assert.IsTrue(settingSummary.IsActive);
                Assert.IsTrue(settingSummary.HasSettingsSummary);
                Assert.IsTrue(settingSummary.Identifier == MySettingsSummaryItemIdentifier.CalculationSummaryStatistics);
            }
        }
    }
}