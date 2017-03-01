using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Taxon quality summary
    /// </summary>
    public class TaxonChildQualityStatistics : ITaxonChildQualityStatistics
    {
        /// <summary>
        /// Number of child taxa with this taxon quality.
        /// The root taxon is included in the result.
        /// </summary>
        public Int32 ChildTaxaCount { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for the taxon quality.
        /// This value is defined by species fact with id 2115
        /// ("Kvalitetsdeklaration av information i Dyntaxa").
        /// Possible values:
        /// 0 => Not set
        /// 1 => Bad
        /// 2 => Acceptable
        /// 3 => Good
        /// </summary>
        public Int32 QualityId { get; set; }

        /// <summary>
        /// Root taxon.
        /// This taxon child quality statistics object is 
        /// related to specified root taxon and taxon quality.
        /// </summary>
        public ITaxon RootTaxon { get; set; }
    }
}