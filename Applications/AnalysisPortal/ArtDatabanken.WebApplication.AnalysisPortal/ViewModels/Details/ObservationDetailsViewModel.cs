//using System.Collections.Generic;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;

//namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details
//{
//    /// <summary>
//    /// View details model for species observations
//    /// </summary>
//    public class ObservationDetailsViewModel
//    {

//        /// <summary>
//        /// Private field for labels
//        /// </summary>
//        private readonly ModelLabels labels = new ModelLabels();
        
//        /// <summary>
//        /// Dictionary KEY as resource string langugae supported valu and VALUE as SpeciesObservationPropertyValues class
//        /// </summary>
//        public Dictionary<string, SpeciesObservationModelHelper> DetailsList { get; set; }

//        /// <summary>
//        /// All species observation properties. Note! these mig be indicated as not referenced in code by they are referenced by reflection. 
//        /// Don't remove them.
//        /// </summary>
//        public string AccessRights { get; set; }
//        public string BasisOfRecord { get; set; }
//        public string BibliographicCitation { get; set; }
//        public string CollectionCode { get; set; }
//        public string CollectionID { get; set; }
//        public string ActionPlan { get; set; }
//        public string ConservationRelevant { get; set; }
//        public string Natura2000 { get; set; }
//        public string ProtectedByLaw { get; set; }
//        public string ProtectionLevel { get; set; }
//        public string RedlistCategory { get; set; }
//        public string SwedishImmigrationHistory { get; set; }
//        public string SwedishOccurrence { get; set; }
//        public string DataGeneralizations { get; set; }
//        public string DatasetID { get; set; }
//        public string DatasetName { get; set; }
//        public string DynamicProperties { get; set; }
//        public string Day { get; set; }
//        public string End { get; set; }
//        public string EndDayOfYear { get; set; }
//        public string EventDate { get; set; }
//        public string EventID { get; set; }
//        public string EventRemarks { get; set; }
//        public string EventTime { get; set; }
//        public string FieldNotes { get; set; }
//        public string FieldNumber { get; set; }
//        public string Habitat { get; set; }
//        public string Month { get; set; }
//        public string SamplingEffort { get; set; }
//        public string SamplingProtocol { get; set; }
//        public string Start { get; set; }
//        public string StartDayOfYear { get; set; }
//        public string VerbatimEventDate { get; set; }
//        public string Year { get; set; }
//        public string Bed { get; set; }
//        public string EarliestAgeOrLowestStage { get; set; }
//        public string EarliestEonOrLowestEonothem { get; set; }
//        public string EarliestEpochOrLowestSeries { get; set; }
//        public string EarliestEraOrLowestErathem { get; set; }
//        public string EarliestPeriodOrLowestSystem { get; set; }
//        public string Formation { get; set; }
//        public string GeologicalContextID { get; set; }
//        public string Group { get; set; }
//        public string HighestBiostratigraphicZone { get; set; }
//        public string LatestAgeOrHighestStage { get; set; }
//        public string LatestEonOrHighestEonothem { get; set; }
//        public string LatestEpochOrHighestSeries { get; set; }
//        public string LatestEraOrHighestErathem { get; set; }
//        public string LatestPeriodOrHighestSystem { get; set; }
//        public string LithostratigraphicTerms { get; set; }
//        public string LowestBiostratigraphicZone { get; set; }
//        public string Member { get; set; }
//        public string DateIdentified { get; set; }
//        public string Id { get; set; }
//        public string IdentificationID { get; set; }
//        public string IdentificationQualifier { get; set; }
//        public string IdentificationReferences { get; set; }
//        public string IdentificationRemarks { get; set; }
//        public string IdentificationVerificationStatus { get; set; }
//        public string IdentifiedBy { get; set; }
//        public string TypeStatus { get; set; }
//        public string UncertainDetermination { get; set; }
//        public string InformationWithheld { get; set; }
//        public string InstitutionCode { get; set; }
//        public string InstitutionID { get; set; }
//        public string Language { get; set; }
//        public string Continent { get; set; }
//        public string CoordinateM { get; set; }
//        public string CoordinatePrecision { get; set; }
//        public string CoordinateSystemWkt { get; set; }
//        public string CoordinateUncertaintyInMeters { get; set; }
//        public string CoordinateX { get; set; }
//        public string CoordinateY { get; set; }
//        public string CoordinateZ { get; set; }
//        public string Country { get; set; }
//        public string CountryCode { get; set; }
//        public string County { get; set; }
//        public string DecimalLatitude { get; set; }
//        public string DecimalLongitude { get; set; }
//        public string FootprintSpatialFit { get; set; }
//        public string FootprintSRS { get; set; }
//        public string FootprintWKT { get; set; }
//        public string GeodeticDatum { get; set; }
//        public string GeoreferencedBy { get; set; }
//        public string GeoreferencedDate { get; set; }
//        public string GeoreferenceProtocol { get; set; }
//        public string GeoreferenceRemarks { get; set; }
//        public string GeoreferenceSources { get; set; }
//        public string GeoreferenceVerificationStatus { get; set; }
//        public string HigherGeography { get; set; }
//        public string HigherGeographyID { get; set; }
//        public string Island { get; set; }
//        public string IslandGroup { get; set; }
//        public string Locality { get; set; }
//        public string LocationAccordingTo { get; set; }
//        public string LocationId { get; set; }
//        public string LocationRemarks { get; set; }
//        public string LocationURL { get; set; }
//        public string MaximumDepthInMeters { get; set; }
//        public string MaximumDistanceAboveSurfaceInMeters { get; set; }
//        public string MaximumElevationInMeters { get; set; }
//        public string MinimumDepthInMeters { get; set; }
//        public string MinimumDistanceAboveSurfaceInMeters { get; set; }
//        public string MinimumElevationInMeters { get; set; }
//        public string Municipality { get; set; }
//        public string Parish { get; set; }
//        public string PointRadiusSpatialFit { get; set; }
//        public string StateProvince { get; set; }
//        public string VerbatimCoordinates { get; set; }
//        public string VerbatimCoordinateSystem { get; set; }
//        public string VerbatimDepth { get; set; }
//        public string VerbatimElevation { get; set; }
//        public string VerbatimLatitude { get; set; }
//        public string VerbatimLocality { get; set; }
//        public string VerbatimLongitude { get; set; }
//        public string VerbatimSRS { get; set; }
//        public string WaterBody { get; set; }
//        public string MeasurementAccuracy { get; set; }
//        public string MeasurementDeterminedBy { get; set; }
//        public string MeasurementDeterminedDate { get; set; }
//        public string MeasurementID { get; set; }
//        public string MeasurementMethod { get; set; }
//        public string MeasurementRemarks { get; set; }
//        public string MeasurementType { get; set; }
//        public string MeasurementUnit { get; set; }
//        public string MeasurementValue { get; set; }
//        public string Modified { get; set; }
//        public string AssociatedMedia { get; set; }
//        public string AssociatedOccurrences { get; set; }
//        public string AssociatedReferences { get; set; }
//        public string AssociatedSequences { get; set; }
//        public string AssociatedTaxa { get; set; }
//        public string Behavior { get; set; }
//        public string CatalogNumber { get; set; }
//        public string Disposition { get; set; }
//        public string EstablishmentMeans { get; set; }
//        public string IndividualCount { get; set; }
//        public string IndividualID { get; set; }
//        public string IsNaturalOccurrence { get; set; }
//        public string IsNeverFoundObservation { get; set; }
//        public string IsNotRediscoveredObservation { get; set; }
//        public string IsPositiveObservation { get; set; }
//        public string LifeStage { get; set; }
//        public string OccurrenceID { get; set; }
//        public string OccurrenceRemarks { get; set; }
//        public string OccurrenceStatus { get; set; }
//        public string OccurrenceURL { get; set; }
//        public string OtherCatalogNumbers { get; set; }
//        public string Preparations { get; set; }
//        public string PreviousIdentifications { get; set; }
//        public string Quantity { get; set; }
//        public string QuantityUnit { get; set; }
//        public string RecordedBy { get; set; }
//        public string RecordNumber { get; set; }
//        public string ReproductiveCondition { get; set; }
//        public string Sex { get; set; }
//        public string Substrate { get; set; }
//        public string Owner { get; set; }
//        public string OwnerInstitutionCode { get; set; }
//        public string IsPublic { get; set; }
//        public string ProjectCategory { get; set; }
//        public string ProjectDescription { get; set; }
//        public string ProjectEndDate { get; set; }
//        public string ProjectID { get; set; }
//        public string ProjectName { get; set; }
//        public string ProjectOwner { get; set; }
//        public string ProjectStartDate { get; set; }
//        public string ProjectURL { get; set; }
//        public string SurveyMethod { get; set; }
//        public string References { get; set; }
//        public string ReportedBy { get; set; }
//        public string RelatedResourceID { get; set; }
//        public string RelationshipAccordingTo { get; set; }
//        public string RelationshipEstablishedDate { get; set; }
//        public string RelationshipOfResource { get; set; }
//        public string RelationshipRemarks { get; set; }
//        public string ResourceID { get; set; }
//        public string ResourceRelationshipID { get; set; }
//        public string Rights { get; set; }
//        public string RightsHolder { get; set; }
//        public string SpeciesObservationURL { get; set; }
//        public string AcceptedNameUsage { get; set; }
//        public string AcceptedNameUsageID { get; set; }
//        public string Class { get; set; }
//        public string DyntaxaTaxonID { get; set; }
//        public string Family { get; set; }
//        public string Genus { get; set; }
//        public string HigherClassification { get; set; }
//        public string InfraspecificEpithet { get; set; }
//        public string Kingdom { get; set; }
//        public string NameAccordingTo { get; set; }
//        public string NameAccordingToID { get; set; }
//        public string NamePublishedIn { get; set; }
//        public string NamePublishedInID { get; set; }
//        public string NamePublishedInYear { get; set; }
//        public string NomenclaturalCode { get; set; }
//        public string NomenclaturalStatus { get; set; }
//        public string Order { get; set; }
//        public string OrganismGroup { get; set; }
//        public string OriginalNameUsage { get; set; }
//        public string OriginalNameUsageID { get; set; }
//        public string ParentNameUsage { get; set; }
//        public string ParentNameUsageID { get; set; }
//        public string Phylum { get; set; }
//        public string ScientificName { get; set; }
//        public string ScientificNameAuthorship { get; set; }
//        public string ScientificNameID { get; set; }
//        public string SpecificEpithet { get; set; }
//        public string Subgenus { get; set; }
//        public string TaxonConceptID { get; set; }
//        public string TaxonConceptStatus { get; set; }
//        public string TaxonID { get; set; }
//        public string TaxonomicStatus { get; set; }
//        public string TaxonRank { get; set; }
//        public string TaxonRemarks { get; set; }
//        public string TaxonSortOrder { get; set; }
//        public string TaxonURL { get; set; }
//        public string VerbatimScientificName { get; set; }
//        public string VerbatimTaxonRank { get; set; }
//        public string VernacularName { get; set; }
//        public string Type { get; set; }
//        public string ValidationStatus { get; set; }

           
//        /// <summary>
//         /// All localized labels
//         /// </summary>
//         public ModelLabels Labels
//         {
//             get { return labels; }
//         }


//        /// <summary>
//        /// Localized labels
//        /// </summary>
//        public class ModelLabels
//        {
//            public string Title
//            {
//                 get { return Resources.Resource.DetailsDetailTitleLabel; }
//            }
//            public string ToggleButton
//            {
//                get { return Resources.Resource.SharedSpeciesObservationDetailsToggleButton; }
//            }
//            public string DetailsHeader
//            {
//                get { return Resources.Resource.SharedSpeciesObservationDetailsHeader; }
//            }
//        }
//    }
//}
