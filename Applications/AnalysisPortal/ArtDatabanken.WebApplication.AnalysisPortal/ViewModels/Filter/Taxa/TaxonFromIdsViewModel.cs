using System.Collections.Generic;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Localization;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa
{
    /// <summary>
    /// This class is a view model for Filter/TaxonFromIds
    /// </summary>
    public class TaxonFromIdsViewModel
    {
        /// <summary>
        /// String pasted into textarea
        /// </summary>
        [LocalizedDisplayName("SharedClipboard", NameResourceType = typeof(Resources.Resource))]
        [AllowHtml]
        public string ClipBoard { get; set; }

        public List<TaxonViewModel> Taxa { get; set; }

        /// <summary>
        /// Type of the Column delimiter
        /// </summary>
        [LocalizedDisplayName("SharedRowDelimeter", NameResourceType = typeof(Resources.Resource))]
        public RowDelimiter RowDelimiter { get; set; }

        public SelectList RowDelimiterSelectList { get; set; }

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
            public string TitleLabel { get { return Resources.Resource.FilterTaxonFromIdsTitle; } }
            public string SearchLabel { get { return Resources.Resource.FilterTaxonFromIdsMatch; } }
            public string SearchLabelTooltip { get { return Resources.Resource.FilterTaxonFromIdsMatchTooltip; } }
            public string AddTaxaButtonTooltip { get { return Resources.Resource.FilterTaxonFromIdsAddSelectedTaxaTooltip; } }
            public string RemoveAllLabel { get { return Resources.Resource.SharedRemoveAll; } }
        }
    }
}
