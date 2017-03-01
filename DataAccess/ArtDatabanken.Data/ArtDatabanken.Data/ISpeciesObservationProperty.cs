using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information about a property
    /// that is included in a species observation class.
    /// </summary>
    public interface ISpeciesObservationProperty
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Use predefined name as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        SpeciesObservationPropertyId Id
        { get; set; }

        /// <summary>
        /// Identifier for species observation property.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Get the name of this species observation property.
        /// </summary>
        /// <returns>The name of this species observation property.</returns>
        String GetName();
    }
}
