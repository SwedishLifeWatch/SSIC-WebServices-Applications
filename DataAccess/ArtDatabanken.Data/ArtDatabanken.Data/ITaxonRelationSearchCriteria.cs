using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface used when searching for taxon relations.
    /// </summary>
    public interface ITaxonRelationSearchCriteria
    {
        /// <summary>
        /// Limit returned taxon trees based on if the relation between
        /// two taxa is a main relation or not.
        /// </summary>
        Boolean? IsMainRelation
        { get; set; }

        /// <summary>
        /// Limit returned taxon relations to valid
        /// or not valid taxon relations.
        /// </summary>
        Boolean? IsValid
        { get; set; }

        /// <summary>
        /// These taxa are used to limit the returned taxon relations.
        /// All taxon relations that matches the other search
        /// criterias are returned if no taxa are specified.
        /// </summary>
        TaxonList Taxa
        { get; set; }

        /// <summary>
        /// Scope for this taxon relation search.
        /// Defines if relations above or beneath specified taxons
        /// should be returned.
        /// This property is not relevant if property TaxonIds
        /// contains no values.
        /// </summary>
        TaxonRelationSearchScope Scope
        { get; set; }
    }
}
