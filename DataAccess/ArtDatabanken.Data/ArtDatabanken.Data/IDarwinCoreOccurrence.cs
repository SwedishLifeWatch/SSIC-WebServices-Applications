using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains occurrence information about a species
    /// observation in Darwin Core 1.5 compatible format.
    /// Further information about the properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public interface IDarwinCoreOccurrence
    {
        /// <summary>
        /// Darwin Core term name: associatedMedia.
        /// A list (concatenated and separated) of identifiers
        /// (publication, global unique identifier, URI) of
        /// media associated with the Occurrence.
        /// This property is currently not used.
        /// </summary>
        String AssociatedMedia
        { get; set; }

        /// <summary>
        /// Darwin Core term name: associatedOccurrences.
        /// A list (concatenated and separated) of identifiers of
        /// other Occurrence records and their associations to
        /// this Occurrence.
        /// This property is currently not used.
        /// </summary>
        String AssociatedOccurrences
        { get; set; }

        /// <summary>
        /// Darwin Core term name: associatedReferences.
        /// A list (concatenated and separated) of identifiers
        /// (publication, bibliographic reference, global unique
        /// identifier, URI) of literature associated with
        /// the Occurrence.
        /// This property is currently not used.
        /// </summary>
        String AssociatedReferences
        { get; set; }

        /// <summary>
        /// Darwin Core term name: associatedSequences.
        /// A list (concatenated and separated) of identifiers of
        /// other Occurrence records and their associations to
        /// this Occurrence.
        /// This property is currently not used.
        /// </summary>
        String AssociatedSequences
        { get; set; }

        /// <summary>
        /// Darwin Core term name: associatedTaxa.
        /// A list (concatenated and separated) of identifiers or
        /// names of taxa and their associations with the Occurrence.
        /// This property is currently not used.
        /// </summary>
        String AssociatedTaxa
        { get; set; }

        /// <summary>
        /// Darwin Core term name: behavior.
        /// A description of the behavior shown by the subject at
        /// the time the Occurrence was recorded.
        /// Recommended best practice is to use a controlled vocabulary.
        /// </summary>
        String Behavior
        { get; set; }

        /// <summary>
        /// Darwin Core term name: catalogNumber.
        /// An identifier (preferably unique) for the record
        /// within the data set or collection.
        /// Currently this id does not work as supposed. For example: 
        /// one specific observation may have another id tomorrow.
        /// </summary>
        String CatalogNumber
        { get; set; }

        /// <summary>
        /// Darwin Core term name: disposition.
        /// The current state of a specimen with respect to the
        /// collection identified in collectionCode or collectionID.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        String Disposition
        { get; set; }

        /// <summary>
        /// Darwin Core term name: establishmentMeans.
        /// The process by which the biological individual(s)
        /// represented in the Occurrence became established at the
        /// location.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        String EstablishmentMeans
        { get; set; }

        /// <summary>
        /// Darwin Core term name: individualCount.
        /// The number of individuals represented present
        /// at the time of the Occurrence.
        /// </summary>
        String IndividualCount
        { get; set; }

        /// <summary>
        /// Darwin Core term name: individualID.
        /// An identifier for an individual or named group of
        /// individual organisms represented in the Occurrence.
        /// Meant to accommodate resampling of the same individual
        /// or group for monitoring purposes. May be a global unique
        /// identifier or an identifier specific to a data set.
        /// This property is currently not used.
        /// </summary>
        String IndividualID
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this species occurrence is natural or
        /// if it is a result of human activity.
        /// </summary>
        Boolean IsNaturalOccurrence
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this observation is a never found observation.
        /// "Never found observation" is an observation that says
        /// that the specified species was not found in a location
        /// deemed appropriate for the species.
        /// </summary>
        Boolean IsNeverFoundObservation
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this observation is a 
        /// not rediscovered observation.
        /// "Not rediscovered observation" is an observation that says
        /// that the specified species was not found in a location
        /// where it has previously been observed.
        /// </summary>
        Boolean IsNotRediscoveredObservation
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this observation is a positive observation.
        /// "Positive observation" is a normal observation indicating
        /// that a species has been seen at a specified location.
        /// </summary>
        Boolean IsPositiveObservation
        { get; set; }

        /// <summary>
        /// Darwin Core term name: lifeStage.
        /// The age class or life stage of the biological individual(s)
        /// at the time the Occurrence was recorded.
        /// Recommended best practice is to use a controlled vocabulary.
        /// </summary>
        String LifeStage
        { get; set; }

        /// <summary>
        /// Darwin Core term name: occurrenceID.
        /// An identifier for the Occurrence (as opposed to a
        /// particular digital record of the occurrence).
        /// In the absence of a persistent global unique identifier,
        /// construct one from a combination of identifiers in
        /// the record that will most closely make the occurrenceID
        /// globally unique.
        /// The format LSID (Life Science Identifiers) is used as GUID
        /// (Globally unique identifier) for species observations.
        /// Currently known GUIDs:
        /// Species Gateway (Artportalen) 1,
        /// urn:lsid:artportalen.se:Sighting:{reporting system}.{id}
        /// where {reporting system} is one of Bird, Bugs, Fish, 
        /// MarineInvertebrates, PlantAndMushroom or Vertebrate.
        /// Species Gateway (Artportalen) 2,
        /// urn:lsid:artportalen.se:Sighting:{id}
        /// Red list database: urn:lsid:artdata.slu.se:SpeciesObservation:{id}
        /// </summary>
        String OccurrenceID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: occurrenceRemarks.
        /// Comments or notes about the Occurrence.
        /// </summary>
        String OccurrenceRemarks
        { get; set; }

        /// <summary>
        /// Darwin Core term name: occurrenceStatus.
        /// A statement about the presence or absence of a Taxon at a
        /// Location.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        String OccurrenceStatus
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// occurrence. The information should be accessible
        /// from the most commonly used web browsers.
        /// </summary>
        String OccurrenceURL
        { get; set; }

        /// <summary>
        /// Darwin Core term name: otherCatalogNumbers.
        /// A list (concatenated and separated) of previous or
        /// alternate fully qualified catalog numbers or other
        /// human-used identifiers for the same Occurrence,
        /// whether in the current or any other data set or collection.
        /// This property is currently not used.
        /// </summary>
        String OtherCatalogNumbers
        { get; set; }

        /// <summary>
        /// Darwin Core term name: preparations.
        /// A list (concatenated and separated) of preparations
        /// and preservation methods for a specimen.
        /// This property is currently not used.
        /// </summary>
        String Preparations
        { get; set; }

        /// <summary>
        /// Darwin Core term name: previousIdentifications.
        /// A list (concatenated and separated) of previous
        /// assignments of names to the Occurrence.
        /// This property is currently not used.
        /// </summary>
        String PreviousIdentifications
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Quantity of observed species, for example distribution area.
        /// Unit is specified in property QuantitiyUnit.
        /// </summary>
        String Quantity
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Unit for quantity value of observed species.
        /// </summary>
        String QuantityUnit
        { get; set; }

        /// <summary>
        /// Darwin Core term name: recordedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations responsible for recording the
        /// original Occurrence. The primary collector or observer,
        /// especially one who applies a personal identifier
        /// (recordNumber), should be listed first.
        /// </summary>
        String RecordedBy
        { get; set; }

        /// <summary>
        /// Darwin Core term name: recordNumber.
        /// An identifier given to the Occurrence at the time it was
        /// recorded. Often serves as a link between field notes and
        /// an Occurrence record, such as a specimen collector's number.
        /// This property is currently not used.
        /// </summary>
        String RecordNumber
        { get; set; }

        /// <summary>
        /// Darwin Core term name: reproductiveCondition.
        /// The reproductive condition of the biological individual(s)
        /// represented in the Occurrence.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        String ReproductiveCondition
        { get; set; }

        /// <summary>
        /// Darwin Core term name: sex.
        /// The sex of the biological individual(s) represented in
        /// the Occurrence.
        /// Recommended best practice is to use a controlled vocabulary.
        /// </summary>
        String Sex
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Substrate on which the species was observed.
        /// </summary>
        String Substrate
        { get; set; }
    }
}
