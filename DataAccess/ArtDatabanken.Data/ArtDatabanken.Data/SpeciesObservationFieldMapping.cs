using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information describing the mapping between Species observation fields and data fields provided by a specific Data provider (DataProviderId).
    /// </summary>
    public class SpeciesObservationFieldMapping : ISpeciesObservationFieldMapping
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Id of the Data provider.
        /// </summary>
        public Int32 DataProviderId
        { get; set; }

        /// <summary>
        /// Default value to be used as field value or if no value exists at the data provider.
        /// </summary>
        public String DefaultValue
        { get; set; }

        /// <summary>
        /// Documentation on how the values of this field are obtained from this Data provider.
        /// </summary>
        public String Documentation
        { get; set; }

        /// <summary>
        /// Id of the field.
        /// </summary>
        public Int32 FieldId
        { get; set; }

        public string GUID
        { get; set; }

        /// <summary>
        /// Id of this Field Mapping object.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is implemented for this Data provider
        /// </summary>
        public Boolean IsImplemented
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is planned for this Data provider
        /// </summary>
        public Boolean IsPlanned
        { get; set; }

        /// <summary>
        /// Name of the method that generates field values if no single provider data field corresponds to the Darwin Core field or if not default value can be set.
        /// </summary>
        public String Method
        { get; set; }

        /// <summary>
        /// Possible project id if field is related to a project parameter.
        /// </summary>
        public Int32? ProjectId { get; set; }

        /// <summary>
        /// The ProjectName
        /// </summary>
        public string ProjectName
        { get; set; }



        /// <summary>
        /// The PropertyIdentifier
        /// </summary>
        public string PropertyIdentifier
        { get; set; }


        /// <summary>
        /// The name of the Data provider field that corresponds to the species observation field.
        /// </summary>
        public String ProviderFieldName
        { get; set; }
    }
}
