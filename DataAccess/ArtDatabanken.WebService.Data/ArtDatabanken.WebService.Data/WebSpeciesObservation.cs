using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// This class contains information about one species observation.
    /// How much information that is included about the species
    /// observation depends on context and how much information
    /// that was requested. This class may contain anything from
    /// all information to a single value.
    /// 
    /// Predefined abstract species observation classes together with
    /// expected properties: A * after the name indicates that the class
    /// or property is not part of Darwin Core 1.5.
    /// 
    /// SpeciesObservation: AccessRights, BasisOfRecord,
    /// BibliographicCitation, CollectionCode, CollectionID,
    /// DataGeneralizations, DatasetID, DatasetName, DynamicProperties,
    /// Id*, InformationWithheld, InstitutionCode, InstitutionID,
    /// Language, Modified, Owner*, OwnerInstitutionCode, ReportedBy*,
    /// Rights, RightsHolder, SpeciesObservationURL*, Type,
    /// ValidationStatus*.
    /// 
    /// Conservation*: ConservationRelevant*, ActionPlan*, Natura2000*,
    /// ProtectedByLaw*, ProtectionLevel*, RedlistCategory*,
    /// SwedishImmigrationHistory*, SwedishOccurrence*.
    /// 
    /// Event: Day, End*, EndDayOfYear, EventDate, EventID, EventRemarks,
    /// EventTime, FieldNotes, FieldNumber, Habitat, Month,
    /// SamplingEffort, SamplingProtocol, Start*, StartDayOfYear,
    /// VerbatimEventDate, Year.
    /// 
    /// GeologicalContext: Bed, EarliestAgeOrLowestStage,
    /// EarliestEonOrLowestEonothem, EarliestEpochOrLowestSeries,
    /// EarliestEraOrLowestErathem, EarliestPeriodOrLowestSystem,
    /// Formation, GeologicalContextID, Group,
    /// HighestBiostratigraphicZone, LatestAgeOrHighestStage,
    /// LatestEonOrHighestEonothem, LatestEpochOrHighestSeries,
    /// LatestEraOrHighestErathem, LatestPeriodOrHighestSystem,
    /// LithostratigraphicTerms, LowestBiostratigraphicZone, Member.
    /// 
    /// Identification: DateIdentified, IdentificationID,
    /// IdentificationQualifier, IdentificationReferences,
    /// IdentificationRemarks, IdentifiedBy, UncertainDetermination*,
    /// TypeStatus.
    /// 
    /// Location: Continent, CoordinateM*, CoordinatePrecision,
    /// CoordinateSystemWkt*, CoordinateUncertaintyInMeters, CoordinateX*,
    /// CoordinateY*, CoordinateZ*, CoordinatePrecision, Country,
    /// CountryCode, County, DecimalLatitude, DecimalLongitude,
    /// FootprintSpatialFit, FootprintSRS, FootprintWKT, GeodeticDatum,
    /// GeoreferencedBy, GeoreferenceProtocol, GeoreferenceRemarks,
    /// GeoreferenceSources, GeoreferenceVerificationStatus,
    /// HigherGeography, HigherGeographyID, Island, IslandGroup, Locality,
    /// LocationAccordingTo, LocationRemarks, LocationURL*,
    /// MaximumDepthInMeters, MaximumDistanceAboveSurfaceInMeters,
    /// MaximumElevationInMeters, MinimumDepthInMeters,
    /// MinimumDistanceAboveSurfaceInMeters, MinimumElevationInMeters,
    /// Municipality, Parish*, PointRadiusSpatialFit, StateProvince,
    /// VerbatimCoordinates, VerbatimCoordinateSystem, VerbatimDepth,
    /// VerbatimElevation, VerbatimLatitude, VerbatimLocality,
    /// VerbatimLongitude, VerbatimSRS, WaterBody.
    /// 
    /// MeasurementOrFact: MeasurementAccuracy, MeasurementDeterminedBy,
    /// MeasurementDeterminedDate, MeasurementID, MeasurementMethod,
    /// MeasurementRemarks, MeasurementType, MeasurementUnit,
    /// MeasurementValue.
    /// 
    /// Occurrence: AssociatedMedia, AssociatedOccurrences,
    /// AssociatedReferences, AssociatedSequences, AssociatedTaxa,
    /// Behavior, CatalogNumber, Disposition, EstablishmentMeans,
    /// IndividualCount, IndividualID, IsNeverFoundObservation*,
    /// IsNaturalOccurrence*, IsNotRediscoveredObservation*, IsPositiveObservation*,
    /// LifeStage, OccurrenceDetails, OccurrenceID, OccurrenceRemarks,
    /// OccurrenceStatus, OccurrenceURL*, OtherCatalogNumbers,
    /// Preparations, PreviousIdentifications, Quantity*, QuantityUnit*
    /// RecordedBy, RecordNumber, ReproductiveCondition, Sex, Substrate*.
    /// 
    /// Project*: IsPublic*, ProjectCategory*, ProjectDescription*,
    /// ProjectEndDate*, ProjectID*, ProjectName*, ProjectOwner*,
    /// ProjectStartDate*, ProjectURL*, SurveyMethod*.
    /// 
    /// ResourceRelationship: RelatedResourceID, RelationshipAccordingTo,
    /// RelationshipEstablishedDate, RelationshipOfResource,
    /// RelationshipRemarks, ResourceID, ResourceRelationshipID.
    /// 
    /// Taxon: AcceptedNameUsage, AcceptedNameUsageID, Class,
    /// DyntaxaTaxonID*, Family, Genus, HigherClassification,
    /// InfraspecificEpithet, Kingdom, NameAccordingTo,
    /// NameAccordingToID, NamePublishedIn, NamePublishedInID,
    /// NomenclaturalCode, NomenclaturalStatus, Order, OriginalNameUsage,
    /// OriginalNameUsageID, ParentNameUsage, ParentNameUsageID, Phylum,
    /// ScientificName, ScientificNameAuthorship, ScientificNameID,
    /// SpecificEpithet, Subgenus, TaxonConceptID, TaxonConceptStatus*,
    /// TaxonID, TaxonRemarks, TaxonSortOrder*, TaxonomicStatus,
    /// TaxonRank, TaxonURL*, VerbatimTaxonRank, VernacularName.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataContract]
    public class WebSpeciesObservation : WebData
    {
        /// <summary>
        /// Information about one species observation. 
        /// How much information that a WebSpeciesObservation contains
        /// depends on context, how much information that was requested
        /// and how much information that is available.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationField> Fields { get; set; }
    }
}
