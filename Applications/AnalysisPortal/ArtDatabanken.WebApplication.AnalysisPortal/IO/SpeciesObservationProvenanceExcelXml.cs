using System;
using System.Text;
using System.Linq;
using Resources;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservationProvenanceResult;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observation provenances.
    /// </summary>
    public class SpeciesObservationProvenanceExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with species observation provenances.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        public SpeciesObservationProvenanceExcelXml(IUserContext currentUser, bool createWoorkbook = true)
            : base()
        {
            var resultCalculator = new SpeciesObservationProvenanceResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetSpeciesObservationProvenances();

            var rowsXml = new StringBuilder();

            //Add row with column headers
            rowsXml.AppendLine(base.GetRowStart());
            rowsXml.AppendLine(base.GetColumnNameRowLine(Resource.ResultViewSpeciesObservationProvenanceTableColumnNameHeader));
            rowsXml.AppendLine(base.GetColumnNameRowLine(Resource.ResultViewSpeciesObservationProvenanceTableColumnValueHeader));
            rowsXml.AppendLine(base.GetColumnNameRowLine(Resource.ResultViewSpeciesObservationProvenanceTableColumnSpeciesObservationCountHeader));
            rowsXml.AppendLine(base.GetRowEnd());

            //Data values
            foreach (var row in data)
            {
                rowsXml.AppendLine(base.GetRowStart());
                rowsXml.AppendLine(base.GetDataRowLine("String", row.Name));
                rowsXml.AppendLine(base.GetRowEnd());

                //Data subvalues
                foreach (var details in row.Values)
                {
                    rowsXml.AppendLine(base.GetRowStart());
                    rowsXml.AppendLine(base.GetDataRowLine("String", string.Empty));
                    rowsXml.AppendLine(base.GetDataRowLine("String", details.Value ?? "-", true));
                    rowsXml.AppendLine(base.GetDataRowLine("Number", details.SpeciesObservationCount.ToString()));
                    rowsXml.AppendLine(base.GetRowEnd());
                }
            }

            var onlySheet = !createWoorkbook;
            _xmlBuilder = new StringBuilder();

            // Create initial section or a new worksheet
            _xmlBuilder.AppendLine(GetInitialSectionOrNewWorksheet(ref createWoorkbook, Resource.SpeciesObservationProvenanceReportSheetName));

            var rowCount = data.Select(v => v.Values.Count).Sum() + data.Count;

            // Specify column and row counts
            _xmlBuilder.AppendLine(GetColumnInitialSection(4, rowCount));

            // Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(250));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));

            _xmlBuilder.Append(rowsXml);

            if (onlySheet)
            {
                return;
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection());
            _xmlBuilder.Replace("&", "&amp;");
        }
    }
}
