using System;
using System.Text;
using Resources;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of grid statistics on species counts.
    /// </summary>
    public class GridStatisticsOnSpeciesCountExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with grid statistics on species counts.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="coordinateSystem">Coordinate system of the transformated coordinates.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public GridStatisticsOnSpeciesCountExcelXml(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new TaxonGridCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetTaxonGridResultFromCacheIfAvailableOrElseCalculate();

            _xmlBuilder = new StringBuilder();

            //Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            //Specify column and row counts
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(8, data.Cells.Count));
            ////_xmlBuilder.AppendLine(base.getColumnInitialSection(10, data.Cells.Count));

            //Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(60));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            ////_xmlBuilder.AppendLine(base.getColumnWidthLine(170));
            ////_xmlBuilder.AppendLine(base.getColumnWidthLine(170));

            //Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Id"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.GridStatisticsTaxaCount));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.GridStatisticsObservationCount));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate X ({0})", data.GridCellCoordinateSystem)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate Y ({0})", data.GridCellCoordinateSystem)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate X ({0})", coordinateSystem.ToString())));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate Y ({0})", coordinateSystem.ToString())));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Grid cell width (metres)"));
            ////_xmlBuilder.AppendLine(base.getColumnNameRowLine("long"));
            ////_xmlBuilder.AppendLine(base.getColumnNameRowLine("lat"));
            _xmlBuilder.AppendLine(base.GetRowEnd());

            //Data values
            foreach (TaxonGridCellResult row in data.Cells)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Identifier));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.SpeciesCount.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.ObservationCount.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.OriginalCentreCoordinateX.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.OriginalCentreCoordinateY.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.CentreCoordinateX.ToString(base.GetApprotiateGlobalization())));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.CentreCoordinateY.ToString(base.GetApprotiateGlobalization())));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", data.GridCellSize.ToString()));
                ////Point convertedPoint = GisTools.CoordinateConversionManager.GetConvertedPoint(
                ////    new Point(row.CentreCoordinateX, row.CentreCoordinateY),
                ////    new CoordinateSystem(CoordinateSystemId.GoogleMercator),
                ////    new CoordinateSystem(CoordinateSystemId.WGS84));
                ////_xmlBuilder.AppendLine(base.getDataRowLine("Number", convertedPoint.X.ToString(CultureInfo.InvariantCulture.NumberFormat)));
                ////_xmlBuilder.AppendLine(base.getDataRowLine("Number", convertedPoint.Y.ToString(CultureInfo.InvariantCulture.NumberFormat)));
                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
