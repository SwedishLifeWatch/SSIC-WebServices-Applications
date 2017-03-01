using System.Collections.Generic;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observations Excel file.
    /// </summary>
    public class SpeciesObservationsExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationsExcelXml"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <param name="coordinateSystemId">The coordinate system.</param>
        /// <param name="speciesObservationTableColumnsSetId">The table columns set to use.</param>
        /// <param name="useLabelAsColumnHeader">Use label as column header.</param>        
        public SpeciesObservationsExcelXml(
            IUserContext currentUser, 
            bool addSettings, 
            bool addProvenance, 
            CoordinateSystemId coordinateSystemId,
            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId,
            bool useLabelAsColumnHeader)
        {
            var resultCalculator = new SpeciesObservationResultCalculator(currentUser, SessionHandler.MySettings);
            var speciesObservations = resultCalculator.GetTableResult(coordinateSystemId, speciesObservationTableColumnsSetId);
            
            _xmlBuilder = new StringBuilder();

            // Add file definitions and basic format settings
            _xmlBuilder.AppendLine(GetInitialSection());

            // Specify column and row counts
            List<string> columns = new List<string>();
            if (speciesObservations.Count > 0)
            {
                foreach (ViewTableField tableField in speciesObservations[0].Keys)
                {
                    if (useLabelAsColumnHeader)
                    {
                        columns.Add(tableField.Title);
                    }
                    else
                    {
                        columns.Add(tableField.DataField);
                    }                    
                }
            }

            _xmlBuilder.AppendLine(GetColumnInitialSection(columns.Count, speciesObservations.Count));

            // Specify column widths
            foreach (string column in columns)
            {
                _xmlBuilder.AppendLine(GetColumnWidthLine(100));    
            }

            // Add row with column headers
            _xmlBuilder.AppendLine(GetRowStart());
            foreach (string column in columns)
            {
                _xmlBuilder.AppendLine(GetColumnNameRowLine(column));
            }            

            _xmlBuilder.AppendLine(GetRowEnd());

            // Data values
            foreach (Dictionary<ViewTableField, string> speciesObservation in speciesObservations)
            {
                _xmlBuilder.AppendLine(GetRowStart());
                foreach (var val in speciesObservation.Values)
                {
                    _xmlBuilder.AppendLine(GetDataRowLine("String", val));
                }

                _xmlBuilder.AppendLine(GetRowEnd());                
            }

            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
