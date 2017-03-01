using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about a taxon category.
    /// </summary>
    [DataContract]
    public class WebTaxonCategory : WebData
    {
        /// <summary>
        /// Id of taxon category.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Test if taxon category is a main category.
        /// </summary>
        [DataMember]
        public Boolean IsMainCategory { get; set; }

        /// <summary>
        /// Test if taxon category is taxonomic.
        /// </summary>
        [DataMember]
        public Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Name of the taxon category.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Id of parent taxon category.
        /// </summary> 
        [DataMember]
        public Int32 ParentId { get; set; }

        /// <summary>
        /// Sort order for this taxon category.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }
    }
}
