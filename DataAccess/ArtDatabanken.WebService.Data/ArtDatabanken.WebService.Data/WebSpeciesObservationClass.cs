using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a class
    /// that is included in a species observation.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationClass : WebData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        public WebSpeciesObservationClass(SpeciesObservationClassId classId)
        {
            Id = classId;
        }

        /// <summary>
        /// 
        /// </summary>
        public WebSpeciesObservationClass()
        {
           
        }

        /// <summary>
        /// Use predefined identifier as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        [DataMember]
        public SpeciesObservationClassId Id
        { get; set; }

        /// <summary>
        /// Identifier for species observation class.
        /// Set property Id to value None if property Identifier is used.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }
    }
}
