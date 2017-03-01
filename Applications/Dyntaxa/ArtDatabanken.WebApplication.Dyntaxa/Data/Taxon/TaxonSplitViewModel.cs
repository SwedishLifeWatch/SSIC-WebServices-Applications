using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonSplitViewModel
    {
        private readonly TaxonSplitViewModelLabels labels = new TaxonSplitViewModelLabels();
        
        /// <summary>
        /// Gets and sets a list of taxa to split 
        /// </summary>
        public IList<TaxonParentViewModelHelper> SplitTaxon { get; set; }

        /// <summary>
        /// Gets and sets selected taxa to be splitted
        /// </summary>
        public IList<string> SelectedTaxonList { get; set; }

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
        /// Indicates if replacing taxa list is set
        /// </summary>
        public bool IsAnyReplacingTaxonSet { get; set; }

        /// <summary>
        /// Indicates if it any taxa is set to be used as split taxon
        /// </summary>
        public bool IsSplitTaxonSet { get; set; }

        /// <summary>
        /// Holds informatin on taxa that has been selecetd to be removed from lump list.
        /// </summary>
        public int[] SelectedTaxa { get; set; }

        /// <summary>
        /// Indicates if selected taxon to lump has children
        /// </summary>
        public bool IsSelectedTaxonChildless { get; set; }

        /// <summary>
        /// Indicates if it selected taxon is set in replacing list
        /// </summary>
        public bool IsSelectedTaxonAlreadyInReplacingList { get; set; }

        /// <summary>
        /// Indicates if it selected taxon is set as taxon to be splitted
        /// </summary>
        public bool IsSelectedTaxonSetAsSplitTaxon { get; set; }

        /// <summary>
        /// Taxon information for selected taxon
        /// </summary>
        public IList<TaxonParentViewModelHelper> SelectedTaxon { get; set; }

        /// <summary>
        /// Taxon information on replacing taxa when doing a split
        /// </summary>
        public IList<TaxonParentViewModelHelper> ReplacingTaxonList { get; set; }

        /// <summary>
        /// Indication if it's ok to split
        /// </summary>
        public bool IsOkToSplit { get; set; }

        /// <summary>
        /// Indicated if the view is reloaded ie indicatino that no error message is to be displayed.
        /// </summary>
        public bool IsReloaded { get; set; }

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
        /// Indicates if replacing taxon is valid.
        /// </summary>
        public bool IsReplacingTaxonValid { get; set; }

        /// <summary>
        /// Indicates if split taxon is valid.
        /// </summary>
        public bool IsSplitTaxonValid { get; set; }

        /// <summary>
        /// Indicates if replacing taxon category is valid.
        /// </summary>
        public bool IsReplacingTaxonCategoryValid { get; set; }

        /// <summary>
        /// Name of failing taxon
        /// </summary>
        public string TaxonErrorName { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public TaxonSplitViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in addParent view
        /// </summary>
        public class TaxonSplitViewModelLabels
        {
            public string GetSelectedSplit
            {
                get { return "GetSelectedSplit"; }
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

            public string TaxonSplitReplacingTaxonLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitReplacingTaxonLabel; }
            }

            public string TaxonSplitAddCurrentTaxonToListButtonLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitAddCurrentTaxonToListButtonText; }
            }

            public string TaxonSplitSetCurrentTaxonToReplacingTaxonButtonLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitSetCurrentTaxonToSplittingTaxonButtonText; }
            }

            public string TaxonSplitNoTaxonSelectedErrorLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitNoTaxaSelectedErrorText; }
            }

            public string TaxonSplitNoReplacingTaxaErrorLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitNoReplacingTaxonErrorText; }
            }

            public string TaxonSplitTaxonToBeSplittedLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitTaxonToBeSplittedText; }
            }

            public string TaxonSplitHeaderLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitHeaderText; }
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
            public string TaxonSplitButtonLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitButtonText; }
            }

            public string ConfirmTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string ResetButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedResetButtonText; }
            }

            /// <summary>
            /// Text information in pop up dialog
            /// </summary>
            public string TaxonSplitHaveChildrenErrorText
            {
                get { return Resources.DyntaxaResource.TaxonSplitHaveChildrenErrorText; }
            }

            /// <summary>
            /// Text information in popup dialog title
            /// </summary>
            public string TaxonSplitPopUpText
            {
                get { return Resources.DyntaxaResource.TaxonSplitPopUpText; }
            }

            public string TaxonSplitAlreadyInReplaceListErrorLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitAlreadyInReplaceListErrorText; }
            }

            public string TaxonSplitAlreadyInSetAsSplittingErrorLabel
            {
                get { return Resources.DyntaxaResource.TaxonSplitAlreadyInSetAsSplittingErrorText; }
            }

            public string SharedReferencesHeaderLabel
            {
                get { return Resources.DyntaxaResource.SharedReferencesHeaderText; }
            }

            public string TaxonNotOkToSplitErrorText
            {
                get { return Resources.DyntaxaResource.TaxonSplitNotOkToSplitErrorText; }
            }

            public string TaxonSplitReplacingTaxonErrorText
            {
                get { return Resources.DyntaxaResource.TaxonSplitReplacingTaxonErrorText + " "; }
            }

            public string TaxonSplitTaxonCategoryErrorText
            {
                get { return Resources.DyntaxaResource.TaxonSplitTaxonCategoryErrorText + " "; }
            }

            public string TaxonSplitTaxonErrorText
            {
                get { return Resources.DyntaxaResource.TaxonSplitTaxonErrorText + " "; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            } 
        }
    }
}
