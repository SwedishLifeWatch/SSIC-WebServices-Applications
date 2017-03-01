using System;

namespace ArtDatabanken.Data
{    
    /// <summary>
    /// This class contains information about a property
    /// that is included in a species observation class.
    /// </summary>
    public class SpeciesObservationProperty : ISpeciesObservationProperty
    {
        public SpeciesObservationProperty(SpeciesObservationPropertyId speciesObservationPropertyId)
        {
            Id = speciesObservationPropertyId;
        }

        public SpeciesObservationProperty()
        {
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Use predefined name as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        public SpeciesObservationPropertyId Id
        { get; set; }

        /// <summary>
        /// Identifier for species observation property.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// Get the name of this species observation property.
        /// </summary>
        /// <returns>The name of this species observation property.</returns>
        public String GetName()
        {
            if (Id == SpeciesObservationPropertyId.None)
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
