using System;
using System.Text;
using Resources;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of grid statistics on species observation counts.
    /// </summary>
    public class GridStatisticsOnSpeciesObservationCountExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with grid statistics on species observation counts.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="coordinateSystem">Coordinate system of the transformated coordinates.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public GridStatisticsOnSpeciesObservationCountExcelXml(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new SpeciesObservationGridCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetSpeciesObservationGridResultFromCacheIfAvailableOrElseCalculate();

            _xmlBuilder = new StringBuilder();

            //Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            //Specify column and row counts
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(7, data.Cells.Count));

            //Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));

            //Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Id"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.GridStatisticsObservationCount));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate X ({0})", data.GridCellCoordinateSystem)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate Y ({0})", data.GridCellCoordinateSystem)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate X ({0})", coordinateSystem.ToString())));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate Y ({0})", coordinateSystem.ToString())));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Grid cell width (metres)"));
            _xmlBuilder.AppendLine(base.GetRowEnd());

            //Data values
            foreach (SpeciesObservationGridCellResult row in data.Cells)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Identifier));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.ObservationCount.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.OriginalCentreCoordinateX.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.OriginalCentreCoordinateY.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.CentreCoordinateX.ToString(base.GetApprotiateGlobalization())));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.CentreCoordinateY.ToString(base.GetApprotiateGlobalization())));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", data.GridCellSize.ToString()));
                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
