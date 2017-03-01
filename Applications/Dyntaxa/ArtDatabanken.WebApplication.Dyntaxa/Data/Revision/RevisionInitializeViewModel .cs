using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionInitializeViewModel
    {
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

        ///// <summary>
        ///// Revision identifier
        ///// </summary>
        public string RevisionId { get; set; }
    }
}
