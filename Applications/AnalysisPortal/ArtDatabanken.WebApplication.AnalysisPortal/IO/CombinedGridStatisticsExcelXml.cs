using System;
using System.Globalization;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of combined grid statistics.
    /// </summary>
    public class CombinedGridStatisticsExcelXml : ExcelXmlBase
    {
         /// <summary>
        /// Constructor of an excel xml file with combined grid statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public CombinedGridStatisticsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
            : base()
        {
            var gridStatisticsSetting = SessionHandler.MySettings.Calculation.GridStatistics;
            var coordinateSystemId = gridStatisticsSetting.CoordinateSystemId.GetValueOrDefault((int)GridCoordinateSystem.SWEREF99_TM);
            var gridSize = gridStatisticsSetting.GridSize.GetValueOrDefault(10000);
            var wfsLayerId = gridStatisticsSetting.WfsGridStatisticsLayerId.GetValueOrDefault(-1);
            if (wfsLayerId < 0)
            {
                throw new Exception("Error when trying to create Excel file. You must select an environmental data layer in Grid statistics settings.");
            }

            var resultCalculator = new CombinedGridStatisticsCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.CalculateCombinedGridResult(coordinateSystemId, gridSize, wfsLayerId);

            _xmlBuilder = new StringBuilder();

            //Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            //Specify column and row counts
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(12, data.Cells.Count));

            //Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(120));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(120));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(120));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(120));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(190));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(190));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(190));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(190));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));

            //Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Id"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.GridStatisticsTaxaCount));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.GridStatisticsObservationCount));

            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{1} ({0})", Resource.GridStatisticsEnvironmentalData, Resource.GridStatisticsCalculationModeCount)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{1} ({0})", Resource.GridStatisticsEnvironmentalData, Resource.GridStatisticsCalculationModeArea)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{1} ({0})", Resource.GridStatisticsEnvironmentalData, Resource.GridStatisticsCalculationModeLength)));

             _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{0} X - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsCalculation, data.CalculationCoordinateSystemName)));
             _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{0} Y - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsCalculation, data.CalculationCoordinateSystemName)));
             _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{0} X - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsDisplay, data.DisplayCoordinateSystemName)));
             _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{0} Y - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsDisplay, data.DisplayCoordinateSystemName)));
            
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.GridStatisticsCellSizeMeters));

            _xmlBuilder.AppendLine(base.GetRowEnd());

            //Data values
            foreach (var row in data.Cells)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Identifier));                                
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.SpeciesCount.ToString(CultureInfo.InvariantCulture)));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.ObservationCount.ToString(CultureInfo.InvariantCulture)));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.FeatureCount.ToString(CultureInfo.InvariantCulture)));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.FeatureArea.ToString(CultureInfo.InvariantCulture)));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.FeatureLength.ToString(CultureInfo.InvariantCulture)));                

                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.OriginalCentreCoordinateX.ToString(CultureInfo.InvariantCulture))); //"Centrum X (SWEREF 99)"
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.OriginalCentreCoordinateY.ToString(CultureInfo.InvariantCulture))); //"Centrum Y (SWEREF 99)"
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.CentreCoordinateX.ToString(CultureInfo.InvariantCulture))); //"Centrum X (Google Mercator)"
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.CentreCoordinateY.ToString(CultureInfo.InvariantCulture))); //"Centrum Y (Google Mercator)"

                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", data.GridCellSize.ToString(CultureInfo.InvariantCulture))); //"Centrum X (Google Mercator)"

                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
