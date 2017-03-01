using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    using System;

    using Resources;

    /// <summary>
    /// A class that can be used for downloads of summary statistics per polygon.
    /// </summary>
    public class SummaryStatisticsPerPolygonExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with summary statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public SummaryStatisticsPerPolygonExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            var resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetSummaryStatisticsPerPolygonFromCacheIfAvailableOrElseCalculate();

            _xmlBuilder = new StringBuilder();

            // Add file definitions and basic format settings
            _xmlBuilder.AppendLine(GetInitialSection());

            // Specify column and row counts
            _xmlBuilder.AppendLine(GetColumnInitialSection(3, data.Count));

            // Specify column widths
            _xmlBuilder.AppendLine(GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(GetColumnWidthLine(200));

            // Add row with column headers
            _xmlBuilder.AppendLine(GetRowStart());
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.SummaryStatisticsObservationCount));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.SummaryStatisticsTaxaCount));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.SummaryStatisticsPolygon));
            _xmlBuilder.AppendLine(GetRowEnd());

            // Data values
            foreach (SpeciesObservationsCountPerPolygon row in data)
            {
                string[] multiLine = row.Properties.Split(new[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);

                _xmlBuilder.AppendLine(GetRowStart(true, multiLine.Length));
                _xmlBuilder.AppendLine(GetDataRowLine(row.SpeciesObservationsCount == "-" ? "String" : "Number", row.SpeciesObservationsCount));
                _xmlBuilder.AppendLine(GetDataRowLine(row.SpeciesCount == "-" ? "String" : "Number", row.SpeciesCount));
                _xmlBuilder.AppendLine(GetDataRowLine("String", row.Properties.Replace("<br />", "\n"), true));
                _xmlBuilder.AppendLine(GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}