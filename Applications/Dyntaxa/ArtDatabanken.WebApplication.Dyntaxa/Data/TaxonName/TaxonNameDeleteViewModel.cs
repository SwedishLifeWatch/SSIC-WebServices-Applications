namespace ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName
{
    /// <summary>
    /// View model for /TaxonName/Delete/
    /// </summary>
    public class TaxonNameDeleteViewModel
    {
        private readonly ModelLabels _labels = new ModelLabels();

        public int TaxonId { get; set; }
        
        public int Version { get; set; }

        public string Name { get; set; }
        
        public string Comment { get; set; }
        
        public string Author { get; set; }
        
        public string Category { get; set; }

        public string NameUsage { get; set; }
        
        public string NameStatus { get; set; }

        public bool IsRecommended { get; set; }

        /// <summary>
        /// Localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteTitle; } }
            public string NameLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteName; } }
            public string AuthorLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteAuthor; } }
            public string CategoryLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteCategory; } }
            public string NameStatusLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteNameUsage; } }
            public string RecommendedLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteRecommended; } }
            public string DoYouWantToDeleteLabel { get { return Resources.DyntaxaResource.TaxonNameDeleteDoYouWantToDelete; } }
            public string SharedDialogInformationHeader { get { return Resources.DyntaxaResource.SharedDialogInformationHeader; } }

            public string GetSelectedDelete
            {
                get { return "GetSelectedDelete"; }
            }

            public string CancelButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string DeleteInfoTextLabel
            {
                get { return Resources.DyntaxaResource.TaxonNameDeleteInfoText; }
            }

            public string SharedDeleteButtonText
            {
                get { return Resources.DyntaxaResource.SharedDeleteButtonText; }
            }

            public string NameUsageLabel
            {
                get { return Resources.DyntaxaResource.TaxonNameSharedNameUsage; }
            }
        }
    }
}
