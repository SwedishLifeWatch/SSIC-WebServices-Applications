using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a class
    /// that is included in a species observation.
    /// </summary>
    public class SpeciesObservationClass : ISpeciesObservationClass
    {
        public SpeciesObservationClass(SpeciesObservationClassId classId)
        {
            Id = classId;
        }

        public SpeciesObservationClass()
        {
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Use predefined identifier as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        public SpeciesObservationClassId Id
        { get; set; }

        /// <summary>
        /// Identifier for species observation class.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// Get the name of this species observation class.
        /// </summary>
        /// <returns>The name of this species observation class.</returns>
        public String GetName()
        {
            if (Id == SpeciesObservationClassId.None)
            {
                return Identifier;
            }
            else
            {
                return Id.ToString();
            }
        }
    }
}
