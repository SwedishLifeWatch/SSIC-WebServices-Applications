using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observations count per polygon and for each selected taxon.
    /// </summary>
    public class TaxonSpecificSpeciesObservationCountPerPolygonExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with species observations count per polygon and for each selected taxon.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the result cells will be set to 1 if there are any observations; otherwise 0.
        /// </param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TaxonSpecificSpeciesObservationCountPerPolygonExcelXml(IUserContext currentUser, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.CalculateSpeciesObservationCountPerPolygonAndTaxa(SessionHandler.MySettings.Filter.Taxa.TaxonIds.ToList());

            _xmlBuilder = new StringBuilder();
        
            int nrColumns = 1 + data.Taxa.Count;

            // Add file definitions and basic format settings.
            _xmlBuilder.AppendLine(base.GetInitialSection());

            // Specify column and row counts.
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(nrColumns, data.SpeciesObservationCountPerPolygon.Count));

            // Specify column widths.
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));

            for (int i = 0; i < data.Taxa.Count; i++)
            {
                _xmlBuilder.AppendLine(base.GetColumnWidthLine(140));
            }

            // Add row with column headers.
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Polygon"));            
            foreach (TaxonViewModel taxon in data.Taxa)
            {
                _xmlBuilder.AppendLine(base.GetColumnNameRowLine(string.Format("{0} (TaxonId {1})", taxon.ScientificName, taxon.TaxonId)));                
            }            

            _xmlBuilder.AppendLine(base.GetRowEnd());

            // Data values.
            foreach (KeyValuePair<string, Dictionary<int, long>> pair in data.SpeciesObservationCountPerPolygon)
            {
                string[] multiLine = pair.Key.Split(new[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
                _xmlBuilder.AppendLine(GetRowStart(true, multiLine.Length));
                _xmlBuilder.AppendLine(GetDataRowLine("String", pair.Key.Replace("<br />", "\n"), true));                
                foreach (TaxonViewModel taxon in data.Taxa)
                {
                    long speciesObservationCount = 0;
                    pair.Value.TryGetValue(taxon.TaxonId, out speciesObservationCount);
                    
                    if (formatCountAsOccurrence)
                    {
                        int binaryVal = speciesObservationCount > 0 ? 1 : 0;
                        _xmlBuilder.AppendLine(base.GetDataRowLine("Number", binaryVal.ToString(CultureInfo.InvariantCulture)));    
                    }
                    else
                    {
                        _xmlBuilder.AppendLine(base.GetDataRowLine("Number", speciesObservationCount.ToString(CultureInfo.InvariantCulture)));
                    }
                }

                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}
