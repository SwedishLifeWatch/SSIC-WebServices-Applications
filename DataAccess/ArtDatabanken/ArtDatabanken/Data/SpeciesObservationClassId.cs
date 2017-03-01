using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of predefined classes that are associated
    /// with species observations.
    /// </summary>
    [DataContract]
    public enum SpeciesObservationClassId
    {
        /// <summary>
        /// Use this species observation class id when none of the
        /// predefined species observation classes can be used.
        /// </summary>
        [EnumMember]
        None,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Conservation related information about the taxon that
        /// the species observation is attached to.
        /// </summary>
        [EnumMember]
        Conservation,

        /// <summary>
        /// Information related to date and time
        /// when the species observation where made.
        /// </summary>
        [EnumMember]
        Event,

        /// <summary>
        /// Information pertaining to a location within a
        /// geological context, such as stratigraphy.
        /// </summary>
        [EnumMember]
        GeologicalContext,

        /// <summary>
        /// Information pertaining to taxonomic determinations
        /// (the assignment of a scientific name).
        /// </summary>
        [EnumMember]
        Identification,

        /// <summary>
        /// Information about the location where the
        /// species observation was made.
        /// </summary>
        [EnumMember]
        Location,

        /// <summary>
        /// Information pertaining to measurements, facts,
        /// characteristics, or assertions about a resource
        /// (instance of data record, such as Occurrence,
        /// Taxon, Location, Event).
        /// </summary>
        [EnumMember]
        MeasurementOrFact,

        /// <summary>
        /// Information pertaining to evidence of an occurrence in
        /// nature, in a collection, or in a dataset
        /// (specimen, observation, etc.).
        /// </summary>
        [EnumMember]
        Occurrence,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the project in which this
        /// species observation was made.
        /// Only some species observations belongs to a project.
        /// </summary>
        [EnumMember]
        Project,

        /// <summary>
        /// Information pertaining to relationships between resources
        /// (instances of data records, such as Occurrences,
        /// Taxa, Locations, Events).
        /// </summary>
        [EnumMember]
        ResourceRelationship,

        /// <summary>
        /// Information about the species observation itself.
        /// Property ClassIndex in class WebSpeciesObservationField
        /// should not be used on this species observation class.
        /// </summary>
        [EnumMember]
        DarwinCore,

        /// <summary>
        /// Information pertaining to taxonomic names,
        /// taxon name usages, or taxon concepts.
        /// </summary>
        [EnumMember]
        Taxon
    }
}
