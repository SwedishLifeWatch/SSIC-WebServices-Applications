using System.Collections.Generic;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultModels;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using System.Globalization;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of time series statistics on species observation abundance index.
    /// </summary>
    public class TimeSeriesOnSpeciesObservationAbundanceIndexExcelXml : ExcelXmlBase
    {
        private int GetNumberOfDataRows(Dictionary<TaxonViewModel, List<AbundanceIndexData>> data)
        {
            return data.Sum(pair => pair.Value.Count);
        }

        /// <summary>
        /// Constructor of an excel xml file with time series statistics on species observation abundance index.
        /// </summary>
        /// <param name="data">Time series data.</param>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="periodicity">The periodicity.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TimeSeriesOnSpeciesObservationAbundanceIndexExcelXml(IUserContext currentUser, Periodicity periodicity, bool addSettings, bool addProvenance)
            : base()
        {
            var calculator = new SpeciesObservationAbundanceIndexDiagramResultCalculator(currentUser, SessionHandler.MySettings);
            var data = calculator.GetAbundanceIndexDataResults((int)periodicity);

            _xmlBuilder = new StringBuilder();

            //Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            //Specify column and row counts
            int nrDataRows = GetNumberOfDataRows(data);
            int nrDataColumns = 6;            
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(nrDataColumns, nrDataRows));

            //Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(100));

            //Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("TaxonId"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Taxon name"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Time step"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Abundance index"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Count"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Total count"));            
            _xmlBuilder.AppendLine(base.GetRowEnd());

            //Data values
            foreach (KeyValuePair<TaxonViewModel, List<AbundanceIndexData>> entry in data)
            {
                foreach (AbundanceIndexData abundanceIndexData in entry.Value)
                {
                    _xmlBuilder.AppendLine(base.GetRowStart());
                    _xmlBuilder.AppendLine(base.GetDataRowLine("Number", entry.Key.TaxonId.ToString(CultureInfo.InvariantCulture)));
                    _xmlBuilder.AppendLine(base.GetDataRowLine("String", entry.Key.FullName));
                    _xmlBuilder.AppendLine(base.GetDataRowLine("String", abundanceIndexData.TimeStep));
                    if (abundanceIndexData.AbundanceIndex.HasValue)
                    {
                        _xmlBuilder.AppendLine(base.GetDataRowLine("Number", abundanceIndexData.AbundanceIndex.Value.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        _xmlBuilder.AppendLine(base.GetDataRowLine("String", ""));
                    }
                    _xmlBuilder.AppendLine(base.GetDataRowLine("Number", abundanceIndexData.Count.ToString(CultureInfo.InvariantCulture)));
                    _xmlBuilder.AppendLine(base.GetDataRowLine("Number", abundanceIndexData.TotalCount.ToString(CultureInfo.InvariantCulture)));

                    _xmlBuilder.AppendLine(base.GetRowEnd());
                }
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
