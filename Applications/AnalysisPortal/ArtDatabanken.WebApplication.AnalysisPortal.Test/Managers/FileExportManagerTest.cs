using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Managers
{

    [TestClass]
    public class FileExportManagerTest : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridSpeciesCountsAsExcelXml()
        {
            LoginApplicationUser();

            GridStatisticsOnSpeciesCountExcelXml file = FileExportManager.GetGridSpeciesCountsAsExcelXml(SessionHandler.UserContext, CoordinateSystemId.GoogleMercator, false, false);
            Assert.IsNotNull(file.XmlAsText);

            // file.XmlFile.Save(@"C:\Users\oskark\Desktop\test3.xml");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridSpeciesObservationCountsAsExcelXml()
        {
            LoginApplicationUser();
            GridStatisticsOnSpeciesObservationCountExcelXml file = FileExportManager.GetGridSpeciesObservationCountsAsExcelXml(SessionHandler.UserContext, CoordinateSystemId.GoogleMercator, false, false);
            Assert.IsNotNull(file.XmlAsText);

            // file.XmlFile.Save(@"C:\Users\oskark\Desktop\testObs.xml");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetDataProviderListAsExcelXml()
        {
            LoginApplicationUser();

            DataProviderListExcelXml file = FileExportManager.GetDataProvidersAsExcelXml(SessionHandler.UserContext);
            Assert.IsNotNull(file.XmlAsText);

            // file.XmlFile.Save(@"C:\Users\oskark\Desktop\testProviders.xml");
        }

        [TestMethod]
        [TestCategory("TimeoutNightlyTestApp")]
        public void GetSummaryStatisticsAsExcelXml()
        {
            LoginApplicationUser();     
            SummaryStatisticsExcelXml file = FileExportManager.GetSummaryStatisticsAsExcelXml(SessionHandler.UserContext, false, false);
            Assert.IsNotNull(file.XmlAsText);

            // file.XmlFile.Save(@"C:\Users\oskark\Desktop\testSummary.xml");
        }

        [TestMethod]
        [TestCategory("TimeoutNightlyTestApp")]
        public void GetSummaryStatisticsPerPolygonAsExcelXml()
        {
            LoginApplicationUser();
            SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
            SummaryStatisticsPerPolygonExcelXml file = FileExportManager.GetSummaryStatisticsPerPolygonAsExcelXml(SessionHandler.UserContext, false, false);
            Assert.IsNotNull(file.XmlAsText);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]        
        public void GetTimeSeriesOnSpeciesObservationCountPerMonthOfYearsAsExcelXml()
        {
            LoginApplicationUser();

            Periodicity periodicity = Periodicity.Yearly;
            TimeSeriesOnSpeciesObservationCountsExcelXml file = FileExportManager.GetTimeSeriesOnSpeciesObservationCountsAsExcelXml(SessionHandler.UserContext, periodicity, false, false);
            Assert.IsNotNull(file.XmlAsText);

            // file.XmlFile.Save(@"C:\Users\oskark\Desktop\testTimeSeries.xml");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGetObservedTaxonListAsExcelXml()
        {
            LoginApplicationUser();

            ObservedTaxonListAsExcelXml file = FileExportManager.GetObservedTaxonListAsExcelXml(SessionHandler.UserContext, false, false);
            Assert.IsNotNull(file.XmlAsText);

            // file.XmlFile.Save(@"C:\Users\oskark\Desktop\testTaxonList.xml");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetObservedTaxonCountListAsExcelXml()
        {
            LoginApplicationUser();

            ObservedTaxonCountListAsExcelXml file = FileExportManager.GetObservedTaxonCountListAsExcelXml(SessionHandler.UserContext, false, false);
            
            Assert.IsNotNull(file.XmlAsText);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSettingsReportAsExcelXml()
        {
            LoginApplicationUser();

            var settingsReport = new List<object>();
            var dataProvidersSettingSummary = (DataProvidersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataProviders);
            var dataProviders = dataProvidersSettingSummary.GetSettingsSummaryModel().Where(dataProvider => dataProvider.IsSelected).ToList();

            settingsReport.Add(dataProviders);

            var taxaSettingSummary = (TaxaSettingSummary) MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterTaxa);
            var taxonList = taxaSettingSummary.GetSettingsSummaryModel();

            settingsReport.Add(taxonList);

            var regionSettingSummary = (RegionSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterRegion);
            var regions = regionSettingSummary.GetSettingsSummaryModel();

            settingsReport.Add(regions);

            var gridStatisticsSettingSummary = (GridStatisticsSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationGridStatistics);
            var gridStatisticsModel = gridStatisticsSettingSummary.GetSettingsSummaryModel(SessionHandler.UserContext);

            settingsReport.Add(gridStatisticsModel);

            var summaryStatisticsSettingSummary = (SummaryStatisticsSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationSummaryStatistics);
            var summaryStatistics = summaryStatisticsSettingSummary.GetSettingsSummaryModel(SessionHandler.UserContext);

            settingsReport.Add(summaryStatistics);
            
            var file = FileExportManager.GetSettingsReportAsExcelXml(SessionHandler.UserContext, true, true);
            Assert.IsNotNull(file.XmlAsText);
        }
    }
}