using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information about a species
    /// observation. This interface is used when a flexible species 
    /// observation format is required. This interface also includes 
    /// all properties available in Darwin Core 1.5 se interface IDarwinCore.
    /// Further information about the Darwin Core 1.5 properties can
    /// be found at http://rs.tdwg.org/dwc/terms/.
    /// </summary>
    public interface ISpeciesObservation : IDataId64
    {
        /// <summary>
        /// Darwin Core term name: dcterms:accessRights.
        /// Information about who can access the resource or
        /// an indication of its security status.
        /// Access Rights may include information regarding
        /// access or restrictions based on privacy, security,
        /// or other policies.
        /// In Species Gateway this is a value between 1 to 6.
        /// 1 indicates access and 6 is the highest security level.
        /// </summary>
        String AccessRights { get; set; }

        /// <summary>
        /// Darwin Core term name: basisOfRecord.
        /// The specific nature of the data record -
        /// a subtype of the dcterms:type.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Darwin Core Type Vocabulary
        /// (http://rs.tdwg.org/dwc/terms/type-vocabulary/index.htm).
        /// In Species Gateway this property has the value
        /// HumanObservation.
        /// </summary>
        String BasisOfRecord
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:bibliographicCitation.
        /// A bibliographic reference for the resource as a statement
        /// indicating how this record should be cited (attributed)
        /// when used.
        /// Recommended practice is to include sufficient
        /// bibliographic detail to identify the resource as
        /// unambiguously as possible.
        /// This property is currently not used.
        /// </summary>
        String BibliographicCitation
        { get; set; }

        /// <summary>
        /// Darwin Core term name: collectionCode.
        /// The name, acronym, coden, or initialism identifying the 
        /// collection or data set from which the record was derived.
        /// </summary>
        String CollectionCode
        { get; set; }

        /// <summary>
        /// Darwin Core term name: collectionID.
        /// An identifier for the collection or dataset from which
        /// the record was derived.
        /// For physical specimens, the recommended best practice is
        /// to use the identifier in a collections registry such as
        /// the Biodiversity Collections Index
        /// (http://www.biodiversitycollectionsindex.org/).
        /// </summary>
        String CollectionID
        { get; set; }

        /// <summary>
        /// Conservation related information about the taxon that
        /// the species observation is attached to.
        /// </summary>
        ISpeciesObservationConservation Conservation
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Darwin Core term name: dataGeneralizations.
        /// Actions taken to make the shared data less specific or
        /// complete than in its original form.
        /// Suggests that alternative data of higher quality
        /// may be available on request.
        /// This property is currently not used.
        /// </summary>
        String DataGeneralizations
        { get; set; }

        /// <summary>
        /// Darwin Core term name: datasetID.
        /// An identifier for the set of data.
        /// May be a global unique identifier or an identifier
        /// specific to a collection or institution.
        /// </summary>
        String DatasetID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: datasetName.
        /// The name identifying the data set
        /// from which the record was derived.
        /// </summary>
        String DatasetName
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dynamicProperties.
        /// A list (concatenated and separated) of additional
        /// measurements, facts, characteristics, or assertions
        /// about the record. Meant to provide a mechanism for
        /// structured content such as key-value pairs.
        /// This property is currently not used.
        /// </summary>
        String DynamicProperties
        { get; set; }

        /// <summary>
        /// Darwin Core term name: Event.
        /// The category of information pertaining to an event (an 
        /// action that occurs at a place and during a period of time).
        /// </summary>
        ISpeciesObservationEvent Event
        { get; set; }

        /// <summary>
        /// Generic representation of all data that this
        /// species observation contains.
        /// </summary>
        SpeciesObservationFieldList Fields { get; set; }

        /// <summary>
        /// Darwin Core term name: GeologicalContext.
        /// The category of information pertaining to a location
        /// within a geological context, such as stratigraphy.
        /// This property is currently not used.
        /// </summary>
        ISpeciesObservationGeologicalContext GeologicalContext
        { get; set; }

        /// <summary>
        /// Darwin Core term name: Identification.
        /// The category of information pertaining to taxonomic
        /// determinations (the assignment of a scientific name).
        /// </summary>
        ISpeciesObservationIdentification Identification
        { get; set; }

        /// <summary>
        /// Darwin Core term name: informationWithheld.
        /// Additional information that exists, but that has
        /// not been shared in the given record.
        /// This property is currently not used.
        /// </summary>
        String InformationWithheld
        { get; set; }

        /// <summary>
        /// Darwin Core term name: institutionCode.
        /// The name (or acronym) in use by the institution
        /// having custody of the object(s) or information
        /// referred to in the record.
        /// Currently this property has the value ArtDatabanken.
        /// </summary>
        String InstitutionCode
        { get; set; }

        /// <summary>
        /// Darwin Core term name: institutionID.
        /// An identifier for the institution having custody of 
        /// the object(s) or information referred to in the record.
        /// This property is currently not used.
        /// </summary>
        String InstitutionID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:language.
        /// A language of the resource.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as RFC 4646 [RFC4646].
        /// This property is currently not used.
        /// </summary>
        String Language
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:Location.
        /// A spatial region or named place. For Darwin Core,
        /// a set of terms describing a place, whether named or not.
        /// </summary>
        ISpeciesObservationLocation Location
        { get; set; }

        /// <summary>
        /// Darwin Core term name: MeasurementOrFact.
        /// The category of information pertaining to measurements,
        /// facts, characteristics, or assertions about a resource
        /// (instance of data record, such as Occurrence, Taxon,
        /// Location, Event).
        /// This property is currently not used.
        /// </summary>
        ISpeciesObservationMeasurementOrFact MeasurementOrFact
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:modified.
        /// The most recent date-time on which the resource was changed.
        /// For Darwin Core, recommended best practice is to use an
        /// encoding scheme, such as ISO 8601:2004(E).
        /// This property is currently not used.
        /// </summary>
        DateTime? Modified
        { get; set; }

        /// <summary>
        /// Darwin Core term name: Occurrence.
        /// The category of information pertaining to evidence of
        /// an occurrence in nature, in a collection, or in a
        /// dataset (specimen, observation, etc.).
        /// </summary>
        ISpeciesObservationOccurrence Occurrence
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of the organization or person that
        /// owns the species observation.
        /// </summary>
        String Owner
        { get; set; }

        /// <summary>
        /// Darwin Core term name: ownerInstitutionCode.
        /// The name (or acronym) in use by the institution having
        /// ownership of the object(s) or information referred
        /// to in the record.
        /// This property is currently not used.
        /// </summary>
        String OwnerInstitutionCode
        { get; set; }

        /// <summary>
        /// Information about the project in which this
        /// species observation was made.
        /// </summary>
        ISpeciesObservationProject Project
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:references.
        /// A related resource that is referenced, cited,
        /// or otherwise pointed to by the described resource.
        /// This property is currently not used.
        /// </summary>
        String References
        { get; set; }

        /// <summary>
        /// Name of the person that reported the species observation.
        /// </summary>
        String ReportedBy
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date and time when the species observation was reported.
        /// </summary>
        DateTime? ReportedDate
        { get; set; }

        /// <summary>
        /// Darwin Core term name: ResourceRelationship.
        /// The category of information pertaining to relationships
        /// between resources (instances of data records, such as
        /// Occurrences, Taxa, Locations, Events).
        /// This property is currently not used.
        /// </summary>
        ISpeciesObservationResourceRelationship ResourceRelationship
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:rights.
        /// Information about rights held in and over the resource.
        /// Typically, rights information includes a statement
        /// about various property rights associated with the resource,
        /// including intellectual property rights.
        /// This property is currently not used.
        /// </summary>
        String Rights
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:rightsHolder.
        /// A person or organization owning or
        /// managing rights over the resource.
        /// This property is currently not used.
        /// </summary>
        String RightsHolder
        { get; set; }

        /// <summary>
        /// Web address that leads to more information about the
        /// species observation. The information should be accessible
        /// from the most commonly used web browsers.
        /// </summary>
        String SpeciesObservationURL
        { get; set; }

        /// <summary>
        /// Darwin Core term name: Taxon.
        /// The category of information pertaining to taxonomic names,
        /// taxon name usages, or taxon concepts.
        /// </summary>
        ISpeciesObservationTaxon Taxon
        { get; set; }

        /// <summary>
        /// Darwin Core term name: dcterms:type.
        /// The nature or genre of the resource.
        /// For Darwin Core, recommended best practice is
        /// to use the name of the class that defines the
        /// root of the record.
        /// This property is currently not used.
        /// </summary>
        String Type
        { get; set; }

        /// <summary>
        /// Information about current validation status
        /// for the species observation.
        /// </summary>
        String ValidationStatus
        { get; set; }
    }
}
