using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Search criterias used when retrieving taxon trees.
    /// </summary>
    [DataContract]
    public class WebTaxonTreeSearchCriteria : WebData
    {
        /// <summary>
        /// Indicates if only main taxon relations
        /// should be included in returned trees.
        /// </summary>
        [DataMember]
        public Boolean IsMainRelationRequired
        { get; set; }

        /// <summary>
        /// Indicates if only valid taxon relations and taxa
        /// should be included in returned trees.
        /// </summary>
        [DataMember]
        public Boolean IsValidRequired
        { get; set; }

        /// <summary>
        /// Scope for this taxon tree search.
        /// Defines if relations above or beneath specified taxons
        /// should be returned.
        /// This property is not relevant if property TaxonIds
        /// contains no values.
        /// </summary>
        [DataMember]
        public TaxonTreeSearchScope Scope
        { get; set; }

        /// <summary>
        /// Limit returned taxon tree to specified taxon categories.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonCategoryIds
        { get; set; }

        /// <summary>
        /// These taxa are used to limit the returned taxon trees.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }
    }
}
