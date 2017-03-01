namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface holds parameters used to
    /// search for species observations.
    /// </summary>
    public interface ISpeciesObservationFieldSortOrder
    {
        /// <summary>
        /// Information about which species observation class
        /// that the sort order should be applied to.
        /// </summary>
        ISpeciesObservationClass Class
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about which species observation property
        /// that the sort order should be applied to.
        /// </summary>
        ISpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Sort order for species observations.
        /// </summary>
        SortOrder SortOrder
        { get; set; }
    }
}
