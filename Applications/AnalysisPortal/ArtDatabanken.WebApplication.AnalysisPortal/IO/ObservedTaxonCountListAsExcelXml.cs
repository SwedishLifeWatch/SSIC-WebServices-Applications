using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloading a list 
    /// of observed taxa and taxa count as Excel (xml).
    /// </summary>
    public class ObservedTaxonCountListAsExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with summary statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public ObservedTaxonCountListAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new SpeciesObservationTaxonSpeciesObservationCountTableResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetResultFromCacheIfAvailableOrElseCalculate();

            _xmlBuilder = new StringBuilder();

            // Add file definitions and basic format settings
            _xmlBuilder.AppendLine(GetInitialSection());

            // Specify column and row counts
            _xmlBuilder.AppendLine(GetColumnInitialSection(8, data.Count));

            // Specify column widths
            _xmlBuilder.AppendLine(GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(GetColumnWidthLine(60));
            _xmlBuilder.AppendLine(GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(GetColumnWidthLine(90));

            // Add row with column headers
            _xmlBuilder.AppendLine(GetRowStart());
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.LabelTaxon));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.LabelAuthor));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.LabelSwedishName));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.LabelCategory));
            _xmlBuilder.AppendLine(GetColumnNameRowLine("Status"));
            _xmlBuilder.AppendLine(GetColumnNameRowLine("Dyntaxa info"));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.LabelTaxonId));
            _xmlBuilder.AppendLine(GetColumnNameRowLine(Resource.LabelSpeciesObservationCount));
            _xmlBuilder.AppendLine(GetRowEnd());

            // Data values
            foreach (TaxonSpeciesObservationCountViewModel row in data)
            {
                _xmlBuilder.AppendLine(GetRowStart());
                _xmlBuilder.AppendLine(GetDataRowLine("String", row.ScientificName));
                _xmlBuilder.AppendLine(GetDataRowLine("String", row.Author));
                _xmlBuilder.AppendLine(GetDataRowLine("String", row.CommonName));
                _xmlBuilder.AppendLine(GetDataRowLine("String", row.Category));
                _xmlBuilder.AppendLine(GetDataRowLine("String", row.TaxonStatus.ToString()));
                _xmlBuilder.AppendLine(GetDataRowLine("String", @"Http://Dyntaxa.se/Taxon/Info/" + row.TaxonId.ToString()));
                _xmlBuilder.AppendLine(GetDataRowLine("Number", row.TaxonId.ToString()));
                _xmlBuilder.AppendLine(GetDataRowLine("Number", row.SpeciesObservationCount.ToString()));
                _xmlBuilder.AppendLine(GetRowEnd());
            }

            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
            _xmlBuilder.Replace("&", "&amp;");
        }
    }
}
