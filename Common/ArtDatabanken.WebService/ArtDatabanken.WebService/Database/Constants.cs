// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="">
//   
// </copyright>
// <summary>
//   Constants used when accessing column information in database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArtDatabanken.WebService.Database
{
    using System;

    /// <summary>
    /// Constants used when accessing column information in database.
    /// </summary>
    public struct ColumnLenghtData
    {
        #region Constants

        /// <summary>
        /// The ColumnName.
        /// </summary>
        public const string COLUMN_NAME = "ColumnName";

        /// <summary>
        /// The TableName.
        /// </summary>
        public const string TABLE_NAME = "TableName";

        #endregion
    }

    /// <summary>
    /// Constants used when accessing species observation field
    /// information in database.
    /// </summary>
    public struct DarwinCoreData
    {
        #region Constants

        /// <summary>
        /// Access rights.
        /// </summary>
        public const string ACCESS_RIGHTS = "accessRights";

        /// <summary>
        /// The ActivityID.
        /// </summary>
        public const string ACTIVITY_ID = "activityId";

        /// <summary>
        /// Associated media.
        /// </summary>
        public const string ASSOCIATED_MEDIA = "associatedMedia";

        /// <summary>
        /// Associated occurrences.
        /// </summary>
        public const string ASSOCIATED_OCCURRENCES = "associatedOccurrences";

        /// <summary>
        /// Associated references.
        /// </summary>
        public const string ASSOCIATED_REFERENCES = "associatedReferences";

        /// <summary>
        /// Associated sequences.
        /// </summary>
        public const string ASSOCIATED_SEQUENCES = "associatedSequences";

        /// <summary>
        /// Associated taxa.
        /// </summary>
        public const string ASSOCIATED_TAXA = "associatedTaxa";

        /// <summary>
        /// Basis of record.
        /// </summary>
        public const string BASIS_OF_RECORD = "basisOfRecord";

        /// <summary>
        /// The Behavior.
        /// </summary>
        public const string BEHAVIOR = "behavior";

        /// <summary>
        /// Bibliographic citation.
        /// </summary>
        public const string BIBLIOGRAPHIC_CITATION = "bibliographicCitation";

        /// <summary>
        /// The ActivityID.
        /// </summary>
        public const string BIRD_NEST_ACTIVITY_ID = "birdNestActivityId";

        /// <summary>
        /// Catalog number.
        /// </summary>
        public const string CATALOG_NUMBER = "catalogNumber";

        /// <summary>
        /// Collection code.
        /// </summary>
        public const string COLLECTION_CODE = "collectionCode";

        /// <summary>
        /// Collection id.
        /// </summary>
        public const string COLLECTION_ID = "collectionID";

        /// <summary>
        /// The Continent.
        /// </summary>
        public const string CONTINENT = "continent";

        /// <summary>
        /// Coordinate m.
        /// </summary>
        public const string COORDINATE_M = "coordinateM";

        /// <summary>
        /// Coordinate precision.
        /// </summary>
        public const string COORDINATE_PRECISION = "coordinatePrecision";

        /// <summary>
        /// Coordinate uncertainty in meters.
        /// </summary>
        public const string COORDINATE_UNCERTAINTY_IN_METERS = "coordinateUncertaintyInMeters";

        /// <summary>
        /// Coordinate x.
        /// </summary>
        public const string COORDINATE_X = "coordinateX";

        /// <summary>
        /// Coordinate x.
        /// </summary>
        public const string COORDINATE_X_ETRS89_LAEA = "coordinateX_ETRS89_LAEA";

        /// <summary>
        /// Coordinate x.
        /// </summary>
        public const string COORDINATE_X_RT90 = "coordinateX_RT90";

        /// <summary>
        /// Coordinate x.
        /// </summary>
        public const string COORDINATE_X_SWEREF99 = "coordinateX_SWEREF99";

        /// <summary>
        /// Coordinate y.
        /// </summary>
        public const string COORDINATE_Y = "coordinateY";

        /// <summary>
        /// Coordinate y.
        /// </summary>
        public const string COORDINATE_Y_ETRS89_LAEA = "coordinateY_ETRS89_LAEA";

        /// <summary>
        /// Coordinate y.
        /// </summary>
        public const string COORDINATE_Y_RT90 = "coordinateY_RT90";

        /// <summary>
        /// Coordinate y.
        /// </summary>
        public const string COORDINATE_Y_SWEREF99 = "coordinateY_SWEREF99";

        /// <summary>
        /// Coordinate z.
        /// </summary>
        public const string COORDINATE_Z = "coordinateZ";

        /// <summary>
        /// The Country.
        /// </summary>
        public const string COUNTRY = "country";

        /// <summary>
        /// Country code.
        /// </summary>
        public const string COUNTRY_CODE = "countryCode";

        /// <summary>
        /// The County.
        /// </summary>
        public const string COUNTY = "county";

        /// <summary>
        /// Name of Create darwin core table in database.
        /// </summary>
        public const string CREATE_TABLE_NAME = "TempCreateDarwinCoreObservation";

        /// <summary>
        /// Data generalizations.
        /// </summary>
        public const string DATA_GENERALIZATIONS = "dataGeneralizations";

        /// <summary>
        /// Data Provider id.
        /// </summary>
        public const string DATA_PROVIDER_ID = "dataProviderId";

        /// <summary>
        /// Date identified.
        /// </summary>
        public const string DATE_IDENTIFIED = "dateIdentified";

        /// <summary>
        /// The Day.
        /// </summary>
        public const string DAY = "day";

        /// <summary>
        /// Decimal latitude.
        /// </summary>
        public const string DECIMAL_LATITUDE = "decimalLatitude";

        /// <summary>
        /// Decimal longitude.
        /// </summary>
        public const string DECIMAL_LONGITUDE = "decimalLongitude";

        /// <summary>
        /// The Disposition.
        /// </summary>
        public const string DISPOSITION = "disposition";

        /// <summary>
        /// Dynamic properties.
        /// </summary>
        public const string DYNAMIC_PROPERTIES = "dynamicProperties";

        /// <summary>
        /// Dyntaxa taxon id.
        /// </summary>
        public const string DYNTAXA_TAXON_ID = "dyntaxaTaxonId";

        /// <summary>
        /// The End.
        /// </summary>
        public const string END = "end";

        /// <summary>
        /// End day of year.
        /// </summary>
        public const string END_DAY_OF_YEAR = "endDayOfYear";

        /// <summary>
        /// Establishment means.
        /// </summary>
        public const string ESTABLISHMENT_MEANS = "establishmentMeans";

        /// <summary>
        /// Event date.
        /// </summary>
        public const string EVENT_DATE = "eventDate";

        /// <summary>
        /// Event id.
        /// </summary>
        public const string EVENT_ID = "eventID";

        /// <summary>
        /// Event remarks.
        /// </summary>
        public const string EVENT_REMARKS = "eventRemarks";

        /// <summary>
        /// Event time.
        /// </summary>
        public const string EVENT_TIME = "eventTime";

        /// <summary>
        /// Field notes.
        /// </summary>
        public const string FIELD_NOTES = "fieldNotes";

        /// <summary>
        /// Field number.
        /// </summary>
        public const string FIELD_NUMBER = "fieldNumber";

        /// <summary>
        /// Footprint spatial fit.
        /// </summary>
        public const string FOOTPRINT_SPATIAL_FIT = "footprintSpatialFit";

        /// <summary>
        /// Footprint SRS.
        /// </summary>
        public const string FOOTPRINT_SRS = "footprintSRS";

        /// <summary>
        /// Footprint WKT.
        /// </summary>
        public const string FOOTPRINT_WKT = "footprintWKT";

        /// <summary>
        /// Geodetic datum.
        /// </summary>
        public const string GEODETIC_DATUM = "geodeticDatum";

        /// <summary>
        /// Geo referenced by.
        /// </summary>
        public const string GEOREFERENCED_BY = "georeferencedBy";

        /// <summary>
        /// Geo referenced date.
        /// </summary>
        public const string GEOREFERENCED_DATE = "georeferencedDate";

        /// <summary>
        /// Geo referenced protocol.
        /// </summary>
        public const string GEOREFERENCE_PROTOCOL = "georeferenceProtocol";

        /// <summary>
        /// Geo referenced remarks.
        /// </summary>
        public const string GEOREFERENCE_REMARKS = "georeferenceRemarks";

        /// <summary>
        /// Geo referenced sources.
        /// </summary>
        public const string GEOREFERENCE_SOURCES = "georeferenceSources";

        /// <summary>
        /// Geo reference verification status.
        /// </summary>
        public const string GEOREFERENCE_VERIFICATION_STATUS = "georeferenceVerificationStatus";

        /// <summary>
        /// The Habitat.
        /// </summary>
        public const string HABITAT = "habitat";

        /// <summary>
        /// Higher geography.
        /// </summary>
        public const string HIGHER_GEOGRAPHY = "higherGeography";

        /// <summary>
        /// Higher geography id.
        /// </summary>
        public const string HIGHER_GEOGRAPHY_ID = "higherGeographyID";

        /// <summary>
        /// The Id.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Identification id.
        /// </summary>
        public const string IDENTIFICATION_ID = "identificationId";

        /// <summary>
        /// Identification qualifier.
        /// </summary>
        public const string IDENTIFICATION_QUALIFIER = "identificationQualifier";

        /// <summary>
        /// Identification references.
        /// </summary>
        public const string IDENTIFICATION_REFERENCES = "identificationReferences";

        /// <summary>
        /// Identification remarks.
        /// </summary>
        public const string IDENTIFICATION_REMARKS = "identificationRemarks";

        /// <summary>
        /// Identification verification status.
        /// </summary>
        public const string IDENTIFICATION_VERIFICATION_STATUS = "identificationVerificationStatus";

        /// <summary>
        /// Identified by.
        /// </summary>
        public const string IDENTIFIED_BY = "identifiedBy";

        /// <summary>
        /// Individual count.
        /// </summary>
        public const string INDIVIDUAL_COUNT = "individualCount";

        /// <summary>
        /// Individual id.
        /// </summary>
        public const string INDIVIDUAL_ID = "individualID";

        /// <summary>
        /// Information withheld.
        /// </summary>
        public const string INFORMATION_WITHHELD = "informationWithheld";

        /// <summary>
        /// Institution code.
        /// </summary>
        public const string INSTITUTION_CODE = "institutionCode";

        /// <summary>
        /// Institution id.
        /// </summary>
        public const string INSTITUTION_ID = "institutionID";

        /// <summary>
        /// The Island.
        /// </summary>
        public const string ISLAND = "island";

        /// <summary>
        /// Island group.
        /// </summary>
        public const string ISLAND_GROUP = "islandGroup";

        /// <summary>
        /// Is natural occurrence.
        /// </summary>
        public const string IS_NATURAL_OCCURRENCE = "isNaturalOccurrence";

        /// <summary>
        /// Is never found observation.
        /// </summary>
        public const string IS_NEVER_FOUND_OBSERVATION = "isNeverFoundObservation";

        /// <summary>
        /// Is not rediscovered observation.
        /// </summary>
        public const string IS_NOT_REDISCOVERED_OBSERVATION = "isNotRediscoveredObservation";

        /// <summary>
        /// Is positive observation.
        /// </summary>
        public const string IS_POSITIVE_OBSERVATION = "isPositiveObservation";

        /// <summary>
        /// The Language.
        /// </summary>
        public const string LANGUAGE = "language";

        /// <summary>
        /// Life stage.
        /// </summary>
        public const string LIFE_STAGE = "lifeStage";

        /// <summary>
        /// The Locality.
        /// </summary>
        public const string LOCALITY = "locality";

        /// <summary>
        /// Location according to.
        /// </summary>
        public const string LOCATION_ACCORDING_TO = "locationAccordingTo";

        /// <summary>
        /// Location id.
        /// </summary>
        public const string LOCATION_ID = "locationID";

        /// <summary>
        /// Location remarks.
        /// </summary>
        public const string LOCATION_REMARKS = "locationRemarks";

        /// <summary>
        /// Location URL.
        /// </summary>
        public const string LOCATION_URL = "locationURL";

        /// <summary>
        /// Maximum depth in meters.
        /// </summary>
        public const string MAXIMUM_DEPTH_IN_METERS = "maximumDepthInMeters";

        /// <summary>
        /// Maximum distance above surface in meters.
        /// </summary>
        public const string MAXIMUM_DISTANCE_ABOVE_SURFACE_IN_METERS = "maximumDistanceAboveSurfaceInMeters";

        /// <summary>
        /// Maximum elevation in meters.
        /// </summary>
        public const string MAXIMUM_ELEVATION_IN_METERS = "maximumElevationInMeters";

        /// <summary>
        /// Minimum depth in meters.
        /// </summary>
        public const string MINIMUM_DEPTH_IN_METERS = "minimumDepthInMeters";

        /// <summary>
        /// Minimum distance above surface in meters.
        /// </summary>
        public const string MINIMUM_DISTANCE_ABOVE_SURFACE_IN_METERS = "minimumDistanceAboveSurfaceInMeters";

        /// <summary>
        /// Minimum elevation in meters.
        /// </summary>
        public const string MINIMUM_ELEVATION_IN_METERS = "minimumElevationInMeters";

        /// <summary>
        /// The Modified.
        /// </summary>
        public const string MODIFIED = "modified";

        /// <summary>
        /// The Month.
        /// </summary>
        public const string MONTH = "month";

        /// <summary>
        /// The Municipality.
        /// </summary>
        public const string MUNICIPALITY = "Municipality";

        /// <summary>
        /// Occurrence id.
        /// </summary>
        public const string OCCURRENCE_ID = "occurrenceID";

        /// <summary>
        /// Occurrence remarks.
        /// </summary>
        public const string OCCURRENCE_REMARKS = "occurrenceRemarks";

        /// <summary>
        /// Occurrence status.
        /// </summary>
        public const string OCCURRENCE_STATUS = "occurrenceStatus";

        /// <summary>
        /// Occurrence URL.
        /// </summary>
        public const string OCCURRENCE_URL = "occurrenceURL";

        /// <summary>
        /// Other catalog numbers.
        /// </summary>
        public const string OTHER_CATALOG_NUMBERS = "otherCatalogNumbers";

        /// <summary>
        /// The Owner.
        /// </summary>
        public const string OWNER = "owner";

        /// <summary>
        /// Owner institution code.
        /// </summary>
        public const string OWNER_INSTITUTION_CODE = "ownerInstitutionCode";

        /// <summary>
        /// The Parish.
        /// </summary>
        public const string PARISH = "parish";

        /// <summary>
        /// Point radius spatial fit.
        /// </summary>
        public const string POINT_GOOGLEMERCATOR = "point_GoogleMercator";

        /// <summary>
        /// Point radius spatial fit.
        /// </summary>
        public const string POINT_RADIUS_SPATIAL_FIT = "pointRadiusSpatialFit";

        /// <summary>
        /// The Preparations.
        /// </summary>
        public const string PREPARATIONS = "preparations";

        /// <summary>
        /// Previous identifications.
        /// </summary>
        public const string PREVIOUS_IDENTIFICATIONS = "previousIdentifications";

        /// <summary>
        /// Project category.
        /// </summary>
        public const string PROJECT_CATEGORY = "projectCategory";

        /// <summary>
        /// Project description.
        /// </summary>
        public const string PROJECT_DESCRIPTION = "projectDescription";

        /// <summary>
        /// Project end date.
        /// </summary>
        public const string PROJECT_END_DATE = "projectEndDate";

        /// <summary>
        /// Project id.
        /// </summary>
        public const string PROJECT_ID = "projectId";

        /// <summary>
        /// Project is public.
        /// </summary>
        public const string PROJECT_IS_PUBLIC = "projectIsPublic";

        /// <summary>
        /// Project name.
        /// </summary>
        public const string PROJECT_NAME = "projectName";

        /// <summary>
        /// Project owner.
        /// </summary>
        public const string PROJECT_OWNER = "projectOwner";

        /// <summary>
        /// Project start date.
        /// </summary>
        public const string PROJECT_START_DATE = "projectStartDate";

        /// <summary>
        /// Project survey method.
        /// </summary>
        public const string PROJECT_SURVEY_METHOD = "projectSurveyMethod";

        /// <summary>
        /// Project URL.
        /// </summary>
        public const string PROJECT_URL = "projectUrl";

        /// <summary>
        /// Protection level.
        /// </summary>
        public const string PROTECTION_LEVEL = "protectionLevel";

        /// <summary>
        /// The Quantity.
        /// </summary>
        public const string QUANTITY = "quantity";

        /// <summary>
        /// Quantity unit.
        /// </summary>
        public const string QUANTITY_UNIT = "quantityUnit";

        /// <summary>
        /// Recorded by.
        /// </summary>
        public const string RECORDED_BY = "recordedBy";

        /// <summary>
        /// Record number.
        /// </summary>
        public const string RECORD_NUMBER = "recordNumber";

        /// <summary>
        /// The References.
        /// </summary>
        public const string REFERENCES = "references";

        /// <summary>
        /// Reported by.
        /// </summary>
        public const string REPORTED_BY = "reportedBy";

        /// <summary>
        /// Reported date.
        /// </summary>
        public const string REPORTED_DATE = "reportedDate";

        /// <summary>
        /// Reproductive condition.
        /// </summary>
        public const string REPRODUCTIVE_CONDITION = "reproductiveCondition";

        /// <summary>
        /// The Rights.
        /// </summary>
        public const string RIGHTS = "rights";

        /// <summary>
        /// Rights holder.
        /// </summary>
        public const string RIGHTS_HOLDER = "rightsHolder";

        /// <summary>
        /// Sampling effort.
        /// </summary>
        public const string SAMPLING_EFFORT = "samplingEffort";

        /// <summary>
        /// Sampling protocol.
        /// </summary>
        public const string SAMPLING_PROTOCOL = "samplingProtocol";

        /// <summary>
        /// The Sex.
        /// </summary>
        public const string SEX = "sex";

        /// <summary>
        /// Species observation URL.
        /// </summary>
        public const string SPECIES_OBSERVATION_URL = "speciesObservationURL";

        /// <summary>
        /// The Start.
        /// </summary>
        public const string START = "start";

        /// <summary>
        /// Start day of year.
        /// </summary>
        public const string START_DAY_OF_YEAR = "startDayOfYear";

        /// <summary>
        /// State province.
        /// </summary>
        public const string STATE_PROVINCE = "stateProvince";

        /// <summary>
        /// The Substrate.
        /// </summary>
        public const string SUBSTRATE = "substrate";

        /// <summary>
        /// Name of Create darwin core table in database.
        /// </summary>
        public const string TABLE_NAME = "DarwinCoreObservation";

        /// <summary>
        /// Taxon remarks.
        /// </summary>
        public const string TAXON_REMARKS = "taxonRemarks";

        /// <summary>
        /// The Type.
        /// </summary>
        public const string TYPE = "type";

        /// <summary>
        /// Type status.
        /// </summary>
        public const string TYPE_STATUS = "typeStatus";

        /// <summary>
        /// Uncertain determination.
        /// </summary>
        public const string UNCERTAIN_DETERMINATION = "uncertainDetermination";

        /// <summary>
        /// Name of Update darwin core table in database.
        /// </summary>
        public const string UPDATE_TABLE_NAME = "TempUpdateDarwinCoreObservation";

        /// <summary>
        /// Validation status.
        /// </summary>
        public const string VALIDATION_STATUS = "validationStatus";

        /// <summary>
        /// Verbatim coordinates.
        /// </summary>
        public const string VERBATIM_COORDINATES = "verbatimCoordinates";

        /// <summary>
        /// Verbatim coordinate system.
        /// </summary>
        public const string VERBATIM_COORDINATE_SYSTEM = "verbatimCoordinateSystem";

        /// <summary>
        /// Verbatim depth.
        /// </summary>
        public const string VERBATIM_DEPTH = "verbatimDepth";

        /// <summary>
        /// Verbatim elevation.
        /// </summary>
        public const string VERBATIM_ELEVATION = "verbatimElevation";

        /// <summary>
        /// Verbatim event date.
        /// </summary>
        public const string VERBATIM_EVENT_DATE = "verbatimEventDate";

        /// <summary>
        /// Verbatim latitude.
        /// </summary>
        public const string VERBATIM_LATITUDE = "verbatimLatitude";

        /// <summary>
        /// Verbatim locality.
        /// </summary>
        public const string VERBATIM_LOCALITY = "verbatimLocality";

        /// <summary>
        /// Verbatim longitude.
        /// </summary>
        public const string VERBATIM_LONGITUDE = "verbatimLongitude";

        /// <summary>
        /// Verbatim scientific name.
        /// </summary>
        public const string VERBATIM_SCIENTIFIC_NAME = "verbatimScientificName";

        /// <summary>
        /// Verbatim SRS.
        /// </summary>
        public const string VERBATIM_SRS = "verbatimSRS";

        /// <summary>
        /// Verbatim taxon rank.
        /// </summary>
        public const string VERBATIM_TAXON_RANK = "verbatimTaxonRank";

        /// <summary>
        /// Water body.
        /// </summary>
        public const string WATER_BODY = "waterBody";

        /// <summary>
        /// The Year.
        /// </summary>
        public const string YEAR = "year";

        #endregion
    }

    /// <summary>
    /// Constants used when accessing data field description information in database.
    /// </summary>
    public struct SpeciesObservationFieldDescriptionData
    {
        #region Constants

        /// <summary>
        /// The Class.
        /// </summary>
        public const string CLASS = "Class";

        /// <summary>
        /// The Definition.
        /// </summary>
        public const string DEFINITION = "Definition";

        /// <summary>
        /// The DefinitionUrl.
        /// </summary>
        public const string DEFINITION_URL = "DefinitionUrl";

        /// <summary>
        /// The Documentation.
        /// </summary>
        public const string DOCUMENTATION = "Documentation";

        /// <summary>
        /// The DocumentationUrl.
        /// </summary>
        public const string DOCUMENTATION_URL = "DocumentationUrl";

        /// <summary>
        /// The GUID.
        /// </summary>
        public const string GUID = "GUID";

        /// <summary>
        /// The Id.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// The Importance.
        /// </summary>
        public const string IMPORTANCE = "Importance";

        /// <summary>
        /// Is Accepted By tdwg.
        /// </summary>
        public const string IS_ACCEPTED_BY_TDWG = "IsAcceptedByTdwg";

        /// <summary>
        /// The IsClassName.
        /// </summary>
        public const string IS_CLASS_NAME = "IsClassName";

        /// <summary>
        /// The IsDarwinCore.
        /// </summary>
        public const string IS_DARWINCORE = "IsDarwinCore";

        /// <summary>
        /// The IsImplemented.
        /// </summary>
        public const string IS_IMPLEMENTED = "IsImplemented";

        /// <summary>
        /// The IsMandatory.
        /// </summary>
        public const string IS_MANDATORY = "IsMandatory";

        /// <summary>
        /// The IsMandatoryFromProvider.
        /// </summary>
        public const string IS_MANDATORY_FROM_PROVIDER = "IsMandatoryFromProvider";

        /// <summary>
        /// The IsObtainedFromProvider.
        /// </summary>
        public const string IS_OBTAINED_FROM_PROVIDER = "IsObtainedFromProvider";

        /// <summary>
        /// The IsPlanned.
        /// </summary>
        public const string IS_PLANNED = "IsPlanned";

        /// <summary>
        /// The IsPublic.
        /// </summary>
        public const string IS_PUBLIC = "IsPublic";

        /// <summary>
        /// The IsSearchableField.
        /// </summary>
        public const string IS_SEARCHABLEFIELD = "IsSearchableField";

        /// <summary>
        /// The IsSortable.
        /// </summary>
        public const string IS_SORTABLE = "IsSortable";

        /// <summary>
        /// The Label.
        /// </summary>
        public const string LABEL = "Label";

        /// <summary>
        /// The Name.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// The IsSortable.
        /// </summary>
        public const string PERSISTED_IN_TABLE = "PersistedInTable";

        /// <summary>
        /// The Remarks.
        /// </summary>
        public const string REMARKS = "Remarks";

        /// <summary>
        /// The SortOrder.
        /// </summary>
        public const string SORT_ORDER = "SortOrder";

        /// <summary>
        /// The SortOrder.
        /// </summary>
        public const string SWEDISH_LABEL = "SwedishLabel";

        /// <summary>
        /// The Table Name.
        /// </summary>
        public const string TABLE_NAME = "SpeciesObservationFieldDescription";

        /// <summary>
        /// The Type.
        /// </summary>
        public const string TYPE = "Type";

        /// <summary>
        /// The UUID.
        /// </summary>
        public const string UUID = "UUID";

        #endregion
    }

    /// <summary>
    /// Constants used when accessing data field mapping information in database.
    /// </summary>
    public struct SpeciesObservationFieldMappingData
    {
        #region Constants

        /// <summary>
        /// The Id
        /// </summary>
        public const string CLASS = "Class";

        /// <summary>
        /// The Id
        /// </summary>
        public const string DATA_PROVIDER_ID = "DataProviderId";

        /// <summary>
        /// The Id
        /// </summary>
        public const string DEFAULT_VALUE = "DefaultValue";

        /// <summary>
        /// The Id
        /// </summary>
        public const string DOCUMENTATION = "Documentation";

        /// <summary>
        /// The Id
        /// </summary>
        public const string FIELD_ID = "FieldId";

        /// <summary>
        /// The Id
        /// </summary>
        public const string FIELD_NAME = "FieldName";

        /// <summary>
        /// The GUID
        /// </summary>
        public const string GUID = "GUID";

        /// <summary>
        /// The Id
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// The Id
        /// </summary>
        public const string IS_IMPLEMENTED = "IsImplemented";

        /// <summary>
        /// The Id
        /// </summary>
        public const string IS_PLANNED = "IsPlanned";

        /// <summary>
        /// The Id
        /// </summary>
        public const string METHOD = "Method";

        /// <summary>
        /// Project id.
        /// </summary>
        public const string PROJECT_ID = "ProjectId";

        /// <summary>
        /// The PropertyIdentifier.
        /// </summary>
        public const string PROJECT_NAME = "ProjectName";

        /// <summary>
        /// The PropertyIdentifier.
        /// </summary>
        public const string PROPERTY_IDENTIFIER = "PropertyIdentifier";

        /// <summary>
        /// The Id
        /// </summary>
        public const string PROVIDER_FIELD_NAME = "ProviderFieldName";

        /// <summary>
        /// The Table Name.
        /// </summary>
        public const string TABLE_NAME = "SpeciesObservationFieldMapping";

        #endregion
    }

    /// <summary>
    /// Constants used when accessing species
    /// observation information in database.
    /// </summary>
    public struct SpeciesObservationData
    {
        #region Constants

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string CHANGED_TYPE = @"@type";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string CHANGE_ID = @"@changeId";

        /// <summary>
        /// Coordinate x.
        /// </summary>
        public const string COORDINATE_X = "coordinateX";

        /// <summary>
        /// Coordinate y.
        /// </summary>
        public const string COORDINATE_Y = "coordinateY";

        /// <summary>
        /// Dyntaxa taxon id.
        /// </summary>
        public const string DYNTAXA_TAXON_ID = "dyntaxaTaxonId";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string END_ROW = @"@EndRow";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string FROM_DATE = @"@fromDate";

        /// <summary>
        /// Geometry where condition.
        /// </summary>
        public const string GEOMETRY_WHERE_CONDITION = @"@GeometryWhereCondition";

        /// <summary>
        /// The Id.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Join condition.
        /// </summary>
        public const string JOIN_CONDITION = @"@JoinCondition";

        /// <summary>
        /// Locale id.
        /// </summary>
        public const string LOCALE_ID = "LocaleId";

        /// <summary>
        /// Max protection level.
        /// </summary>
        public const string MAX_PROTECTION_LEVEL = "MaxProtectionLevel";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string MAX_RETURNED_CHANGES = @"@maxReturnedChanges";

        /// <summary>
        /// Multi polygon WKT.
        /// </summary>
        public const string MULTI_POLYGON_WKT = @"@MultiPolygonWkt";

        /// <summary>
        /// The Polygons.
        /// </summary>
        public const string POLYGONS = @"@Polygons";

        /// <summary>
        /// Protection level.
        /// </summary>
        public const string PROTECTION_LEVEL = "protectionLevel";

        /// <summary>
        /// Region ids.
        /// </summary>
        public const string REGION_IDS = @"@RegionIds";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string SORT_ORDER = @"@SortOrder";

        /// <summary>
        /// Species observation ids.
        /// </summary>
        public const string SPECIES_OBSERVATION_IDS = "SpeciesObservationIdTable";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string START_ROW = @"@StartRow";

        /// <summary>
        /// Taxon ids.
        /// </summary>
        public const string TAXON_IDS = @"@TaxonIds";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string TO_DATE = @"@toDate";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const string WHERE_CONDITION = @"@WhereCondition";

        #endregion
    }

    /// <summary>
    /// Constants used when accessing species observation
    /// information in database.
    /// </summary>
    public struct SpeciesObservationElasticsearchData
    {
        /// <summary>
        /// Change id.
        /// </summary>
        public const String CHANGE_ID = "ChangeId";

        /// <summary>
        /// Is debug.
        /// </summary>
        public const String IS_DEBUG = "IsDebug";

        /// <summary>
        /// Max returned changes.
        /// </summary>
        public const String MAX_RETURNED_CHANGES = "MaxReturnedChanges";
    }

    /// <summary>
    /// Constants used when accessing web service log in database.
    /// </summary>
    public struct WebServiceLogData
    {
        #region Constants

        /// <summary>
        /// The ApplicationIdentifier.
        /// </summary>
        public const string APPLICATION_IDENTIFIER = "ApplicationIdentifier";

        /// <summary>
        /// The Information.
        /// </summary>
        public const string INFORMATION = "Information";

        /// <summary>
        /// The RowCount.
        /// </summary>
        public const string ROW_COUNT = "RowCount";

        /// <summary>
        /// The SqlServerUser.
        /// </summary>
        public const string SQL_SERVER_USER = "SqlServerUser";

        /// <summary>
        /// The Table name.
        /// </summary>
        public const string TABLE_NAME = "WebServiceLog";

        /// <summary>
        /// The TCP-IP.
        /// </summary>
        public const string TCP_IP = "TcpIp";

        /// <summary>
        /// The Text.
        /// </summary>
        public const string TEXT = "Text";

        /// <summary>
        /// The Time.
        /// </summary>
        public const string TIME = "Time";

        /// <summary>
        /// The Type.
        /// </summary>
        public const string TYPE = "Type";

        /// <summary>
        /// The WebServiceUser.
        /// </summary>
        public const string WEB_SERVICE_USER = "WebServiceUser";

        #endregion
    }

    /// <summary>
    /// Constants used when accessing species
    /// observation data source information in database.
    /// </summary>
    public struct SpeciesObservationDataProviderData
    {
        #region Constants

        /// <summary>
        /// Start date for when species observations
        /// were inserted into the data source.
        /// </summary>
        public const string BEGIN_HARVEST_FROM_DATE = "BeginHarvestFromDate";

        /// <summary>
        /// Contact email.
        /// </summary>
        public const string CONTACT_EMAIL = "ContactEmail";

        /// <summary>
        /// Contact person.
        /// </summary>
        public const string CONTACT_PERSON = "ContactPerson";

        /// <summary>
        /// The Description.
        /// </summary>
        public const string DESCRIPTION = "Description";

        /// <summary>
        /// The Guid.
        /// </summary>
        public const string GUID = "Guid";

        /// <summary>
        /// The Id.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Is Active Harvest.
        /// </summary>
        public const string IS_ACTIVE_HARVEST = "IsActiveHarvest";

        /// <summary>
        /// Indicates if property MaxChangeId has a value or not.
        /// </summary>
        public const string IS_MAX_CHANGE_ID_SPECIFIED = "IsMaxChangeIdSpecified";

        /// <summary>
        /// Latest date for when any of the species observations
        /// from this data source was changed in the data source.
        /// </summary>
        public const string LATEST_CHANGED_DATE = "LatestChangedDate";

        /// <summary>
        /// Latest date for which changes of species observations
        /// was retrieved from the data provider.
        /// </summary>
        public const string LATEST_HARVEST_DATE = "LatestHarvestDate";

        /// <summary>
        /// Highest id of the changes in species observations that
        /// have been downloaded from the data source.
        /// Not all data providers has this value.
        /// Property IsMaxChangeIdSpecified indicates if this
        /// property has a value or not.
        /// </summary>
        public const string MAX_CHANGE_ID = "MaxChangeId";

        /// <summary>
        /// The Name.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// Non Public Species Observation Count.
        /// </summary>
        public const string NON_PUBLIC_SPECIES_OBSERVATION_COUNT = "NonPublicSpeciesObservationCount";

        /// <summary>
        /// The Organization.
        /// </summary>
        public const string ORGANIZATION = "Organization";

        /// <summary>
        /// Public species observation count.
        /// </summary>
        public const string PUBLIC_SPECIES_OBSERVATION_COUNT = "PublicSpeciesObservationCount";

        /// <summary>
        /// Species observation count.
        /// </summary>
        public const string SPECIES_OBSERVATION_COUNT = "SpeciesObservationCount";

        /// <summary>
        /// The Url.
        /// </summary>
        public const string URL = "Url";

        #endregion
    }
}