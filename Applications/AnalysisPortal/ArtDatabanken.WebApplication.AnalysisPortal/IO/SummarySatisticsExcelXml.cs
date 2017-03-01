using System.Collections.Generic;
using System.Text;
using Resources;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of summary statistics.
    /// </summary>
    public class SummaryStatisticsExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with summary statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public SummaryStatisticsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new SummaryStatisticsResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetSummaryStatisticsFromCacheIfAvailableOrElseCalculate();
            _xmlBuilder = new StringBuilder();

            //Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            //Specify column and row counts
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(2, data.Count));

            //Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            
            //Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.ResultViewSummaryStatisticsSpeciesObservationTableColumnCalculationHeader));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.ResultViewSummaryStatisticsSpeciesObservationTableColumnCountHeader));
             _xmlBuilder.AppendLine(base.GetRowEnd());

            //Data values
             foreach (KeyValuePair<string, string> row in data)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Key));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.Value));
                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

             // Add final section of the xml document.
             _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
