using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information describing the mapping between Species observation fields and data fields provided by a specific Data provider (DataProviderId).
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationFieldMapping : WebData
    {
        /// <summary>
        /// Id of the Data provider.
        /// </summary>
        [DataMember]
        public Int32 DataProviderId
        { get; set; }

        /// <summary>
        /// Default value to be used as field value or if no value exists at the data provider.
        /// </summary>
        [DataMember]
        public String DefaultValue
        { get; set; }

        /// <summary>
        /// Documentation on how the values of this field are obtained from this Data provider.
        /// </summary>
        [DataMember]
        public String Documentation
        { get; set; }

        /// <summary>
        /// Id of the species observation field description.
        /// </summary>
        [DataMember]
        public Int32 FieldId
        { get; set; }

        /// <summary>
        /// Id of this Field Mapping object.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// A string that can be used to map fields by their lsid for this Data provider
        /// </summary>
        public String Information
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is implemented for this Data provider
        /// </summary>
        [DataMember]
        public Boolean IsImplemented
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is planned for this Data provider
        /// </summary>
        [DataMember]
        public Boolean IsPlanned
        { get; set; }

        /// <summary>
        /// Name of the method that generates field values if no single provider data field corresponds to the Darwin Core field or if not default value can be set.
        /// </summary>
        [DataMember]
        public String Method
        { get; set; }

        /// <summary>
        /// The name of the Data provider field that corresponds to the species observation field.
        /// </summary>
        [DataMember]
        public String ProviderFieldName
        { get; set; }
    }
}
