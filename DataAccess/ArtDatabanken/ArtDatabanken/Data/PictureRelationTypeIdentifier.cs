namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of identifiers for picture relation types.
    /// Using ToString() on an enumeration value should return a value
    /// that is the same as the picture relation type identifier.
    /// </summary>
    public enum PictureRelationTypeIdentifier
    {
        /// <summary>
        /// Identifier for picture relation type Factor.
        /// </summary>
        Factor,

        /// <summary>
        /// Identifier for picture relation type SpeciesFact.
        /// </summary>
        SpeciesFact,

        /// <summary>
        /// Identifier for picture relation type TaxonSpeciesIndication.
        /// </summary>
        TaxonSpeciesIndication,

        /// <summary>
        /// Identifier for picture relation type TaxonRedList.
        /// </summary>
        TaxonRedList
    }
}
