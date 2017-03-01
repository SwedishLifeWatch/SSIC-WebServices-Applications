using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonAddParentViewModel
    {
        private readonly TaxonAddParentViewModelLabels labels = new TaxonAddParentViewModelLabels();    
        
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
        public string TaxonId { get; set; }

        public Dictionary<int, TaxonCategoryViewModel> TaxonCategories { get; set; }

        public Dictionary<int, List<TaxonParentViewModelHelper>> TaxonDictionary { get; set; }

        /// <summary>
        /// Get and sets the internal revision object.
        /// </summary>
        public string RevisionId { get; set; }

        /// <summary>
        /// Indicates if it should be possible to add a new parent to selected taxon
        /// </summary>
        public bool EnableSaveNewParentTaxonButton { get; set; }

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
        /// All localized labels
        /// </summary>
        public TaxonAddParentViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        ///  Test to be displayed if Add Parent failes..
        /// </summary>
        public string AddParentErrorText { get; set; }

        /// <summary>
        /// Indicates if it is ok to performe add...
        /// </summary>
        public bool IsOkToAdd { get; set; }

        /// <summary>
        /// Localized labels used in addParent view
        /// </summary>
        public class TaxonAddParentViewModelLabels
        {
            public string ExistingParentsLabel
            {
                get { return Resources.DyntaxaResource.RevisionAddParentExistingParentsLabel; }
            }

            public string ParentHeaderLabel
            {
                get { return Resources.DyntaxaResource.TaxonAddParentHeaderText; }
            }
                 
            public string AddButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedAddButtonText; }
            }

            public string AddParentLabel
            {
                get { return Resources.DyntaxaResource.TaxonAddParentLabelText; }
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
