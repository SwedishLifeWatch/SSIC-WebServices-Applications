using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about child taxa count.
    /// Each taxon child statistics object is related to 
    /// specified root taxon and taxon category.
    /// 
    /// The following dynamic properties exists: 
    /// NumberOfSwedishRepro(Int32)
    /// 
    /// </summary>
    [DataContract]
    public class WebTaxonChildStatistics : WebData
    {
        /// <summary>
        /// Id for the taxon category.
        /// This taxon child statistics object is related to 
        /// specified root taxon and taxon category.
        /// </summary>
        [DataMember]
        public Int32 CategoryId { get; set; }

        /// <summary>
        /// Number of child taxa in this taxon category.
        /// </summary>
        [DataMember]
        public Int32 ChildTaxaCount { get; set; }

        /// <summary>
        /// Id for the root taxon.
        /// This taxon child statistics object is related to 
        /// specified root taxon and taxon category.
        /// </summary>
        [DataMember]
        public Int32 RootTaxonId { get; set; }

        /// <summary>
        /// Number of swedish child taxa in this taxon category.
        /// </summary>
        [DataMember]
        public Int32 SwedishChildTaxaCount { get; set; }
    }
}
