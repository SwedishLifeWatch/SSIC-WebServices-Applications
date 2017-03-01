using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about underlying taxa to a specific root taxon.
    /// Each WebTaxonStatistics instance contains information
    /// about one taxon category.
    /// </summary>
    public class TaxonChildStatistics : ITaxonChildStatistics
    {
        /// <summary>
        /// The taxon statistics is related to this taxon category.
        /// </summary>
        public ITaxonCategory Category { get; set; }

        /// <summary>
        /// This property should be removed when usage of it is
        /// has been removed from GUI in Dyntaxa.
        /// </summary>
        public String CategoryName
        {
            get
            {
                if (Category.IsNull())
                {
                    return null;
                }
                else
                {
                    return Category.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets NumberInDyntaxa.
        /// </summary>
        public Int32 ChildTaxaCount { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Root taxon.
        /// This taxon child statistics object is related to 
        /// specified root taxon and taxon category.
        /// </summary>
        public ITaxon RootTaxon { get; set; }

        /// <summary>
        /// Gets or sets NumberOfSwedishOccurrence.
        /// </summary>
        public Int32 SwedishChildTaxaCount { get; set; }

        /// <summary>
        /// Gets or sets SwedishReproCount
        /// </summary>
        public Int32 SwedishReproCount { get; set; }
    }
}