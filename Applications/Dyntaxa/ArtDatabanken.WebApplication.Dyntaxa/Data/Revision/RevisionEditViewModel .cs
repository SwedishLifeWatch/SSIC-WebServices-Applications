using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionEditViewModel 
    {
        private readonly RevisionEditViewModelLabels labels = new RevisionEditViewModelLabels();

        /// <summary>
        /// Expected start date
        /// </summary>
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)] 
        [LocalizedDisplayName("SharedRevisionStartDateText", NameResourceType = typeof(Resources.DyntaxaResource))]
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// Expected publishing date
        /// </summary>
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [LocalizedDisplayName("SharedRevisionPublishingDateText", NameResourceType = typeof(Resources.DyntaxaResource))]
        public DateTime ExpectedPublishingDate { get; set; }

        /// <summary>
        /// A intruction for the revision task
        /// </summary>
        [Required]
        // [StringLength(250, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedErrorStringToLong250")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("SharedTaxonConceptDescription", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string RevisionDescription { get; set; }

        /// <summary>
        /// Description of the taxon quality.
        /// </summary>
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedErrorStringToLong250")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("SharedTaxonConceptDescription", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string RevisionQualityDescription { get; set; }

        /// <summary>
        /// Quality status of the taxon.
        /// </summary>
        [LocalizedDisplayName("RevisionEditQualityLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int RevisionQualityId { get; set; }

        /// <summary>
        /// List of all avaliable quality values for a taxon
        /// </summary>
        public IList<TaxonDropDownModelHelper> RevisionQualityList { get; set; }

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
        /// List of all revision referenses
        /// </summary>
        [LocalizedDisplayName("SharedReferences", NameResourceType = typeof(Resources.DyntaxaResource))]
        public IList<int> RevisionReferencesList { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedPickReferenceText")]
        [LocalizedDisplayName("SharedReferences", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int NoOfRevisionReferences { get; set; }

        /// <summary>
        /// Indication of whether or not initialize button should be enabeled.
        /// </summary>
        public bool ShowInitalizeButton { get; set; }

        /// <summary>
        /// Indication of whether or not finalized button should be enabeled.
        /// </summary>
        public bool ShowFinalizeButton { get; set; }

        /// <summary>
        /// Indicate if species facts needs to be updated
        /// </summary>
        public bool ShowUpdateSpeciesFactButton { get; set; }

        /// <summary>
        /// Indicate if reference relations needs to be updated
        /// </summary>
        public bool ShowUpdateReferenceRelationsButton { get; set; }

        /// <summary>
        /// Holds information on all avaliable users that have taxon editor revision role.
        /// </summary>
        public IList<RevisionUserItemModelHelper> UserList { get; set; }

        /// <summary>
        /// List of selected users set in revision
        /// </summary>
        public IList<RevisionUserItemModelHelper> SelectedUserList { get; set; }

        /// <summary>
        /// Holds informatin on users ie user id that has been selecetd to be working on this revision.
        /// </summary>
        public int[] SelectedUsers { get; set; }

        /// <summary>
        /// Get the selected taxon object.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Get the revision taxon object.
        /// </summary>
        public string RevisionTaxonId { get; set; }

        /// <summary>
        /// Gets and sets the user admin link
        /// </summary>
        public LinkItem UserAdminLink { get; set; }

        /// <summary>
        /// Indicates if taxon is alreday in a revision.
        /// </summary>
        public bool IsTaxonInRevision { get; set; }

        /// <summary>
        /// Indicated if deletd button is to be shown..
        /// </summary>
        public bool ShowDeleteButton { get; set; }

        /// <summary>
        /// True if revision published to Artfakta
        /// </summary>
        public bool IsSpeciesFactPublished { get; set; }

        /// <summary>
        /// True if reference relations published to Artfakta
        /// </summary>
        public bool IsReferenceRelationsPublished { get; set; }

        /// <summary>
        /// Gets revision guid.
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Sets the id of failing taxon
        /// </summary>
        public string TaxonErrorId { get; set; }

        /// <summary>
        /// Set Indicated role
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public RevisionEditViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Class holding information on root taxon in revision.
        /// </summary>
        public RevisionTaxonInfoViewModel RevisionTaxonInfoViewModel { get; set; }        

        /// <summary>
        /// Localized labels used in edit revision view
        /// </summary>
        public class RevisionEditViewModelLabels
        {
            public string GetSelectedSave
            {
                get { return "GetSelectedSave"; }
            }

            public string GetSelectedInRevision
            {
                get { return "GetSelectedInRevision"; }
            }

            public string GetSelectedInitialize
            {
                get { return "GetSelectedInitialize"; }
            }
            public string GetSelectedFinalize
            {
                get { return "GetSelectedFinalize"; }
            }

            public string GetSelectedSaveNoReferences
            {
                get { return "GetSelectedSaveNoReferences"; }
            }
            public string AddButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedAddButtonText; }
            }

            public string ResetButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedResetButtonText; }
            }

            public string SaveButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedSaveButtonText; }
            }
            
            public string DeleteButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedDeleteButtonText; }
            }

            public string CancelButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            }

             public string InitializingLabel
            {
                get { return Resources.DyntaxaResource.SharedInitializing; }
            }

             public string FinalizingLabel
            {
                get { return Resources.DyntaxaResource.SharedFinalizing; }
            }
            
            public string InitializeButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedRevisionInitializeButtonText; }
            }

             public string FinalizeButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedRevisionFinalizeButtonText; }
            }

            public string SpeciesFactButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedRevisionUpdateSpeciesFactsButton; }
            }

            public string ConfirmButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string DialogInitTitlePopUpText
            {
                get { return Resources.DyntaxaResource.SharedRevisionInitializeDialogTitleText; }
            }

            public string DialogInitTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedRevisionInitializeDialogText; }
            }

             public string DialogFinalizeTitlePopUpText
            {
                get { return Resources.DyntaxaResource.SharedRevisionFinalizeDialogTitleText; }
            }

            public string DialogFinalizeTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedRevisionFinalizeDialogText; }
            }

            public string DialogNoEditorsTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedRevisionNoEditorsSelectedErrorText; }
            }

            public string DialogNoEditorsTitlePopUpText
            {
                get { return Resources.DyntaxaResource.RevisionEditMainHeaderText; }
            }

             public string DialogTaxonIsInRevisionTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedRevisionTaxonIsInRevisionDialogText; }
            }

             public string AvaliableLabel
            {
                get { return Resources.DyntaxaResource.SharedAvaliableLabel; }
            }

             public string SelectedLabel
            {
                get { return Resources.DyntaxaResource.SharedSelectedLabel; }
            }

             public string RevisionEditActionHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionEditActionHeaderText; }
            }
             public string RevisionEditMainHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionEditMainHeaderText; }
            }
             public string RevisionAddPropertiesHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionAddPropertiesHeaderText; }
            }
             public string RevisionEditReferencesHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionEditReferencesHeaderText; }
            }
             public string RevisionEditQualityHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionEditQualityHeaderText; }
            }
            public string RevisionEditEditorsHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionEditEditorsHeaderText; }
            }
            public string RevisionYesButtonText
            {
                get { return Resources.DyntaxaResource.SharedDialogButtonTextYes; }
            }
            public string RevisionNoButtonText
            {
                get { return Resources.DyntaxaResource.SharedDialogButtonTextNo; }
            }
            public string NoReferencesAvaliableText
            {
                get { return Resources.DyntaxaResource.SharedNoValidReferenceErrorText; }
            }

            public string ReferencesText
            {
                get { return Resources.DyntaxaResource.SharedReferences; }
            }

            public string DialogNoReferencesTitlePopUpText
            {
                get { return Resources.DyntaxaResource.RevisionEditMainHeaderText; }
            }

            public string DialogNoReferencesTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedNoValidReferenceErrorInfoText; }
            }
        }
    }
}
