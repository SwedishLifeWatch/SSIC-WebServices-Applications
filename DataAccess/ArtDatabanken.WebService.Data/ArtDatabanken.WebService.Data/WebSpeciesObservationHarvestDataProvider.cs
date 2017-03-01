using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish life watch.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationHarvestDataProvider : WebData
    {
        /// <summary>
        /// Id for the data source.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if this data provider is harvested or not.
        /// </summary>
        [DataMember]
        public Boolean IsSelected { get; set; }

        /// <summary>
        /// Name of the data source.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
