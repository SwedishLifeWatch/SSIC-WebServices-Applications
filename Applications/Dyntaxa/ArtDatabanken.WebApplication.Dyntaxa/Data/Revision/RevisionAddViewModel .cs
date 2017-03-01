using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class RevisionAddViewModel
    {
        private readonly RevisionAddViewModelLabels labels = new RevisionAddViewModelLabels();

        /// <summary>
        /// Holds information on all avaliable users that have taxon editor revision role.
        /// </summary>
        public IList<RevisionUserItemModelHelper> UserList { get; set; }

        /// <summary>
        /// Holds informatin on users ie user id that has been selecetd to be working on this revision.
        /// </summary>
        public int[] SelectedUsers { get; set; }
       
        /////// <summary>
        /////// Gets and sets the  taxon object included in .
        /////// </summary>
        public string TaxonId { get; set; }

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
        /// 
        /// </summary>
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        [LocalizedDisplayName("SharedRevisionStartDateText", NameResourceType = typeof(Resources.DyntaxaResource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ExpectedStartDate { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        [LocalizedDisplayName("SharedRevisionPublishingDateText", NameResourceType = typeof(Resources.DyntaxaResource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ExpectedPublishingDate { get; set; }

        /////// <summary>
        /////// Gets and sets the revision id
        /////// </summary>
        public string RevisionId { get; set; }

        /// <summary>
        /// Indicated if initialize button should be enabeled.
        /// </summary>
        public bool ShowInitalizeButton { get; set; }

        /// <summary>
        /// Gets and sets the user admin link
        /// </summary>
        public LinkItem UserAdminLink { get; set; }

        /// <summary>
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public RevisionAddViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in add revision view
        /// </summary>
        public class RevisionAddViewModelLabels
        {
            public string GetSelectedSave
            {
                get { return "GetSelectedSave"; }
            }

            public string GetSelectedInRevision
            {
                get { return "GetSelectedInRevision"; }
            }

            public string AddButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedAddButtonText; }
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

           public string ResetButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedResetButtonText; }
            }

            public string ConfirmButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string AvaliableLabel
            {
                get { return Resources.DyntaxaResource.SharedAvaliableLabel; }
            }

            public string SelectedLabel
            {
                get { return Resources.DyntaxaResource.SharedSelectedLabel; }
            }

            public string RevisionAddActionHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionAddActionHeaderText; }
            }
            public string RevisionAddMainHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionAddMainHeaderText; }
            }
            public string RevisionAddPropertiesHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionAddPropertiesHeaderText; }
            }
            public string RevisionEditReferencesHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionEditReferencesHeaderText; }
            }

            public string RevisionAddSelectEditorsHeaderText
            {
                get { return Resources.DyntaxaResource.RevisionAddSelectEditorsHeaderText; }
            }

            public string DialogAddRevisionInfoText
            {
                get { return Resources.DyntaxaResource.RevisionAddCreateRevisionText; }
            }

            public string CreateLabel
            {
                get { return Resources.DyntaxaResource.SharedCreating; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            }
        }
    }
}
