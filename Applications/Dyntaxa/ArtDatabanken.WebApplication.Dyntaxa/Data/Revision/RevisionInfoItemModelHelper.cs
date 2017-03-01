using System;
using System.ComponentModel.DataAnnotations;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Holds information to be needed for displaying revision info.
    /// </summary>
    public class RevisionInfoItemModelHelper
    {
        private readonly RevisionInfoItemModelHelperLabels labels = new RevisionInfoItemModelHelperLabels();

        /// <summary>
        /// Get the internal taxon object.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Taxon category the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string TaxonCategory { get; set; }

        /// <summary>
        /// Recommended Scientific Name of the taxon.
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        public string CommonName { get; set; }

        public string FormName { get; set; }

        /// <summary>
        /// A intruction for the revision task
        /// </summary>
        [LocalizedDisplayName("SharedTaxonConceptDescription", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string RevisionDescription { get; set; }

        /// <summary>
        /// Expected start date
        /// </summary>
        [LocalizedDisplayName("SharedRevisionStartDateText", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string ExpectedStartDate { get; set; }

        /// <summary>
        /// Expected end date
        /// </summary>
        [LocalizedDisplayName("SharedRevisionPublishingDateText", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string ExpectedPublishingDate { get; set; }

        /// <summary>
        /// Get the revision object.
        /// </summary>
        [LocalizedDisplayName("SharedRevisionIdLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string RevisionId { get; set; }

        /// <summary>
        /// The status of the revision ie Created, Ongoing etc
        /// </summary>
        [LocalizedDisplayName("RevisionListRevisionStatusTableHeaderText", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string RevisionStatus { get; set; }

        /// <summary>
        /// Indicates if enable revision button is enabled. Used by Start and Stop, not used for Info ie set to false..
        /// </summary>
        public bool EnableRevisionEditingButton { get; set; }
        
        /// <summary>
        /// Text to show on button for editing data. Used by Start and Stop, not used for revision info ie ""..
        /// </summary>
        public string RevisionEditingButtonText { get; set; }

        /// <summary>
        /// Indicated if button for editing data is to be shown... Used by Start and Stop, not used for Info ie set to false..
        /// </summary>
        public bool ShowRevisionEditingButton { get; set; }
  
        /// <summary>
        /// Information on that revision has been selected, if not selected the string is empty.
        /// </summary>
        public string SelectedRevisionForEditingText { get; set; }

        /// <summary>
        /// Indicates if revision information is to be shown enabled. Used by Start and Stop, not used for Info ie set to false..
        /// </summary>
        public bool ShowRevisionInformation { get; set; }

        /// <summary>
        /// Display label when waiting on data...
        /// </summary>
        public string RevisionWaitingLabel { get; set; }

        /// <summary>
        /// Which action to perform when pushing editing button
        /// </summary>
        public string EditingAction { get; set; }

        /// <summary>
        /// Which controller to perform when pushing editing button
        /// </summary>
        public string EditingController { get; set; }

        /// <summary>
        /// All localized labels.
        /// </summary>
        public RevisionInfoItemModelHelperLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in edit revision view
        /// </summary>
        public class RevisionInfoItemModelHelperLabels
        {
            public string GetSelected
            {
                get { return "GetSelected"; }
            }

            public string CancelButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string ConfirmButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string DialogTitlePopUpText
            {
                get { return Resources.DyntaxaResource.RevisionDeleteDialogTitleText; }
            }

            public string DialogTextPopUpText
            {
                get { return Resources.DyntaxaResource.RevisionDeleteDialogText; }
            }
        }
    }
}
