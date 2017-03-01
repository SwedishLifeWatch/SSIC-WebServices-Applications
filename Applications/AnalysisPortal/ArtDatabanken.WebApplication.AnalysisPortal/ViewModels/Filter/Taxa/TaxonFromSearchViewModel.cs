namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa
{
    /// <summary>
    /// This class is a view model for the TaxonFromSearch page
    /// </summary>
    public class TaxonFromSearchViewModel
    {
        public TaxonSearchOptions SearchOptions { get; set; }

        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }

        public bool IsSettingsDefault { get; set; }

        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.Resource.FilterTaxonFromSearchTitle; } }
            public string SearchLabel { get { return Resources.Resource.SharedSearch; } }
            public string AddTaxaButtonTooltip { get { return Resources.Resource.FilterTaxonFromIdsAddSelectedTaxaTooltip; } }
            public string SearchOptionsTooltip { get { return Resources.Resource.TaxonSearchOptionsTooltip; } }
            public string RemoveAllLabel { get { return Resources.Resource.SharedRemoveAll; } }
        }
    }
}
