using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo
{
    using System.Web.UI;

    /// <summary>
    /// This class contain information about Pages.
    /// This is used to navigate in the application and
    /// to know how to render different buttons.
    /// </summary>
    public static class PageInfoManager
    {
        #region Variables
        private static readonly Dictionary<string, PageInfo> _dicPageInfos = new Dictionary<string, PageInfo>();

        #endregion Variables
        
        /// <summary>
        /// Initializes the <see cref="PageInfoManager"/> static class.
        /// </summary>
        static PageInfoManager()
        {
            // Home controller
            var homeIndexPage = AddPageInfo("Home", "Index", StateButtonIdentifier.None, ButtonGroupIdentifier.Overview, null, () => Resource.StartPage, true);
            AddPageInfo("Home", "Contact", StateButtonIdentifier.None, ButtonGroupIdentifier.Overview, homeIndexPage, () => Resource.SharedAboutAnalysisPortal, true);
            AddPageInfo("Home", "Cookies", StateButtonIdentifier.None, ButtonGroupIdentifier.Overview, homeIndexPage, () => Resource.SharedAboutCookiesHeader, true);
            AddPageInfo("Home", "VersionNumber", StateButtonIdentifier.None, ButtonGroupIdentifier.Overview, homeIndexPage, () => Resource.SharedVersionNumberText, true);
            AddPageInfo("Home", "NotImplemented", StateButtonIdentifier.None, ButtonGroupIdentifier.Overview, homeIndexPage, () => "Not implemented", true);
            AddPageInfo("Home", "Status", StateButtonIdentifier.None, ButtonGroupIdentifier.Overview, homeIndexPage, () => "Status", true);

            // Filter controller
            var filterIndexPage = AddPageInfo("Filter", "Index", StateButtonIdentifier.None, ButtonGroupIdentifier.Filter, homeIndexPage, () => Resource.HeadMenuFilter, false);
            var filterTaxaPage = AddPageInfo("Filter", "Taxa", StateButtonIdentifier.FilterTaxa, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterTaxa, true);
            AddPageInfo("Filter", "TaxonFromIds", StateButtonIdentifier.FilterTaxa, ButtonGroupIdentifier.Filter, filterTaxaPage, () => Resource.FilterTaxonFromIdsButton, true);
            AddPageInfo("Filter", "TaxonFromSearch", StateButtonIdentifier.FilterTaxa, ButtonGroupIdentifier.Filter, filterTaxaPage, () => Resource.FilterTaxonFromSearchButton, true);
            AddPageInfo("Filter", "TaxonByTaxonAttributes", StateButtonIdentifier.FilterTaxa, ButtonGroupIdentifier.Filter, filterTaxaPage, () => Resource.FilterTaxonByTaxonAttributes, true);
            AddPageInfo("Filter", "RedList", StateButtonIdentifier.FilterRedList, ButtonGroupIdentifier.Filter, filterTaxaPage, () => Resource.FilterRedListButton, true);

            var filterSpatialPage = AddPageInfo("Filter", "Spatial", StateButtonIdentifier.FilterSpatial, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterSpatial, true);
            AddPageInfo("Filter", "PolygonFromMapLayer", StateButtonIdentifier.FilterSpatial, ButtonGroupIdentifier.Filter, filterSpatialPage, () => Resource.FilterSpatialMapLayersTitle, true);
            AddPageInfo("Filter", "SpatialCommonRegions", StateButtonIdentifier.FilterSpatial, ButtonGroupIdentifier.Filter, filterSpatialPage, () => Resource.FilterSpatialCommonRegionsTitle, true);
            AddPageInfo("Filter", "SpatialDrawPolygon", StateButtonIdentifier.FilterSpatial, ButtonGroupIdentifier.Filter, filterSpatialPage, () => Resource.FilterSpatialDrawPolygonTitle, true);
            AddPageInfo("Filter", "Locality", StateButtonIdentifier.FilterSpatial, ButtonGroupIdentifier.Filter, filterSpatialPage, () => Resource.FilterSpatialLocality, true);
            AddPageInfo("Filter", "Occurrence", StateButtonIdentifier.FilterOccurrence, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterOccurrence, true);
            AddPageInfo("Filter", "Quality", StateButtonIdentifier.FilterQuality, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterQuality, true);
            AddPageInfo("Filter", "Accuracy", StateButtonIdentifier.FilterAccuracy, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterAccuracy, true);
            AddPageInfo("Filter", "Temporal", StateButtonIdentifier.FilterTemporal, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterTemporal, true);
            AddPageInfo("Filter", "Field", StateButtonIdentifier.FilterFields, ButtonGroupIdentifier.Filter, filterIndexPage, () => Resource.StateButtonFilterFields, true);

            // Settings

            // Format controller
            var presentationIndexPage = AddPageInfo("Format", "Index", StateButtonIdentifier.None, ButtonGroupIdentifier.Presentation, homeIndexPage, () => Resource.HeadMenuSettings, false);
            AddPageInfo("Format", "Map", StateButtonIdentifier.PresentationMap, ButtonGroupIdentifier.Presentation, presentationIndexPage, () => Resource.StateButtonPresentationCoordinateSystem, true);
            var presentationTablePage = AddPageInfo("Format", "Table", StateButtonIdentifier.PresentationTable, ButtonGroupIdentifier.Presentation, presentationIndexPage, () => Resource.StateButtonPresentationTable, false);
            AddPageInfo("Format", "SpeciesObservationTable", StateButtonIdentifier.PresentationTable, ButtonGroupIdentifier.Presentation, presentationTablePage, () => Resource.PresentationSpeciesObservationTable, true);
            AddPageInfo("Format", "Diagram", StateButtonIdentifier.PresentationDiagram, ButtonGroupIdentifier.Presentation, presentationIndexPage, () => Resource.StateButtonPresentationDiagram, true);
            AddPageInfo("Format", "Report", StateButtonIdentifier.PresentationReport, ButtonGroupIdentifier.Presentation, presentationIndexPage, () => Resource.StateButtonPresentationReport, true);
            AddPageInfo("Format", "FileFormat", StateButtonIdentifier.PresentationFileFormat, ButtonGroupIdentifier.Presentation, presentationIndexPage, () => Resource.StateButtonPresentationFileFormat, true);

            // Calculation controller
            var calculationIndexPage = AddPageInfo("Calculation", "Index", StateButtonIdentifier.None, ButtonGroupIdentifier.Calculation, homeIndexPage, () => Resource.HeadMenuSettings, false);
            AddPageInfo("Calculation", "GridStatistics", StateButtonIdentifier.CalculationGridStatistics, ButtonGroupIdentifier.Calculation, calculationIndexPage, () => Resource.StateButtonCalculationGridStatistics, true);
            AddPageInfo("Calculation", "SummaryStatistics", StateButtonIdentifier.CalculationSummaryStatistics, ButtonGroupIdentifier.Calculation, calculationIndexPage, () => Resource.StateButtonCalculationSummaryStatistics, true);
            AddPageInfo("Calculation", "TimeSeries", StateButtonIdentifier.CalculationTimeSeries, ButtonGroupIdentifier.Calculation, calculationIndexPage, () => Resource.StateButtonCalculationTimeSeries, true);

            // Account controller
            AddPageInfo("Account", "LogIn", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.AccountLogInText, true);
            AddPageInfo("Account", "LogOut", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.AccountLogOutText, true);
            AddPageInfo("Account", "AccessIsNotAllowed", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.AccountAccessIsNotAllowedTitle, true);
            AddPageInfo("Account", "ChangeUserRole", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.AccountChangeUserRoleUserRoleTiltle, true);            

            // Data controller
            var dataprovidersIndexPage = AddPageInfo("Data", "Index", StateButtonIdentifier.None, ButtonGroupIdentifier.DataProviders, homeIndexPage, () => Resource.ButtonGroupDataProviders, false);
            AddPageInfo("Data", "DataProviders", StateButtonIdentifier.DataProvidersSpeciesObservation, ButtonGroupIdentifier.DataProviders, dataprovidersIndexPage, () => Resource.StateButtonDataProvidersSpeciesObservation, true); //() => Resource.DataProvidersDataProvidersTitle
            var wfsLayersPage = AddPageInfo("Data", "WfsLayers", StateButtonIdentifier.DataProvidersWfsLayers, ButtonGroupIdentifier.DataProviders, dataprovidersIndexPage, () => Resource.StateButtonDataProvidersWfsLayers, true);
            AddPageInfo("Data", "WFSLayerEditor", StateButtonIdentifier.DataProvidersWfsLayers, ButtonGroupIdentifier.DataProviders, wfsLayersPage, () => Resource.DataSourcesWfsLayerEditorTitle, true);
            AddPageInfo("Data", "AddWfsLayer", StateButtonIdentifier.DataProvidersWfsLayers, ButtonGroupIdentifier.DataProviders, wfsLayersPage, () => Resource.DataAddWfsLayer, true);
            var wmsLayersPage = AddPageInfo("Data", "WmsLayers", StateButtonIdentifier.DataProvidersWmsLayers, ButtonGroupIdentifier.DataProviders, dataprovidersIndexPage, () => Resource.StateButtonDataProvidersWmsLayers, true);
            AddPageInfo("Data", "AddWmsLayer", StateButtonIdentifier.DataProvidersWmsLayers, ButtonGroupIdentifier.DataProviders, wmsLayersPage, () => Resource.DataProvidersAddWmsLayer, true);
            AddPageInfo("Data", "EditWmsLayer", StateButtonIdentifier.DataProvidersWmsLayers, ButtonGroupIdentifier.DataProviders, wmsLayersPage, () => Resource.DataProvidersEditWmsLayer, true);
            AddPageInfo("Data", "MetadataSearch", StateButtonIdentifier.DataProvidersMetadataSearch, ButtonGroupIdentifier.DataProviders, dataprovidersIndexPage, () => Resource.StateButtonDataProvidersMetadataSearch, true);

            // Result controller
            var resultInfoPage = AddPageInfo("Result", "Info", StateButtonIdentifier.None, ButtonGroupIdentifier.Result, homeIndexPage, () => Resource.ButtonGroupResult, false);
            var resultIndexPage = AddPageInfo("Result", "Index", StateButtonIdentifier.ResultView, ButtonGroupIdentifier.Result, homeIndexPage, () => Resource.ResultViews, false);
            var resultMapsPage = AddPageInfo("Result", "Maps", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultInfoPage, () => Resource.ResultGroupMaps, true);            
            AddPageInfo("Result", "SpeciesObservationClusterPointMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultViewSpeciesObservationClusterPointMap, true);
            AddPageInfo("Result", "SpeciesObservationMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultViewSpeciesObservationMap, true);
            AddPageInfo("Result", "SpeciesObservationGridMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultViewSpeciesObservationGridMap, true);

            AddPageInfo("Result", "WfsGridStatisticsMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultViewSpeciesObservationGridMap, true);
            AddPageInfo("Result", "SpeciesRichnessGridMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultViewSpeciesRichnessGridMap, true);
            AddPageInfo("Result", "DefaultSpeciesObservationGridMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultViewSpeciesObservationGridMap, true);
            AddPageInfo("Result", "TaxonHeatMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultTaxonHeatMap, true);
            AddPageInfo("Result", "SpeciesObservationHeatMapImage", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultSpeciesObservationHeatMap, true);
            AddPageInfo("Result", "SpeciesRichnessHeatMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultSpeciesRichnessHeatMap, true);
            AddPageInfo("Result", "SpeciesObservationPointMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultSpeciesObservationPointMap, true);
            AddPageInfo("Result", "SpeciesObservationHeatMap", StateButtonIdentifier.MapResultView, ButtonGroupIdentifier.Result, resultMapsPage, () => Resource.ResultSpeciesObservationHeatMap, true);

            var resultTablesPage = AddPageInfo("Result", "Tables", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultInfoPage, () => Resource.ResultGroupTables, true);
            AddPageInfo("Result", "SpeciesObservationTable", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewSpeciesObservationTable, true);
            AddPageInfo("Result", "GridStatisticsTableOnSpeciesObservationCounts", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewSpeciesObservationTaxonTable, true);
            AddPageInfo("Result", "GridStatisticsTableOnSpeciesRichness", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewTaxonGridTable, true);
            AddPageInfo("Result", "SpeciesObservationTaxonTable", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewSpeciesObservationTaxonTable, true);
            AddPageInfo("Result", "SpeciesObservationTaxonWithSpeciesObservationCountTable", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewSpeciesObservationTaxonSpeciesObservationCountTable, true);
            AddPageInfo("Result", "CombinedGridStatisticsTable", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultCombinedGridStatisticsTable, true);
            AddPageInfo("Result", "TimeSeriesTableOnSpeciesObservationCounts", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewTimeSeriesOnSpeciesObservationCountsTable, true);
            AddPageInfo("Result", "SummaryStatisticsPerPolygonTable", StateButtonIdentifier.TableResultView, ButtonGroupIdentifier.Result, resultTablesPage, () => Resource.ResultViewSummaryStatisticsPerPolygonTable, true);

            var resultDiagramsPage = AddPageInfo("Result", "Diagrams", StateButtonIdentifier.DiagramResultView, ButtonGroupIdentifier.Result, resultInfoPage, () => Resource.ResultGroupDiagrams, true);
            AddPageInfo("Result", "TimeSeriesHistogramOnSpeciesObservationCounts", StateButtonIdentifier.DiagramResultView, ButtonGroupIdentifier.Result, resultDiagramsPage, () => Resource.ResultViewSpeciesObservationDiagram, true);
            AddPageInfo("Result", "TimeSeriesDiagramOnSpeciesObservationAbundanceIndex", StateButtonIdentifier.DiagramResultView, ButtonGroupIdentifier.Result, resultDiagramsPage, () => Resource.ResultViewSpeciesObservationAbundanceIndexDiagram, true);

            var resultReportsPage = AddPageInfo("Result", "Reports", StateButtonIdentifier.ReportResultView, ButtonGroupIdentifier.Result, resultInfoPage, () => Resource.ResultGroupReports, true);
            AddPageInfo("Result", "SettingsReport", StateButtonIdentifier.ReportResultView, ButtonGroupIdentifier.Result, resultReportsPage, () => Resource.ResultViewSettingsSummary, true);
            AddPageInfo("Result", "SummaryStatisticsReport", StateButtonIdentifier.ReportResultView, ButtonGroupIdentifier.Result, resultReportsPage, () => Resource.ResultViewSummaryStatistics, true);
            AddPageInfo("Result", "ProvenanceReport", StateButtonIdentifier.ReportResultView, ButtonGroupIdentifier.Result, resultReportsPage, () => Resource.ResultViewProvenance, true);

            AddPageInfo("Result", "Download", StateButtonIdentifier.ResultDownload, ButtonGroupIdentifier.Result, resultInfoPage, () => Resource.ResultDownloads, true);
            AddPageInfo("Result", "Workflow", StateButtonIdentifier.ResultDownload, ButtonGroupIdentifier.Result, resultInfoPage, () => "Workflow", true);

            // Details controller
            AddPageInfo("Details", "ObservationDetail", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.ObservationDetailsTitle, true);

            // MySettings controller
            AddPageInfo("MySettings", "SpatialPolygonsSummaryPrintable", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => "SpatialPolygonSummaryPrintable", true);

            // Errors controller
            AddPageInfo("Errors", "General", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.SharedError, true);
            AddPageInfo("Errors", "Http404", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.SharedError, true);
            AddPageInfo("Errors", "Http403", StateButtonIdentifier.None, ButtonGroupIdentifier.None, homeIndexPage, () => Resource.SharedError, true);
        }

        /// <summary>
        /// Gets a dictionary key.
        /// </summary>
        /// <param name="controller">The controller name.</param>
        /// <param name="action">The action name.</param>
        /// <returns></returns>
        private static string GetKey(string controller, string action)
        {
            return string.Format("{0}{1}", controller, action).ToLower();
        }

        /// <summary>
        /// Adds a page info to the dictionary.
        /// </summary>
        /// <param name="controller">The controller name.</param>
        /// <param name="action">The action name.</param>
        /// <param name="currentStateButton">The associated state button.</param>
        /// <param name="buttonGroup">The associeated button group.</param>
        /// <param name="parentPage">The parent page.</param>
        /// <param name="titleExpression">The title expression.</param>
        /// <param name="breadcrumbNavigation"></param>
        /// <returns></returns>
        private static PageInfo AddPageInfo(string controller, string action, StateButtonIdentifier currentStateButton, ButtonGroupIdentifier buttonGroup, PageInfo parentPage, Expression<Func<string>> titleExpression, bool breadcrumbNavigation)
        {
            var pageInfo = new PageInfo(controller, action, currentStateButton, buttonGroup, parentPage, titleExpression, breadcrumbNavigation);
            _dicPageInfos.Add(GetKey(controller, action), pageInfo);
            return pageInfo;   
        }

        /// <summary>
        /// Gets a page infor object from the dictionary.
        /// </summary>
        /// <param name="controller">The controller name.</param>
        /// <param name="action">The action name.</param>
        /// <returns>A PageInfo object or null if it wasn't found</returns>
        public static PageInfo GetPageInfo(string controller, string action)
        {
            string key = GetKey(controller, action);
            PageInfo pageInfo;
            _dicPageInfos.TryGetValue(key, out pageInfo);
            return pageInfo;            
        }
    
        /// <summary>
        /// Determines whether <paramref name="parentPage"/> is equal to or ancestor to <paramref name="childPage"/>.
        /// </summary>
        /// <param name="parentPage">The parent page.</param>
        /// <param name="childPage">The child page.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="parentPage"/> is equal to or ancestor to <paramref name="childPage"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPageEqualToOrAncestorToOtherPage(PageInfo parentPage, PageInfo childPage)
        {
            if (parentPage == childPage)
            {
                return true;
            }

            return IsPageAncestorToOtherPage(parentPage, childPage);
        }

        /// <summary>
        /// Determines whether <paramref name="parentPage"/> is ancestor to <paramref name="childPage"/>.
        /// </summary>
        /// <param name="parentPage">The parent page.</param>
        /// <param name="childPage">The child page.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="parentPage"/> is ancestor to <paramref name="childPage"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPageAncestorToOtherPage(PageInfo parentPage, PageInfo childPage)
        {
            while (childPage.ParentPage != null)
            {
                childPage = childPage.ParentPage;
                if (childPage == parentPage)
                {
                    return true;
                }
            }

            return false;
        }
    }
}