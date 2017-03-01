using System;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Class used to hold information that other parts of the code can use to
    /// distinguish between different configurations related to species observations.
    /// </summary>
    public class SpeciesObservationConfiguration
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static SpeciesObservationConfiguration()
        {
            IsElasticsearchUsed = false;
        }

        /// <summary>
        /// Define if Elasticsearch is used.
        /// </summary>
        public static Boolean IsElasticsearchUsed { get; set; }
    }
}
