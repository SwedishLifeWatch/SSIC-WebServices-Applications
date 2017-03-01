using System.Text;
using Resources;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloading a list of observed taxa as Excel (xml).
    /// </summary>
    public class ObservedTaxonListAsExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with summary statistics.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public ObservedTaxonListAsExcelXml(IUserContext currentUser, bool addSettings, bool addProvenance)
            : base()
        {
            var resultCalculator = new SpeciesObservationTaxonTableResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetResultFromCacheIfAvailableOrElseCalculate();

            _xmlBuilder = new StringBuilder();

            // Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            // Specify column and row counts
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(7, data.Count));

            // Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(200));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(150));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(60));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(90));
            
            // Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.LabelTaxon));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.LabelAuthor));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.LabelSwedishName));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.LabelCategory));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Status"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Dyntaxa info"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.LabelTaxonId));
            _xmlBuilder.AppendLine(base.GetRowEnd());

            // Data values
             foreach (TaxonViewModel row in data)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.ScientificName));
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Author));
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.CommonName));
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Category));
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.TaxonStatus.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", @"Http://Dyntaxa.se/Taxon/Info/" + row.TaxonId.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.TaxonId.ToString()));
                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

             // Add final section of the xml document.
             _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
            _xmlBuilder.Replace("&", "&amp;");
        }
    }
}
