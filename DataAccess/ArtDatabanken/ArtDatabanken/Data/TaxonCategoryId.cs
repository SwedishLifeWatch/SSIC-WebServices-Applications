namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of taxon categories.
    /// </summary>
    public enum TaxonCategoryId
    {
        /// <summary>
        /// Kingdom (swedish name rike).
        /// </summary>
        Kingdom = 1,
        /// <summary>
        /// Phylum (swedish name stam).
        /// </summary>
        Phylum = 2,
        /// <summary>
        /// Class (swedish name klass).
        /// </summary>
        Class = 5,
        /// <summary>
        /// Order (swedish name ordning).
        /// </summary>
        Order = 8,
        /// <summary>
        /// Family (swedish name familj).
        /// </summary>
        Family = 11,
        /// <summary>
        /// Genus (swedish name släkte).
        /// </summary>
        Genus = 14,
        /// <summary>
        /// Subgenus (swedish name undersläkte).
        /// </summary>
        Subgenus = 15,

        /// <summary>
        /// Species (swedish name art).
        /// </summary>
        Species = 17,

        /// <summary>
        /// Subspecies (swedish name underart).
        /// </summary>
        Subspecies = 18,

        /// <summary>
        /// Small species
        /// </summary>
        SmallSpecies = 50
    }
}
