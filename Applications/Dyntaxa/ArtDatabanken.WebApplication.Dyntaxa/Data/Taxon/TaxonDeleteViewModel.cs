namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonDeleteViewModel
    {
        private readonly TaxonDeleteTaxonViewModelLabels labels = new TaxonDeleteTaxonViewModelLabels();

        /// <summary>
        /// Get and sets the internal taxon object.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Get and sets the internal revision object.
        /// </summary>
        public string RevisionId { get; set; }

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
        /// Indicates if selected taxon to lump has children
        /// </summary>
        public bool IsSelectedTaxonChildless { get; set; }

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
        public TaxonDeleteTaxonViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in addParent view
        /// </summary>
        public class TaxonDeleteTaxonViewModelLabels
        {
            public string GetSelectedSave
            {
                get { return "GetSelectedSave"; }
            }

            public string CancelButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string ConfirmTextPopUpText
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }
            public string TitleLabel
            {
                get { return Resources.DyntaxaResource.TaxonDeleteInfoHeaderText; }
            }

            public string DoYouWantToDeleteLabel
            {
                get { return Resources.DyntaxaResource.TaxonDeleteText; }
            }
            
            public string DeleteInfoTextLabel
            {
                get { return Resources.DyntaxaResource.TaxonDeleteInfoText; }
            }

            public string SharedDeleteButtonText
            {
                get { return Resources.DyntaxaResource.SharedDeleteButtonText; }
            }

            public string SharedDialogInformationHeader
            {
                get { return Resources.DyntaxaResource.SharedDialogInformationHeader; }
            }

            /// <summary>
            /// Text information in pop up dialog
            /// </summary>
            public string TaxonHaveChildrenErrorText
            {
                get { return Resources.DyntaxaResource.TaxonHaveChildrenErrorText; }
            }

            public string TaxonIdLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchTaxonId; }
            }

            public string ScientificNameLabel
            {
                get { return Resources.DyntaxaResource.TaxonInfoScientificNameLabel + " " + Resources.DyntaxaResource.TaxonSharedName; }
            }

            public string CommonNameLabel
            {
                get { return Resources.DyntaxaResource.TaxonInfoCommonNameLabel + " " + Resources.DyntaxaResource.TaxonSharedName; }
            }

            public string CategoryLabel
            {
                get { return Resources.DyntaxaResource.TaxonInfoTaxonStatisticsCategoryNameLabel; }
            }
        }
    }
}