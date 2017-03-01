using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using Newtonsoft.Json;
using Resources;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// Controller for generating Excel files.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CompressFilter]
    public class DownloadController : BaseController
    {
        /// <summary>
        /// The XML excel file MIME type.
        /// </summary>
        private const string XmlExcelFileMimeType = "application/xml";

        /// <summary>
        /// Excel XLSX file MIME type.
        /// </summary>
        private const string XlsxExcelFileMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// Creates the file name of the XML excel file.
        /// </summary>
        /// <param name="name">The name of the Excel file.</param>
        /// <returns>A valid filename.</returns>
        private string CreateXmlExcelFileName(string name)
        {
            return FilenameGenerator.CreateFilename(name, FileType.ExcelXml);                  
        }

        /// <summary>
        /// Creates the file name of the XLSX excel file.
        /// </summary>
        /// <param name="name">The name of the Excel file.</param>
        /// <returns>A valid filename.</returns>
        private string CreateXlsxExcelFileName(string name)
        {
            return FilenameGenerator.CreateFilename(name, FileType.ExcelXlsx);            
        }

        private bool NoFilterSelected
        {
            get
            {
                return SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0 &&
                         SessionHandler.MySettings.Filter.Spatial.Polygons.Count == 0 &&
                         SessionHandler.MySettings.Filter.Spatial.RegionIds.Count == 0;
            }
        }

        /// <summary>
        /// Gets the coordinate system identifier from argument.
        /// </summary>
        /// <param name="presentationCoordinateSystem">The presentation coordinate system.</param>
        /// <param name="defaultCoordinateSystemId">Default coordinate system id.</param>
        /// <returns>The presentation coordinate system specified as argument if it's valid; otherwise the default coordinate system id.</returns>
        private CoordinateSystemId GetCoordinateSystemIdFromArgument(int? presentationCoordinateSystem, CoordinateSystemId defaultCoordinateSystemId)
        {
            CoordinateSystemId coordinateSystemId;

            if (presentationCoordinateSystem.HasValue)
            {
                if (!presentationCoordinateSystem.Value.TryParseEnum(out coordinateSystemId))
                {
                    coordinateSystemId = defaultCoordinateSystemId;
                }                
            }
            else
            {
                coordinateSystemId = defaultCoordinateSystemId;
            }

            return coordinateSystemId;
        }

        /// <summary>
        /// Method that generates an Excel (XML/XLSX) file with species observations.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <param name="presentationCoordinateSystem">Coordinate system to use.</param>
        /// <param name="columnsSet">Which columns set to use.</param>
        /// <param name="columnsHeadersType">Columns headers type to use.</param>
        /// <returns>An Excel file of the type XML or XLSX.</returns>
        public FileResult SpeciesObservationsAsExcel(
            bool? addSettings, 
            bool? addProvenance, 
            int? presentationCoordinateSystem, 
            int? columnsSet,
            int? columnsHeadersType)
        {
            MemoryStream returnStream;
            string fileName;            
            addSettings = addSettings.GetValueOrDefault(true);            
            addProvenance = addProvenance.GetValueOrDefault(false);
            CoordinateSystemId coordinateSystemId = GetCoordinateSystemIdFromArgument(
                presentationCoordinateSystem,
                SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId);
            
            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId =
                GetSpeciesObservationTableColumnsSetIdFromArgument(
                    columnsSet,
                    SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.SpeciesObservationTableColumnsSetId);

            bool useLabelAsColumnHeader = GetUselabelAsColumnHeaderFromArgument(
                columnsHeadersType,
                SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.UseLabelAsColumnHeader);

            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                SpeciesObservationsExcelXlsx file = FileExportManager.GetSpeciesObservationsAsExcelXlsx(
                    GetCurrentUser(), 
                    addSettings.Value, 
                    addProvenance.Value, 
                    coordinateSystemId,
                    speciesObservationTableColumnsSetId,
                    useLabelAsColumnHeader);
                returnStream = file.ToStream();                
                fileName = CreateXlsxExcelFileName("SpeciesObservations");
            }
            else
            {
                SpeciesObservationsExcelXml file = 
                    FileExportManager.GetSpeciesObservationsAsExcelXml(
                        GetCurrentUser(), 
                        addSettings.Value, 
                        addProvenance.Value, 
                        coordinateSystemId,
                        speciesObservationTableColumnsSetId,
                        useLabelAsColumnHeader);

                returnStream = file.ToStream();                
                fileName = CreateXmlExcelFileName("SpeciesObservations");
            }

            SetServerDone();

            return File(returnStream, XlsxExcelFileMimeType, fileName);
        }

        /// <summary>
        /// Gets the species observation table columns set identifier from argument.
        /// </summary>
        /// <param name="columnsSet">The columns set.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private SpeciesObservationTableColumnsSetId GetSpeciesObservationTableColumnsSetIdFromArgument(
            int? columnsSet, 
            SpeciesObservationTableColumnsSetId defaultValue)
        {
            if (columnsSet.HasValue)
            {
                var systemDefinedTables = SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.SystemDefinedTables;
                var userDefinedTables = SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.UserDefinedTables;
                if (columnsSet.Value < systemDefinedTables.Count) // system defined table
                {
                    return new SpeciesObservationTableColumnsSetId(false, columnsSet.Value);
                }
                else // user defined table
                {
                    int userDefinedTableIndex = columnsSet.Value - systemDefinedTables.Count;
                    if (userDefinedTables != null && userDefinedTables.Count > userDefinedTableIndex)
                    {
                        return new SpeciesObservationTableColumnsSetId(true, userDefinedTableIndex);
                    }
                    else
                    {
                        return defaultValue;
                    }
                }
            }

            return defaultValue;            
        }

        /// <summary>
        /// Gets if label should be used as column header.
        /// </summary>
        /// <param name="columnsHeadersType">Type of the columns headers.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>True if columnsHeadersType=0; otherwise false or default value.</returns>
        private bool GetUselabelAsColumnHeaderFromArgument(int? columnsHeadersType, bool defaultValue)
        {
            if (columnsHeadersType.HasValue)
            {
                if (columnsHeadersType.Value == 0)
                {
                    return true; // Label
                }
                else
                {
                    return false; // DarwinCore
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Method that generates an CSV file with species observations.
        /// </summary>
        /// <param name="presentationCoordinateSystem">Coordinate system to use.</param>
        /// <param name="columnsSet">Which columns set to use.</param>
        /// <param name="columnsHeadersType">Columns headers type to use.</param>
        /// <returns>A CSV file.</returns>
        public FileResult SpeciesObservationsAsCsv(
            int? presentationCoordinateSystem,            
            int? columnsSet,
            int? columnsHeadersType)
        {
            if (NoFilterSelected)
            {
                throw new Exception("Too much data! You must set taxa filter or spatial filter.");
            }
            var coordinateSystemId = GetCoordinateSystemIdFromArgument(
                presentationCoordinateSystem,
                SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId);

            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId =
                GetSpeciesObservationTableColumnsSetIdFromArgument(
                    columnsSet,
                    SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.SpeciesObservationTableColumnsSetId);

            bool useLabelAsColumnHeader = GetUselabelAsColumnHeaderFromArgument(
                columnsHeadersType,
                SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.UseLabelAsColumnHeader);

            SpeciesObservationResultCalculator resultCalculator = new SpeciesObservationResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
            List<Dictionary<ViewTableField, string>> result = 
                resultCalculator.GetTableResult(
                    coordinateSystemId,
                    speciesObservationTableColumnsSetId); 

            SpeciesObservationsCsv file = FileExportManager.GetSpeciesObservationsAsCsv(
                result,                
                useLabelAsColumnHeader);
            MemoryStream returnStream = file.ToStream();

            SetServerDone();
            return File(
                returnStream.ToArray(),
                "text/csv",
                FilenameGenerator.CreateFilename("SpeciesObservations", FileType.Csv));                
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with grid based counts of number of species.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult GridStatisticsOnSpeciesCountsAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                GridStatisticsOnSpeciesCountExcelXlsx file = FileExportManager.GetGridSpeciesCountsAsExcelXlsx(GetCurrentUser(), SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("GridStatisticsOnSpeciesCounts"));
            }
            else
            {
                GridStatisticsOnSpeciesCountExcelXml file = FileExportManager.GetGridSpeciesCountsAsExcelXml(GetCurrentUser(), SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("GridStatisticsOnSpeciesCounts"));
            }
        }

        /// <summary>
        /// Renders the species richness grid map to a png file.                        
        /// </summary>
        /// <returns>A png image.</returns>
        public FileResult SpeciesRichnessGridMapImage()
        {
            SetServerDone();
            try
            {
                FileStreamResult fileStream = CreateImageUsingPhantomJs(Url.Action("SpeciesRichnessGridMap", "Result"), "mainPanel", 1.5);
                return File(
                    fileStream.FileStream,
                    "image/png",
                    FilenameGenerator.CreateFilename("SpeciesRichnessGridMap", FileType.Png));
            }
            catch (Exception e)
            {
                Logger.WriteException(e);
                throw new Exception("An error occurred when generating the image.", e);                
            }
        }

        /// <summary>
        /// Renders the species observation grid map to a png file.
        /// </summary>
        /// <returns>A png image.</returns>
        public ActionResult SpeciesObservationGridMapImage()
        {
            SetServerDone();
            try
            {
                FileStreamResult fileStream = CreateImageUsingPhantomJs(Url.Action("SpeciesObservationGridMap", "Result"), "mainPanel", 1.5);
                return File(
                    fileStream.FileStream,
                    "image/png",
                    FilenameGenerator.CreateFilename("SpeciesObservationGridMap", FileType.Png));                    
            }
            catch (Exception e)
            {
                Logger.WriteException(e);
                throw new Exception("An error occurred when generating the image.", e);
            }
        }

        /// <summary>
        /// Creates a species observation CSV file and stores it on the server in the 
        /// Temp/Export/ folder.
        /// This can be used in workflow scenarios.
        ///         
        /// </summary>
        /// <returns>JSON data with information about the URL to the generated file.</returns>
        public JsonNetResult SpeciesObservationGridAsCsvStoreFileOnServer()
        {
            JsonModel jsonModel;
            SpeciesObservationGridCalculator resultCalculator = null;
            SpeciesObservationGridResult result = null;
            try
            {
                IUserContext currentUser = GetCurrentUser();                
                if (currentUser.IsAuthenticated() && !currentUser.IsCurrentRolePrivatePerson())
                {
                    throw new Exception(Resource.ResultWorkflowCurrentRoleIsNotPrivatePerson);
                }

                resultCalculator = new SpeciesObservationGridCalculator(currentUser, SessionHandler.MySettings);
                result = resultCalculator.GetSpeciesObservationGridResultFromCacheIfAvailableOrElseCalculate();
                string filename = FileExportManager.CreateSpeciesObservationGridAsCsvAndStoreOnServer(currentUser, result);
                string url = FileSystemManager.ExportFolderUrl + filename;
                jsonModel = JsonModel.CreateFromObject(url);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);                                
            }

            return new JsonNetResult(jsonModel);            
        }

        /// <summary>
        /// Generates an Excel (xml) file with grid based occurrence for each selected taxon.
        /// </summary>        
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>
        /// An Excel file of the type xml.
        /// </returns>
        public FileResult TaxonSpecificGridOccurrenceAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            return TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcel(
                "TaxonSpecificGridOccurrence",
                true,
                addSettings,
                addProvenance);
        }

        /// <summary>
        /// Generates an Excel (xml) file with grid based counts of number of species observations for each selected taxon.
        /// </summary> 
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>       
        /// <returns>
        /// An Excel file of the type xml.
        /// </returns>
        public FileResult TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            return TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcel(
                "TaxonSpecificGridStatisticsOnSpeciesObservationCounts",
                false,
                addSettings, 
                addProvenance);
        }

        /// <summary>
        /// Generates an Excel (xml/xlsx) file with grid based counts of number of species observations for each selected taxon.
        /// </summary>
        /// <param name="filename">
        /// The filename (without file extension).
        /// </param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the observation count will be written as 1 if the count > 0 and 0 if count = 0;        
        /// If set to <c>false</c> the observation count will be written.
        /// </param>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>
        /// A Excel file of the type xml or xlsx.
        /// </returns>
        private FileResult TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcel(string filename, bool formatCountAsOccurrence, bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXlsx file = 
                    FileExportManager.GetTaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcelXlsx(
                        GetCurrentUser(), 
                        SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId, 
                        formatCountAsOccurrence, 
                        addSettings, 
                        addProvenance);

                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName(filename));
            }
            else
            {
                TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXml file = FileExportManager.GetTaxonSpecificGridStatisticsOnSpeciesObservationCountsAsExcelXml(GetCurrentUser(), SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId, formatCountAsOccurrence, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName(filename));
            }
        }

        /// <summary>
        /// Generates a CSV file with grid based occurrence for each selected taxon.
        /// </summary>        
        /// <returns>
        /// A CSV file.
        /// </returns>
        public FileResult TaxonSpecificGridOccurrenceAsCsv()
        {
            return TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsCsv(
                "TaxonSpecificGridOccurrence",
                true);
        }

        /// <summary>
        /// Generates a CSV file with grid based counts of number of species observations for each selected taxon.
        /// </summary>        
        /// <returns>
        /// A CSV file.
        /// </returns>
        public FileResult TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsCsv()
        {
            return TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsCsv(
                "TaxonSpecificGridStatisticsOnSpeciesObservationCounts",
                false);
        }

        /// <summary>
        /// Generates a CSV file with grid based counts of number of species observations for each selected taxon.
        /// </summary>
        /// <param name="filename">
        /// The filename (without file extension).
        /// </param>
        /// <param name="convertCountToOccurrenceBoolean">
        /// If set to <c>true</c> the observation count will be written as 1 if the count > 0 and 0 if count = 0;        
        /// If set to <c>false</c> the observation count will be written.
        /// </param>
        /// <returns>
        /// A CSV file.
        /// </returns>
        private FileResult TaxonSpecificGridStatisticsOnSpeciesObservationCountsAsCsv(string filename, bool convertCountToOccurrenceBoolean)
        {
            SpeciesObservationGridCalculator resultCalculator = new SpeciesObservationGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
            TaxonSpecificGridSpeciesObservationCountResult result = resultCalculator.CalculateMultipleSpeciesObservationGrid();
            TaxonSpecificSpeciesObservationCountGridCsv file = FileExportManager.GetTaxonSpecificGridStatisticsOnSpeciesObservationCountsAsCsv(result, convertCountToOccurrenceBoolean);
            MemoryStream returnStream = file.ToStream();
            SetServerDone();
            return File(
                returnStream.ToArray(),
                "text/csv",
                FilenameGenerator.CreateFilename(filename, FileType.Csv));
        }

        /// <summary>
        /// Method that generates an Excel (xml) file with grid based counts of number of species observations.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml.</returns>
        public FileResult GridStatisticsOnSpeciesObservationCountsAsExcel(bool addSettings = true, bool addProvenance = true)
        {            
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                GridStatisticsOnSpeciesObservationCountExcelXlsx file = FileExportManager.GetGridSpeciesObservationCountsAsExcelXlsx(GetCurrentUser(), SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("GridStatisticsOnSpeciesObservationCounts"));
            }
            else
            {
                GridStatisticsOnSpeciesObservationCountExcelXml file = FileExportManager.GetGridSpeciesObservationCountsAsExcelXml(GetCurrentUser(), SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("GridStatisticsOnSpeciesObservationCounts"));
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with 
        /// statistics on total number of species observations 
        /// currently available from data providers associated with Swedish LifeWatch.
        /// </summary>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult DataProviderStatisticsAsExcel()
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                DataProviderListExcelXlsx file = FileExportManager.GetDataProvidersAsExcelXlsx(GetCurrentUser());
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("DataProviders"));
            }
            else
            {
                DataProviderListExcelXml file = FileExportManager.GetDataProvidersAsExcelXml(GetCurrentUser());
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("DataProviders"));
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with summary statistics.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult SummaryStatisticsAsExcel(bool addSettings = true, bool addProvenance = true)
        {            
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                SummaryStatisticsExcelXlsx file = FileExportManager.GetSummaryStatisticsAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("SummaryStatistics"));
            }
            else
            {
                SummaryStatisticsExcelXml file = FileExportManager.GetSummaryStatisticsAsExcelXml(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("SummaryStatistics"));
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with summary statistics per polygon.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult SummaryStatisticsPerPolygonAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();

            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                SummaryStatisticsPerPolygonExcelXlsx file = FileExportManager.GetSummaryStatisticsPerPolygonAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("SummaryStatisticsPerPolygon"));
            }
            else
            {
                SummaryStatisticsPerPolygonExcelXml file = FileExportManager.GetSummaryStatisticsPerPolygonAsExcelXml(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();

                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("SummaryStatisticsPerPolygon"));
            }
        }

        /// <summary>
        /// Generates an Excel (xml) file with species observations count per polygon and for each selected taxon.
        /// </summary>      
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>          
        /// <returns>
        /// A Excel file of the type xml.
        /// </returns>
        public FileResult TaxonSpecificSpeciesObservationCountPerPolygonAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            return TaxonSpecificSpeciesObservationCountPerPolygonAsExcel(
                "TaxonSpecificSpeciesObservationCountPerPolygon",
                false,
                addSettings,
                addProvenance);
        }

        /// <summary>
        /// Generates an Excel (xml) file with occurrence per polygon and for each selected taxon.
        /// </summary>      
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>          
        /// <returns>
        /// A Excel file of the type xml.
        /// </returns>
        public FileResult TaxonSpecificOccurrencePerPolygonAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            return TaxonSpecificSpeciesObservationCountPerPolygonAsExcel(
                "TaxonSpecificOccurrencePerPolygon",
                true,
                addSettings,
                addProvenance);
        }

        /// <summary>
        /// Generates an Excel (xml/xlsx) file with species observations count per polygon and for each selected taxon.
        /// </summary>
        /// <param name="filename">
        /// The filename (without file extension).
        /// </param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the observation count will be written as 1 if the count > 0 and 0 if count = 0;        
        /// If set to <c>false</c> the observation count will be written.
        /// </param>      
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>
        /// A Excel file of the type xml or xlsx.
        /// </returns>
        private FileResult TaxonSpecificSpeciesObservationCountPerPolygonAsExcel(string filename, bool formatCountAsOccurrence, bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx file = FileExportManager.GetTaxonSpecificSpeciesObservationCountPerPolygonAsExcelXlsx(GetCurrentUser(), formatCountAsOccurrence, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName(filename));
            }
            else
            {
                TaxonSpecificSpeciesObservationCountPerPolygonExcelXml file = FileExportManager.GetTaxonSpecificSpeciesObservationCountPerPolygonAsExcelXml(GetCurrentUser(), formatCountAsOccurrence, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName(filename));
            }
        }
        
        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with combined grid statistics.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult CombinedGridStatisticsAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();

            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                CombinedGridStatisticsExcelXlsx file = FileExportManager.GetCombinedGridStatisticsAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("CombinedGridStatistics"));
            }
            else
            {
                CombinedGridStatisticsExcelXml file = FileExportManager.GetCombinedGridStatisticsAsExcelXml(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("CombinedGridStatistics"));
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with observed taxon list.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult ObservedTaxonListAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                ObservedTaxonListAsExcelXlsx file = FileExportManager.GetObservedTaxonListAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("ObservedTaxa"));
            }
            else
            {
                ObservedTaxonListAsExcelXml file = FileExportManager.GetObservedTaxonListAsExcelXml(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("ObservedTaxa"));            
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with observed taxon list and taxon count.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult SpeciesObservationCountPerTaxonAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                ObservedTaxonCountListAsExcelXlsx file = FileExportManager.GetObservedTaxonCountListAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("SpeciesObservationCountPerTaxon"));
            }
            else
            {
                ObservedTaxonCountListAsExcelXml file = FileExportManager.GetObservedTaxonCountListAsExcelXml(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("SpeciesObservationCountPerTaxon"));
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with a time series of species observation counts.
        /// </summary>
        /// <param name="periodicity">A string value representing the alternative periodicities of the time series.</param>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult TimeSeriesOnSpeciesObservationCountsAsExcel(String periodicity, bool addSettings = true, bool addProvenance = true)
        {
            Periodicity periodicityEnumValue = (Periodicity)SessionHandler.MySettings.Calculation.TimeSeries.DefaultPeriodicityIndex;
           
            if (!string.IsNullOrEmpty(periodicity))
            {                
                switch (periodicity.ToLower())
                {
                    case "dayoftheyear":
                        periodicityEnumValue = Periodicity.DayOfTheYear;
                        break;
                    case "weekoftheyear":
                        periodicityEnumValue = Periodicity.WeekOfTheYear;
                        break;
                    case "monthoftheyear":
                        periodicityEnumValue = Periodicity.MonthOfTheYear;
                        break;
                    case "daily":
                        periodicityEnumValue = Periodicity.Daily;
                        break;
                    case "weekly":
                        periodicityEnumValue = Periodicity.Weekly;
                        break;
                    case "monthly":
                        periodicityEnumValue = Periodicity.Monthly;
                        break;
                    default:
                        periodicityEnumValue = Periodicity.Yearly;
                        break;
                }
            }

            SetServerDone();

            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                TimeSeriesOnSpeciesObservationCountsExcelXlsx file = 
                    FileExportManager.GetTimeSeriesOnSpeciesObservationCountsAsExcelXlsx(
                        GetCurrentUser(), 
                        periodicityEnumValue, 
                        addSettings, 
                        addProvenance);

                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("TimeSeriesOnSpeciesObservationCounts"));
            }
            else
            {
                TimeSeriesOnSpeciesObservationCountsExcelXml file = FileExportManager.GetTimeSeriesOnSpeciesObservationCountsAsExcelXml(GetCurrentUser(), periodicityEnumValue, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("TimeSeriesOnSpeciesObservationCounts"));
            }
        }

        /// <summary>
        /// Generates an Excel (xml/xlsx) file with a time series of species observation abundance index for all selected taxa.
        /// </summary>
        /// <param name="periodicity">A string value representing the alternative periodicities of the time series.</param>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult TimeSeriesOnSpeciesObservationAbundanceIndexAsExcel(String periodicity, bool addSettings = true, bool addProvenance = true)
        {
            Periodicity periodicityEnumValue = (Periodicity)SessionHandler.MySettings.Calculation.TimeSeries.DefaultPeriodicityIndex;

            if (!string.IsNullOrEmpty(periodicity))
            {
                switch (periodicity.ToLower())
                {
                    case "dayoftheyear":
                        periodicityEnumValue = Periodicity.DayOfTheYear;
                        break;
                    case "weekoftheyear":
                        periodicityEnumValue = Periodicity.WeekOfTheYear;
                        break;
                    case "monthoftheyear":
                        periodicityEnumValue = Periodicity.MonthOfTheYear;
                        break;
                    case "daily":
                        periodicityEnumValue = Periodicity.Daily;
                        break;
                    case "weekly":
                        periodicityEnumValue = Periodicity.Weekly;
                        break;
                    case "monthly":
                        periodicityEnumValue = Periodicity.Monthly;
                        break;
                    default:
                        periodicityEnumValue = Periodicity.Yearly;
                        break;
                }
            }

            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                TimeSeriesOnSpeciesObservationAbundanceIndexExcelXlsx file = FileExportManager.GetTimeSeriesOnSpeciesObservationAbundanceIndexAsExcelXlsx(GetCurrentUser(), periodicityEnumValue, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("SpeciesObservationAbundanceIndex"));
            }
            else
            {
                TimeSeriesOnSpeciesObservationAbundanceIndexExcelXml file = FileExportManager.GetTimeSeriesOnSpeciesObservationAbundanceIndexAsExcelXml(GetCurrentUser(), periodicityEnumValue, addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("SpeciesObservationAbundanceIndex"));
            }
        }

        /// <summary>
        /// Method that generates an Excel (xml) file with the current selected settings.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml.</returns>
        public FileResult SettingsReportAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                var file = FileExportManager.GetSettingsReportAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);

                return File(file.ToStream(), XlsxExcelFileMimeType, CreateXlsxExcelFileName("SettingsReport"));
            }
            else
            {
                var file = FileExportManager.GetSettingsReportAsExcelXml(GetCurrentUser(), addSettings, addProvenance);

                return File(file.ToStream(), XmlExcelFileMimeType, CreateXmlExcelFileName("SettingsReport"));
            }   
        }

        /// <summary>
        /// Method that generates an Excel (xml/xlsx) file with provenances.
        /// </summary>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <returns>An Excel file of the type xml or xlsx.</returns>
        public FileResult SpeciesObservationProvenancesAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            SetServerDone();
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                SpeciesObservationProvenanceExcelXlsx file = FileExportManager.GetSpeciesObservationProvenanceAsExcelXlsx(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XlsxExcelFileMimeType, CreateXlsxExcelFileName("SpeciesObservationProvenances"));
            }
            else
            {
                SpeciesObservationProvenanceExcelXml file = FileExportManager.GetSpeciesObservationProvenanceAsExcelXml(GetCurrentUser(), addSettings, addProvenance);
                MemoryStream returnStream = file.ToStream();
                return File(returnStream, XmlExcelFileMimeType, CreateXmlExcelFileName("SpeciesObservationProvenances"));
            }
        }

        /// <summary>
        /// Generates an GeoJson (.geojson) file with grid statistics on species observation counts.
        /// </summary>
        /// <returns>A .geojson file.</returns>
        public FileResult GridStatisticsOnSpeciesObservationCountsAsGeoJson()
        {
            SpeciesObservationGridCalculator resultCalculator = null;
            string geojson = null;
            CalculatedDataItem<FeatureCollection> result;            
            resultCalculator = new SpeciesObservationGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
            geojson = resultCalculator.GetSpeciesObservationGridAsGeoJson();
            SetServerDone();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(geojson);
            return File(
                bytes,
                "application/json",
                FilenameGenerator.CreateFilename("SpeciesObservationCountsGrid", FileType.GeoJSON));                
        }

        /// <summary>
        /// Generates an GeoJson (.geojson) file with grid statistics on species richness.
        /// </summary>
        /// <returns>A .geojson file.</returns>
        public FileResult GridStatisticsOnSpeciesRichnessAsGeoJson()
        {
            TaxonGridCalculator resultCalculator = null;
            string geojson = null;
            resultCalculator = new TaxonGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
            geojson = resultCalculator.GetTaxonGridAsGeoJson();
            SetServerDone();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(geojson);
            return File(
                bytes,
                "application/json",
                FilenameGenerator.CreateFilename("SpeciesRichnessGrid", FileType.GeoJSON));                
        }

        /// <summary>
        /// Generates an GeoJson (.geojson) file with the spatial filter.
        /// </summary>
        /// <returns>A .geojson file.</returns>
        public FileResult SpatialFilterAsGeoJson()
        {
            FeatureCollection featureCollection;
            try
            {
                SpatialFilterViewManager viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                featureCollection = viewManager.GetSpatialFilterAsFeatureCollection();
                featureCollection.CRS = new NamedCRS(SessionHandler.MySettings.Presentation.Map.PresentationCoordinateSystemId.EpsgCode());
            }
            catch (Exception)
            {                
                throw;
            }

            SetServerDone();
            string geojson = JsonConvert.SerializeObject(featureCollection, JsonHelper.GetDefaultJsonSerializerSettings());
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(geojson);
            return File(
                bytes,
                "application/json",
                FilenameGenerator.CreateFilename("SpatialFilter", FileType.GeoJSON));
        }

        public FileResult GetAOOEOOForTaxonAsExcel(bool addSettings = true, bool addProvenance = true)
        {
            if (!SessionHandler.MySettings.Filter.Taxa.HasSettings)
            {
                return null;
            }

            string layerName;
            var parameters = new Dictionary<string, object>()
            {
                { "alphaValue", 0 },
                { "useCenterPoint", false }
            };

            //Save current taxa filter
            var tmpTaxonIds = SessionHandler.MySettings.Filter.Taxa.TaxonIds.AsEnumerable().ToArray();

            var data = new Tuple<int, string, string, string, string>[tmpTaxonIds.Length];
            var i = 0;
            foreach (var taxonId in tmpTaxonIds)
            {
                //Get AOO and EOO for one taxon at the time
                SessionHandler.MySettings.Filter.Taxa.ResetSettings();
                SessionHandler.MySettings.Filter.Taxa.AddTaxonId(taxonId);
                var taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
                var scientificName = taxon.ScientificName;
                var commonName = taxon.CommonName;

                var geoJson = MapLayerManager.GetLayerGeojson(GetCurrentUser(), MapLayerManager.EooConcaveHullLayerId, SessionHandler.MySettings.Presentation.Map.DisplayCoordinateSystem.Id, parameters, out layerName, null);
                if (geoJson != null)
                {
                    var featureCollection = JsonConvert.DeserializeObject(geoJson, typeof(FeatureCollection)) as FeatureCollection;

                    var aoo = (string)featureCollection.Features[0].Properties["AOO"];
                    var eoo = (string)featureCollection.Features[0].Properties["EOO"];

                    data[i] = new Tuple<int, string, string, string, string>(
                        taxon.Id, scientificName, commonName, aoo, eoo);
                }
                else
                {
                    data[i] = new Tuple<int, string, string, string, string>(
                        taxon.Id, scientificName, commonName, "0", "0");
                }
                i++;
            }

            //Restore taxon filter
            SessionHandler.MySettings.Filter.Taxa.AddTaxonIds(tmpTaxonIds);

            MemoryStream returnStream;
            string fileName;
            if (SessionHandler.MySettings.Presentation.FileFormat.ExcelFileSettings.IsSettingsDefault())
            {
                var file = FileExportManager.GetSpeciesAOOEOOAsExcelXlsx(GetCurrentUser(), data, addSettings, addProvenance);
                returnStream = file.ToStream();
                fileName = CreateXlsxExcelFileName("TaxonAOOEOO");
            }
            else
            {
                var file = FileExportManager.GetSpeciesAOOEOOAsExcelXml(GetCurrentUser(), data, addSettings, addProvenance);
                returnStream = file.ToStream();
                fileName = CreateXmlExcelFileName("TaxonAOOEOO");
            }

            SetServerDone();

            return File(returnStream, XlsxExcelFileMimeType, fileName);
        }

        #region Ajax
        [HttpGet]
        public JsonResult IsFilterSelected()
        {
            return Json(!NoFilterSelected, JsonRequestBehavior.AllowGet);
        }
        #endregion Ajax
    }
}