using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about underlying taxa to a specific root taxon.
    /// Each ITaxonChildStatistics instance contains information
    /// about one taxon category.
    /// </summary>
    public interface ITaxonChildStatistics
    {
        /// <summary>
        /// The taxon statistics is related to this taxon category.
        /// </summary>
        ITaxonCategory Category { get; set; }

        /// <summary>
        /// Number of child taxa in this taxon category.
        /// </summary> 
        Int32 ChildTaxaCount { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Root taxon.
        /// This taxon child statistics object is related to 
        /// specified root taxon and taxon category.
        /// </summary>
        ITaxon RootTaxon { get; set; }

        /// <summary>
        /// Number of swedish child taxa in this taxon category.
        /// </summary>
        Int32 SwedishChildTaxaCount { get; set; }

        /// <summary>
        /// Number of swedish child taxa in this taxon category that 
        /// are reproducing in Sweden.
        /// </summary>
        Int32 SwedishReproCount { get; set; }
    }
}
