using System.Collections.Generic;
using System.Web.UI.WebControls;
using ArtDatabanken.Data;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.RedList
{
    /// <summary>
    /// This class is a view model for the RedList page
    /// </summary>
    public class RedListViewModel
    {
        public RedListViewModel()
        {
            Labels = new ModelLabels();

            Categories = new Dictionary<RedListCategory, ListItem>
            {
                { RedListCategory.EX, new ListItem { Text = string.Format("{0} (EX)", Resource.RedListCategoryEX), Value = RedListCategory.EX.ToString() } },
                { RedListCategory.RE, new ListItem { Text = string.Format("{0} (RE)", Resource.RedListCategoryRE), Value = RedListCategory.RE.ToString() } },
                { RedListCategory.CR, new ListItem { Text = string.Format("{0} (CR)", Resource.RedListCategoryCR), Value = RedListCategory.CR.ToString() } },
                { RedListCategory.EN, new ListItem { Text = string.Format("{0} (EN)", Resource.RedListCategoryEN), Value = RedListCategory.EN.ToString() } },
                { RedListCategory.VU, new ListItem { Text = string.Format("{0} (VU)", Resource.RedListCategoryVU), Value = RedListCategory.VU.ToString() } },
                { RedListCategory.NT, new ListItem { Text = string.Format("{0} (NT)", Resource.RedListCategoryNT), Value = RedListCategory.NT.ToString() } },
                { RedListCategory.DD, new ListItem { Text = string.Format("{0} (DD)", Resource.RedListCategoryDD), Value = RedListCategory.DD.ToString() } },
                { RedListCategory.LC, new ListItem { Text = string.Format("{0} (LC)", Resource.RedListCategoryLC), Value = RedListCategory.LC.ToString() } },
                { RedListCategory.NE, new ListItem { Text = string.Format("{0} (NE)", Resource.RedListCategoryNE), Value = RedListCategory.NE.ToString() } },
                { RedListCategory.NA, new ListItem { Text = string.Format("{0} (NA)", Resource.RedListCategoryNA), Value = RedListCategory.NA.ToString() } }
            };
        }

        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels { get; private set; }

        public bool IsSettingsDefault { get; set; }

        public IDictionary<RedListCategory, ListItem> Categories { get; private set; }

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resource.FilterByRedListTitle; } }
            public string SearchLabel { get { return Resource.SharedSearch; } }
            public string AddTaxaButtonTooltip { get { return Resource.FilterTaxonFromIdsAddSelectedTaxaTooltip; } }
            public string GroupRedlistedTaxa { get { return Resource.RedListGroupRedlisted; } }
            public string GroupThreatenedTaxa { get { return Resource.RedListGroupThreatened; } }
        }
    }
}