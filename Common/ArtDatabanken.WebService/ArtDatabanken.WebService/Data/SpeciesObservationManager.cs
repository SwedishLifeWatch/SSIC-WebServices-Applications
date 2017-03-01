using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Enum that holds column positon in table DarwinCoreObservation
    /// </summary>
    public enum DarwinCoreColumn
    { //x
#pragma warning disable 1591
        Id = 0,
        AccessRights = 1,
        AssociatedMedia = 2,
        AssociatedOccurrences = 3,
        AssociatedReferences = 4,
        AssociatedSequences = 5,
        AssociatedTaxa = 6,
        BasisOfRecord = 7,
        Behavior = 8,
        CatalogNumber = 9,
        CollectionCode = 10,
        CollectionId = 11,
        CoordinateM = 12,
        CoordinatePrecision = 13,
        CoordinateUncertaintyInMeters = 14,
        CoordinateX = 15,
        CoordinateY = 16,
        CoordinateZ = 17,
        CountryCode = 18,
        County = 19,
        DataGeneralizations = 20,
        DataProviderId = 21,
        DateIdentified = 22,
        Day = 23,
        DecimalLatitude = 24,
        DecimalLongitude = 25,
        Disposition = 26,
        DynamicProperties = 27,
        DyntaxaTaxonId = 28,
        End = 29,
        EndDayOfYear = 30,
        EstablishmentMeans = 31,
        EventDate = 32,
        EventId = 33,
        EventRemarks = 34,
        EventTime = 35,
        FieldNotes = 36,
        FieldNumber = 37,
        FootprintSpatialFit = 38,
        FootprintSrs = 39,
        FootprintWkt = 40,
        GeodeticDatum = 41,
        GeoreferencedBy = 42,
        GeoreferencedDate = 43,
        GeoreferenceProtocol = 44,
        GeoreferenceRemarks = 45,
        GeoreferenceSources = 46,
        GeoreferenceVerificationStatus = 47,
        Habitat = 48,
        HigherGeography = 49,
        HigherGeographyId = 50,
        IdentificationId = 51,
        IdentificationQualifier = 52,
        IdentificationReferences = 53,
        IdentificationRemarks = 54,
        IdentificationVerificationStatus = 55,
        IdentifiedBy = 56,
        IndividualCount = 57,
        IndividualId = 58,
        InformationWithheld = 59,
        InstitutionCode = 60,
        InstitutionId = 61,
        Island = 62,
        IslandGroup = 63,
        IsNaturalOccurrence = 64,
        IsNeverFoundObservation = 65,
        IsNotRediscoveredObservation = 66,
        IsPositiveObservation = 67,
        Language = 68,
        LifeStage = 69,
        Locality = 70,
        LocationAccordingTo = 71,
        LocationId = 72,
        LocationRemarks = 73,
        LocationUrl = 74,
        MaximumDepthInMeters = 75,
        MaximumDistanceAboveSurfaceInMeters = 76,
        MaximumElevationInMeters = 77,
        MinimumDepthInMeters = 78,
        MinimumDistanceAboveSurfaceInMeters = 79,
        MinimumElevationInMeters = 80,
        Modified = 81,
        Month = 82,
        Municipality = 83,
        OccurrenceId = 84,
        OccurrenceRemarks = 85,
        OccurrenceStatus = 86,
        OccurrenceUrl = 87,
        OtherCatalogNumbers = 88,
        Owner = 89,
        OwnerInstitutionCode = 90,
        Parish = 91,
        PointRadiusSpatialFit = 92,
        Preparations = 93,
        PreviousIdentifications = 94,
        ProjectCategory = 95,
        ProjectDescription = 96,
        ProjectEndDate = 97,
        ProjectId = 98,
        ProjectIsPublic = 99,
        ProjectName = 100,
        ProjectOwner = 101,
        ProjectStartDate = 102,
        ProjectSurveyMethod = 103,
        ProjectUrl = 104,
        ProtectionLevel = 105,
        Quantity = 106,
        QuantityUnit = 107,
        RecordedBy = 108,
        RecordNumber = 109,
        References = 110,
        ReportedBy = 111,
        ReportedDate = 112,
        ReproductiveCondition = 113,
        Rights = 114,
        RightsHolder = 115,
        SamplingEffort = 116,
        SamplingProtocol = 117,
        Sex = 118,
        SpeciesObservationUrl = 119,
        Start = 120,
        StartDayOfYear = 121,
        StateProvince = 122,
        Substrate = 123,
        TaxonRemarks = 124,
        Type = 125,
        TypeStatus = 126,
        UncertainDetermination = 127,
        ValidationStatus = 128,
        VerbatimCoordinates = 129,
        VerbatimCoordinateSystem = 130,
        VerbatimDepth = 131,
        VerbatimElevation = 132,
        VerbatimEventDate = 133,
        VerbatimLatitude = 134,
        VerbatimLocality = 135,
        VerbatimLongitude = 136,
        VerbatimScientificName = 137,
        VerbatimSrs = 138,
        VerbatimTaxonRank = 139,
        WaterBody = 140,
        Year = 141,
        point_GoogleMercator = 142,
        CoordinateX_RT90 = 143,
        CoordinateY_RT90 = 144,
        CoordinateX_SWEREF99 = 145,
        CoordinateY_SWEREF99 = 146,
        CoordinateX_ETRS89_LAEA = 147,
        CoordinateY_ETRS89_LAEA = 148,
        ActivityId = 149,
        BirdNestActivityId = 150

#pragma warning restore 1591
    }

    /// <summary>
    /// Class that handles species observation related information.
    /// </summary>
    public class SpeciesObservationManager : ManagerBase, ISpeciesObservationManager
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SpeciesObservationManager()
        {
            SpeciesObservationCoordinateSystem = new WebCoordinateSystem();
            SpeciesObservationCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
        }

        /// <summary>
        /// Coordinate system used for species observations that 
        /// are retrieved from SwedishSpeciesObservation database.
        /// </summary>
        public WebCoordinateSystem SpeciesObservationCoordinateSystem
        { get; set; }

        /// <summary>
        /// Get all county regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All county regions</returns>
        public virtual List<WebRegion> GetCountyRegions(WebServiceContext context)
        {
            List<WebRegion> regions;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesObservationCountyRegionCacheKey;
            regions = (List<WebRegion>)context.GetCachedObject(cacheKey);

            WebClientInformation clientInformation;
            if (regions.IsNull())
            {
                clientInformation = GetClientInformation(context, WebServiceId.SwedishSpeciesObservationService);
                regions = WebServiceProxy.SwedishSpeciesObservationService.GetCountyRegions(clientInformation);
                
                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        regions,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return regions;
        }

       // <summary>
        /// Get DataTable with same definition as table
        /// DarwinCoreObservation in database.
        /// </summary>
        /// <returns>
        /// DataTable with same definition as table
        /// DarwinCoreObservation in database.
        /// </returns>
        public DataTable GetDarwinCoreTable()
        {
            DataTable darwinCoreTable = new DataTable(DarwinCoreData.TABLE_NAME);
            darwinCoreTable.Columns.Add(DarwinCoreData.ID, typeof(Int64));
            darwinCoreTable.Columns.Add(DarwinCoreData.ACCESS_RIGHTS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ASSOCIATED_MEDIA, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ASSOCIATED_OCCURRENCES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ASSOCIATED_REFERENCES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ASSOCIATED_SEQUENCES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ASSOCIATED_TAXA, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.BASIS_OF_RECORD, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.BEHAVIOR, typeof(String));
            //darwinCoreTable.Columns.Add(DarwinCoreData.BIBLIOGRAPHIC_CITATION, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.CATALOG_NUMBER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.COLLECTION_CODE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.COLLECTION_ID, typeof(String));
           // darwinCoreTable.Columns.Add(DarwinCoreData.CONTINENT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_M, typeof(Double));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_PRECISION, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_UNCERTAINTY_IN_METERS, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_X, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_Y, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_Z, typeof(Int32));
           // darwinCoreTable.Columns.Add(DarwinCoreData.COUNTRY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.COUNTRY_CODE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.COUNTY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.DATA_GENERALIZATIONS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.DATA_PROVIDER_ID, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.DATE_IDENTIFIED, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.DAY, typeof(Int16));
            darwinCoreTable.Columns.Add(DarwinCoreData.DECIMAL_LATITUDE, typeof(Double));
            darwinCoreTable.Columns.Add(DarwinCoreData.DECIMAL_LONGITUDE, typeof(Double));
            darwinCoreTable.Columns.Add(DarwinCoreData.DISPOSITION, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.DYNAMIC_PROPERTIES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.DYNTAXA_TAXON_ID, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.END, typeof(DateTime));
            darwinCoreTable.Columns.Add(DarwinCoreData.END_DAY_OF_YEAR, typeof(Int16));
            darwinCoreTable.Columns.Add(DarwinCoreData.ESTABLISHMENT_MEANS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.EVENT_DATE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.EVENT_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.EVENT_REMARKS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.EVENT_TIME, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.FIELD_NOTES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.FIELD_NUMBER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.FOOTPRINT_SPATIAL_FIT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.FOOTPRINT_SRS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.FOOTPRINT_WKT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEODETIC_DATUM, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEOREFERENCED_BY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEOREFERENCED_DATE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEOREFERENCE_PROTOCOL, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEOREFERENCE_REMARKS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEOREFERENCE_SOURCES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.GEOREFERENCE_VERIFICATION_STATUS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.HABITAT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.HIGHER_GEOGRAPHY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.HIGHER_GEOGRAPHY_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IDENTIFICATION_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IDENTIFICATION_QUALIFIER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IDENTIFICATION_REFERENCES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IDENTIFICATION_REMARKS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IDENTIFICATION_VERIFICATION_STATUS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IDENTIFIED_BY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.INDIVIDUAL_COUNT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.INDIVIDUAL_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.INFORMATION_WITHHELD, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.INSTITUTION_CODE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.INSTITUTION_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ISLAND, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.ISLAND_GROUP, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.IS_NATURAL_OCCURRENCE, typeof(Boolean));
            darwinCoreTable.Columns.Add(DarwinCoreData.IS_NEVER_FOUND_OBSERVATION, typeof(Boolean));
            darwinCoreTable.Columns.Add(DarwinCoreData.IS_NOT_REDISCOVERED_OBSERVATION, typeof(Boolean));
            darwinCoreTable.Columns.Add(DarwinCoreData.IS_POSITIVE_OBSERVATION, typeof(Boolean));
            darwinCoreTable.Columns.Add(DarwinCoreData.LANGUAGE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.LIFE_STAGE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.LOCALITY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.LOCATION_ACCORDING_TO, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.LOCATION_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.LOCATION_REMARKS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.LOCATION_URL, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MAXIMUM_DEPTH_IN_METERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MAXIMUM_DISTANCE_ABOVE_SURFACE_IN_METERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MAXIMUM_ELEVATION_IN_METERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MINIMUM_DEPTH_IN_METERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MINIMUM_DISTANCE_ABOVE_SURFACE_IN_METERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MINIMUM_ELEVATION_IN_METERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.MODIFIED, typeof(DateTime));
            darwinCoreTable.Columns.Add(DarwinCoreData.MONTH, typeof(Int16));
            darwinCoreTable.Columns.Add(DarwinCoreData.MUNICIPALITY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OCCURRENCE_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OCCURRENCE_REMARKS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OCCURRENCE_STATUS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OCCURRENCE_URL, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OTHER_CATALOG_NUMBERS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OWNER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.OWNER_INSTITUTION_CODE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PARISH, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.POINT_RADIUS_SPATIAL_FIT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PREPARATIONS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PREVIOUS_IDENTIFICATIONS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_CATEGORY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_DESCRIPTION, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_END_DATE, typeof(DateTime));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_ID, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_IS_PUBLIC, typeof(Boolean));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_NAME, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_OWNER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_START_DATE, typeof(DateTime));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_SURVEY_METHOD, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROJECT_URL, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.PROTECTION_LEVEL, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.QUANTITY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.QUANTITY_UNIT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.RECORDED_BY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.RECORD_NUMBER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.REFERENCES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.REPORTED_BY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.REPORTED_DATE, typeof(DateTime));
            darwinCoreTable.Columns.Add(DarwinCoreData.REPRODUCTIVE_CONDITION, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.RIGHTS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.RIGHTS_HOLDER, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.SAMPLING_EFFORT, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.SAMPLING_PROTOCOL, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.SEX, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.SPECIES_OBSERVATION_URL, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.START, typeof(DateTime));
            darwinCoreTable.Columns.Add(DarwinCoreData.START_DAY_OF_YEAR, typeof(Int16));
            darwinCoreTable.Columns.Add(DarwinCoreData.STATE_PROVINCE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.SUBSTRATE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.TAXON_REMARKS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.TYPE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.TYPE_STATUS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.UNCERTAIN_DETERMINATION, typeof(Boolean));
            darwinCoreTable.Columns.Add(DarwinCoreData.VALIDATION_STATUS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_COORDINATES, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_COORDINATE_SYSTEM, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_DEPTH, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_ELEVATION, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_EVENT_DATE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_LATITUDE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_LOCALITY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_LONGITUDE, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_SCIENTIFIC_NAME, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_SRS, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.VERBATIM_TAXON_RANK, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.WATER_BODY, typeof(String));
            darwinCoreTable.Columns.Add(DarwinCoreData.YEAR, typeof(Int16));
            darwinCoreTable.Columns.Add(DarwinCoreData.POINT_GOOGLEMERCATOR, typeof(Microsoft.SqlServer.Types.SqlGeography));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_X_RT90, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_Y_RT90, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_X_SWEREF99, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_Y_SWEREF99, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_X_ETRS89_LAEA, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.COORDINATE_Y_ETRS89_LAEA, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.ACTIVITY_ID, typeof(Int32));
            darwinCoreTable.Columns.Add(DarwinCoreData.BIRD_NEST_ACTIVITY_ID, typeof(Int32));

            return darwinCoreTable;
        }

        /// <summary>
        /// Get all province regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All county regions</returns>
        public virtual List<WebRegion> GetProvinceRegions(WebServiceContext context)
        {
            List<WebRegion> regions;
            String cacheKey;

            // Get cached information
            cacheKey = Settings.Default.SpeciesObservationProvinceRegionCacheKey;
            regions = (List<WebRegion>)context.GetCachedObject(cacheKey);

            WebClientInformation clientInformation;

            if (regions.IsNull())
            {
                clientInformation = GetClientInformation(context, WebServiceId.SwedishSpeciesObservationService);
                regions = WebServiceProxy.SwedishSpeciesObservationService.GetProvinceRegions(clientInformation);
                // Add information to cache.
                context.AddCachedObject(cacheKey,
                    regions,
                    DateTime.Now + new TimeSpan(1, 0, 0, 0),
                    CacheItemPriority.High);
            }

            return regions;
        }

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        public virtual WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                                           Int32 speciesObservationDataProviderId)
        {
            foreach (WebSpeciesObservationDataProvider speciesObservationDataProvider in GetSpeciesObservationDataProviders(context))
            {
                if (speciesObservationDataProvider.Id == speciesObservationDataProviderId)
                {
                    return speciesObservationDataProvider;
                }
            }

            return null;
        }

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderGuid">Species observation data provider GUID.</param>
        /// <returns>Specified species observation data provider.</returns>
        public virtual WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                                           String speciesObservationDataProviderGuid)
        {
            foreach (WebSpeciesObservationDataProvider speciesObservationDataProvider in GetSpeciesObservationDataProviders(context))
            {
                if (speciesObservationDataProvider.Guid.ToLower() == speciesObservationDataProviderGuid.ToLower())
                {
                    return speciesObservationDataProvider;
                }
            }

            return null;
        }

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        public virtual WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                                           SpeciesObservationDataProviderId speciesObservationDataProviderId)
        {
            return GetSpeciesObservationDataProvider(context, (Int32)(speciesObservationDataProviderId));
        }

        /// <summary>
        /// Get all species observation data providers.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data providers.</returns>
        public virtual List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebServiceContext context)
        {
            List<WebSpeciesObservationDataProvider> dataProviders;
            String cacheKey;
            WebSpeciesObservationDataProvider dataProvider;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesObservationDataProviderCacheKey + ":" + context.Locale.ISOCode;
            dataProviders = (List<WebSpeciesObservationDataProvider>)(context.GetCachedObject(cacheKey));

            if (dataProviders.IsEmpty())
            {
                // Data not in cache. Get information from database.
                dataProviders = new List<WebSpeciesObservationDataProvider>();
                using (DataReader dataReader = context.GetDatabase().GetSpeciesObservationDataProviders(context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        dataProvider = new WebSpeciesObservationDataProvider();
                        dataProvider.LoadData(dataReader);
                        dataProviders.Add(dataProvider);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        dataProviders,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return dataProviders;
        }

        /// <summary>
        /// Get all species observation data providers.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data providers.</returns>
        public virtual Dictionary<Int32, WebSpeciesObservationDataProvider> GetSpeciesObservationDataProvidersDictionary(WebServiceContext context)
        {
            Dictionary<Int32, WebSpeciesObservationDataProvider> dataProviders;
            String cacheKey;
            WebSpeciesObservationDataProvider dataProvider;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesObservationDataProviderDictionaryCacheKey + ":" + context.Locale.ISOCode;
            dataProviders = (Dictionary<Int32, WebSpeciesObservationDataProvider>)(context.GetCachedObject(cacheKey));

            if (dataProviders.IsEmpty())
            {
                // Data not in cache. Get information from database.
                dataProviders = new Dictionary<int, WebSpeciesObservationDataProvider>();
                using (DataReader dataReader = context.GetDatabase().GetSpeciesObservationDataProviders(context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        dataProvider = new WebSpeciesObservationDataProvider();
                        dataProvider.LoadData(dataReader);
                        dataProviders.Add(dataProvider.Id, dataProvider);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        dataProviders,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return dataProviders;
        }
    }
}
