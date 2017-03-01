using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about child taxa quality.
    /// Each taxon child quality statistics object is related to 
    /// specified root taxon and taxon quality.
    /// The root taxon is included in the result.
    /// </summary>
    public interface ITaxonChildQualityStatistics 
    {
        /// <summary>
        /// Number of child taxa with this taxon quality.
        /// The root taxon is included in the result.
        /// </summary>
        Int32 ChildTaxaCount { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext { get; set; }

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
        Int32 QualityId { get; set; }

        /// <summary>
        /// Id for the root taxon.
        /// This taxon child quality statistics object is 
        /// related to specified root taxon and taxon quality.
        /// </summary>
        ITaxon RootTaxon { get; set; }
    }
}
