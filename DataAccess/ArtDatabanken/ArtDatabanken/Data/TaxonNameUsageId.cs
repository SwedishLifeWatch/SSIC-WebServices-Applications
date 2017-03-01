namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of possible usage for taxon names.
    /// </summary>
    public enum TaxonNameUsageId
    {
        /// <summary>Accepted.</summary>
        Accepted = 0,

        /// <summary>Synonym.</summary>
        Synonym = 1,

        /// <summary>Homotypic.</summary>
        Homotypic = 2,

        /// <summary>Heterotypic.</summary>
        Heterotypic = 3,

        /// <summary>proParte synonym.</summary>
        ProParteSynonym = 4,
        
        /// <summary>Misapplied (auct. name).</summary>
        MisappliedAuctName = 5
    }
}
