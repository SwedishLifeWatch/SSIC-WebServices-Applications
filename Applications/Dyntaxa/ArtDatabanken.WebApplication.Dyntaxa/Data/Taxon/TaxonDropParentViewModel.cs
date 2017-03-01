using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonDropParentViewModel
    {
        private readonly TaxonDropParentViewModelLabels labels = new TaxonDropParentViewModelLabels();    
        
        /// <summary>
        /// Gets and sets a list of avaliable taxa to add ass new parent
        /// </summary>
        public IList<TaxonParentViewModelHelper> TaxonList { get; set; }

        /// <summary>
        /// Gets and sets selected taxon as new parents
        /// </summary>
        public IList<string> SelectedTaxonList { get; set; }

        /// <summary>
        /// Get and sets the internal taxon object.
        /// </summary>
        [Required]
        public string TaxonId { get; set; }

        /// <summary>
        /// Get and sets the internal revision object.
        /// </summary>
        [Required]
        public string RevisionId { get; set; }

        /// <summary>
        /// Indicates if it should be possible to add a new parent to selected taxon
        /// </summary>
        public bool EnableSaveDeleteParentTaxonButton { get; set; }

        /// <summary>
        /// Taxon information on parenents for selected taxon
        /// </summary>
        public IList<TaxonParentViewModelHelper> AvailableParents { get; set; }

        /// <summary>
        /// Get and sets the internal taxon error object.
        /// </summary>
        public string TaxonErrorId { get; set; }

        /// <summary>
        /// Get and sets the internal revision error object.
        /// </summary>
        public string RevisionErrorId { get; set; }

        /// <summary>
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Text information in pop up dialog
        /// </summary>
        public string DialogTextPopUpText { get; set; }

        /// <summary>
        /// Text information in popup dialog title
        /// </summary>
        public string DialogTitlePopUpText { get; set; }

        /// <summary>
        /// Indicated if view is reloaded.
        /// </summary>
        public bool IsReloaded { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public TaxonDropParentViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in addParent view
        /// </summary>
        public class TaxonDropParentViewModelLabels
        {
            public string ExistingParentsLabel
            {
                get { return Resources.DyntaxaResource.RevisionAddParentExistingParentsLabel; }
            }

            public string ParentHeaderLabel
            {
                get { return Resources.DyntaxaResource.TaxonDropParentHeaderText; }
            }

            public string DeleteButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedDeleteButtonText; }
            }

            public string DropParentLabel
            {
                get { return Resources.DyntaxaResource.TaxonDropParentLabelText; }
            }

            public string CancelButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string ConfirmTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            } 
        }
    }
}
