using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa
{
    /// <summary>
    /// View model for ~/Filter/SetTaxonFilterFromFactorDialog.
    /// </summary>
    public class TaxaFilterFromFactorViewModel
    {
        /// <summary>
        /// The factor.
        /// </summary>
        public IFactor Factor { get; set; }
    }
}
