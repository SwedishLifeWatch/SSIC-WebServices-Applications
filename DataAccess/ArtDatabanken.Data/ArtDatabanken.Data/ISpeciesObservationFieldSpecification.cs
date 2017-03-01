namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class is used to specify a subset of the data for a
    /// species observation. Usually exactly one species observation
    /// field for each species observation.
    /// </summary>
    public interface ISpeciesObservationFieldSpecification
    {
        /// <summary>
        /// Specification of the species observation class.
        /// </summary>
        ISpeciesObservationClass Class { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Specification of the species observation property.
        /// </summary>
        ISpeciesObservationProperty Property { get; set; }
    }
}
