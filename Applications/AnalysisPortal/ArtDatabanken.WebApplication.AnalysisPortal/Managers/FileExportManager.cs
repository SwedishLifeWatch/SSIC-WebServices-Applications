using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class handles creation of files for data down load.
    /// </summary>
    public static class FileExportManager
    {
        /// <summary>
        /// Creates an xml excel file with species observations data.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <param name="coordinateSystemId">The cooordinate system.</param>
        /// <param name="speciesObservationTableColumnsSetId">The table columns set to use.</param>
        /// <param name="useLabelAsColumnHeader">Use label as column header.</param>
        /// <returns>The xml Excel file.</returns>
        public static SpeciesObservationsExcelXml GetSpeciesObservationsAsExcelXml(
            IUserContext currentUser, 
            bool addSettings, 
            bool addProvenance, 
            CoordinateSystemId coordinateSystemId,
            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId,
            bool useLabelAsColumnHeader)
        {
            return new SpeciesObservationsExcelXml(
                currentUser, 
                addSettings, 
                addProvenance, 
                coordinateSystemId,
                speciesObservationTableColumnsSetId,
                useLabelAsColumnHeader);
        }

        /// <summary>
        /// Creates an xlsx excel file with species observations data.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <param name="coordinateSystemId">Coordinate system to use.</param>
        /// <param name="speciesObservationTableColumnsSetId">The table columns set to use.</param>
        /// <param name="useLabelAsColumnHeader">Use label as column header.</param>
        /// <returns>
        /// The xlsx Excel file.
        /// </returns>
        public static SpeciesObservationsExcelXlsx GetSpeciesObservationsAsExcelXlsx(
            IUserContext currentUser, 
            bool addSettings, 
            bool addProvenance, 
            CoordinateSystemId coordinateSystemId,
            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId, 
            bool useLabelAsColumnHeader)
        {
            return new SpeciesObservationsExcelXlsx(
                currentUser, 
                addSettings, 
                addProvenance, 
                coordinateSystemId, 
                speciesObservationTableColumnsSetId, 
                useLabelAsColumnHeader);
        }

        /// <summary>
        /// Creates an xml excel file with species observations data.
        /// </summary>
        /// <param name="speciesObservations">The species observations data.</param>        
        /// <param name="useLabelAsColumnHeader">Use label as column header.</param>
        /// <returns>The xml Excel file.</returns>
        public static SpeciesObservationsCsv GetSpeciesObservationsAsCsv(
            List<Dictionary<ViewTableField, string>> speciesObservations,            
            bool useLabelAsColumnHeader)
        {            
            SpeciesObservationsCsv file = new SpeciesObservationsCsv(
                speciesObservations,                
                useLabelAsColumnHeader);
            return file;
        }

        /// <summary>
        /// Creates an xml excel file with grid based data on species counts.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml Excel file.</returns>
        public static GridStatisticsOnSpeciesCountExcelXml GetGridSpeciesCountsAsExcelXml(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
        {
            GridStatisticsOnSpeciesCountExcelXml file = new GridStatisticsOnSpeciesCountExcelXml(currentUser, coordinateSystem, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with grid based data on species counts.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx Excel file.</returns>
        public static GridStatisticsOnSpeciesCountExcelXlsx GetGridSpeciesCountsAsExcelXlsx(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
        {
            GridStatisticsOnSpeciesCountExcelXlsx file = new GridStatisticsOnSpeciesCountExcelXlsx(currentUser, coordinateSystem, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xml excel file with grid based data on species observation counts. 
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml Excel file.</returns>
        public static GridStatisticsOnSpeciesObservationCountExcelXml GetGridSpeciesObservationCountsAsExcelXml(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
        {
            GridStatisticsOnSpeciesObservationCountExcelXml file = new GridStatisticsOnSpeciesObservationCountExcelXml(currentUser, coordinateSystem, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with grid based data on species observation counts. 
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx Excel file.</returns>
        public static GridStatisticsOnSpeciesObservationCountExcelXlsx GetGridSpeciesObservationCountsAsExcelXlsx(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
        {
            GridStatisticsOnSpeciesObservationCountExcelXlsx file = new GridStatisticsOnSpeciesObservationCountExcelXlsx(currentUser, coordinateSystem, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xml excel file with a list of data providers and statistics on total number of currently available species observations.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <returns>The xml Excel file.</returns>
        public static DataProviderListExcelXml GetDataProvidersAsExcelXml(IUserContext currentUser)
        {
            DataProviderListExcelXml file = new DataProviderListExcelXml(currentUser);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with a list of data providers and statistics on total number of currently available species observations.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <returns>The xlsx Excel file.</returns>
        public static DataProviderListExcelXlsx GetDataProvidersAsExcelXlsx(IUserContext currentUser)
        {
            DataProviderListExcelXlsx file = new DataProviderListExcelXlsx(currentUser);
            return file;
        }
        
        /// <summary>
        /// Creates an xml excel file with summary statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml Excel file.</returns>
        public static SummaryStatisticsExcelXml GetSummaryStatisticsAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SummaryStatisticsExcelXml file = new SummaryStatisticsExcelXml(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with summary statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx Excel file.</returns>
        public static SummaryStatisticsExcelXlsx GetSummaryStatisticsAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SummaryStatisticsExcelXlsx file = new SummaryStatisticsExcelXlsx(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xml excel file with summary statistics per polygon.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static SummaryStatisticsPerPolygonExcelXml GetSummaryStatisticsPerPolygonAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SummaryStatisticsPerPolygonExcelXml file = new SummaryStatisticsPerPolygonExcelXml(currentUser, addSettings, addProvenance);

            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with summary statistics per polygon.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static SummaryStatisticsPerPolygonExcelXlsx GetSummaryStatisticsPerPolygonAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SummaryStatisticsPerPolygonExcelXlsx file = new SummaryStatisticsPerPolygonExcelXlsx(currentUser, addSettings, addProvenance);

            return file;
        }

        /// <summary>
        /// Creates an xml excel file with the number of observed species over a selected period of time.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static TimeSeriesOnSpeciesObservationCountsExcelXml GetTimeSeriesOnSpeciesObservationCountsAsExcelXml(IUserContext currentUser, Periodicity periodicity, bool addSettings, bool addProvenance)
        {
            TimeSeriesOnSpeciesObservationCountsExcelXml file = new TimeSeriesOnSpeciesObservationCountsExcelXml(currentUser, periodicity, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with the number of observed species over a selected period of time.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static TimeSeriesOnSpeciesObservationCountsExcelXlsx GetTimeSeriesOnSpeciesObservationCountsAsExcelXlsx(
            IUserContext currentUser, 
            Periodicity periodicity, 
            bool addSettings, 
            bool addProvenance)
        {
            TimeSeriesOnSpeciesObservationCountsExcelXlsx file = 
                new TimeSeriesOnSpeciesObservationCountsExcelXlsx(
                    currentUser, 
                    periodicity, 
                    addSettings, 
                    addProvenance);

            return file;
        }

        /// <summary>        
        /// Creates an xml excel file with time series statistics on species observation abundance index.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static TimeSeriesOnSpeciesObservationAbundanceIndexExcelXml GetTimeSeriesOnSpeciesObservationAbundanceIndexAsExcelXml(IUserContext currentUser, Periodicity periodicity, bool addSettings, bool addProvenance)
        {
            return new TimeSeriesOnSpeciesObservationAbundanceIndexExcelXml(currentUser, periodicity, addSettings, addProvenance);
        }

        /// <summary>        
        /// Creates an xlsx excel file with time series statistics on species observation abundance index.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static TimeSeriesOnSpeciesObservationAbundanceIndexExcelXlsx GetTimeSeriesOnSpeciesObservationAbundanceIndexAsExcelXlsx(IUserContext currentUser, Periodicity periodicity, bool addSettings, bool addProvenance)
        {
            return new TimeSeriesOnSpeciesObservationAbundanceIndexExcelXlsx(currentUser, periodicity, addSettings, addProvenance);
        }

        /// <summary>
        /// Creates an xml excel file with the list of observed taxon.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static ObservedTaxonListAsExcelXml GetObservedTaxonListAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            ObservedTaxonListAsExcelXml file = new ObservedTaxonListAsExcelXml(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with the list of observed taxon.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static ObservedTaxonListAsExcelXlsx GetObservedTaxonListAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            ObservedTaxonListAsExcelXlsx file = new ObservedTaxonListAsExcelXlsx(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xml excel file with the list of observed taxon.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static ObservedTaxonCountListAsExcelXml GetObservedTaxonCountListAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            ObservedTaxonCountListAsExcelXml file = new ObservedTaxonCountListAsExcelXml(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with the list of observed taxon.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static ObservedTaxonCountListAsExcelXlsx GetObservedTaxonCountListAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            ObservedTaxonCountListAsExcelXlsx file = new ObservedTaxonCountListAsExcelXlsx(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xml excel file with combined grid statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static CombinedGridStatisticsExcelXml GetCombinedGridStatisticsAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            CombinedGridStatisticsExcelXml file = new CombinedGridStatisticsExcelXml(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with combined grid statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static CombinedGridStatisticsExcelXlsx GetCombinedGridStatisticsAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            CombinedGridStatisticsExcelXlsx file = new CombinedGridStatisticsExcelXlsx(currentUser, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Creates an xsl excel file with the current settings.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static SettingsReportExcelXml GetSettingsReportAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SettingsReportExcelXml file = new SettingsReportExcelXml(currentUser, true);

            return file;
        }

        /// <summary>
        ///  Creates an Xlsx excel file with the current settings.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns></returns>
        public static SettingsReportExcelXlsx GetSettingsReportAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            var file = new SettingsReportExcelXlsx(currentUser);

            return file;
        }

        /// <summary>
        /// Creates an xml excel file with provenances.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml Excel file.</returns>
        public static SpeciesObservationProvenanceExcelXml GetSpeciesObservationProvenanceAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SpeciesObservationProvenanceExcelXml file = new SpeciesObservationProvenanceExcelXml(currentUser);
            return file;
        }

        /// <summary>
        /// Creates an xlsx excel file with provenances.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx Excel file.</returns>
        public static SpeciesObservationProvenanceExcelXlsx GetSpeciesObservationProvenanceAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            SpeciesObservationProvenanceExcelXlsx file = new SpeciesObservationProvenanceExcelXlsx(currentUser);
            return file;
        }

        /// <summary>
        /// Creates the species observation grid as a comma separated file (CSV) and saves it on the server.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="data">The data.</param>
        /// <returns>The filename.</returns>
        public static string CreateSpeciesObservationGridAsCsvAndStoreOnServer(IUserContext userContext, SpeciesObservationGridResult data)
        {
            var speciesObservationGridCsv = new SpeciesObservationGridCsv();
            var path = "~/Temp/Export/";            
            var filename = FileSystemManager.CreateRandomFilenameWithPrefix("data", ".csv");            
            var absoluteFilePath = FileSystemManager.GetAbsoluteFilePath(path, filename);
            var taxaName = ((TaxaSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterTaxa)).GetShortDescription();            
            FileSystemManager.EnsureFolderExists(path);            
            speciesObservationGridCsv.WriteDataToFile(absoluteFilePath, data, new CoordinateSystem(CoordinateSystemId.WGS84), taxaName);
            return filename;
        }

        /// <summary>        
        /// Creates an xml excel file with multiple grid species observation counts.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="formatCountAsOccurrence">if set to <c>true</c> the result cells will be set to 1 if there are any observations; otherwise 0.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xml excel file.</returns>
        public static TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXml GetTaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcelXml(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
        {
            var file = new TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXml(currentUser, coordinateSystem, formatCountAsOccurrence, addSettings, addProvenance);
            return file;
        }

        /// <summary>        
        /// Creates an xlsx excel file with multiple grid species observation counts.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="formatCountAsOccurrence">if set to <c>true</c> the result cells will be set to 1 if there are any observations; otherwise 0.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>The xlsx excel file.</returns>
        public static TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXlsx GetTaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcelXlsx(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
        {
            var file = new TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXlsx(currentUser, coordinateSystem, formatCountAsOccurrence, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Generates a CSV file with grid based counts of number of species observations for each selected taxon.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the observation count will be written as 1 if the count &gt; 0 and 0 if count = 0;        
        /// If set to <c>false</c> the observation count will be written.
        /// </param>
        /// <returns>
        /// A CSV file.
        /// </returns>
        public static TaxonSpecificSpeciesObservationCountGridCsv GetTaxonSpecificGridStatisticsOnSpeciesObservationCountsAsCsv(TaxonSpecificGridSpeciesObservationCountResult data, bool formatCountAsOccurrence)
        {
            TaxonSpecificSpeciesObservationCountGridCsv file = new TaxonSpecificSpeciesObservationCountGridCsv(data, formatCountAsOccurrence);
            return file;
        }

        /// <summary>
        /// Generates an Excel (xml) file with species observations count per polygon and for each selected taxon.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the observation count will be written as 1 if the count &gt; 0 and 0 if count = 0;        
        /// If set to <c>false</c> the observation count will be written.
        /// </param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>
        /// A Excel file of the type xml.
        /// </returns>
        public static TaxonSpecificSpeciesObservationCountPerPolygonExcelXml GetTaxonSpecificSpeciesObservationCountPerPolygonAsExcelXml(IUserContext currentUser, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
        {
            TaxonSpecificSpeciesObservationCountPerPolygonExcelXml file = new TaxonSpecificSpeciesObservationCountPerPolygonExcelXml(currentUser, formatCountAsOccurrence, addSettings, addProvenance);
            return file;
        }

        /// <summary>
        /// Generates an Excel (xlsx) file with species observations count per polygon and for each selected taxon.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the observation count will be written as 1 if the count > 0 and 0 if count = 0;        
        /// If set to <c>false</c> the observation count will be written.
        /// </param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <returns>
        /// A Excel file of the type xlsx.
        /// </returns>
        public static TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx GetTaxonSpecificSpeciesObservationCountPerPolygonAsExcelXlsx(IUserContext currentUser, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
        {
            TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx file = new TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx(currentUser, formatCountAsOccurrence, addSettings, addProvenance);
            return file;
        }

        public static SpeciesAOOEOOExcelXml GetSpeciesAOOEOOAsExcelXml(IUserContext currentUser, Tuple<int, string, string, string, string>[] data, bool addSettings, bool addProvenance)
        {
            return new SpeciesAOOEOOExcelXml(currentUser, data, addSettings, addProvenance, new AooEooExcelFormatter());
        }

        public static SpeciesAOOEOOExcelXlsx GetSpeciesAOOEOOAsExcelXlsx(IUserContext currentUser, Tuple<int, string, string, string, string>[] data, bool addSettings, bool addProvenance)
        {
            return new SpeciesAOOEOOExcelXlsx(currentUser, data, addSettings, addProvenance, new AooEooExcelFormatter());
        }        
    }
}