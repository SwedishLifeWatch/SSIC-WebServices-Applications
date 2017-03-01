namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of identifiers for picture relation data types.
    /// Using ToString() on an enumeration value should return a value
    /// that is the same as the picture relation data type identifier.
    /// </summary>
    public enum PictureRelationDataTypeIdentifier
    {
        /// <summary>
        /// Identifier for picture relations to data of type factor.
        /// </summary>
        Factor = 1,

        /// <summary>
        /// Identifier for picture relations to data of type species fact.
        /// </summary>
        SpeciesFact = 2,

        /// <summary>
        /// Identifier for picture relations to data of type taxon.
        /// </summary>
        Taxon = 3
    }
}
