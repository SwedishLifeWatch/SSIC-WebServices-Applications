using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of grid statistics on species observation counts for each selected taxon.
    /// </summary>
    public class TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with multiple grid statistics on species observation counts.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system of the transformated coordinates.
        /// </param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the result cells will be set to 1 if there are any observations; otherwise 0.
        /// </param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXml(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new SpeciesObservationGridCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.CalculateMultipleSpeciesObservationGrid();
            _xmlBuilder = new StringBuilder();
        
            int nrColumns = 6 + data.Taxa.Count;

            // Add file definitions and basic format settings.
            _xmlBuilder.AppendLine(base.GetInitialSection());

            // Specify column and row counts.
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(nrColumns, data.GridCells.Count));

            // Specify column widths.
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(170));

            for (int i = 0; i < data.Taxa.Count; i++)
            {
                _xmlBuilder.AppendLine(base.GetColumnWidthLine(140));
            }

            string gridCoordinateSystemDescription = "";
            if (data.GridCells.Count > 0)
            {
                gridCoordinateSystemDescription = data.GridCells.First().Key.GridCoordinateSystem.ToString();
            }

            // Add row with column headers.
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Id"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate X ({0})", gridCoordinateSystemDescription)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate Y ({0})", gridCoordinateSystemDescription)));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate X ({0})", coordinateSystem.ToString())));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(String.Format("Centre coordinate Y ({0})", coordinateSystem.ToString())));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Grid cell width (metres)"));

            foreach (TaxonViewModel taxon in data.Taxa)
            {
                _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{0} (TaxonId {1})", taxon.ScientificName, taxon.TaxonId)));                
            }            

            _xmlBuilder.AppendLine(base.GetRowEnd());

            // Data values.
            List<IGridCellBase> orderedGridCells = data.GridCells.Keys.OrderBy(x => x.Identifier).ToList();
            foreach (IGridCellBase gridCell in orderedGridCells)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", gridCell.Identifier));                                
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", gridCell.OrginalGridCellCentreCoordinate.X.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", gridCell.OrginalGridCellCentreCoordinate.Y.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", gridCell.GridCellCentreCoordinate.X.ToString(base.GetApprotiateGlobalization())));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", gridCell.GridCellCentreCoordinate.Y.ToString(base.GetApprotiateGlobalization())));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", gridCell.GridCellSize.ToString()));

                foreach (TaxonViewModel taxon in data.Taxa)
                {
                    long nrObservations = 0;
                    if (data.GridCells[gridCell].ContainsKey(taxon.TaxonId))
                    {
                        nrObservations = data.GridCells[gridCell][taxon.TaxonId].ObservationCount;
                    }

                    if (formatCountAsOccurrence)
                    {
                        int binaryVal = nrObservations > 0 ? 1 : 0;
                        _xmlBuilder.AppendLine(base.GetDataRowLine("Number", binaryVal.ToString(CultureInfo.InvariantCulture)));    
                    }
                    else
                    {
                        _xmlBuilder.AppendLine(base.GetDataRowLine("Number", nrObservations.ToString(CultureInfo.InvariantCulture)));                        
                    }
                }

                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
