namespace ArtDatabanken.Database
{
    /// <summary>
    /// Enumeration of logical name for databases.
    /// That is not the same as the actual name for the database.
    /// </summary>
    public enum DatabaseName
    {
        /// <summary>
        /// Name for the database SpeciesObservation_Export.
        /// </summary>
        Artportalen,

        /// <summary>
        /// Name for the database observationsdatabas.
        /// </summary>
        Observation,

        /// <summary>
        /// Name for the database Picture.
        /// </summary>
        Picture,

        /// <summary>
        /// Name for the database Reference.
        /// </summary>
        Reference,

        /// <summary>
        /// Name for the database SwedishSpeciesObservation.
        /// </summary>
        SwedishSpeciesObservation,

        /// <summary>
        /// Name for the database TaxonAttribute.
        /// </summary>
        TaxonAttribute,

        /// <summary>
        /// Name for the database Taxon.
        /// </summary>
        Taxon,

        /// <summary>
        /// Name for the database ArtDataUser.
        /// </summary>
        User
    }
}
