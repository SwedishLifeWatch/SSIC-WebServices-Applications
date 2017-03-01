using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information describing the mapping between Species observation fields and data fields provided by a specific Data provider (DataProviderId). 
    /// </summary>
    public interface ISpeciesObservationFieldMapping : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Id of the Data source (Data provider).
        /// </summary>
        Int32 DataProviderId
        { get; set; }

        /// <summary>
        /// Default value to be used as field value or if no value exists at the data provider.
        /// </summary>
        String DefaultValue
        { get; set; }

        /// <summary>
        /// Documentation on how the values of this field are obtained from this Data provider.
        /// </summary>
        String Documentation
        { get; set; }

        /// <summary>
        /// Id of the field.
        /// </summary>
        Int32 FieldId
        { get; set; }

        string GUID
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is implemented for this Data provider
        /// </summary>
        Boolean IsImplemented
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is planned for this Data provider
        /// </summary>
        Boolean IsPlanned
        { get; set; }

        /// <summary>
        /// Name of the method that generates field values if no single provider data field corresponds to the Darwin Core field or if not default value can be set.
        /// </summary>
        String Method
        { get; set; }

        /// <summary>
        /// Possible project id if field is related to a project parameter.
        /// </summary>
        Int32? ProjectId { get; set; }

        /// <summary>
        /// The ProjectName
        /// </summary>
        string ProjectName
        { get; set; }

        /// <summary>
        /// The PropertyIdentifier
        /// </summary>
        string PropertyIdentifier
        { get; set; }

        /// <summary>
        /// The name of the Data provider field that corresponds to the species observation field.
        /// </summary>
        String ProviderFieldName
        { get; set; }
    }
}
