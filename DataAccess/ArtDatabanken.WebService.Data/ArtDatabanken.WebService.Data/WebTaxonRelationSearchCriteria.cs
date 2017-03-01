using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Search criterias used when retrieving taxon relations.
    /// </summary>
    [DataContract]
    public class WebTaxonRelationSearchCriteria : WebData
    {
        /// <summary>
        /// Specifies if property IsMainRelation contains a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsIsMainRelationSpecified
        { get; set; }

        /// <summary>
        /// Specifies if property IsValid contains a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsIsValidSpecified
        { get; set; }

        /// <summary>
        /// Limit returned taxon trees based on if the relation between
        /// two taxa is a main relation or not.
        /// </summary>
        [DataMember]
        public Boolean IsMainRelation
        { get; set; }

        /// <summary>
        /// Limit returned taxon relations to valid or not valid taxon relations.
        /// </summary>
        [DataMember]
        public Boolean IsValid
        { get; set; }

        /// <summary>
        /// Scope for this taxon relation search.
        /// Defines if relations above or beneath specified taxons
        /// should be returned.
        /// This property is not relevant if property TaxonIds
        /// contains no values.
        /// </summary>
        [DataMember]
        public TaxonRelationSearchScope Scope
        { get; set; }

        /// <summary>
        /// These taxa are used to limit the returned taxon relations.
        /// All taxon relations that matches the other search
        /// criterias are returned if no taxon ids are specified.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }
    }
}
