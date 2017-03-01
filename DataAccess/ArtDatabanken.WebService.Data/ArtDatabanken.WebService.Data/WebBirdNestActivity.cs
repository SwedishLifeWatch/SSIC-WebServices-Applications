using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents information about a bird nest activity.
    /// Bird nest activities are ordered.
    /// Lower id indicates a higher probability of bird breeding.
    /// This class is only used by SwedishSpeciesObservationSOAPService.
    /// </summary>
    [DataContract]
    public class WebBirdNestActivity : WebData
    {
        /// <summary>
        /// Id for this bird nest activity.
        /// Bird nest activities are ordered.
        /// Lower id indicates a higher probability of bird breeding.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this bird nest activity.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
