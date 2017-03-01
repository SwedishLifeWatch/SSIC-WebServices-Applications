using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains a time step specific count of species observations.
    /// </summary>
    [DataContract]
    public class WebTimeStepSpeciesObservationCount : WebData
    {
        /// <summary>
        /// Number of species observations is based on selected species
        /// observation search criteria and time step properties.
        /// </summary>
        [DataMember]
        public Int64 Count { get; set; }

        /// <summary>
        /// The date time representing the start of the time step.
        /// This property is only applicable for some ot the TimeStepTypes.
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Id for the time step.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates whether or not Date is set and valid.
        /// </summary>
        [DataMember]
        public Boolean IsDateSpecified { get; set; }

        /// <summary>
        /// The name of the time step.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// The type of time step. This property gives indication on how to interprete the temporal extent of the time step.
        /// </summary>
        [DataMember]
        public Periodicity Periodicity { get; set; }
    }
}
