namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class is used to specify a subset of the data for a
    /// species observation.
    /// It is the union of species observation fields defined by
    /// property Fields and Specification that defines the subset.
    /// </summary>
    public class SpeciesObservationSpecification : ISpeciesObservationSpecification
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Species observation fields that are
        /// included in the specification.
        /// </summary>
        public SpeciesObservationFieldSpecificationList Fields { get; set; }

        /// <summary>
        /// Enumeration value that specifies a predefined
        /// set of species observation fields.
        /// </summary>
        public SpeciesObservationSpecificationId Specification { get; set; }
    }
}
