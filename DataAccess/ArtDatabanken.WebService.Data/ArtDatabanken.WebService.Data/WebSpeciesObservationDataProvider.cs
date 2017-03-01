using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish life watch.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationDataProvider : WebData
    {
        /// <summary>
        /// Start date for when species observations
        /// were inserted into the data source.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime BeginHarvestFromDate { get; set; }

        /// <summary>
        /// Email address to contact person.
        /// </summary>
        [DataMember]
        public String ContactEmail { get; set; }

        /// <summary>
        /// Name of contact person.
        /// </summary>
        [DataMember]
        public String ContactPerson { get; set; }

        /// <summary>
        /// Information about the data source.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of
        /// the record holding the information included in this object. 
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Id for the data source.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if this data provider is harvested or not.
        /// </summary>
        [DataMember]
        public Boolean IsActiveHarvest { get; set; }

        /// <summary>
        /// Indicates if property MaxChangeId has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsMaxChangeIdSpecified { get; set; }

        /// <summary>
        /// Latest date for when any of the species observations
        /// from this data source was changed in the data source.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime LatestChangedDate { get; set; }

        /// <summary>
        /// Latest date for which changes of species observations
        /// was retrieved from the data provider.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime LatestHarvestDate { get; set; }

        /// <summary>
        /// Highest id of the changes in species observations that
        /// have been downloaded from the data source.
        /// Not all data providers has this value.
        /// Property IsMaxChangeIdSpecified indicates if this
        /// property has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int64 MaxChangeId { get; set; }

        /// <summary>
        /// Name of the data source.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Number of non public species observations in data source.
        /// </summary>
        [DataMember]
        public Int64 NonPublicSpeciesObservationCount { get; set; }

        /// <summary>
        /// Name of the organization that is responsible
        /// for the data source.
        /// </summary>
        [DataMember]
        public String Organization { get; set; }

        /// <summary>
        /// Number of public species observations in data source.
        /// </summary>
        [DataMember]
        public Int64 PublicSpeciesObservationCount { get; set; }

        /// <summary>
        /// Total number of species observations in data source.
        /// </summary>
        [DataMember]
        public Int64 SpeciesObservationCount { get; set; }

        /// <summary>
        /// Web address (that can be used in a web browser) where more
        /// information about the data source can be found.
        /// </summary>
        [DataMember]
        public String Url { get; set; }
    }
}
