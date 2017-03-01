using System;
using System.Collections.Generic;
using System.Text;
using Resources;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of time series statistics on species observation counts.
    /// </summary>
    public class TimeSeriesOnSpeciesObservationCountsExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with time series statistics on species observation counts.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="periodicity">Time step enum.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TimeSeriesOnSpeciesObservationCountsExcelXml(IUserContext currentUser, Periodicity periodicity, bool addSettings, bool addProvenance)
            : base()
        {
            var calculator = new SpeciesObservationDiagramResultCalculator(currentUser, SessionHandler.MySettings);
            var data = calculator.GetDiagramResult((int)periodicity);

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
            String columnHeader = "";
            switch (periodicity)
            {
                case Periodicity.Yearly:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityYearlyLabel;
                    break;
                case Periodicity.Monthly:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityMonthlyLabel;
                    break;
                case Periodicity.Weekly:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityWeeklyLabel;
                    break;
                case Periodicity.Daily:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityDailyLabel;
                    break;
                case Periodicity.MonthOfTheYear:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityMonthOfTheYearLabel;
                    break;
                case Periodicity.WeekOfTheYear:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityWeekOfTheYearLabel;
                    break;
                case Periodicity.DayOfTheYear:
                    columnHeader = Resource.ResultTimeSeriesPeriodicityDayOfTheYearLabel;
                    break;
            }
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(columnHeader));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.ResultDiagramTimeSeriesNoOfObservationsTitle));
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
