﻿using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains metadata about a specific data field used in species observation web services.
    /// </summary>
    public interface ISpeciesObservationFieldDescription : IDataId32
    {
        /// <summary>
        /// Species observation class that contains this data field.
        /// This property is mandatory. 
        /// </summary>
        ISpeciesObservationClass Class
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Definition of the data field according to TDWG or Swedish LifeWatch. 
        /// This property is mandatory. 
        /// </summary>
        String Definition
        { get; set; }

        /// <summary>
        /// Url to the original definition of the data field.
        /// </summary>
        String DefinitionUrl
        { get; set; }

        /// <summary>
        /// Description of the implementation of this field in the Swedish Species Observation Services.
        /// </summary>
        String Documentation
        { get; set; }

        /// <summary>
        /// Url to the original description of the implementation of this field in the Swedish Species Observation Services.
        /// </summary>
        String DocumentationUrl
        { get; set; }

        /// <summary>
        /// The LSID of this data field.
        /// This property is mandatory. 
        /// </summary>
        String Guid
        { get; set; }

        /// <summary>
        /// Classification of the importance of the field. The lower the value the more gerneral and important is this field.
        /// This property is mandatory. 
        /// </summary>
        Int32 Importance
        { get; set; }

        /// <summary>
        /// Indication of whether or not this data field is an accepted Darwin Core term.
        /// This property is mandatory. 
        /// </summary>
        Boolean IsAcceptedByTdwg
        { get; set; }

        /// <summary>
        /// Indication of whether this object represents a full class representing a group of fields or an acutal field.
        /// This property is mandatory. 
        /// </summary>
        Boolean IsClass
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is implemented in the Swedish Species Observation Service.
        /// This property is mandatory. 
        /// </summary>
        Boolean IsImplemented
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is mandatory at Swedsih Specis Observation Service. As a mandatory field values are excpected for all records.
        /// This property is mandatory. 
        /// </summary>
        Boolean IsMandatory
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is mandatory when harvesting from a Data provider.
        /// This property is mandatory.
        /// </summary>
        Boolean IsMandatoryFromProvider
        { get; set; }

        /// <summary>
        /// Indication of whether this field is obtained from the Data provider or generated by other means.
        /// This property is mandatory.
        /// </summary>
        Boolean IsObtainedFromProvider
        { get; set; }

        /// <summary>
        /// Indication of whether or not this field is planned for the Swedish Species Observation Service.
        /// This property is mandatory.
        /// </summary>
        Boolean IsPlanned
        { get; set; }

        /// <summary>
        /// Indicates if the species observation field, that is described
        /// in this class, can be used in flexible field search in class
        /// WebSpeciesObservationSearchCriteria.
        /// </summary>
        Boolean IsSearchable
        { get; set; }

        /// <summary>
        /// Indication if this species observation field
        /// may be used for sorting when species observations
        /// are retrieved from web service. 
        /// </summary>
        Boolean IsSortable
        { get; set; }

        /// <summary>
        /// A recommended field label to be used in presentations of data. The label is by defauld in english, but swedish labels exists for most fields. The language of the label is determined by the Locale.
        /// This property is mandatory.
        /// </summary>
        String Label
        { get; set; }

        /// <summary>
        /// List of provider specific field mapping objects containing
        /// docomuntetion on how the vales of data field are obtained
        /// from the data providers.
        /// </summary>
        SpeciesObservationFieldMappingList Mappings
        { get; set; }

        /// <summary>
        /// The name of this data field.
        /// This property is mandatory.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Species observation property that contains this data field.
        /// This property has the value null if
        /// property IsClass has the value True.
        /// </summary>
        ISpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Comments on the current or planned implementation.
        /// </summary>
        String Remarks
        { get; set; }

        /// <summary>
        /// Sort order of the data field.
        /// This property is mandatory.
        /// </summary>
        Int32 SortOrder
        { get; set; }

        /// <summary>
        /// Data type of the field values.
        /// This property is mandatory.
        /// </summary>
        DataType Type
        { get; set; }

        /// <summary>
        /// The UUID of this data field.
        /// This property is currently not used.
        /// </summary>
        String Uuid
        { get; set; }
    }
}
