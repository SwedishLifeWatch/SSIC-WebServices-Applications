using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonLumpViewModel
    {
        private readonly TaxonLumpViewModelLabels labels = new TaxonLumpViewModelLabels();    
        
        /// <summary>
        /// Gets and sets a list of taxa to lump 
        /// </summary>
        public IList<TaxonParentViewModelHelper> LumpTaxonList { get; set; }

        ///// <summary>
        ///// Gets and sets selected taxa to be lumped
        ///// </summary>
        //public IList<string> SelectedTaxonList { get; set; }        

        /// <summary>
        /// Get and sets the internal taxon object.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Get and sets the internal taxon replace object.
        /// </summary>
        public string ReplacingTaxonId { get; set; }

        /// <summary>
        /// Get and sets the internal revision object.
        /// </summary>
        public string RevisionId { get; set; }

        /// <summary>
        /// Indicates if replacing taxon is set
        /// </summary>
        public bool IsReplacingTaxonSet { get; set; }

        /// <summary>
        /// Indicates if it any taxa is set to be lumped
        /// </summary>
        public bool IsAnyLumpTaxonSet { get; set; }

        /// <summary>
        /// Holds informatin on taxa that has been selecetd to be removed from lump list.
        /// </summary>
        public int[] SelectedTaxa { get; set; }

        /// <summary>
        /// Indicates if selected taxon to lump has children
        /// </summary>
        public bool IsSelectedTaxonChildless { get; set; }

        /// <summary>
        /// Indicates if it selected taxon is set in lumplist
        /// </summary>
        public bool IsSelectedTaxonAlreadyInLumpList { get; set; }

        /// <summary>
        /// Indicates if it selected taxon is set as replacing taxon
        /// </summary>
        public bool IsSelectedTaxonSetAsReplacingTaxon { get; set; }

        ///// <summary>
        ///// Taxon information for selected taxon
        ///// </summary>
        //public IList<TaxonParentViewModelHelper> SelectedTaxon { get; set; }

        /// <summary>
        /// Taxon information on replacing taxon when doing a lump
        /// </summary>
        public IList<TaxonParentViewModelHelper> ReplacingTaxon { get; set; }

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
        /// Indicated if it is ok to lump..
        /// </summary>
        public bool IsOkToLump { get; set; }

        /// <summary>
        /// Indicated if the view is reloaded ie indicatino that no error message is to be displayed.
        /// </summary>
        public bool IsReloaded { get; set; }

        /// <summary>
        /// Indicates if replacing taxon is valid.
        /// </summary>
        public bool IsReplacingTaxonValid { get; set; }

        /// <summary>
        /// Indicates if lump taxon is valid.
        /// </summary>
        public bool IsLumpTaxonValid { get; set; }

        /// <summary>
        /// Indicates if lump taxon category is valid.
        /// </summary>
        public bool IsLumpTaxonCategoryValid { get; set; }

        /// <summary>
        /// Name of failing taxon
        /// </summary>
        public string TaxonErrorName { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public TaxonLumpViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in addParent view
        /// </summary>
        public class TaxonLumpViewModelLabels
        {
            public string GetSelectedLump
            {
                get { return "GetSelectedLump"; }
            }

            public string SetCurrentTaxon
            {
                get { return "SetCurrentTaxon"; }
            }

            public string AddCurrentTaxonToList
            {
                get { return "AddCurrentTaxonToList"; }
            }
            public string RemoveSelectedTaxon
            {
                get { return "RemoveSelectedTaxon"; }
            }
            public string RemoveReplacingTaxon
            {
                get { return "RemoveReplacingTaxon"; }
            }

            public string TaxonLumpReplacingTaxonLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpReplacingTaxonLabel; }
            }

            public string TaxonLumpAddCurrentTaxonToListButtonLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpAddCurrentTaxonToListButtonText; }
            }

            public string TaxonLumpSetCurrentTaxonToReplacingTaxonButtonLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpSetCurrentTaxonToReplacingTaxonButtonText; }
            }

            public string TaxonLumpNoTaxaSelectedErrorLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpNoTaxaSelectedErrorText; }
            }

            public string TaxonLumpNoReplacingTaxonErrorLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpNoReplacingTaxonErrorText; }
            }

            public string TaxonLumpListTaxaToLumpLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpListTaxaToLumpText; }
            }

            public string TaxonLumpHeaderLabel
            {
                get { return Resources.DyntaxaResource.TaxonLumpHeaderText; }
            }

            public string AddButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedAddButtonText; }
            }

            public string DeleteButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedDeleteButtonText; }
            }

            public string CancelButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

             public string SaveButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedSaveButtonText; }
            }

             public string ResetButtonLabel
             {
                 get { return Resources.DyntaxaResource.SharedResetButtonText; }
             }

            public string TaxonLumpButtonLabel
             {
                 get { return Resources.DyntaxaResource.TaxonLumpButtonText; }
             }

            public string ConfirmTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            /// <summary>
            /// Text information in pop up dialog
            /// </summary>
            public string TaxonLumpHaveChildrenErrorText 
            {
                get { return Resources.DyntaxaResource.TaxonLumpHaveChildrenErrorText; }
            }

            /// <summary>
            /// Text information in popup dialog title
            /// </summary>
            public string TaxonLumpPopUpText 
            {
                get { return Resources.DyntaxaResource.TaxonLumpPopUpText; }
            }

            public string TaxonLumpAlreadyInLumpListErrorText
            {
                get { return Resources.DyntaxaResource.TaxonLumpAlreadyInLumpListErrorText; }
            }

            public string TaxonLumpSetAsReplacingErrorText
            {
                get { return Resources.DyntaxaResource.TaxonLumpSetAsReplacingErrorText; }
            }

            public string SharedReferencesHeaderLabel
            {
                get { return Resources.DyntaxaResource.SharedReferencesHeaderText; }
            }

            public string TaxonNotOkToLumpErrorText
            {
                get { return Resources.DyntaxaResource.TaxonLumpNotOkToLumpErrorText; }
            }

             public string TaxonLumpReplacingTaxonErrorText
            {
                get { return Resources.DyntaxaResource.TaxonLumpReplacingTaxonErrorText + " "; }
            }

             public string TaxonLumpTaxonCategoryErrorText
            {
                get { return Resources.DyntaxaResource.TaxonLumpTaxonCategoryErrorText + " "; }
            }

             public string TaxonLumpTaxonErrorText
            {
                get { return Resources.DyntaxaResource.TaxonLumpTaxonErrorText + " "; }
            }

             public string SavingLabel
             {
                 get { return Resources.DyntaxaResource.SharedSaving; }
             } 
        }
    }
}
