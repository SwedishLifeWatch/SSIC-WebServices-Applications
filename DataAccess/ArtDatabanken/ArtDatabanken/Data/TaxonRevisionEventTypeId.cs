namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of taxon revision event types.
    /// </summary>
    public enum TaxonRevisionEventTypeId
    {
        /// <summary>
        /// Add taxon.
        /// </summary>
        AddTaxon = 1,
        
        /// <summary>
        /// Remove taxon.
        /// </summary>
        RemoveTaxon = 2,
        
        /// <summary>
        /// Change taxon parent.
        /// </summary>
        ChangeTaxonParent = 3,
        
        /// <summary>
        /// Add taxon parent.
        /// </summary>
        AddTaxonParent = 4,
        
        /// <summary>
        /// Remove taxon parent.
        /// </summary>
        RemoveTaxonParent = 5,
        
        /// <summary>
        /// Change taxon sort order.
        /// </summary>
        ChangeTaxonSortOrder = 6,
        
        /// <summary>
        /// Change taxon category.
        /// </summary>
        ChangeTaxonCategory = 7,
        
        /// <summary>
        /// Lump taxa.
        /// </summary>
        LumpTaxa = 8,
        
        /// <summary>
        /// Split taxa.
        /// </summary>
        SplitTaxa = 9,
        
        /// <summary>
        /// Add taxon name.
        /// </summary>
        AddTaxonName = 10,
        
        /// <summary>
        /// Edit taxon Name.
        /// </summary>
        EditTaxonName = 11,
        
        /// <summary>
        /// Delete taxon name.
        /// </summary>
        DeleteTaxonName = 12,
        
        /// <summary>
        /// Change recommended name.
        /// </summary>
        ChangeRecommendedName = 13,
        
        /// <summary>
        /// Change taxon name category.
        /// </summary>
        ChangeTaxonNameCategory = 14,
        
        /// <summary>
        /// Change taxon name usage type.
        /// </summary>
        ChangeTaxonNameUsageType = 15,
        
        /// <summary>
        /// Edit taxon.
        /// </summary>
        EditTaxon = 16,
        
        /// <summary>
        /// Change Swedish occurrence.
        /// </summary>
        ChangeSwedishOccurrence = 17,
        
        /// <summary>
        /// Change Swedish immigration history.
        /// </summary>
        ChangeSwedishImmigrationHistory = 18,
        
        /// <summary>
        /// Change reference relation.
        /// </summary>
        ChangeReferenceRelation = 19
    }
}
