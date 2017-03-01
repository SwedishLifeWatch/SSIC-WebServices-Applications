namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class holds parameters used to
    /// search for species observations.
    /// </summary>
    public class SpeciesObservationFieldSortOrder: ISpeciesObservationFieldSortOrder
    {
        /// <summary>
        /// Information about which species observation class
        /// that the sort order should be applied to.
        /// </summary>
        public ISpeciesObservationClass Class
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about which species observation property
        /// that the sort order should be applied to.
        /// </summary>
        public ISpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Sort order for species observations.
        /// </summary>
        public SortOrder SortOrder
        { get; set; }
    }
}
