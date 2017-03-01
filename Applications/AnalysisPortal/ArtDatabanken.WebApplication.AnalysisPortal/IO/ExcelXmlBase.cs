using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// Base class for excel exports.
    /// </summary>
    public class ExcelXmlBase
    {
        /// <summary>
        /// Contains the generated xml.
        /// </summary>
        protected StringBuilder _xmlBuilder;

        /// <summary>
        /// The xml string reprensentation of the document.
        /// </summary>
        public string XmlAsText
        {
            get
            {
                return _xmlBuilder == null ? string.Empty : _xmlBuilder.ToString();
            }
        }

        /// <summary>
        /// The xml file content.
        /// </summary>
        public XmlDocument XmlFile
        {
            get
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(_xmlBuilder.ToString());
                return xml;
            }
        }

        /// <summary>
        /// Gets a stream representation of the xml file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            var xmlStream = new MemoryStream();
            XmlFile.Save(xmlStream);
            xmlStream.Position = 0;

            return xmlStream;
        }

        /// <summary>
        /// The appropriate globalization.
        /// </summary>
        private System.Globalization.CultureInfo _appropriateGlobalization = null;
        
        /// <summary>
        /// First part of the xml to be generated.
        /// </summary>
        private string _firstXmlPart = 
@"<?xml version='1.0'?>
<?mso-application progid='Excel.Sheet'?>
<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
 xmlns:o='urn:schemas-microsoft-com:office:office'
 xmlns:x='urn:schemas-microsoft-com:office:excel'
 xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'
 xmlns:html='http://www.w3.org/TR/REC-html40'>
 <DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>
  <Author>oskark</Author>
  <LastAuthor>oskark</LastAuthor>
  <Created>2013-07-24T08:28:15Z</Created>
  <LastSaved>2013-07-24T08:38:40Z</LastSaved>
  <Company>SLU</Company>
  <Version>12.00</Version>
 </DocumentProperties>
 <ExcelWorkbook xmlns='urn:schemas-microsoft-com:office:excel'>
  <WindowHeight>7995</WindowHeight>
  <WindowWidth>20115</WindowWidth>
  <WindowTopX>240</WindowTopX>
  <WindowTopY>45</WindowTopY>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
 <Styles>
  <Style ss:ID='Default' ss:Name='Normal'>
   <Alignment ss:Vertical='Bottom'/>
   <Borders/>
   <Font ss:FontName='Calibri' x:Family='Swiss' ss:Size='11' ss:Color='#000000'/>
   <Interior/>
   <NumberFormat/>
   <Protection/>
  </Style>
  <Style ss:ID='s38' ss:Name='Färg1'>
   <Font ss:FontName='Calibri' x:Family='Swiss' ss:Size='11' ss:Color='#FFFFFF'/>
   <Interior ss:Color='#4F81BD' ss:Pattern='Solid'/>
  </Style>
  <Style ss:ID='s62'>
   <Alignment ss:Vertical='Bottom' ss:WrapText='1'/>
  </Style>
  <Style ss:ID='s64'>
   <Font ss:FontName='Consolas' x:Family='Modern' ss:Size='11' ss:Color='#000000'/>
  </Style>
 </Styles>
 <Worksheet ss:Name='[SheetName]'>";

        /// <summary>
        /// Generates a new worksheet.
        /// </summary>
        private string _newWorksheetXmlPart =
@"<Row ss:AutoFitHeight='0'>
    <Cell ss:StyleID='s64'/>
   </Row>
  </Table>
  <WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'>
   <PageSetup>
    <Header x:Margin='0.3'/>
    <Footer x:Margin='0.3'/>
    <PageMargins x:Bottom='0.75' x:Left='0.7' x:Right='0.7' x:Top='0.75'/>
   </PageSetup>
   <Unsynced/>
   <Print>
    <ValidPrinterInfo/>
    <PaperSizeIndex>9</PaperSizeIndex>
    <HorizontalResolution>600</HorizontalResolution>
    <VerticalResolution>600</VerticalResolution>
   </Print>
   <Selected/>
   <FreezePanes/>
   <FrozenNoSplit/>
   <SplitHorizontal>1</SplitHorizontal>
   <TopRowBottomPane>1</TopRowBottomPane>
   <ActivePane>2</ActivePane>
   <Panes>
    <Pane>
     <Number>3</Number>
    </Pane>
    <Pane>
     <Number>2</Number>
    </Pane>
   </Panes>
   <ProtectObjects>False</ProtectObjects>
   <ProtectScenarios>False</ProtectScenarios>
  </WorksheetOptions>
 </Worksheet>
 <Worksheet ss:Name='[SheetName]'>";

        /// <summary>
        /// Last part of the xml to be generated.
        /// </summary>
        private string _lastXmlPart =
 @"[AdditionalSheets]
   <Row ss:AutoFitHeight='0'>
    <Cell ss:StyleID='s64'/>
   </Row>
  </Table>
  <WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'>
   <PageSetup>
    <Header x:Margin='0.3'/>
    <Footer x:Margin='0.3'/>
    <PageMargins x:Bottom='0.75' x:Left='0.7' x:Right='0.7' x:Top='0.75'/>
   </PageSetup>
   <Unsynced/>
   <Print>
    <ValidPrinterInfo/>
    <PaperSizeIndex>9</PaperSizeIndex>
    <HorizontalResolution>600</HorizontalResolution>
    <VerticalResolution>600</VerticalResolution>
   </Print>
   <Selected/>
   <FreezePanes/>
   <FrozenNoSplit/>
   <SplitHorizontal>1</SplitHorizontal>
   <TopRowBottomPane>1</TopRowBottomPane>
   <ActivePane>2</ActivePane>
   <Panes>
    <Pane>
     <Number>3</Number>
    </Pane>
    <Pane>
     <Number>2</Number>
    </Pane>
   </Panes>
   <ProtectObjects>False</ProtectObjects>
   <ProtectScenarios>False</ProtectScenarios>
  </WorksheetOptions>
 </Worksheet>
</Workbook>";

        /// <summary>
        /// Column section.
        /// </summary>
        private string _columnSection =
            @"<Table ss:ExpandedColumnCount='[ColumnCount]' ss:ExpandedRowCount='[RowCount]' x:FullColumns='1'
   x:FullRows='1' ss:DefaultRowHeight='15'>";

        /// <summary>
        /// Column name row.
        /// </summary>
        private string _columnNameRow = @"<Cell ss:StyleID='s38'><Data ss:Type='String'>[ColumnName]</Data></Cell>";

        /// <summary>
        /// Start of a row.
        /// </summary>
        private string _rowStart = @"<Row[Height]>";

        /// <summary>
        /// End of a row.
        /// </summary>
        private string _rowEnd = @"</Row>";

        /// <summary>
        /// A data row.
        /// </summary>
        private string _dataRow = @"<Cell[WrapText]><Data ss:Type='[DataFormat]'>[DataValue]</Data></Cell>";

        /// <summary>
        /// Gets the initial section of the xml.
        /// </summary>
        /// <param name="sheetName" value="SLW Data">The name of the sheet (can not be longer than 31 characters).</param>
        /// <returns>The initial section.</returns>
        protected string GetInitialSection(string sheetName = "SLW Data")
        {
            return _firstXmlPart
                .Replace("[SheetName]", sheetName.Length > 31 ? sheetName.Substring(0, 31) : sheetName)
                .Replace("'", "\"");
        }

        /// <summary>
        /// Creates and gets a new worksheet.
        /// </summary>
        /// <param name="sheetName" value="SLW Data">The name of the sheet (can not be longer than 31 characters).</param>
        /// <returns>The new worksheet.</returns>
        protected string GetNewWorksheet(string sheetName)
        {
            return _newWorksheetXmlPart
                .Replace("[SheetName]", sheetName.Length > 31 ? sheetName.Substring(0, 31) : sheetName)
                .Replace("'", "\"");
        }

        /// <summary>
        /// Creates the initial section of the xml excel file or a new worksheet.
        /// </summary>
        /// <param name="firstSheet">If true, create the initial section of the xml excel file, else create a new worksheet.</param>
        /// <param name="sheetName">The name of the sheet.</param>
        /// <returns>The initial section or a new worksheet.</returns>
        protected string GetInitialSectionOrNewWorksheet(ref bool firstSheet, string sheetName)
        {
            if (firstSheet)
            {
                // Add file definitions and basic format settings
                firstSheet = false;
                return GetInitialSection(sheetName);
            }

            // Create a new worksheet
            return GetNewWorksheet(sheetName);
        }

        /// <summary>
        /// Gets the initial section of columns.
        /// </summary>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <returns>The initial section of columns.</returns>
        protected string GetColumnInitialSection(int columnCount, int rowCount)
        {
            return _columnSection
                .Replace("'", "\"")
                .Replace("[ColumnCount]", columnCount.ToString())
                .Replace("[RowCount]", (rowCount + 2).ToString());
        }

        /// <summary>
        /// Starts a new row.
        /// </summary>
        /// <param name="wrapText">If the text has to be wrapped.</param>
        /// <param name="rows">If wrapTest is true asjusts the heigh of the row accordingly so to show all the multine rows.</param>
        /// <returns>The new row.</returns>
        protected string GetRowStart(bool wrapText = false, int rows = 1)
        {
           return _rowStart
               .Replace("[Height]", wrapText ? " ss:Height='" + (15 * rows) + "'" : " ss:AutoFitHeight='0'")
               .Replace("'", "\"");
        }

        /// <summary>
        /// Ends a row.
        /// </summary>
        /// <returns>The end of the row.</returns>
        protected string GetRowEnd()
        {
            return _rowEnd;
        }

        /// <summary>
        /// Gets the width of a column.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>The width of a column.</returns>
        protected string GetColumnWidthLine(int width)
        {
            return string.Format("<Column ss:Width=\"{0}\"/>", width);
        }

        /// <summary>
        /// Sets the column name.
        /// </summary>
        /// <param name="columnName">The name for the column.</param>
        /// <returns>The column name.</returns>
        protected string GetColumnNameRowLine(string columnName)
        {
            return _columnNameRow
                .Replace("[ColumnName]", columnName)
                .Replace("'", "\"");
        }

        /// <summary>
        /// Sets the data for a row.
        /// </summary>
        /// <param name="format">The data type.</param>
        /// <param name="value">The data value.</param>
        /// <param name="wrapText">If the text has to be wrapped.</param>
        /// <returns>The row with the data.</returns>
        protected string GetDataRowLine(string format, string value, bool wrapText = false)
        {
            if (format.ToLower() == "string")
            {
                return GetDataRowLineForStringDataType(value, wrapText);
            }            

            return _dataRow
                .Replace("[WrapText]", wrapText ? " ss:StyleID='s62'" : string.Empty)
                .Replace("[DataFormat]", format)
                .Replace("[DataValue]", SecurityElement.Escape(value))
                .Replace("'", "\"");
        }

        private string GetDataRowLineForStringDataType(string value, bool wrapText = false)
        {
            return _dataRow
                .Replace("[WrapText]", wrapText ? " ss:StyleID='s62'" : string.Empty)
                .Replace("[DataFormat]", "String")
                .Replace("[DataValue]", wrapText ? "<![CDATA[" + value + "]]>" : SecurityElement.Escape(value))
                .Replace("'", "\"");  
        }

        /// <summary>
        /// Gets the final section of the xml.
        /// </summary>
        /// <returns>The final section.</returns>
        protected string GetFinalSection(string additionalSheets = "")
        {
            return _lastXmlPart.Replace("[AdditionalSheets]", additionalSheets).Replace("'", "\"");
        }

        /// <summary>
        /// Gets/sets the globalization.
        /// </summary>
        /// <returns>The globalization.</returns>
        protected CultureInfo GetApprotiateGlobalization()
        {
            if (_appropriateGlobalization.IsNull())
            {
                _appropriateGlobalization = CultureInfo.CreateSpecificCulture("en-GB");
            }
            return _appropriateGlobalization;
        }

        /// <summary>
        /// Get aditional sheets
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="addSettings"></param>
        /// <param name="addProvenance"></param>
        /// <param name="addQuotation"></param>
        /// <returns></returns>
        protected string GetAditionalSheets(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            var aditionSheets = string.Empty;
            if (addSettings)
            {
                var settingsSheet = new SettingsReportExcelXml(currentUser, false);
                aditionSheets = settingsSheet.XmlAsText;
            }

            if (addProvenance)
            {
                var speciesObservationProvenanceSheet = new SpeciesObservationProvenanceExcelXml(currentUser, false);
                aditionSheets += speciesObservationProvenanceSheet.XmlAsText;
            }

                var quotationSheet = new QuotationExcelXml(currentUser, false);
                aditionSheets += quotationSheet.XmlAsText;

            return aditionSheets;
        }
    }
}
