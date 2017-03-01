using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information about a class
    /// that is included in a species observation.
    /// </summary>
    public interface ISpeciesObservationClass
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Use predefined identifier as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        SpeciesObservationClassId Id
        { get; set; }

        /// <summary>
        /// Identifier for species observation class.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Get the name of this species observation class.
        /// </summary>
        /// <returns>The name of this species observation class.</returns>
        String GetName();
    }
}
