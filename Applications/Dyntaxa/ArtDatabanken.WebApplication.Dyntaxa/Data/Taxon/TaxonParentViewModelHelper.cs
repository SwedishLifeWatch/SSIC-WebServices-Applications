namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Class ParentTaxonmodelHelper, holding information used by add and drop parent functionalities
    /// </summary>
    public class TaxonParentViewModelHelper
    {
        /// <summary>
        /// Taxon id 
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Taxon category for this taxon
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Sortorder for this taxon
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Scentific name for this taxon
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Common name for this taxon
        /// </summary>
        public string CommonName { get; set; }

        // Indicates if parent taxon is main relation
        public bool IsMain { get; set; }
    }
}