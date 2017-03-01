using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a property
    /// that is included in a species observation class.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationProperty : WebData
    {

        /// <summary>
        /// 
        /// </summary>
        public WebSpeciesObservationProperty()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speciesObservationPropertyId"></param>
        public WebSpeciesObservationProperty(SpeciesObservationPropertyId speciesObservationPropertyId)
        {
            Id = speciesObservationPropertyId;
        }
        /// <summary>
        /// Use predefined name as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        [DataMember]
        public SpeciesObservationPropertyId Id
        { get; set; }

        /// <summary>
        /// Identifier for species observation property.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }
    }
}
