using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents one observation of a species.
    /// </summary>
    public class SpeciesObservation
    {
        /// <summary>
        /// Name of the accession id data field in WebSpeciesObservation.
        /// </summary>
        public const String ACCESSION_ID_DATA_FIELD = "AccessionId";
        /// <summary>
        /// Name of the accuracy data field in WebSpeciesObservation.
        /// </summary>
        public const String ACCURACY_DATA_FIELD = "Accuracy";
        /// <summary>
        /// Name of the activity or substrate count data field in WebSpeciesObservation.
        /// </summary>
        public const String ACTIVITY_OR_SUBSTRATE_COUNT_DATA_FIELD = "ActivityOrSubstrateCount";
        /// <summary>
        /// Name of the biotope or substrate data field in WebSpeciesObservation.
        /// </summary>
        public const String BIOTOPE_OR_SUBSTRATE_DATA_FIELD = "BiotopeOrSubstrate";
        /// <summary>
        /// Name of the collection data field in WebSpeciesObservation.
        /// </summary>
        public const String COLLECTION_DATA_FIELD = "Collection";
        /// <summary>
        /// Name of the comment data field in WebSpeciesObservation.
        /// </summary>
        public const String COMMENT_DATA_FIELD = "Comment";
        /// <summary>
        /// Name of the common name data field in WebSpeciesObservation.
        /// </summary>
        public const String COMMON_NAME_DATA_FIELD = "CommonName";
        /// <summary>
        /// Name of the county data field in WebSpeciesObservation.
        /// </summary>
        public const String COUNTY_DATA_FIELD = "County";
        /// <summary>
        /// Name of the database data field in WebSpeciesObservation.
        /// </summary>
        public const String DATABASE_DATA_FIELD = "Database";
        /// <summary>
        /// Name of the database id data field in WebSpeciesObservation.
        /// </summary>
        public const String DATABASE_ID_DATA_FIELD = "DatabaseId";
        /// <summary>
        /// Name of the database observation id data field in WebSpeciesObservation.
        /// </summary>
        public const String DATABASE_OBSERVATION_ID_DATA_FIELD = "DatabaseObservationId";
        /// <summary>
        /// Name of the determinator data field in WebSpeciesObservation.
        /// </summary>
        public const String DETERMINATOR_DATA_FIELD = "Determinator";
        /// <summary>
        /// Name of the end date data field in WebSpeciesObservation.
        /// </summary>
        public const String END_DATE_DATA_FIELD = "EndDate";
        /// <summary>
        /// Name of the east coordinate data field in WebSpeciesObservation.
        /// </summary>
        public const String EAST_COORDINATE_DATA_FIELD = "EastCoordinate";
        /// <summary>
        /// Name of the guid data field in WebSpeciesObservation.
        /// </summary>
        public const String GUID_DATA_FIELD = "GUID";
        /// <summary>
        /// Name of the life stage data field in WebSpeciesObservation.
        /// </summary>
        public const String LIFE_STAGE_DATA_FIELD = "LifeStage";
        /// <summary>
        /// Name of the locality data field in WebSpeciesObservation.
        /// </summary>
        public const String LOCALITY_DATA_FIELD = "Locality";
        /// <summary>
        /// Name of the modified data field in WebSpeciesObservation.
        /// </summary>
        public const String MODIFIED_DATA_FIELD = "Modified";
        /// <summary>
        /// Name of the municipality data field in WebSpeciesObservation.
        /// </summary>
        public const String MUNICIPALITY_DATA_FIELD = "Municipality";
        /// <summary>
        /// Name of the never found data field in WebSpeciesObservation.
        /// </summary>
        public const String NEVER_FOUND_DATA_FIELD = "NeverFound";
        /// <summary>
        /// Name of the north coordinate data field in WebSpeciesObservation.
        /// </summary>
        public const String NORTH_COORDINATE_DATA_FIELD = "NorthCoordinate";
        /// <summary>
        /// Name of the not rediscovered data field in WebSpeciesObservation.
        /// </summary>
        public const String NOT_REDISCOVERED_DATA_FIELD = "NotRediscovered";
        /// <summary>
        /// Name of the observers data field in WebSpeciesObservation.
        /// </summary>
        public const String OBSERVERS_DATA_FIELD = "Observers";
        /// <summary>
        /// Name of the organism group sort order data field in WebSpeciesObservation.
        /// </summary>
        public const String ORGANISM_GROUP_SORT_ORDER_DATA_FIELD = "OrganismGroupSortOrder";
        /// <summary>
        /// Name of the organism group data field in WebSpeciesObservation.
        /// </summary>
        public const String ORGANISM_GROUP_DATA_FIELD = "OrganismGroup";
        /// <summary>
        /// Name of the origin data field in WebSpeciesObservation.
        /// </summary>
        public const String ORIGIN_DATA_FIELD = "Origin";
        /// <summary>
        /// Name of the parish data field in WebSpeciesObservation.
        /// </summary>
        public const String PARISH_DATA_FIELD = "Parish";
        /// <summary>
        /// Name of the protection level data field in WebSpeciesObservation.
        /// </summary>
        public const String PROTECTION_LEVEL_DATA_FIELD = "ProtectionLevel";
        /// <summary>
        /// Name of the Province data field in WebSpeciesObservation.
        /// </summary>
        public const String PROVINCE_DATA_FIELD = "Province";
        /// <summary>
        /// Name of the quantity or area data field in WebSpeciesObservation.
        /// </summary>
        public const String QUANTITY_OR_AREA_DATA_FIELD = "QuantityOrArea";
        /// <summary>
        /// Name of the redlist category data field in WebSpeciesObservation.
        /// </summary>
        public const String REDLIST_CATEGORY_DATA_FIELD = "RedlistCategory";
        /// <summary>
        /// Name of the reported date data field in WebSpeciesObservation.
        /// </summary>
        public const String REPORTED_DATE_DATA_FIELD = "ReportedDate";
        /// <summary>
        /// Name of the sci code data field in WebSpeciesObservation.
        /// </summary>
        public const String SCI_CODE_DATA_FIELD = "SciCode";
        /// <summary>
        /// Name of the sci name data field in WebSpeciesObservation.
        /// </summary>
        public const String SCI_NAME_DATA_FIELD = "SciName";
        /// <summary>
        /// Name of the scientific name data field in WebSpeciesObservation.
        /// </summary>
        public const String SCIENTIFIC_NAME_DATA_FIELD = "ScientificName";
        /// <summary>
        /// Name of the species activity id data field in WebSpeciesObservation.
        /// </summary>
        public const String SPECIES_ACTIVITY_ID_DATA_FIELD = "SpeciesActivityId";
        /// <summary>
        /// Name of the start date data field in WebSpeciesObservation.
        /// </summary>
        public const String START_DATE_DATA_FIELD = "StartDate";
        /// <summary>
        /// Name of the taxon id data field in WebSpeciesObservation.
        /// </summary>
        public const String TAXON_ID_DATA_FIELD = "TaxonId";
        /// <summary>
        /// Name of the taxon sort order data field in WebSpeciesObservation.
        /// </summary>
        public const String TAXON_SORT_ORDER_DATA_FIELD = "TaxonSortOrder";
        /// <summary>
        /// Name of the taxon uncertainty data field in WebSpeciesObservation.
        /// </summary>
        public const String TAXON_UNCERTAINTY_DATA_FIELD = "TaxonUncertainty";
        /// <summary>
        /// Name of the unit data field in WebSpeciesObservation.
        /// </summary>
        public const String UNIT_DATA_FIELD = "Unit";

        private readonly Boolean _neverFound;
        private readonly Boolean _notRediscovered;
        private readonly DateTime _endDate;
        private readonly DateTime _modified;
        private readonly DateTime _reportedDate;
        private readonly DateTime _startDate;
        private readonly Int32? _accuracy;
        private readonly Int32 _databaseId;
        private readonly Int32 _databaseObservationId;
        private readonly Int32 _organismGroupSortOrder;
        private readonly Int32 _protectionLevel;
        private readonly Int32? _speciesActivityId;
        private readonly Int32 _taxonId;
        private readonly Int32 _taxonSortOrder;
        private readonly Int64 _id;
        private readonly String _activityOrSubstrateCount;
        private readonly String _accessionId;
        private readonly String _biotopeOrSubstrate;
        private readonly String _collection;
        private readonly String _comment;
        private readonly String _commonName;
        private readonly String _countyName;
        private readonly String _databaseName;
        private readonly String _determinator;
        private readonly String _guid;
        private readonly String _lifeStage;
        private readonly String _locality;
        private readonly String _municipalityName;
        private readonly String _observers;
        private readonly String _organismGroupName;
        private readonly String _origin;
        private readonly String _quantityOrArea;
        private readonly String _parishName;
        private readonly String _provinceName;
        private readonly String _redlistCategory;
        private readonly String _sciCode;
        private readonly String _scientificName;
        private readonly String _sciName;
        private readonly String _taxonUncertainty;
        private readonly String _unit;

        /// <summary>
        /// Create a SpeciesObservation instance.
        /// </summary>
        /// <param name='id'>Id for species observation.</param>
        /// <param name='organismGroupName'>Name of the organism group that the taxon belongs to.</param>
        /// <param name='scientificName'>Scientific name of the taxon.</param>
        /// <param name='taxonUncertainty'>Indicates if taxon determination is uncertain.</param>
        /// <param name='commonName'>Common name of the taxon.</param>
        /// <param name='startDate'>Start date for the observation.</param>
        /// <param name='endDate'>End date for the observation.</param>
        /// <param name='locality'>Locality for the observation.</param>
        /// <param name='parishName'>Name of the parish in which the locality is located.</param>
        /// <param name='municipalityName'>Name of the municipality in which the locality is located.</param>
        /// <param name='countyName'>Name of the county in which the locality is located.</param>
        /// <param name='provinceName'>Name of the province in which the locality is located.</param>
        /// <param name='northCoordinate'>North coordinate for the species observation.</param>
        /// <param name='eastCoordinate'>East coordinate for the species observation.</param>
        /// <param name='accuracy'>Accuracy of the coordinate for the species observation.</param>
        /// <param name='observers'>List of people who made the observation.</param>
        /// <param name='origin'>
        /// Origin of the observation.
        /// For private observations this is the same as the observer.
        /// For other purposes this is the context in which the observation was performed, e.g. inventory.
        /// </param>
        /// <param name='quantityOrArea'>Number of observed individuals or area of the observed species.</param>
        /// <param name='unit'>Information related to parameter quantityOrArea and the observed species.</param>
        /// <param name='lifeStage'>Life stage of the observed species.</param>
        /// <param name='activityOrSubstrateCount'>Activity or substrate count.</param>
        /// <param name='biotopeOrSubstrate'>Biotope or substrate.</param>
        /// <param name='comment'>Comment about the observation.</param>
        /// <param name='determinator'>The name(s) of the person(s) who applied the ScientificName to the observation.</param>
        /// <param name='collection'>The observation belongs to this collection.</param>
        /// <param name='accessionId'>Original id in the observers collection.</param>
        /// <param name='notRediscovered'>Not rediscovered in a locality where it previously has been observed.</param>
        /// <param name='neverFound'>Never found in a locality with a suitable biotope for the species.</param>
        /// <param name='databaseObservationId'>Observation id in the database which the observation belongs to.</param>
        /// <param name='databaseName'>Name of the database which the observation belongs to.</param>
        /// <param name='taxonSortOrder'>Sort order for the taxon.</param>
        /// <param name='taxonId'>Taxon id.</param>
        /// <param name='organismGroupSortOrder'>Organism group sort order.</param>
        /// <param name='protectionLevel'>Protection level for this observation.</param>
        /// <param name='sciCode'>SCI code.</param>
        /// <param name='sciName'>SCI name.</param>
        /// <param name='redlistCategory'>Redlist category.</param>
        /// <param name='guid'>GUID.</param>
        /// <param name='databaseId'>Database id.</param>
        /// <param name='reportedDate'>Reported date.</param>
        /// <param name='speciesActivityId'>Species activity id.</param>
        /// <param name='modified'>Date and time when the species observation was last modified.</param>
        public SpeciesObservation(Int64 id,
                                  String organismGroupName,
                                  String scientificName,
                                  String taxonUncertainty,
                                  String commonName,
                                  DateTime startDate,
                                  DateTime endDate,
                                  String locality,
                                  String parishName,
                                  String municipalityName,
                                  String countyName,
                                  String provinceName,
                                  Int32 northCoordinate,
                                  Int32 eastCoordinate,
                                  Int32? accuracy,
                                  String observers,
                                  String origin,
                                  String quantityOrArea,
                                  String unit,
                                  String lifeStage,
                                  String activityOrSubstrateCount,
                                  String biotopeOrSubstrate,
                                  String comment,
                                  String determinator,
                                  String collection,
                                  String accessionId,
                                  Boolean notRediscovered,
                                  Boolean neverFound,
                                  Int32 databaseObservationId,
                                  String databaseName,
                                  Int32 taxonSortOrder,
                                  Int32 taxonId,
                                  Int32 organismGroupSortOrder,
                                  Int32 protectionLevel,
                                  String sciCode,
                                  String sciName,
                                  String redlistCategory,
                                  String guid,
                                  Int32 databaseId,
                                  DateTime reportedDate,
                                  Int32? speciesActivityId,
                                  DateTime modified)
        {
            _accessionId = accessionId;
            _accuracy = accuracy;
            _activityOrSubstrateCount = activityOrSubstrateCount;
            _biotopeOrSubstrate = biotopeOrSubstrate;
            _collection = collection;
            _comment = comment;
            _commonName = commonName;
            _countyName = countyName;
            _databaseId = databaseId;
            _databaseName = databaseName;
            _databaseObservationId = databaseObservationId;
            _determinator = determinator;
            EastCoordinate = eastCoordinate;
            _endDate = endDate;
            _guid = guid;
            _id = id;
            _lifeStage = lifeStage;
            _locality = locality;
            _modified = modified;
            _municipalityName = municipalityName;
            _neverFound = neverFound;
            NorthCoordinate = northCoordinate;
            _notRediscovered = notRediscovered;
            _observers = observers;
            _organismGroupName = organismGroupName;
            _organismGroupSortOrder = organismGroupSortOrder;
            _origin = origin;
            _parishName = parishName;
            _protectionLevel = protectionLevel;
            _provinceName = provinceName;
            _quantityOrArea = quantityOrArea;
            _redlistCategory = redlistCategory;
            _reportedDate = reportedDate;
            _sciCode = sciCode;
            _sciName = sciName;
            _scientificName = scientificName;
            _speciesActivityId = speciesActivityId;
            _startDate = startDate;
            _taxonId = taxonId;
            _taxonSortOrder = taxonSortOrder;
            _taxonUncertainty = taxonUncertainty;
            _unit = unit;
        }

        /// <summary>
        /// Get original id in the observers collection.
        /// </summary>
        public String AccessionId
        {
            get { return _accessionId; }
        }

        /// <summary>
        /// Get accuracy of the coordinate for the species observation.
        /// </summary>
        public Int32? Accuracy
        {
            get { return _accuracy; }
        }

        /// <summary>
        /// Get activity or substrate count.
        /// </summary>
        public String ActivityOrSubstrateCount
        {
            get { return _activityOrSubstrateCount; }
        }

        /// <summary>
        /// Get biotope or substrate.
        /// </summary>
        public String BiotopeOrSubstrate
        {
            get { return _biotopeOrSubstrate; }
        }

        /// <summary>
        /// Get collection.
        /// The observation belongs to this collection.
        /// </summary>
        public String Collection
        {
            get { return _collection; }
        }

        /// <summary>
        /// Get comment about the observation.
        /// </summary>
        public String Comment
        {
            get { return _comment; }
        }

        /// <summary>
        /// Get common name of the taxon.
        /// </summary>
        public String CommonName
        {
            get { return _commonName; }
        }

        /// <summary>
        /// Get name of the county in which the locality is located.
        /// </summary>
        public String CountyName
        {
            get { return _countyName; }
        }

        /// <summary>
        /// Get name of the database which the observation belongs to.
        /// </summary>
        public Int32 DatabaseId
        {
            get { return _databaseId; }
        }

        /// <summary>
        /// Get name of the database which the observation belongs to.
        /// </summary>
        public String DatabaseName
        {
            get { return _databaseName; }
        }

        /// <summary>
        /// Get observation id in the database which the observation belongs to.
        /// </summary>
        public Int32 DatabaseObservationId
        {
            get { return _databaseObservationId; }
        }

        /// <summary>
        /// Get the name(s) of the person(s) who applied the ScientificName to the observation.
        /// </summary>
        public String Determinator
        {
            get { return _determinator; }
        }

        /// <summary>
        /// East coordinate for the species observation.
        /// </summary>
        public Double EastCoordinate
        { get; set; }

        /// <summary>
        /// Get end date for the observation.
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
        }

        /// <summary>
        /// Get GUID (Globally unique identifier).
        /// </summary>
        public String Guid
        {
            get { return _guid; }
        }

        /// <summary>
        /// Get id for species observation.
        /// </summary>
        public Int64 Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Get life stage of the observed species.
        /// </summary>
        public String LifeStage
        {
            get { return _lifeStage; }
        }

        /// <summary>
        /// Get locality for the observation.
        /// </summary>
        public String Locality
        {
            get { return _locality; }
        }

        /// <summary>
        /// Get date and time when the species observation was last modified.
        /// </summary>
        public DateTime Modified
        {
            get { return _modified; }
        }

        /// <summary>
        /// Get name of the municipality in which the locality is located.
        /// </summary>
        public String MunicipalityName
        {
            get { return _municipalityName; }
        }

        /// <summary>
        /// Get never found.
        /// Indicates if this observation is a statement about if
        /// the species was not found in a locality with a
        /// suitable biotope.
        /// </summary>
        public Boolean NeverFound
        {
            get { return _neverFound; }
        }

        /// <summary>
        /// North coordinate for the species observation.
        /// </summary>
        public Double NorthCoordinate
        { get; set; }

        /// <summary>
        /// Get not rediscovered.
        /// Indicates if this observation is a statement about if
        /// the species was not rediscovered in a locality where
        /// it previously has been observed.
        /// </summary>
        public Boolean NotRediscovered
        {
            get { return _notRediscovered; }
        }

        /// <summary>
        /// Get list of people who made the observation.
        /// </summary>
        public String Observers
        {
            get { return _observers; }
        }

        /// <summary>
        /// Get name of the organism group that the taxon belongs to.
        /// </summary>
        public String OrganismGroupName
        {
            get { return _organismGroupName; }
        }

        /// <summary>
        /// Get sort order of the organism group that the taxon belongs to.
        /// </summary>
        public Int32 OrganismGroupSortOrder
        {
            get { return _organismGroupSortOrder; }
        }

        /// <summary>
        /// Get Origin of the observation.
        /// For private observations this is the same as the observer.
        /// For other purposes this is the context in which the observation was performed, e.g. inventory.
        /// </summary>
        public String Origin
        {
            get { return _origin; }
        }

        /// <summary>
        /// Get name of the parish in which the locality is located.
        /// </summary>
        public String ParishName
        {
            get { return _parishName; }
        }

        /// <summary>
        /// Get protection level for this observation.
        /// </summary>
        public Int32 ProtectionLevel
        {
            get { return _protectionLevel; }
        }

        /// <summary>
        /// Get name of the province in which the locality is located.
        /// </summary>
        public String ProvinceName
        {
            get { return _provinceName; }
        }

        /// <summary>
        /// Get number of observed individuals or area of the observed species.
        /// </summary>
        public String QuantityOrArea
        {
            get { return _quantityOrArea; }
        }

        /// <summary>
        /// Get redlist category.
        /// </summary>
        public String RedlistCategory
        {
            get { return _redlistCategory; }
        }

        /// <summary>
        /// Get reported date.
        /// </summary>
        public DateTime ReportedDate
        {
            get { return _reportedDate; }
        }

        /// <summary>
        /// Get SCI code.
        /// </summary>
        public String SciCode
        {
            get { return _sciCode; }
        }

        /// <summary>
        /// Get scientific name of the taxon.
        /// </summary>
        public String ScientificName
        {
            get { return _scientificName; }
        }

        /// <summary>
        /// Get SCI name.
        /// </summary>
        public String SciName
        {
            get { return _sciName; }
        }

        /// <summary>
        /// Get species activity id.
        /// </summary>
        public Int32? SpeciesActivityId
        {
            get { return _speciesActivityId; }
        }

        /// <summary>
        /// Get start date for the observation.
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
        }

        /// <summary>
        /// Get taxon id.
        /// </summary>
        public Int32 TaxonId
        {
            get { return _taxonId; }
        }

        /// <summary>
        /// Get sort order for the taxon.
        /// </summary>
        public Int32 TaxonSortOrder
        {
            get { return _taxonSortOrder; }
        }

        /// <summary>
        /// Get indication if taxon determination is uncertain.
        /// </summary>
        public String TaxonUncertainty
        {
            get { return _taxonUncertainty; }
        }

        /// <summary>
        /// Get information related to parameter quantityOrArea and the observed species.
        /// </summary>
        public String Unit
        {
            get { return _unit; }
        }
    }
}
